#ifndef DISKBURNSEQUENCE_H
#define DISKBURNSEQUENCE_H

/***
 * Handle sequence of actions to burn a single image
 * Microboards COM3 decode
9600 8n1

H - drop dvd, more motion of shutter
G - spindle up
J - something, stops motor sound
M - something
O - something
R - drop spindle
U - spins up dvd
V - spit out version info
W - something
X - something
A - something, conveyor advance? E if no DVD on conveyor, X if present. Drops DVD on conveyor if spindle up
B - returns M
C - returns E
D - drop dvd

Arduino COM5
F continuous forward
S stop
B continuous reverse
f forward 500ms
b backward 500ms

Recipe:
COM3: R
COM5: F (1000ms) S
c> nircmd cdrom open e:
G (wait 2s) D  (wait 2s) R (wait 2s)
c> nircmd cdrom close e:
(wait 3s)
Old method:
c> isoburn /Q E: Vastu-DVD.iso
(wait for completion of command)
New method:
c> burniso.exe --iso=Vastu-DVD.iso --burner=e: --statusfile=foo.log --automate
(look for line starting with fail or success in foo.log)

Failed:
COM3: G
nircmd cdrom close e:
COM3: R
COM5: b
(rotate accessible gear toward you to eject)

Success:
COM3: G
nircmd cdrom close e:
COM3: R
COM5: F (`000ms) S
(rotate accessible gear away many times to feed)

 ***/

#include "burnqueueentry.h"

#include <QObject>
#include <QString>
#include <QStringList>
#include <QList>
#include <QTemporaryFile>

#undef _WIN32_WINNT
#define _WIN32_WINNT    0x600
#include <windows.h>

class DiskBurnSequence : public QObject
{
    Q_OBJECT
public:
    explicit DiskBurnSequence(QObject *parent = 0);
    ~DiskBurnSequence();

protected:
    QString NIRCMD;
    QString DISKBURNER;

    QString m_disk;
    QTemporaryFile *m_statusFile;
    qint64 m_statusFileSize;
    QString m_statusFileName;
    bool m_statusFileComplete; // true if we've gotten success or error
    bool m_statusFileSuccess;
    int m_statusFileWait; // Wait in seconds for status file

    int m_sequenceIndex;
    QStringList m_sequence;

    QList<BurnQueueEntry*> m_queue;
    int m_queueIndex;
    bool m_queueActive;
    int m_queueSuccessCount;
    int m_queueFailureCount;
    QStringList m_queueSuccessList;

    bool m_verbose;

    qint64 m_burnerPid;
    HANDLE m_burnerProcessInfo;

signals:
    void ShowMsg( int level, QString s );
    void QueueCurrent( QString msg );
    void QueueSuccessCount( QString msg );
    void QueueFailureCount( QString msg );
    void ConveyorCmd( QString s );
    void G4Cmd( QString s );
    QString G4Ack( int timeout );
    void QueueCompleted();
    void QueueFailed( int reason );
    void SequenceCompleted();

public slots:
    void SetDisk(QString s) {m_disk = s;}
    bool Eject();
    bool Load();
    bool BurnISO(QString iso);
    bool MoveConveyorFor( int msDirection );
    void StopConveyor();
    void Step();
    void StepQueue();
    void CompleteQueueStep();

    void ResetSequence();
    void RewindSequence();
    void AddSequence( QString cmd );

    void ResetQueue();
    void AddQueueEntry( BurnQueueEntry *q );
    void StartQueue();
    void PauseQueue();
    void StopQueue();

    void ResetStatus();
    bool CheckStatus();
    int FindNextSequence(QString dest);

    bool ShowBurnerProcessInfo();

    void SetVerbose( bool verbose ) {m_verbose = verbose;}
};

#endif // DISKBURNSEQUENCE_H
