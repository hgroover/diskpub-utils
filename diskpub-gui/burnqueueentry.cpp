#include "burnqueueentry.h"

#include <QFileInfo>

BurnQueueEntry::BurnQueueEntry(QString iso, int listRow, int copies, QObject *parent) : QObject(parent)
{
    m_iso = iso;
    m_listRow = listRow;
    QFileInfo fi(iso);
    m_isoMB = fi.size() / (1024.0 * 1024);
    m_copiesDesired = copies;
    m_currentPass = 0;
}

bool BurnQueueEntry::CompleteCopy()
{
    if (m_currentPass >= m_copiesDesired)
    {
        return false;
    }
    m_currentPass++;
    // True if more copies to make
    return (m_currentPass < m_copiesDesired);
}
