#ifndef EXPORTALLSYMBOLS_H
#define EXPORTALLSYMBOLS_H

#include "SymbolDictionary.h"

using namespace EsriRuntimeQt;

class ExportAllSymbols
{
public:
    ExportAllSymbols ();
    ~ExportAllSymbols ();
    
    void ExportAll();
    void ExportSingleSymbol(QString symbolNameOrId);

private:
    QString getValidFilename(QString name);

private:

    SymbolDictionary dictionary;

};

#endif // EXPORTALLSYMBOLS_H

