#include "diskburnsequence.h"

#include <QProcess>
#include <QTimer>
#include <QString>
#include <QStringList>
#include <QFile>
#include <QFileInfo>

DiskBurnSequence::DiskBurnSequence(QObject *parent) : QObject(parent)
{
    NIRCMD = "c:\\Users\\hgroover\\Documents\\dvd-iso\\nircmd.exe";
    DISKBURNER = "../../ISOBurner/bin/ISOBurner.exe"; // "isoburn.exe"; // c:\windows\system32
    m_queueActive = false;
    m_queueIndex = 0;
    m_burnerPid = 0;
    m_burnerProcessInfo = NULL;
    m_statusFile = NULL;
    m_statusFileSize = 0LL;
    m_statusFileComplete = false;
    m_statusFileSuccess = false;
    m_statusFileWait = 0;
}

DiskBurnSequence::~DiskBurnSequence()
{
    ResetStatus();
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
    // FIXME handle queue pause / resume / cancel (outside of burn iso)
    // Split into destination (e, l, g, c, b, w, if, else, fi), post-command timeout, and command
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
    // Suppress if waiting for status file
    if (dest != "if" || m_statusFileWait == 0)
    {
        emit ShowMsg(QString().sprintf("Seq[%d] to%d ", m_sequenceIndex, postCmdTimeout) + m_sequence.at(m_sequenceIndex) );
    }
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
    else if (dest == "w")
    {
        // Simple wait with message
        emit ShowMsg(QString().sprintf("%s (%.1fs)", cmd.toLocal8Bit().constData(), postCmdTimeout / 1000.0));
    }
    else if (dest == "if")
    {
        // Wait for burn result
        if (!CheckStatus())
        {
            // Wait another second
            m_statusFileWait++;
            QTimer::singleShot(1000, this, SLOT(Step()));
            return;
        }
        if (m_statusFileSuccess)
        {
            emit ShowMsg( QString().sprintf("Disk burn success after %d seconds", m_statusFileWait) );
            // Continue with next statement after if
        }
        else
        {
            emit ShowMsg( QString().sprintf("Disk burn FAILED after %d seconds", m_statusFileWait) );
            // Advance to else
            m_sequenceIndex = FindNextSequence("else");
        }
    }
    else if (dest == "else")
    {
        // Failure path for burn result
        if (m_statusFileSuccess)
        {
            // Skip to end of else block
            m_sequenceIndex = FindNextSequence("fi");
        }
    }
    else if (dest == "fi")
    {
        // End of conditional path
        emit ShowMsg("Reached end of if/else/fi");
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

// Find next sequence dest field. Return a safe assignment to
// m_sequenceIndex, which is m_sequenceIndex on error
int DiskBurnSequence::FindNextSequence(QString dest)
{
    int n;
    for (n = m_sequenceIndex + 1; n < m_sequence.length(); n++)
    {
        QStringList e;
        e << m_sequence.at(n).split(':');
        if (e.length() < 3)
        {
            emit ShowMsg("Error: bad cmd " + m_sequence.at(n));
            return m_sequenceIndex;
        }
        if (e[0] == dest)
        {
            // Found our target
            return n;
        }
    }
    emit ShowMsg( "Failed to find " + dest );
    return m_sequenceIndex;
}

void DiskBurnSequence::AddSequence( QString cmd )
{
    m_sequence.append(cmd);
}

void DiskBurnSequence::ResetStatus()
{
    if (m_statusFile)
    {
        m_statusFile->deleteLater();
        m_statusFile = NULL;
    }
    m_statusFileComplete = false;
    m_statusFileSuccess = false;
    m_statusFileSize = 0LL;
    m_statusFileWait = 0;
}

// Read status file. Echo messages (msg) and look for error or success
bool DiskBurnSequence::CheckStatus()
{
    if (m_statusFileComplete)
    {
        return true;
    }
    QFileInfo fi(m_statusFileName);
    if (fi.size() == m_statusFileSize)
    {
        return false; // no change
    }
    QFile f(m_statusFileName);
    if (!f.open(QIODevice::ReadOnly))
    {
        emit ShowMsg("Failed to open " + m_statusFileName);
        m_statusFileSuccess = false;
        m_statusFileComplete = true;
        return true;
    }
    if (!f.seek(m_statusFileSize))
    {
        emit ShowMsg("Failed to seek on " + m_statusFileName);
        m_statusFileSuccess = false;
        m_statusFileComplete = true;
        f.close();
        return true;
    }
    QString s(f.readAll());
    m_statusFileSize += s.length();
    f.close();
    if (s.length() == 0)
    {
        return false; // no change
    }
    QStringList a(s.split('\n', QString::SkipEmptyParts));
    if (a.length() == 0)
    {
        return false; // no change - blank lines perhaps
    }
    int n;
    for (n = 0; n < a.length(); n++)
    {
        if (a[n].startsWith("msg "))
        {
            emit ShowMsg(a[n]);
        }
        else if (a[n].startsWith("error "))
        {
            emit ShowMsg(a[n]);
            m_statusFileSuccess = false;
            m_statusFileComplete = true;
        }
        else if (a[n].startsWith("success "))
        {
            if (m_statusFileComplete)
            {
                emit ShowMsg("Ignoring conflicting " + a[n]);
            }
            else
            {
                emit ShowMsg(a[n]);
                m_statusFileSuccess = true;
                m_statusFileComplete = true;
            }
        }
        else
        {
            emit ShowMsg("Unknown status: " + a[n]);
        }
    }
    return m_statusFileComplete;
}

bool DiskBurnSequence::BurnISO(QString iso)
{
    // ISOBurner.exe --iso=file.iso --burner=d: --automate --statusfile=tempfile.log
    QStringList cmdArgs;
    ResetStatus();
    /** for windows burniso
    cmdArgs << "/Q";
    cmdArgs << m_disk;
    cmdArgs << iso.replace('/', "\\");
    emit ShowMsg(iso + " " + cmdArgs[0] + " " + cmdArgs[1] + " " + cmdArgs[2]);
    ***/
    cmdArgs << ("--isofile=" + iso.replace('/',"\\"));
    cmdArgs << ("--burner=" + m_disk);
    cmdArgs << "--automate";
    m_statusFile = new QTemporaryFile();
    m_statusFile->open();
    m_statusFile->close();
    m_statusFileSize = 0LL;
    m_statusFileName = m_statusFile->fileName();
    // Close and delete file, otherwise it cannot be written to by ISOBurner
    delete m_statusFile;
    m_statusFile = NULL;
    cmdArgs << ("--statusfile=" + m_statusFileName);
    emit ShowMsg(DISKBURNER + " " + cmdArgs[0] + " "  + cmdArgs[1] + " " + cmdArgs[2] + " " + cmdArgs[3]);
    m_burnerPid = 0;
    if (m_burnerProcessInfo)
    {
        ::CloseHandle(m_burnerProcessInfo);
        m_burnerProcessInfo = NULL;
    }
    QProcess::startDetached(DISKBURNER, cmdArgs, QString(), &m_burnerPid);
    if (m_burnerPid)
    {
        m_burnerProcessInfo = ::OpenProcess( PROCESS_QUERY_INFORMATION, FALSE, m_burnerPid );
        emit ShowMsg(QString().sprintf("Burner pid = %llx, hApp = %llx, status = %s\n",
                                       m_burnerPid,
                                       (unsigned long long)m_burnerProcessInfo,
                                       m_statusFileName.toLocal8Bit().constData()
                            ));
    }
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
    if (m_queue[m_queueIndex]->IsoSizeMB() < 0.1)
    {
        emit ShowMsg(QString().sprintf("Invalid iso size %.1fmb", m_queue[m_queueIndex]->IsoSizeMB()));
        StopQueue();
        return;
    }
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
    unsigned long ms = 12000;
    bool isDL = (m_queue[m_queueIndex]->IsoSizeMB() > 4400.0);
    if (isDL)
    {
        // Dual layer needs more time to spin up
        ms += 6000;
    }
    AddSequence("l:2000:");
    AddSequence(QString().sprintf("w:%lu:Waiting for drive to spin up", ms));
    /******
    ms = (unsigned long)(m_queue[m_queueIndex]->IsoSizeMB() * 1000.0 / 9.5);
    if (isDL)
    {
        // For DL, use 4.5mb/s as assumed rate (FIXME this should be calibrated for each drive)
        ms = ms * 95 / 45;
    }
    // Add an extra 4s
    ms += 4000;
    ****/
    ms = 5000;
    AddSequence(QString().sprintf("b:%lu:iso", ms));
    AddSequence("if:1000:statusfile");
    AddSequence("w:6000:Burn complete, allowing time for eject");
    AddSequence("g:1500:G");
    AddSequence("l:3000:");
    AddSequence("g:1500:R");
    AddSequence("c:1500:F");
    AddSequence("c:500:S");
    AddSequence("else:0:");
    AddSequence("w:6000:Burn failed, allowing time for eject");
    AddSequence("g:1500:G");
    AddSequence("l:3000:");
    AddSequence("g:1500:R");
    AddSequence("c:1000:B");
    AddSequence("c:500:S");
    AddSequence("fi:0:");
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

bool DiskBurnSequence::ShowBurnerProcessInfo()
{
    if (m_burnerPid == 0LL || NULL == m_burnerProcessInfo)
    {
        emit ShowMsg("No burner running or no handle");
        return false;
    }
    quint64 cycles;
    if (::QueryProcessCycleTime(m_burnerProcessInfo, &cycles))
    {
        emit ShowMsg(QString().sprintf("Cycle time: %llu", cycles));
    }
    else
    {
        emit ShowMsg("Query cycle time failed");
        return false;
    }
    return true;
}
