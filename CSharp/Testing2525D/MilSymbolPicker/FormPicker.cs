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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Library2525D;

namespace MilSymbolPicker
{
    /// <summary>
    /// 2525D Touch Symbol Picker Form
    /// </summary>
    public partial class FormPicker : Form
    {

        // TODO: this class is a bit of a mess, this was just quick prototyping
        //       probably are easier ways to do what I did here

        public FormPicker()
        {
            InitializeComponent();
        }

        const int BUTTON_ROWS = 5;
        const int BUTTON_COLS = 3;
        List<Button> buttonList = new List<Button>();

        int currentColRowIndex = 0;
        int currentColumn = 1;
        List<string> currentColValues;
        string currentEntityName = string.Empty;
        string currentEntityTypeName = string.Empty;

        MilitarySymbol currentSymbol = new MilitarySymbol();

        public enum PaneSequenceType  // Order of the button panes
        {
            NotSet            = 0,
            AffiliationPane   = 1,
            SymbolSetPane     = 2,
            EntityPane        = 3,
            EntityTypePane    = 4,
            EntitySubTypePane = 5,

            Done = 5,
        }

        PaneSequenceType previousPane = PaneSequenceType.NotSet;
        PaneSequenceType currentPane  = PaneSequenceType.AffiliationPane;
        SymbolLookup symbolLookup = new SymbolLookup();

        private void FormPicker_Load(object sender, EventArgs e)
        {
            symbolLookup.Initialize();

            if (!symbolLookup.Initialized)
                MessageBox.Show("Symbol Search will not work: Could not initialize the Symbol Lookup");

            if (!System.IO.Directory.Exists(MilitarySymbolToGraphicLayersMaker.ImageFilesHome))
                MessageBox.Show("Images will not work: could not find folder: " + MilitarySymbolToGraphicLayersMaker.ImageFilesHome);

            buttonList.Add(this.button11);
            buttonList.Add(this.button12);
            buttonList.Add(this.button13);
            buttonList.Add(this.button14);
            buttonList.Add(this.button15);
            buttonList.Add(this.button21);
            buttonList.Add(this.button22);
            buttonList.Add(this.button23);
            buttonList.Add(this.button24);
            buttonList.Add(this.button25);
            buttonList.Add(this.button31);
            buttonList.Add(this.button32);
            buttonList.Add(this.button33);
            buttonList.Add(this.button34);
            buttonList.Add(this.button35);

            currentSymbol.Id.Affiliation = StandardIdentityAffiliationType.NotSet;
            currentSymbol.Id.SymbolSet = SymbolSetType.NotSet;

            SetPaneState();
        }

        public void SetPaneState()
        {
            bool changedState = (previousPane != currentPane);

            if (changedState)
            {
                System.Diagnostics.Trace.WriteLine("New Pane State: " + currentPane);

                if (currentPane == PaneSequenceType.AffiliationPane)
                {
                    this.labCol1.Text = "Affiliation";
                    this.labCol2.Visible = false;
                    this.labCol3.Visible = false;

                    currentColValues = TypeUtilities.EnumHelper.getEnumValues(typeof(StandardIdentityAffiliationType));

                    currentColRowIndex = 0;

                    enableColumnButtons(1, true);
                    setVisibilityColumnButtons(2, false);
                    setVisibilityColumnButtons(3, false);
                    currentColumn = 1;
                    setColumnValues();
                }
                else if (currentPane == PaneSequenceType.SymbolSetPane)
                {
                    this.labCol2.Text = "Symbol Set";
                    this.labCol2.Visible = true;
                    this.labCol3.Visible = false;

                    currentColValues = TypeUtilities.EnumHelper.getEnumValues(typeof(SymbolSetType));

                    currentColRowIndex = 0;

                    currentColumn = 2;

                    enableColumnButtons(2, true);
                    setColumnValues();
                    enableColumnButtons(1, false);
                    setVisibilityColumnButtons(3, false);
                }
                else if ((currentPane == PaneSequenceType.EntityPane) 
                    || (currentPane == PaneSequenceType.EntityTypePane))
                {
                    this.labCol3.Text = "Entity";
                    this.labCol3.Visible = true;

                    if (currentPane == PaneSequenceType.EntityPane)
                        currentColValues = symbolLookup.GetDistinctEntries(this.currentSymbol.Id.SymbolSet);
                    else
                        if (currentPane == PaneSequenceType.EntityTypePane)
                            currentColValues = symbolLookup.GetDistinctEntries(this.currentSymbol.Id.SymbolSet, currentEntityName);

                    currentColRowIndex = 0;

                    currentColumn = 3;
                    enableColumnButtons(3, true);
                    setColumnValues();
                    enableColumnButtons(1, false);
                    enableColumnButtons(2, false);
                }
            }

            previousPane = currentPane;
        }

