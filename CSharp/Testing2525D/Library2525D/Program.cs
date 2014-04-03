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
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Library2525D
{
    class Program
    {

        /// <summary>
        /// This command line app, just returns types and values that it knows about
        /// first using reflection (if it is a type) & then by doing a look-up of the SymbolSet 
        /// if its not a type
        /// </summary>
        static void Main(string[] args)
        {

            // if command line arguments, run as Consolse App
            Console.WriteLine("Usage: ConsoleApp2525D.exe [Type -or- \"List\" -or- \"ALL\"]");
            Console.WriteLine("       ConsoleApp2525D.exe [SymbolSet (Name or Code)]");

            string typeToExportUpper = "LIST";

            int argLength = args.Length;
            if (argLength == 0)
            {
                // TODO: if you just want to wire in a quick test uncomment:
                // MyAdHocTest();
                // return;
            }
            else
            {
                typeToExportUpper = args[0].ToUpper();
            }

            Console.WriteLine("Looking for Type: " + typeToExportUpper);

            // HACK:
            MilitarySymbol.FormatTagsForStyleFiles = true;

            bool found = false;

            Assembly myAssembly = Assembly.GetExecutingAssembly();
            foreach (Type type in myAssembly.GetTypes())
            {
                if (!type.IsEnum)
                    continue;

                string typeName = type.Name;
                string typeNameUpper = typeName.ToUpper();

                if (typeToExportUpper == "LIST")
                {
                    Console.WriteLine("Type: " + typeName);
                    found = true;
                }

                if (typeToExportUpper.Equals("ALL") || typeNameUpper.Contains(typeToExportUpper))
                {
                    Console.WriteLine(Environment.NewLine + "Found Type: " + typeNameUpper);

                    found = true;
                    List<Enum> enums = TypeUtilities.EnumHelper.getEnumValues(type);

                    foreach (Enum en in enums)
                    {
                        string enumString = en.ToString();
                        int hashCode = en.GetHashCode();

                        Console.WriteLine(enumString + "," + hashCode);
                    }
                } // end if              
            } // end foreach

            // if found, we are done, if not check for a symbol set query 
            if (found && (!typeToExportUpper.Equals("ALL")))
                return;

            string symbolSetUpper = typeToExportUpper;

            Console.WriteLine("Type not found, looking for SymbolSet: " + symbolSetUpper);

            SymbolLookup symbolLookup = new SymbolLookup();
            symbolLookup.Initialize();

            if (!symbolLookup.Initialized) // should not happen, but you never know
            {
                System.Diagnostics.Trace.WriteLine("Failed to initialize symbol list, exiting...");
                return;
            }

            SymbolSetType symbolSet = SymbolSetType.NotSet;

            // find the symbol set selected
            List<Enum> symbolSetEnums = TypeUtilities.EnumHelper.getEnumValues(typeof(SymbolSetType));
            foreach (Enum en in symbolSetEnums)
            {
                string enumStringUpper = en.ToString().ToUpper();

                int hashCode = en.GetHashCode();
                string hashCodeString = Convert.ToString(hashCode);

                if (hashCodeString.Length < 2)
                    hashCodeString = hashCodeString.PadLeft(2, '0');

                // Mildly confusing but allow either the name or the Number
                if (symbolSetUpper.Equals("ALL") || enumStringUpper.Contains(symbolSetUpper) 
                    || hashCodeString.Equals(symbolSetUpper))
                {
                    symbolSet = (SymbolSetType)en;
                    ListSymbolSet(symbolLookup, symbolSet);                  
                }
            }

            if (symbolSet == SymbolSetType.NotSet)
                Console.WriteLine("Warning: SymbolSet not found: " + symbolSetUpper);

        }

        private static void PrintModifier(SymbolSetType symbolSet, int modifierNumber, int modifierCode, string modifierName)
        {
            string symbolSetString = TypeUtilities.EnumHelper.getEnumValAsString(symbolSet, 2);

            string sModifierCode = modifierCode.ToString();
            // this one has to be 2 chars:
            if (sModifierCode.Length < 2)
                sModifierCode = sModifierCode.PadLeft(2, '0');

            string fullModifierName = symbolSet.ToString() + TypeUtilities.NameSeparator +
                "Modifier " + modifierNumber.ToString() + TypeUtilities.NameSeparator +
                modifierName;

            string modifierIconName = MilitarySymbolToGraphicLayersMaker.GetModfierIconName(
                symbolSet, modifierNumber, modifierCode);

            string modifierIconNameWithFolder = MilitarySymbolToGraphicLayersMaker.GetModfierIconNameWithFolder(
                symbolSet, modifierNumber, modifierCode);

            string nameAsTags = fullModifierName.Replace(TypeUtilities.NameSeparator, ";");

            string tags = nameAsTags + ";" + modifierIconNameWithFolder + ";" +
                          fullModifierName + ";" + modifierIconName;

            Console.WriteLine(modifierCode + "," + symbolSet + "," + symbolSetString 
                + "," + modifierNumber.ToString() + "," + sModifierCode
                + "," + modifierIconNameWithFolder
                + "," + fullModifierName
                + "," + modifierIconName 
                + "," + tags);
        }

        private static void ListSymbolSet(SymbolLookup symbolLookup, SymbolSetType symbolSet)
        {
            string symbolSetString = TypeUtilities.EnumHelper.getEnumValAsString(symbolSet, 2);

            Console.WriteLine(Environment.NewLine + "SymbolSet: " + symbolSet + " : " + symbolSetString);
            
            Console.WriteLine("Entities:");

            List<MilitarySymbol> matchingSymbols = symbolLookup.GetMilitarySymbols(symbolSet);

            SymbolIdCode.FormatCodeStringWithCommas = false;

            int matchCount = 0;
            foreach (MilitarySymbol matchSymbol in matchingSymbols)
            {
                matchCount++;

                // Copying so making ref below useless, but had to do this because of use of iterator/ref
                MilitarySymbol currentMilSymbol = matchSymbol; 
                string svgTag = MilitarySymbolToGraphicLayersMaker.GetMainIconNameWithFolder(ref currentMilSymbol);
                string idTag = MilitarySymbolToGraphicLayersMaker.GetMainIconName(ref currentMilSymbol);

                string nameAsTags = matchSymbol.Id.Name.Replace(TypeUtilities.NameSeparator, ";");

                Console.WriteLine(matchCount + "," + symbolSet + "," + symbolSetString
                    + "," + matchSymbol.Id.Name + ","
//                    + matchSymbol.Id.CodeFirstTen + "," + matchSymbol.Id.CodeSecondTen + ","
                    + nameAsTags + ";"
                    + matchSymbol.TagsAsString + svgTag + ";"
                    + matchSymbol.Id.Name + ";"
                    + idTag

                    + "," + svgTag);
            }

            List<string> matchingModifiers = symbolLookup.GetDistinctModifierNames(symbolSet, 1);

            if (matchingModifiers.Count > 0)
            {
                Console.WriteLine(Environment.NewLine + "Modifier 1:");

                matchCount = 0;
                foreach (string match in matchingModifiers)
                {
                    PrintModifier(symbolSet, 1, matchCount, match);
                    matchCount++;
                }
            }

            matchingModifiers = symbolLookup.GetDistinctModifierNames(symbolSet, 2);
            if (matchingModifiers.Count > 0)
            {
                Console.WriteLine(Environment.NewLine + "Modifier 2:");

                matchCount = 0;
                foreach (string match in matchingModifiers)
                {
                    PrintModifier(symbolSet, 2, matchCount, match);

                    matchCount++;
                }
            }

        }

        private static void MyAdHocTest()
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

            matchingStrings = symbolLookup.GetDistinctModifierNames(SymbolSetType.Air, 1);

            matchCount = 0;
            foreach (string distinctMatch in matchingStrings)
            {
                matchCount++;
                System.Diagnostics.Trace.WriteLine("Modifiers: Distinct Match: " + distinctMatch);
            }

            string modifierName = "Government";

            string modifierCode = symbolLookup.GetModifierCodeFromName(SymbolSetType.Air, modifierName);

        }
    }
}
