#-------------------------------------------------
#  ArcGIS Runtime SDK Application Template
#
#  COPYRIGHT 1995-2013 ESRI
#  TRADE SECRETS: ESRI PROPRIETARY AND CONFIDENTIAL
#  Unpublished material - all rights reserved under the
#  Copyright Laws of the United States and applicable international
#  laws, treaties, and conventions.
#
#  For additional information, contact:
#  Environmental Systems Research Institute, Inc.
#  Attn: Contracts and Legal Services Department
#  380 New York Street
#  Redlands, California, 92373
#  USA
#
#  email: contracts@esri.com
#
#-------------------------------------------------



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