        void setSymbolState(string valueSelected)
        {
            if (currentPane == PaneSequenceType.AffiliationPane)
            {
                string affiliationSelectedString = valueSelected;

                StandardIdentityAffiliationType affiliationSelection =
                    (StandardIdentityAffiliationType)
                    TypeUtilities.EnumHelper.getEnumFromString(
                        typeof(StandardIdentityAffiliationType), affiliationSelectedString);

                currentSymbol.Id.Affiliation = affiliationSelection;

                currentPane = PaneSequenceType.SymbolSetPane;
            }
            else if (currentPane == PaneSequenceType.SymbolSetPane)
            {
                string symbolSetSelectedString = valueSelected;

                SymbolSetType symbolSetSelection = (SymbolSetType)
                    TypeUtilities.EnumHelper.getEnumFromString(
                        typeof(SymbolSetType), symbolSetSelectedString);

                currentSymbol.Id.SymbolSet = symbolSetSelection;

                currentPane = PaneSequenceType.EntityPane;
            }
            else if (currentPane == PaneSequenceType.EntityPane)
            {
                currentEntityName = valueSelected;

                currentSymbol.Id.Name = currentEntityName;
                string entityCode = symbolLookup.GetEntityCode(currentSymbol.Id.SymbolSet, currentEntityName);

                currentSymbol.Id.FullEntityCode = entityCode;

                currentPane = PaneSequenceType.EntityTypePane;
            }
            else if (currentPane == PaneSequenceType.EntityTypePane)
            {
                currentEntityTypeName = valueSelected;

                currentSymbol.Id.Name = currentEntityName + TypeUtilities.NameSeparator + currentEntityTypeName;

                string entityCode = symbolLookup.GetEntityCode(currentSymbol.Id.SymbolSet,
                    currentEntityName, currentEntityTypeName);

                currentSymbol.Id.FullEntityCode = entityCode;

                // Go back when we are done
                currentPane = PaneSequenceType.SymbolSetPane;
            }

            setTagLabel();

            updatePictureBox();

            // Go To Next Pane 
            SetPaneState();
        }


        void setVisibilityColumnButtons(int column, bool visible = true)
        {
            for (int i = ((column - 1) * BUTTON_ROWS); i < BUTTON_ROWS * column; i++)
            {
                buttonList[i].Visible = visible;
            }

            if (column == 1)
            {
                butNextCol1.Visible = visible;
            }
            else if (column == 2)
            {
                butBackCol2.Visible = visible;
                butNextCol2.Visible = visible;
            }
            else if (column == 3)
            {
                butBackCol3.Visible = visible;
                butNextCol3.Visible = visible;
            }
        }

        void enableColumnButtons(int column, bool enabled = true)
        {
            for (int i = ((column - 1) * BUTTON_ROWS); i < BUTTON_ROWS * column; i++)
            {
                buttonList[i].Enabled = enabled;
                if (enabled)
                {
                    buttonList[i].UseVisualStyleBackColor = true; // reset the back color for hightlight ones
                    buttonList[i].Visible = true; // make sure visible also
                }
            }

            if (column == 1)
            {
                butNextCol1.Enabled = enabled;
                if (enabled)
                    butNextCol1.Visible = true; // make sure visible also
            }
            else if (column == 2)
            {
                butBackCol2.Enabled = enabled;
                butNextCol2.Enabled = enabled;
                if (enabled)
                {
                    butBackCol2.Visible = true;
                    butNextCol2.Visible = true;
                }
            }
            else if (column == 3)
            {
                butBackCol3.Enabled = enabled;
                butNextCol3.Enabled = enabled;
                if (enabled)
                {
                    butBackCol3.Visible = true;
                    butNextCol3.Visible = true;
                }
            }
        }

