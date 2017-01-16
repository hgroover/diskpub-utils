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
c> isoburn /Q E: Vastu-DVD.iso
(wait for completion of command)

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

#include <QObject>
#include <QString>
#include <QStringList>

class DiskBurnSequence : public QObject
{
    Q_OBJECT
public:
    explicit DiskBurnSequence(QObject *parent = 0);

protected:
    QString NIRCMD;
    QString DISKBURNER;

    QString m_disk;

    int m_sequenceIndex;
    QStringList m_sequence;

signals:
    void ShowMsg( QString s );
    void ConveyorCmd( QString s );
    void G4Cmd( QString s );
    QString G4Ack( int timeout );

public slots:
    void SetDisk(QString s) {m_disk = s;}
    bool Eject();
    bool Load();
    bool BurnISO(QString iso);
    bool MoveConveyorFor( int msDirection );
    void StopConveyor();
    void Step();

    void ResetSequence();
    void AddSequence( QString cmd );
};

#endif // DISKBURNSEQUENCE_H