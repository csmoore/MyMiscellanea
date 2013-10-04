// COPYRIGHT 1995-2013 ESRI
// TRADE SECRETS: ESRI PROPRIETARY AND CONFIDENTIAL
// Unpublished material - all rights reserved under the
// Copyright Laws of the United States and applicable international
// laws, treaties, and conventions.
//
// For additional information, contact:
// Environmental Systems Research Institute, Inc.
// Attn: Contracts and Legal Services Department
// 380 New York Street
// Redlands, California, 92373
// USA
//
// email: contracts@esri.com

#include "ExportAllSymbols.h"
#include <QApplication>
#include "ArcGISRuntime.h"

int main(int argc, char *argv[])
{
    QCoreApplication a(argc, argv);

    ExportAllSymbols exporter;

    QString symbolNameOrId = "SFGAUCIL--AA---";
    QStringList args = a.arguments();
    if (args.count() > 1)
    {
        // if input arg supplied, just export that one
        symbolNameOrId = args[1];
        exporter.ExportSingleSymbol(symbolNameOrId);
    }
    else
    {
        // otherwise export them all
        exporter.ExportAll();
    }
}

