#include "diskburnsequence.h"

#include <QProcess>
#include <QTimer>
#include <QString>
#include <QStringList>

DiskBurnSequence::DiskBurnSequence(QObject *parent) : QObject(parent)
{
    NIRCMD = "c:\\Users\\hgroover\\Documents\\dvd-iso\\nircmd.exe";
    DISKBURNER = "isoburn.exe"; // c:\windows\system32
    m_queueActive = false;
    m_queueIndex = 0;
}

bool DiskBurnSequence::Eject()
{
    QProcess pNir;
    QStringList cmdArgs;
    cmdArgs << "cdrom";
    cmdArgs << "open";
    cmdArgs << m_disk;
    int r = pNir.execute(NIRCMD, cmdArgs);
    if (!pNir.waitForFinished(3000))
    {
        emit ShowMsg("Timeout on eject");
        return false;
    }
    int finalRes = pNir.exitCode();
    emit ShowMsg(QString().sprintf("Eject completed, %d/%d", r, finalRes));
    return true;
}

bool DiskBurnSequence::Load()
{
    QProcess pNir;
    QStringList cmdArgs;
    cmdArgs << "cdrom";
    cmdArgs << "close";
    cmdArgs << m_disk;
    int r = pNir.execute(NIRCMD, cmdArgs);
    if (!pNir.waitForFinished(3000))
    {
        emit ShowMsg("Timeout on load");
        return false;
    }
    int finalRes = pNir.exitCode();
    emit ShowMsg(QString().sprintf("Load completed, %d/%d", r, finalRes));
    return true;
}

bool DiskBurnSequence::MoveConveyorFor(int msDirection)
{
    QString cmd("F");
    if (msDirection < 0)
    {
        cmd = "B";
    }
    emit ConveyorCmd(cmd);
    QTimer::singleShot(abs(msDirection), this, SLOT(StopConveyor()) );
    return true;
}

void DiskBurnSequence::StopConveyor()
{
    emit ConveyorCmd("S");
}

void DiskBurnSequence::Step()
{
    if (m_sequenceIndex < 0 || m_sequenceIndex >= m_sequence.length()) return;
    // Split into destination (e, l, g, c, b), post-command timeout, and command
    QStringList e;
    e << m_sequence.at(m_sequenceIndex).split(':');
    if (e.length() < 3)
    {
        emit ShowMsg("Error: bad cmd " + m_sequence.at(m_sequenceIndex));
        return;
    }
    // e[0] is destination
    QString dest(e[0]);
    int postCmdTimeout = e[1].toInt();
    QString cmd(e[2]);
    emit ShowMsg(QString().sprintf("Seq[%d] to%d ", m_sequenceIndex, postCmdTimeout) + m_sequence.at(m_sequenceIndex) );
    if (dest == "e")
    {
        Eject();
    }
    else if (dest == "l")
    {
        Load();
    }
    else if (dest == "g")
    {
        if (cmd == "ack")
        {
            emit ShowMsg(G4Ack(500));
        }
        else
        {
            emit G4Cmd(cmd);
        }
    }
    else if (dest == "c")
    {
        emit ConveyorCmd(cmd);
    }
    else if (dest == "b")
    {
        // Burner takes only iso
        if (cmd == "iso")
        {
            if (m_queueActive)
            {
                BurnISO(m_queue[m_queueIndex]->Iso());
            }
            // Post command timeout has been calculated at 9.5mb/s or 6mb/s for DL
        }
        else
        {
            emit ShowMsg("Error: unknown burn command " + cmd);
        }
    }
    else
    {
        emit ShowMsg( "Unknown destination " + dest);
    }
    // Post-command timeout has no effect if last
    m_sequenceIndex++;
    if (m_sequenceIndex >= m_sequence.length())
    {
        emit SequenceCompleted();
        return;
    }
    QTimer::singleShot(postCmdTimeout, this, SLOT(Step()));
}

void DiskBurnSequence::RewindSequence()
{
    m_sequenceIndex = 0;
}

void DiskBurnSequence::ResetSequence()
{
    m_sequenceIndex = 0;
    m_sequence.clear();
}

