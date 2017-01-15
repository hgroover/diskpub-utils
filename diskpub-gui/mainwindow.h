#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QMainWindow>

#include "robothandler.h"

namespace Ui {
class MainWindow;
}

class MainWindow : public QMainWindow
{
    Q_OBJECT

public:
    explicit MainWindow(QWidget *parent = 0);
    ~MainWindow();

protected:
    void Shutdown();

signals:
    void ShowReceived( QString s );

protected:
    RobotHandler * m_conveyor;
    RobotHandler * m_g4;

public slots:
    void Log( QString s );

private slots:
    void on_btnQuit_clicked();

    void on_btnAdvance_clicked();

private:
    Ui::MainWindow *ui;
};

#endif // MAINWINDOW_H
