#include "mainwindow.h"
#include "ui_mainwindow.h"

MainWindow::MainWindow(QWidget *parent) :
    QMainWindow(parent),
    ui(new Ui::MainWindow)
{
    ui->setupUi(this);
    m_conveyor = new RobotHandler("COM5");
    m_g4 = new RobotHandler("COM3");
    connect( m_conveyor, SIGNAL(ShowMsg(QString)), this, SLOT(Log(QString)) );
    connect( m_g4, SIGNAL(ShowMsg(QString)), this, SLOT(Log(QString)) );
}

MainWindow::~MainWindow()
{
    delete ui;
}

void MainWindow::Shutdown()
{
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
    ui->txtLog->appendPlainText(s);
}
