#ifndef ROBOTHANDLER_H
#define ROBOTHANDLER_H

#include <QObject>
#include <QtSerialPort>

class RobotHandler : public QObject
{
    Q_OBJECT
public:
    explicit RobotHandler(QString serialPort, int bitRate = 9600, QObject *parent = 0);
    ~RobotHandler();

protected:
    QString m_serialPortName;
    int m_bitRate;
    bool m_portOpen;

    QSerialPort m_serialPort;

signals:
    void ShowMsg( QString s );

public slots:
    bool SendCmd(QString s);
};

#endif // ROBOTHANDLER_H
