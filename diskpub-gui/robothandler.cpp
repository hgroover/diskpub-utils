#include "robothandler.h"

RobotHandler::RobotHandler(QString serialPort, int bitRate, QObject *parent) : QObject(parent)
{
    m_serialPortName = serialPort;
    m_bitRate = bitRate;
    // Try to open
    m_serialPort.setBaudRate(bitRate);
    m_serialPort.setPortName(serialPort);
    m_portOpen = m_serialPort.open(QIODevice::ReadWrite);
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
    return m_serialPort.write(s.toLocal8Bit()) == s.length();
}