void DiskBurnSequence::AddSequence( QString cmd )
{
    m_sequence.append(cmd);
}

bool DiskBurnSequence::BurnISO(QString iso)
{
    //QProcess pBurn;
    QStringList cmdArgs;
    cmdArgs << "/Q";
    cmdArgs << m_disk;
    cmdArgs << iso.replace('/', "\\");
    emit ShowMsg(iso + " " + cmdArgs[0] + " " + cmdArgs[1] + " " + cmdArgs[2]);
    QProcess::startDetached(DISKBURNER, cmdArgs);
    //pBurn.execute(DISKBURNER, cmdArgs);
    return true;
}

void DiskBurnSequence::ResetQueue()
{
    BurnQueueEntry *q;
    while (!m_queue.isEmpty())
    {
        q = m_queue.takeLast();
        delete q;
    }
    m_queue.clear();
}

void DiskBurnSequence::AddQueueEntry( BurnQueueEntry *q )
{
    m_queue.append(q);
}

void DiskBurnSequence::StartQueue()
{
    m_queueIndex = 0;
    m_queueActive = true;
    connect( this, SIGNAL(SequenceCompleted()), this, SLOT(CompleteQueueStep()) );
    emit ShowMsg("Queue started");
    StepQueue();
}

void DiskBurnSequence::PauseQueue()
{
    m_queueActive = !m_queueActive;
    emit ShowMsg(m_queueActive ? "Queue resumed" : "Queue paused");
}

void DiskBurnSequence::StopQueue()
{
    m_queueActive = false;
    emit ShowMsg("Queue aborted");
    disconnect( this, SIGNAL(SequenceCompleted()), this, SLOT(CompleteQueueStep()) );
    emit QueueFailed(-1);
}

void DiskBurnSequence::StepQueue()
{
    if (!m_queueActive) return;
    // Set up entries
    emit ShowMsg(QString().sprintf("Starting queue entry %d, iso %s (%.1fMB)", m_queueIndex + 1, m_queue[m_queueIndex]->Iso().toLocal8Bit().constData(),
                 m_queue[m_queueIndex]->IsoSizeMB()) );
    ResetSequence();
    AddSequence("g:750:R");
    AddSequence("c:1000:F");
    AddSequence("c:100:S");
    AddSequence("e:1000:");
    AddSequence("g:2000:G");
    AddSequence("g:2000:D");
    AddSequence("g:2000:R");
    AddSequence("l:14000:"); // Allow drive to spin up
    unsigned long ms = (unsigned long)(m_queue[m_queueIndex]->IsoSizeMB() * 1000.0 / 9.5);
    if (m_queue[m_queueIndex]->IsoSizeMB() > 4400.0)
    {
        // For DL, use 6mb/s as assumed rate (FIXME this should be calibrated for each drive)
        ms = ms * 95 / 60;
    }
    // Add an extra 4s
    ms += 4000;
    AddSequence(QString().sprintf("b:%lu:iso", ms));
    AddSequence("g:1500:G");
    AddSequence("l:3000:");
    AddSequence("g:1500:R");
    AddSequence("c:1500:F");
    AddSequence("c:500:S");
    QTimer::singleShot(1000, this, SLOT(Step()));
}

void DiskBurnSequence::CompleteQueueStep()
{
    // More copies?
    if (m_queue[m_queueIndex]->CompleteCopy())
    {
        RewindSequence();
        emit ShowMsg(QString().sprintf("Queue entry %d: Copy %d completed", m_queueIndex + 1, m_queue[m_queueIndex]->CurrentCopyIndex()));
        QTimer::singleShot( 2000, this, SLOT(Step()) );
        return;
    }
    m_queueIndex++;
    if (m_queueIndex >= m_queue.count())
    {
        m_queueActive = false;
        emit ShowMsg("Queue completed");
        emit QueueCompleted();
        ResetQueue();
        disconnect( this, SIGNAL(SequenceCompleted()), this, SLOT(CompleteQueueStep()) );
        return;
    }
    emit ShowMsg(QString().sprintf("Queue entry %d completed", m_queueIndex));
    QTimer::singleShot(2000, this, SLOT(StepQueue()));
}
