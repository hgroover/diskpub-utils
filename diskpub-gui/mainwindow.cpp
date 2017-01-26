#include "mainwindow.h"
#include "ui_mainwindow.h"

#include "burnqueueentry.h"

#include <QFileDialog>
#include <QFileInfo>
#include <QSettings>
#include <QList>
#include <QListWidgetItem>

MainWindow::MainWindow(QWidget *parent) :
    QMainWindow(parent),
    ui(new Ui::MainWindow)
{
    ui->setupUi(this);
    LoadSettings();
    m_conveyor = new RobotHandler("COM5");
    m_g4 = new RobotHandler("COM3");
    m_burn = new DiskBurnSequence();
    m_burn->SetDisk("e:");
    connect( m_conveyor, SIGNAL(ShowMsg(QString)), this, SLOT(Log(QString)) );
    connect( m_g4, SIGNAL(ShowMsg(QString)), this, SLOT(Log(QString)) );
    connect( m_burn, SIGNAL(ShowMsg(int,QString)), this, SLOT(LogIfVerbose(int,QString)) );
    connect( m_burn, SIGNAL(ConveyorCmd(QString)), m_conveyor, SLOT(SendCmd(QString)) );
    connect( m_burn, SIGNAL(G4Cmd(QString)), m_g4, SLOT(SendCmd(QString)) );
    connect( m_burn, SIGNAL(G4Ack(int)), m_g4, SLOT(Read(int)) );
    connect( m_burn, SIGNAL(QueueCurrent(QString)), this, SLOT(onQueueCurrent(QString)) );
    connect( m_burn, SIGNAL(QueueFailureCount(QString)), this, SLOT(onQueueFailureCount(QString)) );
    connect( m_burn, SIGNAL(QueueSuccessCount(QString)), this, SLOT(onQueueSuccessCount(QString)) );
    connect( this, SIGNAL(AbortQueue()), m_burn, SLOT(StopQueue()) );
    connect( this, SIGNAL(PauseQueue()), m_burn, SLOT(PauseQueue()) );
    connect( this, SIGNAL(StartQueue()), m_burn, SLOT(StartQueue()) );
    connect( m_burn, SIGNAL(QueueCompleted()), this, SLOT(onQueueCompleted()) );
    connect( m_burn, SIGNAL(QueueFailed(int)), this, SLOT(onQueueFailed(int)) );
    //connect( this, SIGNAL(VerboseChanged(bool)), m_burn, SLOT(SetVerbose(bool)) );
    m_logFile.setFileName("diskpub-gui.log");
    m_logFile.open(QIODevice::Append);
    m_g4->GetResponses();
    m_g4->SetAck(true);
    m_conveyor->GetResponses();
    Log("diskpub-gui " PROGRAM_VERSION);
}

MainWindow::~MainWindow()
{
    delete ui;
}

void MainWindow::Shutdown()
{
    SaveSettings();
    if (m_conveyor)
    {
        delete m_conveyor;
        m_conveyor = NULL;
    }
    if (m_g4)
    {
        delete m_g4;
        m_g4 = NULL;
    }
    close();
}

void MainWindow::on_btnQuit_clicked()
{
    Shutdown();
}

void MainWindow::on_btnAdvance_clicked()
{
    m_conveyor->SendCmd("f");
}

void MainWindow::Log(QString s)
{
    LogIfVerbose(1, s);
}

// Log visibly if chkVerbose is checked or level < 1
void MainWindow::LogIfVerbose(int level, QString s)
{
    if (s.isEmpty()) return;
    LogToFile(s);
    if (level > 0 && !ui->chkVerbose->isChecked()) return;
    ui->txtLog->appendPlainText(s);
}

void MainWindow::LogToFile( QString s )
{
    if (!m_logFile.isOpen()) return;
    m_logFile.write(QDateTime::currentDateTime().toString(LOGFILE_DTFORMAT).append(s).append("\r\n").toLocal8Bit() );
    m_logFile.flush();
}

void MainWindow::on_btnEject_clicked()
{
    m_burn->Eject();
}

void MainWindow::on_btnLoad_clicked()
{
    m_burn->Load();
}

void MainWindow::on_btnTest1_clicked()
{
    m_burn->ResetSequence();
    m_burn->AddSequence("g:750:R");
    m_burn->AddSequence("c:1000:F");
    m_burn->AddSequence("c:100:S");
    m_burn->AddSequence("e:1000:");
    m_burn->AddSequence("g:2000:G");
    m_burn->AddSequence("g:2000:D");
    m_burn->AddSequence("g:2000:R");
    m_burn->AddSequence("l:3000:");
    m_burn->Step();
}

