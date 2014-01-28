/*
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *    http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
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

