/* 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at 
 *    http://www.apache.org/licenses/LICENSE-2.0
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library2525D
{
    class Program
    {
        static void Main(string[] args)
        {
            SymbolIdCode sidc = new SymbolIdCode();
            System.Diagnostics.Trace.WriteLine("SIDC=" + sidc);


            SymbolLookup symbolLookup = new SymbolLookup();
            symbolLookup.Initialize();

            if (!symbolLookup.Initialized)
                System.Diagnostics.Trace.WriteLine("Fail");

            MilitarySymbol ms = symbolLookup.CreateSymbolByEntityName("Fighter/Bomber");

            MilitarySymbolToGraphicLayersMaker.SetMilitarySymbolGraphicLayers(ref ms);

            System.Diagnostics.Trace.WriteLine("MilitarySymbol: " + ms);

            List<MilitarySymbol> matchingSymbols = 
                symbolLookup.GetMilitarySymbols(SymbolSetType.Space);

            int matchCount = 0;
            foreach (MilitarySymbol matchSymbol in matchingSymbols)
            {
                matchCount++;
                System.Diagnostics.Trace.WriteLine("Match: " + matchCount
                    + ", MilitarySymbol: " + matchSymbol); ;
            }

            List<MilitarySymbol> matchingSymbols2 =
                symbolLookup.GetMilitarySymbols(SymbolSetType.Space, StandardIdentityAffiliationType.Friend, 
                "Military");

            matchCount = 0;
            foreach (MilitarySymbol matchSymbol in matchingSymbols2)
            {
                matchCount++;
                System.Diagnostics.Trace.WriteLine("Match: " + matchCount
                    + ", MilitarySymbol: " + matchSymbol); ;
            }

            List<string> matchingStrings = symbolLookup.GetDistinctEntries(SymbolSetType.Space);

            matchCount = 0;
            foreach (string distinctMatch in matchingStrings)
            {
                matchCount++;
                System.Diagnostics.Trace.WriteLine("Distinct Match: " + distinctMatch);
            }

            matchingStrings = symbolLookup.GetDistinctEntries(SymbolSetType.Air, "Military",
                "Fixed Wing");

            matchCount = 0;
            foreach (string distinctMatch in matchingStrings)
            {
                matchCount++;
                System.Diagnostics.Trace.WriteLine("Distinct Match: " + distinctMatch);
            }
        }
    }
}