void MainWindow::on_btnTest2_clicked()
{
    m_startPhase2 = QDateTime::currentDateTime();
    QFileInfo fi(ui->txtISO->text());
    m_isoSize = fi.size();
    m_burn->BurnISO(ui->txtISO->text());
    LogIfVerbose( 0, QString().sprintf("Burning ISO (%.1fmb) %s", m_isoSize / (1024.0 * 1024), fi.fileName().toLocal8Bit().constData()));
}

void MainWindow::on_btnTest3_clicked()
{
    m_startPhase3 = QDateTime::currentDateTime();
    qint64 elapsed = m_startPhase3.toMSecsSinceEpoch() - m_startPhase2.toMSecsSinceEpoch();
    double elapsedSeconds = elapsed / 1000.0;
    double secondsPerMb = (m_isoSize / (1024.0 * 1024)) / elapsedSeconds;
    LogIfVerbose( 0, QString().sprintf("Elapsed: %.1fs for %.1fmb; %.1f MB/s", elapsedSeconds, m_isoSize / (1024.0 * 1024), secondsPerMb));
    m_burn->ResetSequence();
    m_burn->AddSequence("g:1500:G");
    m_burn->AddSequence("l:3000:");
    m_burn->AddSequence("g:1500:R");
    m_burn->AddSequence("c:1500:F");
    m_burn->AddSequence("c:500:S");
    m_burn->Step();
}

void MainWindow::on_btnISOBrowse_clicked()
{
    QFileDialog dlg;
    dlg.selectNameFilter("*.iso");
    QString fileName = QFileDialog::getOpenFileName( this, tr("Select ISO file"), "", tr("ISO files (*.iso)") );
    if (!fileName.isEmpty())
    {
        ui->txtISO->setText(fileName);
        QList<QListWidgetItem*> searchRes = ui->lstHistory->findItems(fileName, Qt::MatchExactly);
        if (searchRes.size() == 0)
        {
            ui->lstHistory->addItem(fileName);
        }
    }
}

void MainWindow::on_btnR_clicked()
{
    m_g4->GetResponses();
    m_g4->SendCmd("R");
    QTimer::singleShot(1000, m_g4, SLOT(GetResponses()));
    QTimer::singleShot(2000, m_g4, SLOT(GetResponses()));
}

void MainWindow::on_btnG_clicked()
{
    m_g4->GetResponses();
    m_g4->SendCmd("G");
    QTimer::singleShot(1000, m_g4, SLOT(GetResponses()));
}

void MainWindow::on_lstHistory_doubleClicked(const QModelIndex &index)
{
    int row = index.row();
    if (row < 0) return;
    ui->txtISO->setText(ui->lstHistory->item(row)->text());
}

void MainWindow::LoadSettings()
{
    QSettings s("GrooverSoft", "DiskPub");
    ui->txtISO->setText(s.value("ISO").toString());
    ui->chkVerbose->setChecked(s.value("Verbose").toBool());
    QStringList aHistory(s.value("History").toStringList());
    int n;
    for (n = 0; n < aHistory.length(); n++)
    {
        ui->lstHistory->addItem(aHistory[n]);
    }
    QStringList aQueue(s.value("Queue").toStringList());
    for (n =  0; n < aQueue.length(); n++)
    {
        ui->lstQueue->addItem(aQueue[n]);
    }
    restoreGeometry(s.value("Geometry").toByteArray());
}

void MainWindow::SaveSettings()
{
    QSettings s("GrooverSoft", "DiskPub");
    s.setValue("Geometry", saveGeometry());
    s.setValue("ISO", ui->txtISO->text());
    s.setValue("Verbose", ui->chkVerbose->isChecked());
    QStringList aHistory;
    int n;
    for (n = 0; n < ui->lstHistory->count(); n++)
    {
        aHistory << ui->lstHistory->item(n)->text();
    }
    s.setValue("History", aHistory);
    QStringList aQueue;
    for (n = 0; n < ui->lstQueue->count(); n++)
    {
        aQueue << ui->lstQueue->item(n)->text();
    }
    s.setValue("Queue", aQueue);
}

void MainWindow::onQueueCurrent(QString msg)
{
    ui->lblQueueCurrent->setText(msg);
}

void MainWindow::onQueueSuccessCount(QString msg)
{
    ui->lblQueueSucceeded->setText(msg);
}

void MainWindow::onQueueFailureCount(QString msg)
{
    ui->lblQueueFailed->setText(msg);
}


void MainWindow::on_lstQueue_itemClicked(QListWidgetItem *item)
{
    // Enable delete
    ui->btnDeleteFromQueue->setEnabled(true);
}

void MainWindow::on_btnAddToQueue_clicked()
{
    // Add current iso
    if (!ui->txtISO->text().isEmpty())
    {
        ui->lstQueue->addItem(ui->txtISO->text());
        return;
    }
    if (ui->lstHistory->currentRow() >= 0 && ui->lstHistory->count() > 0)
    {
        ui->lstQueue->addItem(ui->lstHistory->item(ui->lstHistory->currentRow())->text());
        return;
    }
}

