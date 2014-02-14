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

