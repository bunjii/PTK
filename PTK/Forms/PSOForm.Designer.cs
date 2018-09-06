namespace PTK
{
    partial class PSOForm
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
            this.StartButton = new System.Windows.Forms.Button();
            this.StopButton = new System.Windows.Forms.Button();
            this.ResumeButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SaveGenerationGroup = new System.Windows.Forms.GroupBox();
            this.SaveFolderTextBox = new System.Windows.Forms.TextBox();
            this.OpenSaveFolder = new System.Windows.Forms.Button();
            this.OutputLogBox = new System.Windows.Forms.TextBox();
            this.ConfigurationGroup = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ParticleNumBox = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.WBox = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.C1Box = new System.Windows.Forms.NumericUpDown();
            this.C2Box = new System.Windows.Forms.NumericUpDown();
            this.FitnessBox = new System.Windows.Forms.GroupBox();
            this.MaxRadioButton = new System.Windows.Forms.RadioButton();
            this.MinRadioButton = new System.Windows.Forms.RadioButton();
            this.SaveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.SaveGenerationGroup.SuspendLayout();
            this.ConfigurationGroup.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ParticleNumBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.WBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.C1Box)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.C2Box)).BeginInit();
            this.FitnessBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // StartButton
            // 
            this.StartButton.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.StartButton.Location = new System.Drawing.Point(12, 12);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(144, 51);
            this.StartButton.TabIndex = 0;
            this.StartButton.Text = "Start";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // StopButton
            // 
            this.StopButton.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.StopButton.Location = new System.Drawing.Point(162, 12);
            this.StopButton.Name = "StopButton";
            this.StopButton.Size = new System.Drawing.Size(144, 51);
            this.StopButton.TabIndex = 1;
            this.StopButton.Text = "Stop";
            this.StopButton.UseVisualStyleBackColor = true;
            this.StopButton.Click += new System.EventHandler(this.StopButton_Click);
            // 
            // ResumeButton
            // 
            this.ResumeButton.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ResumeButton.Location = new System.Drawing.Point(312, 12);
            this.ResumeButton.Name = "ResumeButton";
            this.ResumeButton.Size = new System.Drawing.Size(144, 51);
            this.ResumeButton.TabIndex = 2;
            this.ResumeButton.Text = "Resume";
            this.ResumeButton.UseVisualStyleBackColor = true;
            this.ResumeButton.Click += new System.EventHandler(this.ResumeButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 14);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(393, 24);
            this.label1.TabIndex = 8;
            this.label1.Text = "For each generation, genes are recorded in the following file.\r\nWhen you press Re" +
    "sume, calculation resumes from the recorded generation.";
            // 
            // SaveGenerationGroup
            // 
            this.SaveGenerationGroup.Controls.Add(this.label1);
            this.SaveGenerationGroup.Controls.Add(this.SaveFolderTextBox);
            this.SaveGenerationGroup.Controls.Add(this.OpenSaveFolder);
            this.SaveGenerationGroup.Location = new System.Drawing.Point(11, 221);
            this.SaveGenerationGroup.Margin = new System.Windows.Forms.Padding(2);
            this.SaveGenerationGroup.Name = "SaveGenerationGroup";
            this.SaveGenerationGroup.Padding = new System.Windows.Forms.Padding(2);
            this.SaveGenerationGroup.Size = new System.Drawing.Size(444, 64);
            this.SaveGenerationGroup.TabIndex = 15;
            this.SaveGenerationGroup.TabStop = false;
            this.SaveGenerationGroup.Text = "Save Generation";
            // 
            // SaveFolderTextBox
            // 
            this.SaveFolderTextBox.Location = new System.Drawing.Point(9, 41);
            this.SaveFolderTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.SaveFolderTextBox.Name = "SaveFolderTextBox";
            this.SaveFolderTextBox.Size = new System.Drawing.Size(378, 19);
            this.SaveFolderTextBox.TabIndex = 6;
            this.SaveFolderTextBox.Leave += new System.EventHandler(this.SaveFolderTextBox_Leave);
            // 
            // OpenSaveFolder
            // 
            this.OpenSaveFolder.Location = new System.Drawing.Point(391, 41);
            this.OpenSaveFolder.Margin = new System.Windows.Forms.Padding(2);
            this.OpenSaveFolder.Name = "OpenSaveFolder";
            this.OpenSaveFolder.Size = new System.Drawing.Size(21, 18);
            this.OpenSaveFolder.TabIndex = 7;
            this.OpenSaveFolder.Text = "...";
            this.OpenSaveFolder.UseVisualStyleBackColor = true;
            // 
            // OutputLogBox
            // 
            this.OutputLogBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OutputLogBox.Location = new System.Drawing.Point(11, 311);
            this.OutputLogBox.Margin = new System.Windows.Forms.Padding(2);
            this.OutputLogBox.Multiline = true;
            this.OutputLogBox.Name = "OutputLogBox";
            this.OutputLogBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.OutputLogBox.Size = new System.Drawing.Size(444, 65);
            this.OutputLogBox.TabIndex = 16;
            // 
            // ConfigurationGroup
            // 
            this.ConfigurationGroup.Controls.Add(this.tableLayoutPanel1);
            this.ConfigurationGroup.Controls.Add(this.FitnessBox);
            this.ConfigurationGroup.Location = new System.Drawing.Point(12, 74);
            this.ConfigurationGroup.Margin = new System.Windows.Forms.Padding(2);
            this.ConfigurationGroup.Name = "ConfigurationGroup";
            this.ConfigurationGroup.Padding = new System.Windows.Forms.Padding(2);
            this.ConfigurationGroup.Size = new System.Drawing.Size(444, 125);
            this.ConfigurationGroup.TabIndex = 17;
            this.ConfigurationGroup.TabStop = false;
            this.ConfigurationGroup.Text = "Configuration";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.ParticleNumBox, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.WBox, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.C1Box, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.C2Box, 1, 3);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(4, 17);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(190, 95);
            this.tableLayoutPanel1.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(2, 76);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(19, 12);
            this.label5.TabIndex = 6;
            this.label5.Text = "C2";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(2, 5);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "Particle Num";
            // 
            // ParticleNumBox
            // 
            this.ParticleNumBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ParticleNumBox.Location = new System.Drawing.Point(97, 2);
            this.ParticleNumBox.Margin = new System.Windows.Forms.Padding(2);
            this.ParticleNumBox.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.ParticleNumBox.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ParticleNumBox.Name = "ParticleNumBox";
            this.ParticleNumBox.Size = new System.Drawing.Size(90, 19);
            this.ParticleNumBox.TabIndex = 5;
            this.ParticleNumBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.ParticleNumBox.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.ParticleNumBox.ValueChanged += new System.EventHandler(this.ParticleNumBox_ValueChanged);
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(2, 28);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(14, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "W";
            // 
            // WBox
            // 
            this.WBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.WBox.DecimalPlaces = 2;
            this.WBox.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.WBox.Location = new System.Drawing.Point(97, 25);
            this.WBox.Margin = new System.Windows.Forms.Padding(2);
            this.WBox.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.WBox.Name = "WBox";
            this.WBox.Size = new System.Drawing.Size(90, 19);
            this.WBox.TabIndex = 5;
            this.WBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.WBox.Value = new decimal(new int[] {
            4,
            0,
            0,
            65536});
            this.WBox.ValueChanged += new System.EventHandler(this.WBox_ValueChanged);
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(2, 51);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(19, 12);
            this.label4.TabIndex = 4;
            this.label4.Text = "C1";
            // 
            // C1Box
            // 
            this.C1Box.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.C1Box.DecimalPlaces = 2;
            this.C1Box.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.C1Box.Location = new System.Drawing.Point(97, 48);
            this.C1Box.Margin = new System.Windows.Forms.Padding(2);
            this.C1Box.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.C1Box.Name = "C1Box";
            this.C1Box.Size = new System.Drawing.Size(90, 19);
            this.C1Box.TabIndex = 5;
            this.C1Box.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.C1Box.Value = new decimal(new int[] {
            20,
            0,
            0,
            65536});
            this.C1Box.ValueChanged += new System.EventHandler(this.C1Box_ValueChanged);
            // 
            // C2Box
            // 
            this.C2Box.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.C2Box.DecimalPlaces = 2;
            this.C2Box.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.C2Box.Location = new System.Drawing.Point(97, 72);
            this.C2Box.Margin = new System.Windows.Forms.Padding(2);
            this.C2Box.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.C2Box.Name = "C2Box";
            this.C2Box.Size = new System.Drawing.Size(90, 19);
            this.C2Box.TabIndex = 7;
            this.C2Box.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.C2Box.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.C2Box.ValueChanged += new System.EventHandler(this.C2Box_ValueChanged);
            // 
            // FitnessBox
            // 
            this.FitnessBox.Controls.Add(this.MaxRadioButton);
            this.FitnessBox.Controls.Add(this.MinRadioButton);
            this.FitnessBox.Location = new System.Drawing.Point(230, 17);
            this.FitnessBox.Margin = new System.Windows.Forms.Padding(2);
            this.FitnessBox.Name = "FitnessBox";
            this.FitnessBox.Padding = new System.Windows.Forms.Padding(2);
            this.FitnessBox.Size = new System.Drawing.Size(152, 38);
            this.FitnessBox.TabIndex = 3;
            this.FitnessBox.TabStop = false;
            this.FitnessBox.Text = "Fitness";
            // 
            // MaxRadioButton
            // 
            this.MaxRadioButton.AutoSize = true;
            this.MaxRadioButton.Location = new System.Drawing.Point(79, 17);
            this.MaxRadioButton.Margin = new System.Windows.Forms.Padding(2);
            this.MaxRadioButton.Name = "MaxRadioButton";
            this.MaxRadioButton.Size = new System.Drawing.Size(70, 16);
            this.MaxRadioButton.TabIndex = 1;
            this.MaxRadioButton.Text = "Maximize";
            this.MaxRadioButton.UseVisualStyleBackColor = true;
            // 
            // MinRadioButton
            // 
            this.MinRadioButton.AutoSize = true;
            this.MinRadioButton.Checked = true;
            this.MinRadioButton.Location = new System.Drawing.Point(12, 17);
            this.MinRadioButton.Margin = new System.Windows.Forms.Padding(2);
            this.MinRadioButton.Name = "MinRadioButton";
            this.MinRadioButton.Size = new System.Drawing.Size(67, 16);
            this.MinRadioButton.TabIndex = 0;
            this.MinRadioButton.TabStop = true;
            this.MinRadioButton.Text = "Minimize";
            this.MinRadioButton.UseVisualStyleBackColor = true;
            // 
            // SaveFileDialog
            // 
            this.SaveFileDialog.CheckPathExists = false;
            this.SaveFileDialog.Filter = "CSV File(*.csv)|*.csv";
            this.SaveFileDialog.Title = "Change file name when saving";
            // 
            // PSOForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(467, 620);
            this.Controls.Add(this.ConfigurationGroup);
            this.Controls.Add(this.OutputLogBox);
            this.Controls.Add(this.SaveGenerationGroup);
            this.Controls.Add(this.ResumeButton);
            this.Controls.Add(this.StopButton);
            this.Controls.Add(this.StartButton);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PSOForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "PSOForm";
            this.SaveGenerationGroup.ResumeLayout(false);
            this.SaveGenerationGroup.PerformLayout();
            this.ConfigurationGroup.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ParticleNumBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.WBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.C1Box)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.C2Box)).EndInit();
            this.FitnessBox.ResumeLayout(false);
            this.FitnessBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.Button StopButton;
        private System.Windows.Forms.Button ResumeButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox SaveGenerationGroup;
        protected System.Windows.Forms.TextBox SaveFolderTextBox;
        protected System.Windows.Forms.Button OpenSaveFolder;
        protected System.Windows.Forms.TextBox OutputLogBox;
        private System.Windows.Forms.GroupBox ConfigurationGroup;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown ParticleNumBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown WBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown C1Box;
        private System.Windows.Forms.GroupBox FitnessBox;
        protected System.Windows.Forms.RadioButton MaxRadioButton;
        protected System.Windows.Forms.RadioButton MinRadioButton;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown C2Box;
        private System.Windows.Forms.SaveFileDialog SaveFileDialog;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
    }
}