void MainWindow::on_btnStartQueue_clicked()
{
    if (ui->lstQueue->count()==0)
    {
        LogIfVerbose(0, "No entries in queue");
        return;
    }
    ui->btnStartQueue->setEnabled(false);
    ui->btnPauseQueue->setEnabled(true);
    ui->btnStopQueue->setEnabled(true);
    m_burn->ResetQueue();
    int row;
    for (row = 0; row < ui->lstQueue->count(); row++)
    {
        QStringList a(ui->lstQueue->item(row)->text().split('|'));
        int copies = 1;
        if (a.size()>1) copies = a[1].toInt();
        m_burn->AddQueueEntry( new BurnQueueEntry( a[0], row, copies ) );
    }
    LogIfVerbose(0, QString().sprintf("Starting queue with %d entries", ui->lstQueue->count()));
    emit StartQueue();
}

void MainWindow::on_btnPauseQueue_clicked()
{
    // Pause execution
    emit PauseQueue();
}

void MainWindow::on_btnStopQueue_clicked()
{
    emit AbortQueue();
    ui->btnStartQueue->setEnabled(true);
    ui->btnPauseQueue->setEnabled(false);
    ui->btnStopQueue->setEnabled(false);
}

void MainWindow::onQueueCompleted()
{
    ui->btnStartQueue->setEnabled(true);
    ui->btnPauseQueue->setEnabled(false);
    ui->btnStopQueue->setEnabled(false);
}

void MainWindow::onQueueFailed( int reason )
{
    LogIfVerbose(0, QString().sprintf("Queue failure reason %d", reason));
    ui->btnStartQueue->setEnabled(true);
    ui->btnPauseQueue->setEnabled(false);
    ui->btnStopQueue->setEnabled(false);
}

void MainWindow::on_btnDeleteFromQueue_clicked()
{
    int row = ui->lstQueue->currentRow();
    if (row < 0)
    {
        LogIfVerbose(-1, "Invalid row, cannot delete");
        ui->btnDeleteFromQueue->setEnabled(false);
    }
    QListWidgetItem *i = ui->lstQueue->takeItem(row);
    if (i) delete i;
    if (row < ui->lstQueue->count())
    {
        ui->lstQueue->setCurrentRow(row);
    }
    else if (ui->lstQueue->count()>0)
    {
        ui->lstQueue->setCurrentRow(0);
    }
    else
    {
        ui->btnDeleteFromQueue->setEnabled(false);
    }
    LogIfVerbose(0, QString().sprintf("Queue item %d deleted", row));
}

void MainWindow::on_btnReject_clicked()
{
    m_burn->ResetSequence();
    m_burn->AddSequence("g:1500:G");
    m_burn->AddSequence("l:3000:");
    m_burn->AddSequence("g:1500:R");
    m_burn->AddSequence("c:800:B");
    m_burn->AddSequence("c:500:S");
    m_burn->Step();
    LogIfVerbose(0, "Disk sent out back to reject pile");
}

void MainWindow::on_btnMoreCopies_clicked()
{
    int row = ui->lstQueue->currentRow();
    if (row < 0)
    {
        LogIfVerbose(-1, "Invalid current row");
        return;
    }
    QListWidgetItem *item = ui->lstQueue->item(row);
    QString s(item->text());
    QStringList a(s.split('|'));
    if (a.length()<2)
    {
        a << "2";
    }
    else
    {
        a[1] = QString().sprintf("%d", a[1].toInt() + 1);
    }
    item->setText(a[0] + "|" + a[1]);
}

void MainWindow::on_btnLessCopies_clicked()
{
    int row = ui->lstQueue->currentRow();
    if (row < 0)
    {
        LogIfVerbose(-1, "Invalid current row");
        return;
    }
    QListWidgetItem *item = ui->lstQueue->item(row);
    QString s(item->text());
    QStringList a(s.split('|'));
    if (a.length()<2)
    {
        a << "0";
    }
    else
    {
        int v = a[1].toInt();
        if (v > 0)
        {
            a[1] = QString().sprintf("%d", v - 1);
        }
    }
    item->setText(a[0] + "|" + a[1]);
}

void MainWindow::on_lstQueue_currentRowChanged(int currentRow)
{
    if (currentRow < 0)
    {
        ui->btnDeleteFromQueue->setEnabled(false);
        ui->btnMoreCopies->setEnabled(false);
        ui->btnLessCopies->setEnabled(false);
    }
    else
    {
        ui->btnDeleteFromQueue->setEnabled(true);
        ui->btnMoreCopies->setEnabled(true);
        ui->btnLessCopies->setEnabled(true);
    }
}

void MainWindow::on_pushButton_clicked()
{
    m_burn->ShowBurnerProcessInfo();
}

void MainWindow::on_chkVerbose_toggled(bool checked)
{
    emit VerboseChanged( checked );
}
