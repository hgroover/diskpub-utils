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

    void SetAck( bool required ) { m_requiresAck = required; }

protected:
    QString m_serialPortName;
    int m_bitRate;
    bool m_portOpen;
    bool m_requiresAck; // Must have response read before next command

    QSerialPort m_serialPort;

signals:
    void ShowMsg( QString s );

protected slots:
    void	dataTerminalReadyChanged(bool set);
    void	error(QSerialPort::SerialPortError error);
    void	flowControlChanged(QSerialPort::FlowControl flow);
    void	requestToSendChanged(bool set);

public slots:
    bool SendCmd(QString s);
    QString Read(int timeout);
    void GetResponses();
};

#endif // ROBOTHANDLER_H
