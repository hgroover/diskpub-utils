#include "diskburnsequence.h"

#include <QProcess>
#include <QTimer>
#include <QString>
#include <QStringList>

DiskBurnSequence::DiskBurnSequence(QObject *parent) : QObject(parent)
{
    NIRCMD = "c:\\Users\\hgroover\\Documents\\dvd-iso\\nircmd.exe";
    DISKBURNER = "isoburn.exe"; // c:\windows\system32
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
    // Split into destination (e, l, g, c), post-command timeout, and command
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
    else
    {
        emit ShowMsg( "Unknown destination " + dest);
    }
    // Post-command timeout has no effect if last
    m_sequenceIndex++;
    if (m_sequenceIndex >= m_sequence.length()) return;
    QTimer::singleShot(postCmdTimeout, this, SLOT(Step()));
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
