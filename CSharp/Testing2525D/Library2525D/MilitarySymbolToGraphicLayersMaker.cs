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
    /// <summary>
    /// General processing is to convert a Symbol Id Code into a list of strings
    /// These strings represent the paths to the set of images/layers that *should*
    /// represent that Symbol Id
    /// For convenience, it take a MilitarySymbol object and sets the GraphicLayers to these strings
    /// </summary>
    public class MilitarySymbolToGraphicLayersMaker
    {
        // IMPORTANT: If you don't have the expected SVG Files, in the expected folder format
        //            then this class will not do anything.
        // The incomplete SVG snapshot: "2525D_SVG_PNG_062013" was used.
        // The assumed/expected Folder structure:
        // {ImageFilesHome} <--- SEE DEFINITION BELOW
        //  |- Echelon
        //  |- Frames
        //  |- Headquarters
        //  |- Appendices
        //     |- Air
        //     |- Control Measures
        //     |- Cyberspace
        //     |- Land
        // (etc.)

        // IMPORTANT: defaults to {exe folder}\Data\2525D_SVG_Images
        private static readonly string DEFAULT_PATH = 
            System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\2525D_SVG_Images");

        // TODO/IMPORTANT: 
        // You must uncomment & set this to the location on your machine if you don't want to use the default
        private static readonly string ALTERNATE_PATH = @"[!!!!!!!!!!!SET_THIS_FOLDER_!!!!!!!!!!!]";

        public static readonly string ImageFilesHome =
            // ALTERNATE_PATH // <-- TODO: SET THIS to ALTERNATE_PATH if you don't want to use default
            DEFAULT_PATH      // (and comment out this) 
            + System.IO.Path.DirectorySeparatorChar; // NOTE: Ends in DirectorySeparator

        const string ImageSuffix = ".svg";

        public static string GetIconFolderName(SymbolSetType symbolSet)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("Appendices");
            sb.Append(System.IO.Path.DirectorySeparatorChar);

            string symbolSetSubFolderName = string.Empty;
            if (TypeUtilities.SymbolSetToFolderName.ContainsKey(symbolSet))
                symbolSetSubFolderName = TypeUtilities.SymbolSetToFolderName[symbolSet];

            sb.Append(symbolSetSubFolderName);
            sb.Append(System.IO.Path.DirectorySeparatorChar);

            return sb.ToString();
        }

        public static string GetMainIconName(ref MilitarySymbol milSymbol)
        {
            StringBuilder sb = new StringBuilder();

            string symbolSetString = TypeUtilities.EnumHelper.getEnumValAsString(milSymbol.Id.SymbolSet, 2);
            sb.Append(symbolSetString);
            sb.Append(milSymbol.Id.FullEntityCode);

            return sb.ToString();
        }

        public static string GetMainIconNameWithFolder(ref MilitarySymbol milSymbol)
        {
            StringBuilder sb = new StringBuilder();

            string currentAppendixHome = GetIconFolderName(milSymbol.Id.SymbolSet);
            sb.Append(currentAppendixHome);

            string mainIconName = GetMainIconName(ref milSymbol);

            sb.Append(mainIconName);
            sb.Append(ImageSuffix);

            return sb.ToString();
        }

        public static string GetModfierIconName(SymbolSetType symbolSet, int modifierNumber, int modifierCodeInt)
        {
            StringBuilder sb = new StringBuilder();

            if (!((modifierNumber == 1) || (modifierNumber == 2)))
                return string.Empty;

            string sModifierNumber = modifierNumber.ToString();

            string modifierCode = modifierCodeInt.ToString();
            // this one has to be 2 chars:
            if (modifierCode.Length < 2)
                modifierCode = modifierCode.PadLeft(2, '0');

            string symbolSetString = TypeUtilities.EnumHelper.getEnumValAsString(symbolSet, 2);

            string modifierIcon = symbolSetString + modifierCode + sModifierNumber;
            sb.Append(modifierIcon);

            return sb.ToString();
        }

        public static string GetModfierIconName(ref MilitarySymbol milSymbol, int modifierNumber)
        {
            StringBuilder sb = new StringBuilder();

            if (!((modifierNumber == 1) || (modifierNumber == 2)))
                return string.Empty;

            string sModifierNumber = "1";
            string sModifier = milSymbol.Id.FirstModifier;

            if (modifierNumber == 2)
            {
                sModifierNumber = "2";
                sModifier = milSymbol.Id.SecondModifier;
            }

            string symbolSetString = TypeUtilities.EnumHelper.getEnumValAsString(milSymbol.Id.SymbolSet, 2);

            string modifierIcon = symbolSetString + sModifier + sModifierNumber;
            sb.Append(modifierIcon);

            return sb.ToString();
        }

        public static string GetModfierIconNameWithFolder(SymbolSetType symbolSet, int modifierNumber, int modifierCodeInt)
        {
            StringBuilder sb = new StringBuilder();

            if (!((modifierNumber == 1) || (modifierNumber == 2)))
                return string.Empty;

            string sSubFolderName = "mod" + modifierNumber.ToString();

            string currentAppendixHome = GetIconFolderName(symbolSet);
            sb.Append(currentAppendixHome);

            sb.Append(sSubFolderName);
            sb.Append(System.IO.Path.DirectorySeparatorChar);

            string modifierIcon = GetModfierIconName(symbolSet, modifierNumber, modifierCodeInt);
            sb.Append(modifierIcon);

            sb.Append(ImageSuffix);

            return sb.ToString();
        }

        public static string GetModfierIconNameWithFolder(ref MilitarySymbol milSymbol, int modifierNumber)
        {
            if (!((modifierNumber == 1) || (modifierNumber == 2)))
                return string.Empty;

            string sModifierCode = (modifierNumber == 1) ? milSymbol.Id.FirstModifier : milSymbol.Id.SecondModifier;

            int modifierCodeInt = Convert.ToInt32(sModifierCode);

            string modifierIconNameWithFolder = GetModfierIconNameWithFolder(
                milSymbol.Id.SymbolSet, modifierNumber, modifierCodeInt);

            return modifierIconNameWithFolder;
        }

        public static bool SetMilitarySymbolGraphicLayers(ref MilitarySymbol milSymbol)
        {
            if (!System.IO.Directory.Exists(ImageFilesHome))
            {
                System.Diagnostics.Trace.WriteLine("--> Images Home *DOES NOT EXIST* : " + ImageFilesHome);
                return false;
            }

            if ((milSymbol == null) || (milSymbol.Id == null) ||
                (milSymbol.GraphicLayers == null) || (!milSymbol.Id.IsValid))
                return false;

            milSymbol.GraphicLayers.Clear();

            // Frame Layer 
            // = StandardIdentityAffiliationType + SymbolSetType + "(affiliation)"
            // ex. 0520(hostile)
            StringBuilder sb = new StringBuilder();

            sb.Append(ImageFilesHome);
            sb.Append("Frames");
            sb.Append(System.IO.Path.DirectorySeparatorChar);

            string affiliationString = TypeUtilities.EnumHelper.getEnumValAsString(milSymbol.Id.Affiliation, 2);
            sb.Append(affiliationString);

            string symbolSetString = TypeUtilities.EnumHelper.getEnumValAsString(milSymbol.Id.SymbolSet, 2);
            sb.Append(symbolSetString);

            //no longer there as of 3/14 - (but exercise/sim is)
            //string affilName = TypeUtilities.AffiliationTypeToImageName[milSymbol.Id.Affiliation];
            //sb.Append("(" + affilName + ")");

            sb.Append(ImageSuffix);

            milSymbol.GraphicLayers.Add(sb.ToString());

            // Main Icon Layer
            // Appendices\{SymbolSetTypeName}\SymbolSetType + EntityCode 

            sb.Clear();
            sb.Append(ImageFilesHome);

            string mainIconNameWithFolder = GetMainIconNameWithFolder(ref milSymbol);
            sb.Append(mainIconNameWithFolder);
            milSymbol.GraphicLayers.Add(sb.ToString());

            // 
            // TODO: Stop here for Control Measures (Lines/Areas for now) & 
            //       Figure out which of the additional layers apply for which sets
            //
            if ((milSymbol.Shape == ShapeType.Line) || (milSymbol.Shape == ShapeType.Area))
                return true;

            // Center/Main Icon Modifiers: { # = 1 | 2 }
            // Appendices\{SymbolSetTypeName}\Mod{#}\{SymbolSetType} + {ModifierCode} + {#}

            // Main Icon Modfier 1
            if (!string.IsNullOrEmpty(milSymbol.Id.FirstModifier))
            {
                sb.Clear();
                sb.Append(ImageFilesHome);

                string modifierIconNameWithFolder = 
                    MilitarySymbolToGraphicLayersMaker.GetModfierIconNameWithFolder(
                        ref milSymbol, 1);

                sb.Append(modifierIconNameWithFolder);
                milSymbol.GraphicLayers.Add(sb.ToString());
            }

            // Main Icon Modfier 2
            if (!string.IsNullOrEmpty(milSymbol.Id.SecondModifier))
            {
                sb.Clear();
                sb.Append(ImageFilesHome);

                string modifierIconNameWithFolder =
                    MilitarySymbolToGraphicLayersMaker.GetModfierIconNameWithFolder(
                        ref milSymbol, 2);

                sb.Append(modifierIconNameWithFolder);
                milSymbol.GraphicLayers.Add(sb.ToString());
            }

            // Echelon Modifier

            // = StandardIdentityAffiliationType + "100" (not sure what this is) + EchelonMobilityType
            // ex. Friend Team_Crew = 0310011

            sb.Clear();
            sb.Append(ImageFilesHome);
            sb.Append("Echelon");
            sb.Append(System.IO.Path.DirectorySeparatorChar);
            sb.Append(affiliationString);
            sb.Append("100");
            sb.Append(TypeUtilities.EnumHelper.getEnumValAsString(milSymbol.Id.EchelonMobility, 2));
            sb.Append(ImageSuffix);
            milSymbol.GraphicLayers.Add(sb.ToString());

            // Headquarters/TF/FD Modifier

            // TODO

            // Other?

            if (milSymbol.GraphicLayers.Count == 0)
                return false;
            else
                return true;

        }

    }
}
