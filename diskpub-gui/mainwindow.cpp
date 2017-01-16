#include "mainwindow.h"
#include "ui_mainwindow.h"

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
    connect( m_burn, SIGNAL(ShowMsg(QString)), this, SLOT(Log(QString)) );
    connect( m_burn, SIGNAL(ConveyorCmd(QString)), m_conveyor, SLOT(SendCmd(QString)) );
    connect( m_burn, SIGNAL(G4Cmd(QString)), m_g4, SLOT(SendCmd(QString)) );
    connect( m_burn, SIGNAL(G4Ack(int)), m_g4, SLOT(Read(int)) );
    m_g4->GetResponses();
    m_g4->SetAck(true);
    m_conveyor->GetResponses();
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
    if (s.isEmpty()) return;
    ui->txtLog->appendPlainText(s);
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
    Log(QString().sprintf("Burning ISO (%.1fmb) %s", m_isoSize / (1024.0 * 1024), fi.fileName().toLocal8Bit().constData()));
}

void MainWindow::on_btnTest3_clicked()
{
    m_startPhase3 = QDateTime::currentDateTime();
    qint64 elapsed = m_startPhase3.toMSecsSinceEpoch() - m_startPhase2.toMSecsSinceEpoch();
    double elapsedSeconds = elapsed / 1000.0;
    double secondsPerMb = (m_isoSize / (1024.0 * 1024)) / elapsedSeconds;
    Log( QString().sprintf("Elapsed: %.1fs for %.1fmb; %.1f MB/s", elapsedSeconds, m_isoSize / (1024.0 * 1024), secondsPerMb));
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
    QStringList aHistory(s.value("History").toStringList());
    int n;
    for (n = 0; n < aHistory.length(); n++)
    {
        ui->lstHistory->addItem(aHistory[n]);
    }
}

void MainWindow::SaveSettings()
{
    QSettings s("GrooverSoft", "DiskPub");
    s.setValue("ISO", ui->txtISO->text());
    QStringList aHistory;
    int n;
    for (n = 0; n < ui->lstHistory->count(); n++)
    {
        aHistory << ui->lstHistory->item(n)->text();
    }
    s.setValue("History", aHistory);
}

