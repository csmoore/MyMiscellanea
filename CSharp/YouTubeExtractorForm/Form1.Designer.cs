namespace YouTubeExtractorForm
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.lbUrl = new System.Windows.Forms.Label();
            this.editUrl = new System.Windows.Forms.TextBox();
            this.buttonExtractAudio = new System.Windows.Forms.Button();
            this.buttonPasteFromClipboard = new System.Windows.Forms.Button();
            this.labelStatus = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbUrl
            // 
            this.lbUrl.AutoSize = true;
            this.lbUrl.BackColor = System.Drawing.Color.Transparent;
            this.lbUrl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbUrl.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lbUrl.Location = new System.Drawing.Point(12, 9);
            this.lbUrl.Name = "lbUrl";
            this.lbUrl.Size = new System.Drawing.Size(130, 16);
            this.lbUrl.TabIndex = 4;
            this.lbUrl.Text = "Youtube Video URL:";
            // 
            // editUrl
            // 
            this.editUrl.Location = new System.Drawing.Point(12, 28);
            this.editUrl.MaxLength = 200;
            this.editUrl.Name = "editUrl";
            this.editUrl.Size = new System.Drawing.Size(291, 20);
            this.editUrl.TabIndex = 1;
            // 
            // buttonExtractAudio
            // 
            this.buttonExtractAudio.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(32)))));
            this.buttonExtractAudio.Location = new System.Drawing.Point(75, 64);
            this.buttonExtractAudio.Name = "buttonExtractAudio";
            this.buttonExtractAudio.Size = new System.Drawing.Size(144, 23);
            this.buttonExtractAudio.TabIndex = 3;
            this.buttonExtractAudio.Text = "Extract Audio";
            this.buttonExtractAudio.UseVisualStyleBackColor = true;
            this.buttonExtractAudio.Click += new System.EventHandler(this.buttonDownloadAudio_Click);
            // 
            // buttonPasteFromClipboard
            // 
            this.buttonPasteFromClipboard.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonPasteFromClipboard.FlatAppearance.BorderSize = 0;
            this.buttonPasteFromClipboard.Location = new System.Drawing.Point(306, 25);
            this.buttonPasteFromClipboard.Margin = new System.Windows.Forms.Padding(0);
            this.buttonPasteFromClipboard.Name = "buttonPasteFromClipboard";
            this.buttonPasteFromClipboard.Size = new System.Drawing.Size(101, 24);
            this.buttonPasteFromClipboard.TabIndex = 2;
            this.buttonPasteFromClipboard.Text = "From Clipboard";
            this.buttonPasteFromClipboard.UseVisualStyleBackColor = false;
            this.buttonPasteFromClipboard.Click += new System.EventHandler(this.buttonPasteFromClipboard_Click);
            // 
            // labelStatus
            // 
            this.labelStatus.BackColor = System.Drawing.Color.Transparent;
            this.labelStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.labelStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.labelStatus.Location = new System.Drawing.Point(0, 121);
            this.labelStatus.Margin = new System.Windows.Forms.Padding(0);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(416, 23);
            this.labelStatus.TabIndex = 5;
            this.labelStatus.Text = "Status";
            this.labelStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // progressBar
            // 
            this.progressBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBar.Location = new System.Drawing.Point(0, 103);
            this.progressBar.MarqueeAnimationSpeed = 20;
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(416, 18);
            this.progressBar.TabIndex = 6;
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(32)))));
            this.buttonCancel.Location = new System.Drawing.Point(235, 64);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(97, 23);
            this.buttonCancel.TabIndex = 7;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(416, 144);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonPasteFromClipboard);
            this.Controls.Add(this.lbUrl);
            this.Controls.Add(this.editUrl);
            this.Controls.Add(this.buttonExtractAudio);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.labelStatus);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "YouTube Extractor (Audio)";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbUrl;
        private System.Windows.Forms.TextBox editUrl;
        private System.Windows.Forms.Button buttonExtractAudio;
        private System.Windows.Forms.Button buttonPasteFromClipboard;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Button buttonCancel;
    }
}

