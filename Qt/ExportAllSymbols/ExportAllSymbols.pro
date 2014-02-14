TARGET = ExportAllSymbols 
CONFIG += console

QT += core gui sql

greaterThan(QT_MAJOR_VERSION, 4) {
    QT += widgets
}

CONFIG += c++11 esri_runtime_qt_10_2 

win32:CONFIG += \
  embed_manifest_exe


SOURCES += \
	main.cpp \
  ExportAllSymbols.cpp

HEADERS += \
	ExportAllSymbols.h

