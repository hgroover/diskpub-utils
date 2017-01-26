#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QMainWindow>
#include <QDateTime>
#include <QListWidgetItem>
#include <QFile>

#include "robothandler.h"
#include "diskburnsequence.h"

#define PROGRAM_VERSION "0.13"
#define LOGFILE_DTFORMAT "[ddd yyyy-MM-dd hh:mm:ss.zzz] "

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
    void LoadSettings();
    void SaveSettings();

signals:
    void ShowReceived( QString s );
    void StartQueue();
    void PauseQueue();
    void AbortQueue();
    void VerboseChanged( bool verbose );

protected:
    RobotHandler * m_conveyor;
    RobotHandler * m_g4;
    DiskBurnSequence * m_burn;

    QDateTime m_startPhase2;
    QDateTime m_startPhase3;
    qint64 m_isoSize;

    QFile m_logFile;

public slots:
    void Log( QString s ); // Delegate to LogIfVerbose( 1, s )
    void LogIfVerbose( int level, QString s ); // if level >= 1, only display if verbose checked
    void LogToFile( QString s );
    void onQueueCompleted();
    void onQueueFailed( int reason );
    void onQueueCurrent(QString msg);
    void onQueueSuccessCount(QString msg);
    void onQueueFailureCount(QString msg);

private slots:
    void on_btnQuit_clicked();

    void on_btnAdvance_clicked();

    void on_btnEject_clicked();

    void on_btnLoad_clicked();

    void on_btnTest1_clicked();

    void on_btnTest2_clicked();

    void on_btnTest3_clicked();

    void on_btnISOBrowse_clicked();

    void on_btnR_clicked();

    void on_btnG_clicked();

    void on_lstHistory_doubleClicked(const QModelIndex &index);

    void on_lstQueue_itemClicked(QListWidgetItem *item);

    void on_btnAddToQueue_clicked();

    void on_btnStartQueue_clicked();

    void on_btnPauseQueue_clicked();

    void on_btnStopQueue_clicked();

    void on_btnDeleteFromQueue_clicked();

    void on_btnReject_clicked();

    void on_btnMoreCopies_clicked();

    void on_btnLessCopies_clicked();

    void on_lstQueue_currentRowChanged(int currentRow);

    void on_pushButton_clicked();

    void on_chkVerbose_toggled(bool checked);

private:
    Ui::MainWindow *ui;
};

#endif // MAINWINDOW_H
