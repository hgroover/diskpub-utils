#-------------------------------------------------
#
# Project created by QtCreator 2017-01-15T06:44:39
#
#-------------------------------------------------

QT       += core gui
QT       += serialport

greaterThan(QT_MAJOR_VERSION, 4): QT += widgets

TARGET = diskpub-gui
TEMPLATE = app


SOURCES += main.cpp\
        mainwindow.cpp \
    robothandler.cpp \
    diskburnsequence.cpp

HEADERS  += mainwindow.h \
    robothandler.h \
    diskburnsequence.h

FORMS    += mainwindow.ui