        void setColumnValues()
        {
            int column = currentColumn;

            for (int i = ((column - 1) * BUTTON_ROWS); i < BUTTON_ROWS * column; i++)
            {
                if (currentColRowIndex < currentColValues.Count)
                {
                    buttonList[i].Text = currentColValues[currentColRowIndex];
                    buttonList[i].Visible = true;
                    currentColRowIndex++;
                }
                else
                {
                    buttonList[i].Text = String.Empty;
                    buttonList[i].Visible = false;
                }
            }

            if (currentColRowIndex >= currentColValues.Count)
                currentColRowIndex = 0;
        }

        private void updatePictureBox()
        {
            if (!currentSymbol.Id.IsValid)
                return;

            MilitarySymbolToGraphicLayersMaker.SetMilitarySymbolGraphicLayers(ref currentSymbol);

            System.Diagnostics.Trace.WriteLine("MilitarySymbol State After SetMilitarySymbolGraphicLayers : ");
            System.Diagnostics.Trace.WriteLine(this.currentSymbol);

            if (currentSymbol.GraphicLayers.Count == 0)
            {
                System.Diagnostics.Trace.WriteLine("WARNING: No Graphic Layers to Draw");
                return;
            }

            SvgSymbol.ImageSize = new Size(pbPreview.Width, pbPreview.Height);
            pbPreview.Image = SvgSymbol.GetBitmap(currentSymbol.GraphicLayers);

            // Set the Combo Box with the layers
            cbLayers.Items.Clear();
            foreach (string graphicLayer in currentSymbol.GraphicLayers)
            {
                string simpleLayer = graphicLayer.Replace(MilitarySymbolToGraphicLayersMaker.ImageFilesHome,
                    " ");

                if (!System.IO.File.Exists(graphicLayer))
                    simpleLayer = "[MISSING]" + simpleLayer;

                cbLayers.Items.Add(simpleLayer);
            }
            cbLayers.SelectedIndex = 0;
        }

        private void setTagLabel()
        {
            // Also set SIDC here
            if (currentSymbol.Id.IsValid)
            {
                this.labSidcFirst10.Text  = currentSymbol.Id.CodeFirstTen;
                this.labSidcSecond10.Text = currentSymbol.Id.CodeSecondTen;
            }

            StringBuilder tagBuilder = new StringBuilder();

            foreach (string tag in this.currentSymbol.Tags)
            {
                tagBuilder.Append(tag.Replace('_', ' '));
                tagBuilder.Append(";");
            }

            this.labTags.Text = tagBuilder.ToString();
        }

        private void buttonPane_Click(object sender, EventArgs e)
        {
            // Happens on any Pane Button Click

            Button pressedButton = sender as Button;
            pressedButton.BackColor = Color.Yellow;

            string valuePressed = pressedButton.Text;

            setSymbolState(valuePressed);
        }

        private void butNextCol1_Click(object sender, EventArgs e)
        {
            setColumnValues();
        }

        private void butBackCol2_Click(object sender, EventArgs e)
        {
            if (currentColumn != 2)
            {
                System.Diagnostics.Trace.WriteLine("We shouldn't be here...");
                return;
            }

            currentColumn = 1;

            currentPane = PaneSequenceType.AffiliationPane;

            SetPaneState();

            //if (currentPane > PaneSequenceType.AffiliationPane)
            //{
            //    System.Diagnostics.Trace.WriteLine("Going Back from Current Pane: " + currentPane);
            //    currentPane--;
            //}
        }

        private void butNextCol2_Click(object sender, EventArgs e)
        {
            setColumnValues();
        }

        private void butBackCol3_Click(object sender, EventArgs e)
        {
            if (currentColumn != 3)
            {
                System.Diagnostics.Trace.WriteLine("We shouldn't be here...");
                return;
            }

            currentColumn = 2;

            currentPane = PaneSequenceType.SymbolSetPane;

            SetPaneState();
        }

        private void butNextCol3_Click(object sender, EventArgs e)
        {
            setColumnValues();
        }

        private void pbPreview_Click(object sender, EventArgs e)
        {
            if (this.pbPreview.Image == null)
                return;

            // Easter Egg : save image file on click
            SaveFileDialog saveImageFile = new SaveFileDialog();

            string basePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");

            saveImageFile.InitialDirectory = basePath;

            string imageFileName = "SampleImage.png";

            saveImageFile.FileName = imageFileName;
            saveImageFile.Filter = "Text files (*.png)|*.png|All files (*.*)|*.*";

            if (saveImageFile.ShowDialog() == DialogResult.OK)
            {
                Image saveImage = this.pbPreview.Image;
                saveImage.Save(saveImageFile.FileName);
            }
        }
    }
}
