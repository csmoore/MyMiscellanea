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
    /// (NOTE: a TODO should be to remove this MilitarySymbol dependency, this was just done
    ///  for rapid prototyping, isn't needed, & will prevent this class from being easily reused - 
    ///  although there are so many other dependencies on the classes in this assembly,
    ///  one more probably won't hurt)
    /// </summary>
    public class MilitarySymbolToGraphicLayersMaker
    {
        // IMPORTANT: If you don't have the expected SVG Files, in the expected folder format
        //            then this class will not do anything.
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

        // IMPORTANT: defaults to {exe folder}\Data\2525D_SVG_Images (mainly for deployment)
        private static readonly string DEFAULT_PATH = 
            System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\2525D_SVG_Images");

        // TODO/IMPORTANT: 
        // If you don't want to use the default: 
        // 1) You must uncomment & set this to the location on your machine &
        // 2) Change imageFilesHome default/TODO below
        private static readonly string ALTERNATE_PATH = 
                        @"[!!!!!!!!!!!SET_THIS_FOLDER_IF_DESIRED!!!!!!!!!!!]";

        // Allow this property to be set externally at runtime (if desired/needed)
        public static string ImageFilesHome
        {
            get { return imageFilesHome; }
            set
            {
                string checkForDirectorySeparator = value;

                // but make sure it ends in a DirectorySeparatorChar
                checkForDirectorySeparator = 
                    checkForDirectorySeparator.TrimEnd(System.IO.Path.DirectorySeparatorChar) + 
                    System.IO.Path.DirectorySeparatorChar;

                imageFilesHome = checkForDirectorySeparator;
            }
        }
        private static string imageFilesHome =
            // ALTERNATE_PATH // <-- TODO: SET THIS to ALTERNATE_PATH if you don't want to use default
            DEFAULT_PATH      // (and comment out this) 
            + System.IO.Path.DirectorySeparatorChar; // IMPORTANT/NOTE: Ends in DirectorySeparator

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

        public static string GetMainIconName(SymbolSetType symbolSet, string fullEntityCode)
        {
            if (fullEntityCode.Length != 6)
            {
                return "00000000";
            }

            StringBuilder sb = new StringBuilder();

            string symbolSetString = TypeUtilities.EnumHelper.getEnumValAsString(symbolSet, 2);
            sb.Append(symbolSetString);
            sb.Append(fullEntityCode);

            return sb.ToString();
        }

        // returns string.Empty to indicate that hack isn't needed
        private static string CheckForSpecialEntitySubtypeHackIcon(ref MilitarySymbol milSymbol)
        {
            string newIconName = string.Empty;

            // HACK (and a big one)
            // 
            // TODO: see if there is any other way
            // To support special case: "TABLE D-V.  Land unit icons – special entity subtypes"
            // These are the Land unit–special entity subtypes defined:
            // HEADQUARTERS ELEMENT - 10xxxx95
            // DIVISION AND BELOW SUPPORT - 10xxxx96 (NOTE: None of these exist currently)
            // CORPS SUPPORT - 10xxxx97
            // THEATER/ECHELONS ABOVE CORPS SUPPORT -  - 10xxxx98
            if (!string.IsNullOrEmpty(milSymbol.Id.Name))
            {
                if (milSymbol.Id.Name.Contains(": Headquarters Element") ||
                    milSymbol.Id.Name.Contains(" Support"))
                {
                    // then we *might* need this case
                    string subEntityCode = milSymbol.Id.FullEntityCode.Substring(0, 4) + "00";

                    if (milSymbol.Id.Name.Contains(": Headquarters Element"))
                    {
                        newIconName = GetMainIconName(milSymbol.Id.SymbolSet, subEntityCode);
                        string hackSpecialEntityLayer = ImageFilesHome + @"Appendices\Land\10xxxx95.svg";
                        milSymbol.GraphicLayers.Add(hackSpecialEntityLayer);
                    }
                    // NOTE: There are no Division Support Ones
                    else
                        if (milSymbol.Id.Name.Contains(": Corps Support"))
                        {
                            newIconName = GetMainIconName(milSymbol.Id.SymbolSet, subEntityCode);
                            string hackSpecialEntityLayer = ImageFilesHome + @"Appendices\Land\10xxxx97.svg";
                            milSymbol.GraphicLayers.Add(hackSpecialEntityLayer);
                        }
                        else
                            if (milSymbol.Id.Name.Contains(" Above Corps Support"))
                            {
                                newIconName = GetMainIconName(milSymbol.Id.SymbolSet, subEntityCode);
                                string hackSpecialEntityLayer = ImageFilesHome + @"Appendices\Land\10xxxx98.svg";
                                milSymbol.GraphicLayers.Add(hackSpecialEntityLayer);
                            }
                }
            }

            return newIconName;
        }

        public static string GetMainIconName(ref MilitarySymbol milSymbol)
        {
            string mainIcon = GetMainIconName(milSymbol.Id.SymbolSet, milSymbol.Id.FullEntityCode);

            if (milSymbol.Id.SymbolSet == SymbolSetType.Land_Unit)
            {
                // Hack only needed for Land Unit
                string hackIcon = CheckForSpecialEntitySubtypeHackIcon(ref milSymbol);

                if (!string.IsNullOrEmpty(hackIcon))
                {
                    mainIcon = hackIcon;
                }
            }

            return mainIcon;
        }

        // MainIconNameWithFolder
        // Appendices\{SymbolSetTypeName}\SymbolSetType + EntityCode 
        // e.g. Appendices\Air\01110000.svg ==> { 01 } + { 110000 }
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

        // same as ImageFilesHome + GetMainIconNameWithFolder
        public static string GetMainIconNameWithFullPath(ref MilitarySymbol milSymbol)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(ImageFilesHome);

            string mainIconNameWithoutImageFilesHome = GetMainIconNameWithFolder(ref milSymbol);
            sb.Append(mainIconNameWithoutImageFilesHome);

            string mainIconNameFullPath = sb.ToString();

            // WORKAROUND/TRICKY: some symbols have wacky _0, _1, _2, _3 thing instead of base version
            // Method: Find the frame from the affiliation & add _0, _1, _2, _3 accordingly
            if (!System.IO.File.Exists(mainIconNameFullPath))
            {
                string newFrameSuffix =
                    TypeUtilities.AffiliationFrameToSuffixName[milSymbol.Id.Affiliation] + ImageSuffix;

                string subMainIconName = mainIconNameFullPath;
                subMainIconName = subMainIconName.Replace(ImageSuffix, newFrameSuffix);
                if (System.IO.File.Exists(subMainIconName)) // if the other file/version exists, use that one
                    mainIconNameFullPath = subMainIconName;
            }

            return mainIconNameFullPath;
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

        // same as ImageFilesHome + GetModfierIconNameWithFolder
        public static string GetModfierIconNameWithFullPath(SymbolSetType symbolSet, int modifierNumber, int modifierCodeInt)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(ImageFilesHome);

            string modifierIconNameWithoutImageFilesHome = GetModfierIconNameWithFolder(
                symbolSet, modifierNumber, modifierCodeInt);

            sb.Append(modifierIconNameWithoutImageFilesHome);

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

        // Echelon/Mobility Modifier Icon
        // = StandardIdentityAffiliationType + SymbolSet + 
        //        HeadquartersTaskForceDummyType + EchelonMobilityType
        // "Amplifier (Echelon): Uses SIDC pos 3-6 and 8-10"
        // ex. Friend, Team Crew = 0310011 
        //      --> 03 (Friend) + 10 (Ground Unit) + 0 (HQ/TF) + 11 (Echelon = Team/Crew)
        //
        public static string GetEchelonIconNameWithFolder(
            StandardIdentityAffiliationType affiliation,
            SymbolSetType symbolSet,
            HeadquartersTaskForceDummyType hqTfDummy,
            EchelonMobilityType echelonMobility)
        {
            if (echelonMobility == EchelonMobilityType.NoEchelonMobility)
                return string.Empty;

            string affiliationString = "03";
            // TODO: (currently only Friend supported/available), activate below when available
            //     TypeUtilities.EnumHelper.getEnumValAsString(affiliation, 2);

            StringBuilder sb = new StringBuilder();
            sb.Append("Echelon");
            sb.Append(System.IO.Path.DirectorySeparatorChar);
            sb.Append(affiliationString);

            // TODO: symbolSet + hqTfDummy (currently only Ground, no HqTf available)
            // we will need to set this later when the svgs are fully populated 
            sb.Append("100"); // <--- symbolSet + hqTfDummy (not currently used - only 1 version of these svgs) 

            sb.Append(TypeUtilities.EnumHelper.getEnumValAsString(echelonMobility, 2));
            sb.Append(ImageSuffix);

            return sb.ToString();
        }

        // same as ImageFilesHome + GetModfierIconNameWithFolder
        public static string GetEchelonIconNameWithFullPath(
            StandardIdentityAffiliationType affiliation,
            SymbolSetType symbolSet,
            HeadquartersTaskForceDummyType hqTfDummy,
            EchelonMobilityType echelonMobility)
        {
            if (echelonMobility == EchelonMobilityType.NoEchelonMobility)
                return string.Empty;

            StringBuilder sb = new StringBuilder();
            sb.Append(ImageFilesHome);

            string modifierIconNameWithoutImageFilesHome =
                GetEchelonIconNameWithFolder(affiliation, symbolSet, hqTfDummy, echelonMobility);

            sb.Append(modifierIconNameWithoutImageFilesHome);

            return sb.ToString();
        }

        // Frame Icon
        // StandardIdentityAffiliationType + SymbolSetType
        // IMPORTANT: StandardIdentityAffiliationType must be in 
        // { Unknown, Space, Air,Land_Unit, Land_Installation, 
        //   Sea_Surface, Subsurface, Activity } 
        // See: TypeUtilities.SymbolSetToFrameType for mapping
        // ex. 0520
        public static string GetFrameIconNameWithFolder(
                        StandardIdentityRealExerciseSimType realExerciseSim,
                        StandardIdentityAffiliationType affiliation,
                        SymbolSetType symbolSet)
        {
            StringBuilder sb = new StringBuilder();

            // TODO: exercise/sim frames - 
            // StandardIdentityRealExerciseSimType (just add "Sim" "Exercise")

            sb.Append("Frames");
            sb.Append(System.IO.Path.DirectorySeparatorChar);

            string affiliationString = TypeUtilities.EnumHelper.getEnumValAsString(affiliation, 2);
            sb.Append(affiliationString);

            // map the actual symbolSet to the supported/availble frame
            SymbolSetType mappedSymbolSet = TypeUtilities.SymbolSetToFrameMapping[symbolSet];

            string mappedSymbolSetString = TypeUtilities.EnumHelper.getEnumValAsString(mappedSymbolSet, 2);
            sb.Append(mappedSymbolSetString);

            sb.Append(ImageSuffix);

            return sb.ToString();
        }

        public static string GetFrameIconNameWithFullPath(
                        StandardIdentityRealExerciseSimType realExerciseSim,
                        StandardIdentityAffiliationType affiliation,
                        SymbolSetType symbolSet)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(ImageFilesHome);

            string frameIconNameWithoutImageFilesHome =
                GetFrameIconNameWithFolder(realExerciseSim, affiliation, symbolSet);

            sb.Append(frameIconNameWithoutImageFilesHome);

            return sb.ToString();
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

            //////////////////////////////////////////////////////////////////////////
            // Assembly the layers

            // Start with the Frame
            if (TypeUtilities.HasFrame(milSymbol.Id.SymbolSet))
            {
                //sb.Clear();
                //sb.Append(ImageFilesHome);

                string frameIconNameWithFullPath =
                    GetFrameIconNameWithFullPath(
                        milSymbol.Id.StandardIdentity,
                        milSymbol.Id.Affiliation,
                        milSymbol.Id.SymbolSet);

                milSymbol.GraphicLayers.Add(frameIconNameWithFullPath);            
            }
            //////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////
            // Main Icon Layer
            string mainIconNameFullPath = GetMainIconNameWithFullPath(ref milSymbol);

            milSymbol.GraphicLayers.Add(mainIconNameFullPath);
            //////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////
            // Skip the remaining if no more layers needed
            //
            // TODO: Verify this logic
            //       Stop here for Control Measures (Lines/Areas for now) & 
            //       Symbols *without* frames
            //
            bool skipRemainingLayers = false;
            if ((milSymbol.Shape == ShapeType.Line) || (milSymbol.Shape == ShapeType.Area) || 
                (!TypeUtilities.HasFrame(milSymbol.Id.SymbolSet)))
                skipRemainingLayers = true;

            if (!skipRemainingLayers)
            {
                StringBuilder sb = new StringBuilder();

                // Center/Main Icon Modifiers: { # = 1 | 2 }
                // Appendices\{SymbolSetTypeName}\Mod{#}\{SymbolSetType} + {ModifierCode} + {#}

                // Main Icon Modfier 1
                if (!string.IsNullOrEmpty(milSymbol.Id.FirstModifier)
                    && (milSymbol.Id.FirstModifier != "00")) // TODO: find better way of checking that this isn't set/valid
                {
                    sb.Clear();
                    sb.Append(ImageFilesHome);

                    string modifierIconNameWithFolder = 
                        GetModfierIconNameWithFolder(ref milSymbol, 1);

                    sb.Append(modifierIconNameWithFolder);
                    milSymbol.GraphicLayers.Add(sb.ToString());
                }

                // Main Icon Modfier 2
                if (!string.IsNullOrEmpty(milSymbol.Id.SecondModifier)
                    && (milSymbol.Id.SecondModifier != "00")) // TODO: find better way of checking that this isn't set/valid
                {
                    sb.Clear();
                    sb.Append(ImageFilesHome);

                    string modifierIconNameWithFolder = 
                        GetModfierIconNameWithFolder(ref milSymbol, 2);

                    sb.Append(modifierIconNameWithFolder);
                    milSymbol.GraphicLayers.Add(sb.ToString());
                }

                // Echelon Modifier
                if (milSymbol.Id.EchelonMobility != EchelonMobilityType.NoEchelonMobility)
                {
                    sb.Clear();
                    sb.Append(ImageFilesHome);

                    string echelonIconNameWithFullPath =
                        GetEchelonIconNameWithFullPath(
                            milSymbol.Id.Affiliation,
                            milSymbol.Id.SymbolSet,
                            milSymbol.Id.HeadquartersTaskForceDummy,
                            milSymbol.Id.EchelonMobility);

                    if (echelonIconNameWithFullPath.Length > 0)
                        milSymbol.GraphicLayers.Add(echelonIconNameWithFullPath);
                }

                // Headquarters/TF/FD Modifier

                // TODO

                // Other?
            } // end skipRemainingLayers

            //TODO: look at the layers to see if any do not exist:
            foreach (string graphicLayer in milSymbol.GraphicLayers)
            {
                if (!System.IO.File.Exists(graphicLayer))
                    System.Diagnostics.Trace.WriteLine("SetMilitarySymbolGraphicLayers: Could not find layer: " + graphicLayer);
            }

            if (milSymbol.GraphicLayers.Count == 0)
                return false;
            else
                return true;

        }

    }
}
