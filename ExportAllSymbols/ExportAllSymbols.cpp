#include <QtCore/QCoreApplication>
#include <QtSql>

#include "ExportAllSymbols.h"
#include "ArcGISRuntime.h"
#include "SymbolDictionary.h"
#include <iostream>

using namespace std;

// #define APP6B_TEST
#ifdef APP6B_TEST
SymbolDictionaryType dictionaryType = SymbolDictionaryType::App6B;
QString dictionaryPath = "/ArcGISRuntime10.2/resources/symbols/app6b";
QString dictionaryFile = "app6b.dat";
#else
SymbolDictionaryType dictionaryType = SymbolDictionaryType::Mil2525C;
QString dictionaryPath = "/ArcGISRuntime10.2/resources/symbols/mil2525c";
QString dictionaryFile = "mil2525c.dat";
#endif


ExportAllSymbols::ExportAllSymbols()
{
     dictionary = SymbolDictionary(dictionaryType);
}

ExportAllSymbols::~ExportAllSymbols()
{

}

QString ExportAllSymbols::getValidFilename(QString name)
{
    QString validFilename = name;
    validFilename.replace('*','-');
    validFilename.replace(' ','_');

    return validFilename;
}

void ExportAllSymbols::ExportSingleSymbol(QString symbolNameOrId)
{
   if (symbolNameOrId.length() != 15)
   {
       cout << "--> Invalid SIDC: " << qPrintable(symbolNameOrId) << ", skipping export" << endl;
       return;
   }

   // QString symbolNameOrId = "SFGAUCIL--AA---"; // Agricultural Laboratory N"; // Howitzer H";

   int size = 256;

   QImage image = dictionary.symbolImage(symbolNameOrId, size, size);

   if (image.isNull() || (image.size().height() == 0))
   {
       cout << "--> Export Failed: " << qPrintable(symbolNameOrId) << endl;

   }

   QString fname = getValidFilename(symbolNameOrId + ".png");

   std::cout << "Exporting: " << qPrintable(fname) << std::endl;

   image.save(fname);
}

void ExportAllSymbols::ExportAll()
{
    // Install Dir: /home/jbc/arcgis/runtime_sdk/qt10.1.1
    // + ArcGISRuntime10.1.1/resources/symbols/app6b
    QString path = ArcGISRuntime::installDirectory();
    path.append(dictionaryPath);
    QDir dataDir(path); // using QDir to convert to correct file separator
    QString databaseName = dataDir.path() + QDir::separator() + dictionaryFile;

    // Requires either QtSDK installed or data provider deployed with app
    QSqlDatabase db = QSqlDatabase::addDatabase("QSQLITE");
    db.setDatabaseName(databaseName);
    bool opened = db.open();

    cout << "databaseName = " << qPrintable(databaseName) << ", Opened = " << opened << endl;

    QSqlQuery query(QString("SELECT Name,SymbolId FROM SymbolInfo"), db);
    QHash<QString, QString> sidc2Name;
    while (query.next())
    {
      QString name = query.value(0).toString();
      QString sidc = query.value(1).toString();

      if (sidc.length() > 10)
        sidc2Name.insert(sidc, name);
    }
    query.clear();
    db.close();

    QHash<QString, QString>::iterator i;
    for (i = sidc2Name.begin(); i != sidc2Name.end(); ++i)
    {
        QString name = i.value();
        QString sidc = i.key();
        cout << "Name=" << qPrintable(name) << ", SIDC=" << qPrintable(sidc) << endl;
        ExportSingleSymbol(sidc);
    }
}

