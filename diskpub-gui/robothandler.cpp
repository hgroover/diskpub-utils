#include "robothandler.h"

RobotHandler::RobotHandler(QString serialPort, int bitRate, QObject *parent) : QObject(parent)
{
    m_serialPortName = serialPort;
    m_bitRate = bitRate;
    // Try to open
    m_serialPort.setBaudRate(bitRate == 9600 ? QSerialPort::Baud9600 : bitRate);
    m_serialPort.setPortName(serialPort);
    m_portOpen = m_serialPort.open(QIODevice::ReadWrite);
    connect( &m_serialPort, SIGNAL(readyRead()), this, SLOT(GetResponses()) );
    if (m_portOpen)
    {
        connect( &m_serialPort, SIGNAL(dataTerminalReadyChanged(bool)), this, SLOT(dataTerminalReadyChanged(bool)) );
        connect( &m_serialPort, SIGNAL(error(QSerialPort::SerialPortError)), this, SLOT(error(QSerialPort::SerialPortError)) );
        connect( &m_serialPort, SIGNAL(flowControlChanged(QSerialPort::FlowControl)), this, SLOT(flowControlChanged(QSerialPort::FlowControl)) );
        connect( &m_serialPort, SIGNAL(requestToSendChanged(bool)), this, SLOT(requestToSendChanged(bool)) );
    }
}

RobotHandler::~RobotHandler()
{
    if (m_portOpen)
    {
        m_serialPort.close();
        m_portOpen = false;
    }
}

bool RobotHandler::SendCmd(QString s)
{
    if (!m_portOpen) return false;
    GetResponses();
    emit ShowMsg(m_serialPortName + ": sending " + s);
    if (m_requiresAck)
    {
        m_serialPort.setRequestToSend(true);
    }
    int bytesWritten = m_serialPort.write(s.toLocal8Bit());
    m_serialPort.flush();
    if (m_requiresAck)
    {
        //m_serialPort.setRequestToSend(false);
        //m_serialPort.setDataTerminalReady(true);
    }
    if (bytesWritten != s.length()) emit ShowMsg("Error: send " + s + "failed");
    return bytesWritten == s.length();
}

QString RobotHandler::Read(int timeout)
{
    if (!m_portOpen) return QString();
    if (!m_serialPort.waitForReadyRead(timeout))
    {
        //emit ShowMsg(QString().sprintf("Read(%d) timeout elapsed", timeout));
        return QString();
    }
    return m_serialPort.readAll();
}

void RobotHandler::GetResponses()
{
    if (!m_portOpen) return;
    QString s;
    while (!m_serialPort.atEnd())
    {
        s = Read(250);
        if (s.isEmpty()) break;
        emit ShowMsg(m_serialPortName + ": " + s);
    }
}

void	RobotHandler::dataTerminalReadyChanged(bool set)
{
    //emit ShowMsg(m_serialPortName + " DTR changed");
}

void	RobotHandler::error(QSerialPort::SerialPortError error)
{
    switch (error)
    {
    case QSerialPort::UnsupportedOperationError:
    case QSerialPort::TimeoutError:
        return;
    }

    emit ShowMsg(m_serialPortName + QString().sprintf(" error %d: ", error)  + m_serialPort.errorString());
}

void	RobotHandler::flowControlChanged(QSerialPort::FlowControl flow)
{
    //emit ShowMsg(m_serialPortName + " flow changed");
}

void	RobotHandler::requestToSendChanged(bool set)
{
    //emit ShowMsg(m_serialPortName + " rts changed");
}
