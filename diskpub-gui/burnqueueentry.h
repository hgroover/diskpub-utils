#ifndef BURNQUEUEENTRY_H
#define BURNQUEUEENTRY_H

#include <QObject>

class BurnQueueEntry : public QObject
{
    Q_OBJECT
public:
    explicit BurnQueueEntry(QString iso, int listRow, int copies = 1, QObject *parent = 0);

    QString Iso() const { return m_iso;}
    double IsoSizeMB() const { return m_isoMB;}
    int CopiesRequested() const { return m_copiesDesired;}
    int CurrentCopyIndex() const { return m_currentPass;}
    bool CompleteCopy();
    int ListRow() const { return m_listRow;}

protected:
    QString m_iso;
    double m_isoMB;
    int m_copiesDesired;
    int m_currentPass;
    int m_listRow;

signals:

public slots:
};

#endif // BURNQUEUEENTRY_H
