namespace PTK.Optimization
{
    partial class GAForm
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series5 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series6 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GAForm));
            this.MinRadioButton = new System.Windows.Forms.RadioButton();
            this.StartButton = new System.Windows.Forms.Button();
            this.StopButton = new System.Windows.Forms.Button();
            this.FitnessBox = new System.Windows.Forms.GroupBox();
            this.MaxRadioButton = new System.Windows.Forms.RadioButton();
            this.OutputLogBox = new System.Windows.Forms.TextBox();
            this.RestartButton = new System.Windows.Forms.Button();
            this.SaveFolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.SaveFolderTextBox = new System.Windows.Forms.TextBox();
            this.OpenSaveFolder = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.FitnessChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.ExitConditionsGroup = new System.Windows.Forms.GroupBox();
            this.ContinuationBox = new System.Windows.Forms.NumericUpDown();
            this.MaxGenerationBox = new System.Windows.Forms.NumericUpDown();
            this.ContinuationCheck = new System.Windows.Forms.CheckBox();
            this.MaxGenerationCheck = new System.Windows.Forms.CheckBox();
            this.ConfigurationGroup = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.IndivitualNumBox = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.EliteRateBox = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.MutationRateBox = new System.Windows.Forms.NumericUpDown();
            this.SaveGenerationGroup = new System.Windows.Forms.GroupBox();
            this.ToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.SaveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.ChartTabControl = new System.Windows.Forms.TabControl();
            this.AllGenerations = new System.Windows.Forms.TabPage();
            this.LastSeveralGenerations = new System.Windows.Forms.TabPage();
            this.RestrictionFitnessChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.FitnessBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FitnessChart)).BeginInit();
            this.ExitConditionsGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ContinuationBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaxGenerationBox)).BeginInit();
            this.ConfigurationGroup.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.IndivitualNumBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EliteRateBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MutationRateBox)).BeginInit();
            this.SaveGenerationGroup.SuspendLayout();
            this.ChartTabControl.SuspendLayout();
            this.AllGenerations.SuspendLayout();
            this.LastSeveralGenerations.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RestrictionFitnessChart)).BeginInit();
            this.SuspendLayout();
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
            this.ToolTip.SetToolTip(this.MinRadioButton, "Do you try to minimize fitness or maximize it?");
            this.MinRadioButton.UseVisualStyleBackColor = true;
            this.MinRadioButton.CheckedChanged += new System.EventHandler(this.MinRadioButton_CheckedChanged);
            // 
            // StartButton
            // 
            this.StartButton.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.StartButton.Location = new System.Drawing.Point(9, 10);
            this.StartButton.Margin = new System.Windows.Forms.Padding(2);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(136, 42);
            this.StartButton.TabIndex = 1;
            this.StartButton.Text = "Start";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // StopButton
            // 
            this.StopButton.Enabled = false;
            this.StopButton.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.StopButton.Location = new System.Drawing.Point(150, 10);
            this.StopButton.Margin = new System.Windows.Forms.Padding(2);
            this.StopButton.Name = "StopButton";
            this.StopButton.Size = new System.Drawing.Size(136, 42);
            this.StopButton.TabIndex = 2;
            this.StopButton.Text = "Stop";
            this.StopButton.UseVisualStyleBackColor = true;
            this.StopButton.Click += new System.EventHandler(this.StopButton_Click);
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
            this.ToolTip.SetToolTip(this.FitnessBox, "Do you try to minimize fitness or maximize it?");
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
            this.ToolTip.SetToolTip(this.MaxRadioButton, "Do you try to minimize fitness or maximize it?");
            this.MaxRadioButton.UseVisualStyleBackColor = true;
            // 
            // OutputLogBox
            // 
            this.OutputLogBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OutputLogBox.Location = new System.Drawing.Point(9, 300);
            this.OutputLogBox.Margin = new System.Windows.Forms.Padding(2);
            this.OutputLogBox.Multiline = true;
            this.OutputLogBox.Name = "OutputLogBox";
            this.OutputLogBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.OutputLogBox.Size = new System.Drawing.Size(420, 65);
            this.OutputLogBox.TabIndex = 4;
            // 
            // RestartButton
            // 
            this.RestartButton.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.RestartButton.Location = new System.Drawing.Point(291, 10);
            this.RestartButton.Margin = new System.Windows.Forms.Padding(2);
            this.RestartButton.Name = "RestartButton";
            this.RestartButton.Size = new System.Drawing.Size(136, 42);
            this.RestartButton.TabIndex = 5;
            this.RestartButton.Text = "Resume";
            this.RestartButton.UseVisualStyleBackColor = true;
            this.RestartButton.Click += new System.EventHandler(this.ResumeButton_Click);
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
            this.OpenSaveFolder.Click += new System.EventHandler(this.OpenSaveFolder_Click);
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
            // FitnessChart
            // 
            chartArea1.AxisX.IsMarginVisible = false;
            chartArea1.AxisX.LabelAutoFitMaxFontSize = 8;
            chartArea1.AxisX.LabelAutoFitMinFontSize = 8;
            chartArea1.AxisX.LabelAutoFitStyle = ((System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles)(((System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.IncreaseFont | System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.DecreaseFont) 
            | System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.StaggeredLabels)));
            chartArea1.AxisX.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(176)))), ((int)(((byte)(226)))));
            chartArea1.AxisX.MajorTickMark.Enabled = false;
            chartArea1.AxisX.MaximumAutoSize = 10F;
            chartArea1.AxisX.Title = "Generation";
            chartArea1.AxisX.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            chartArea1.AxisY.LabelAutoFitMaxFontSize = 8;
            chartArea1.AxisY.LabelAutoFitMinFontSize = 8;
            chartArea1.AxisY.LabelAutoFitStyle = ((System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles)(((System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.IncreaseFont | System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.DecreaseFont) 
            | System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.StaggeredLabels)));
            chartArea1.AxisY.LabelStyle.Format = "G4";
            chartArea1.AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(176)))), ((int)(((byte)(226)))));
            chartArea1.AxisY.MajorTickMark.Enabled = false;
            chartArea1.AxisY.MaximumAutoSize = 15F;
            chartArea1.AxisY.Title = "Fitness";
            chartArea1.AxisY.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            chartArea1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            chartArea1.Name = "ChartArea1";
            chartArea1.ShadowColor = System.Drawing.Color.Empty;
            this.FitnessChart.ChartAreas.Add(chartArea1);
            this.FitnessChart.Location = new System.Drawing.Point(0, 0);
            this.FitnessChart.Margin = new System.Windows.Forms.Padding(2);
            this.FitnessChart.Name = "FitnessChart";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Range;
            series1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(98)))), ((int)(((byte)(176)))), ((int)(((byte)(226)))));
            series1.Name = "MaxMinChart";
            series1.YValuesPerPoint = 2;
            series2.BorderWidth = 3;
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(85)))), ((int)(((byte)(65)))));
            series2.Name = "AverageChart";
            series3.ChartArea = "ChartArea1";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            series3.MarkerBorderColor = System.Drawing.Color.Black;
            series3.MarkerBorderWidth = 2;
            series3.MarkerColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(181)))), ((int)(((byte)(21)))));
            series3.MarkerSize = 8;
            series3.Name = "BestFitnessUpdate";
            series3.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
            this.FitnessChart.Series.Add(series1);
            this.FitnessChart.Series.Add(series2);
            this.FitnessChart.Series.Add(series3);
            this.FitnessChart.Size = new System.Drawing.Size(412, 230);
            this.FitnessChart.TabIndex = 9;
            this.FitnessChart.Text = "chart1";
            // 
            // ExitConditionsGroup
            // 
            this.ExitConditionsGroup.Controls.Add(this.ContinuationBox);
            this.ExitConditionsGroup.Controls.Add(this.MaxGenerationBox);
            this.ExitConditionsGroup.Controls.Add(this.ContinuationCheck);
            this.ExitConditionsGroup.Controls.Add(this.MaxGenerationCheck);
            this.ExitConditionsGroup.Location = new System.Drawing.Point(9, 154);
            this.ExitConditionsGroup.Margin = new System.Windows.Forms.Padding(2);
            this.ExitConditionsGroup.Name = "ExitConditionsGroup";
            this.ExitConditionsGroup.Padding = new System.Windows.Forms.Padding(2);
            this.ExitConditionsGroup.Size = new System.Drawing.Size(418, 65);
            this.ExitConditionsGroup.TabIndex = 12;
            this.ExitConditionsGroup.TabStop = false;
            this.ExitConditionsGroup.Text = "Exit Conditions";
            // 
            // ContinuationBox
            // 
            this.ContinuationBox.Location = new System.Drawing.Point(172, 41);
            this.ContinuationBox.Margin = new System.Windows.Forms.Padding(2);
            this.ContinuationBox.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ContinuationBox.Name = "ContinuationBox";
            this.ContinuationBox.Size = new System.Drawing.Size(90, 19);
            this.ContinuationBox.TabIndex = 16;
            this.ContinuationBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.ToolTip.SetToolTip(this.ContinuationBox, "If the best individual is the same for generations longer than this value, \r\nthe " +
        "search is terminated.");
            this.ContinuationBox.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.ContinuationBox.ValueChanged += new System.EventHandler(this.ContinuationBox_ValueChanged);
            // 
            // MaxGenerationBox
            // 
            this.MaxGenerationBox.Location = new System.Drawing.Point(172, 18);
            this.MaxGenerationBox.Margin = new System.Windows.Forms.Padding(2);
            this.MaxGenerationBox.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.MaxGenerationBox.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.MaxGenerationBox.Name = "MaxGenerationBox";
            this.MaxGenerationBox.Size = new System.Drawing.Size(90, 19);
            this.MaxGenerationBox.TabIndex = 15;
            this.MaxGenerationBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.ToolTip.SetToolTip(this.MaxGenerationBox, "Forcibly terminate the search when this generation turns to this number.");
            this.MaxGenerationBox.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.MaxGenerationBox.ValueChanged += new System.EventHandler(this.MaxGenerationBox_ValueChanged);
            // 
            // ContinuationCheck
            // 
            this.ContinuationCheck.AutoSize = true;
            this.ContinuationCheck.Checked = true;
            this.ContinuationCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ContinuationCheck.Location = new System.Drawing.Point(10, 42);
            this.ContinuationCheck.Margin = new System.Windows.Forms.Padding(2);
            this.ContinuationCheck.Name = "ContinuationCheck";
            this.ContinuationCheck.Size = new System.Drawing.Size(158, 16);
            this.ContinuationCheck.TabIndex = 13;
            this.ContinuationCheck.Text = "Continuation Best Fitness";
            this.ToolTip.SetToolTip(this.ContinuationCheck, "Stop creating next generation when the best individual is not updated for the giv" +
        "en generations.");
            this.ContinuationCheck.UseVisualStyleBackColor = true;
            this.ContinuationCheck.CheckedChanged += new System.EventHandler(this.ContinuationCheck_CheckedChanged);
            // 
            // MaxGenerationCheck
            // 
            this.MaxGenerationCheck.AutoSize = true;
            this.MaxGenerationCheck.Checked = true;
            this.MaxGenerationCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.MaxGenerationCheck.Location = new System.Drawing.Point(10, 20);
            this.MaxGenerationCheck.Margin = new System.Windows.Forms.Padding(2);
            this.MaxGenerationCheck.Name = "MaxGenerationCheck";
            this.MaxGenerationCheck.Size = new System.Drawing.Size(104, 16);
            this.MaxGenerationCheck.TabIndex = 12;
            this.MaxGenerationCheck.Text = "Max Generation";
            this.ToolTip.SetToolTip(this.MaxGenerationCheck, "The upper limit on the number of generations.\r\n");
            this.MaxGenerationCheck.UseVisualStyleBackColor = true;
            this.MaxGenerationCheck.CheckedChanged += new System.EventHandler(this.MaxGenerationCheck_CheckedChanged);
            // 
            // ConfigurationGroup
            // 
            this.ConfigurationGroup.Controls.Add(this.tableLayoutPanel1);
            this.ConfigurationGroup.Controls.Add(this.FitnessBox);
            this.ConfigurationGroup.Location = new System.Drawing.Point(9, 57);
            this.ConfigurationGroup.Margin = new System.Windows.Forms.Padding(2);
            this.ConfigurationGroup.Name = "ConfigurationGroup";
            this.ConfigurationGroup.Padding = new System.Windows.Forms.Padding(2);
            this.ConfigurationGroup.Size = new System.Drawing.Size(418, 93);
            this.ConfigurationGroup.TabIndex = 13;
            this.ConfigurationGroup.TabStop = false;
            this.ConfigurationGroup.Text = "Configuration";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.IndivitualNumBox, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.EliteRateBox, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.MutationRateBox, 1, 2);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(4, 17);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(190, 69);
            this.tableLayoutPanel1.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(2, 5);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "Individual Num";
            // 
            // IndivitualNumBox
            // 
            this.IndivitualNumBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.IndivitualNumBox.Location = new System.Drawing.Point(97, 2);
            this.IndivitualNumBox.Margin = new System.Windows.Forms.Padding(2);
            this.IndivitualNumBox.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.IndivitualNumBox.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.IndivitualNumBox.Name = "IndivitualNumBox";
            this.IndivitualNumBox.Size = new System.Drawing.Size(90, 19);
            this.IndivitualNumBox.TabIndex = 5;
            this.IndivitualNumBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.ToolTip.SetToolTip(this.IndivitualNumBox, "Number of individuals included in one generation. \r\nThe more it is, the harder it" +
        " is to fall into a local solution, while it takes more time to calculate.");
            this.IndivitualNumBox.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.IndivitualNumBox.ValueChanged += new System.EventHandler(this.IndivitualNumBox_ValueChanged);
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(2, 28);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "Elite Rate";
            // 
            // EliteRateBox
            // 
            this.EliteRateBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.EliteRateBox.DecimalPlaces = 2;
            this.EliteRateBox.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.EliteRateBox.Location = new System.Drawing.Point(97, 25);
            this.EliteRateBox.Margin = new System.Windows.Forms.Padding(2);
            this.EliteRateBox.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.EliteRateBox.Name = "EliteRateBox";
            this.EliteRateBox.Size = new System.Drawing.Size(90, 19);
            this.EliteRateBox.TabIndex = 5;
            this.EliteRateBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.ToolTip.SetToolTip(this.EliteRateBox, "Percentage of excellent individuals that will be preferentially left for next gen" +
        "eration. \r\nA higher value leads to less diversity, while a lower value can lead " +
        "to a worse solution.");
            this.EliteRateBox.Value = new decimal(new int[] {
            2,
            0,
            0,
            65536});
            this.EliteRateBox.ValueChanged += new System.EventHandler(this.EliteRateBox_ValueChanged);
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(2, 51);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 12);
            this.label4.TabIndex = 4;
            this.label4.Text = "Mutation Rate";
            // 
            // MutationRateBox
            // 
            this.MutationRateBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.MutationRateBox.DecimalPlaces = 2;
            this.MutationRateBox.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.MutationRateBox.Location = new System.Drawing.Point(97, 48);
            this.MutationRateBox.Margin = new System.Windows.Forms.Padding(2);
            this.MutationRateBox.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.MutationRateBox.Name = "MutationRateBox";
            this.MutationRateBox.Size = new System.Drawing.Size(90, 19);
            this.MutationRateBox.TabIndex = 5;
            this.MutationRateBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.ToolTip.SetToolTip(this.MutationRateBox, "The rate at which the mutation occurs. \r\nA lower value can lead to fall into a lo" +
        "cal solution, while a higher value can lead to less convergence speed.");
            this.MutationRateBox.Value = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.MutationRateBox.ValueChanged += new System.EventHandler(this.MutationRateBox_ValueChanged);
            // 
            // SaveGenerationGroup
            // 
            this.SaveGenerationGroup.Controls.Add(this.label1);
            this.SaveGenerationGroup.Controls.Add(this.SaveFolderTextBox);
            this.SaveGenerationGroup.Controls.Add(this.OpenSaveFolder);
            this.SaveGenerationGroup.Location = new System.Drawing.Point(9, 224);
            this.SaveGenerationGroup.Margin = new System.Windows.Forms.Padding(2);
            this.SaveGenerationGroup.Name = "SaveGenerationGroup";
            this.SaveGenerationGroup.Padding = new System.Windows.Forms.Padding(2);
            this.SaveGenerationGroup.Size = new System.Drawing.Size(418, 64);
            this.SaveGenerationGroup.TabIndex = 14;
            this.SaveGenerationGroup.TabStop = false;
            this.SaveGenerationGroup.Text = "Save Generation";
            // 
            // SaveFileDialog
            // 
            this.SaveFileDialog.CheckPathExists = false;
            this.SaveFileDialog.Filter = "CSV File(*.csv)|*.csv";
            this.SaveFileDialog.Title = "Change file name when saving";
            // 
            // ChartTabControl
            // 
            this.ChartTabControl.Controls.Add(this.AllGenerations);
            this.ChartTabControl.Controls.Add(this.LastSeveralGenerations);
            this.ChartTabControl.Location = new System.Drawing.Point(9, 369);
            this.ChartTabControl.Margin = new System.Windows.Forms.Padding(2);
            this.ChartTabControl.Name = "ChartTabControl";
            this.ChartTabControl.SelectedIndex = 0;
            this.ChartTabControl.Size = new System.Drawing.Size(420, 254);
            this.ChartTabControl.TabIndex = 15;
            // 
            // AllGenerations
            // 
            this.AllGenerations.Controls.Add(this.FitnessChart);
            this.AllGenerations.Location = new System.Drawing.Point(4, 22);
            this.AllGenerations.Margin = new System.Windows.Forms.Padding(2);
            this.AllGenerations.Name = "AllGenerations";
            this.AllGenerations.Padding = new System.Windows.Forms.Padding(2);
            this.AllGenerations.Size = new System.Drawing.Size(412, 228);
            this.AllGenerations.TabIndex = 1;
            this.AllGenerations.Text = "All Generations";
            this.AllGenerations.UseVisualStyleBackColor = true;
            // 
            // LastSeveralGenerations
            // 
            this.LastSeveralGenerations.Controls.Add(this.RestrictionFitnessChart);
            this.LastSeveralGenerations.Location = new System.Drawing.Point(4, 22);
            this.LastSeveralGenerations.Margin = new System.Windows.Forms.Padding(2);
            this.LastSeveralGenerations.Name = "LastSeveralGenerations";
            this.LastSeveralGenerations.Padding = new System.Windows.Forms.Padding(2);
            this.LastSeveralGenerations.Size = new System.Drawing.Size(412, 228);
            this.LastSeveralGenerations.TabIndex = 2;
            this.LastSeveralGenerations.Text = "Last 20 Generations";
            this.LastSeveralGenerations.UseVisualStyleBackColor = true;
            // 
            // RestrictionFitnessChart
            // 
            chartArea2.AxisX.IsMarginVisible = false;
            chartArea2.AxisX.LabelAutoFitMaxFontSize = 8;
            chartArea2.AxisX.LabelAutoFitMinFontSize = 8;
            chartArea2.AxisX.LabelAutoFitStyle = ((System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles)(((System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.IncreaseFont | System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.DecreaseFont) 
            | System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.StaggeredLabels)));
            chartArea2.AxisX.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(176)))), ((int)(((byte)(226)))));
            chartArea2.AxisX.MajorTickMark.Enabled = false;
            chartArea2.AxisX.MaximumAutoSize = 10F;
            chartArea2.AxisX.Title = "Generation";
            chartArea2.AxisX.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            chartArea2.AxisY.LabelAutoFitMaxFontSize = 8;
            chartArea2.AxisY.LabelAutoFitMinFontSize = 8;
            chartArea2.AxisY.LabelAutoFitStyle = ((System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles)(((System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.IncreaseFont | System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.DecreaseFont) 
            | System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.StaggeredLabels)));
            chartArea2.AxisY.LabelStyle.Format = "G4";
            chartArea2.AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(176)))), ((int)(((byte)(226)))));
            chartArea2.AxisY.MajorTickMark.Enabled = false;
            chartArea2.AxisY.MaximumAutoSize = 15F;
            chartArea2.AxisY.Title = "Fitness";
            chartArea2.AxisY.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            chartArea2.Name = "ChartArea1";
            this.RestrictionFitnessChart.ChartAreas.Add(chartArea2);
            this.RestrictionFitnessChart.Location = new System.Drawing.Point(0, 0);
            this.RestrictionFitnessChart.Margin = new System.Windows.Forms.Padding(2);
            this.RestrictionFitnessChart.Name = "RestrictionFitnessChart";
            series4.ChartArea = "ChartArea1";
            series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Range;
            series4.Color = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(98)))), ((int)(((byte)(176)))), ((int)(((byte)(226)))));
            series4.Name = "MaxMinChart";
            series4.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
            series4.YValuesPerPoint = 2;
            series5.BorderWidth = 3;
            series5.ChartArea = "ChartArea1";
            series5.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series5.Color = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(85)))), ((int)(((byte)(65)))));
            series5.Name = "AverageChart";
            series5.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
            series6.ChartArea = "ChartArea1";
            series6.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            series6.MarkerBorderColor = System.Drawing.Color.Black;
            series6.MarkerBorderWidth = 2;
            series6.MarkerColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(181)))), ((int)(((byte)(21)))));
            series6.MarkerSize = 8;
            series6.Name = "BestFitnessUpdate";
            series6.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
            this.RestrictionFitnessChart.Series.Add(series4);
            this.RestrictionFitnessChart.Series.Add(series5);
            this.RestrictionFitnessChart.Series.Add(series6);
            this.RestrictionFitnessChart.Size = new System.Drawing.Size(412, 230);
            this.RestrictionFitnessChart.TabIndex = 10;
            this.RestrictionFitnessChart.Text = "chart1";
            // 
            // GAForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(436, 630);
            this.Controls.Add(this.ChartTabControl);
            this.Controls.Add(this.SaveGenerationGroup);
            this.Controls.Add(this.ConfigurationGroup);
            this.Controls.Add(this.ExitConditionsGroup);
            this.Controls.Add(this.RestartButton);
            this.Controls.Add(this.OutputLogBox);
            this.Controls.Add(this.StopButton);
            this.Controls.Add(this.StartButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GAForm";
            this.ShowInTaskbar = false;
            this.Text = "GAForm";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.GAForm_Load);
            this.FitnessBox.ResumeLayout(false);
            this.FitnessBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FitnessChart)).EndInit();
            this.ExitConditionsGroup.ResumeLayout(false);
            this.ExitConditionsGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ContinuationBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaxGenerationBox)).EndInit();
            this.ConfigurationGroup.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.IndivitualNumBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EliteRateBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MutationRateBox)).EndInit();
            this.SaveGenerationGroup.ResumeLayout(false);
            this.SaveGenerationGroup.PerformLayout();
            this.ChartTabControl.ResumeLayout(false);
            this.AllGenerations.ResumeLayout(false);
            this.LastSeveralGenerations.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.RestrictionFitnessChart)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.GroupBox FitnessBox;
        protected System.Windows.Forms.Button StartButton;
        protected System.Windows.Forms.RadioButton MinRadioButton;
        protected System.Windows.Forms.Button StopButton;
        protected System.Windows.Forms.RadioButton MaxRadioButton;
        protected System.Windows.Forms.TextBox OutputLogBox;
        protected System.Windows.Forms.Button RestartButton;
        protected System.Windows.Forms.TextBox SaveFolderTextBox;
        protected System.Windows.Forms.Button OpenSaveFolder;
        protected System.Windows.Forms.FolderBrowserDialog SaveFolderBrowserDialog;
        private System.Windows.Forms.Label label1;
        protected System.Windows.Forms.DataVisualization.Charting.Chart FitnessChart;
        private System.Windows.Forms.GroupBox ExitConditionsGroup;
        private System.Windows.Forms.CheckBox MaxGenerationCheck;
        private System.Windows.Forms.NumericUpDown ContinuationBox;
        private System.Windows.Forms.NumericUpDown MaxGenerationBox;
        private System.Windows.Forms.CheckBox ContinuationCheck;
        private System.Windows.Forms.GroupBox ConfigurationGroup;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox SaveGenerationGroup;
        private System.Windows.Forms.NumericUpDown IndivitualNumBox;
        private System.Windows.Forms.NumericUpDown EliteRateBox;
        private System.Windows.Forms.NumericUpDown MutationRateBox;
        private System.Windows.Forms.ToolTip ToolTip;
        private System.Windows.Forms.SaveFileDialog SaveFileDialog;
        private System.Windows.Forms.TabControl ChartTabControl;
        private System.Windows.Forms.TabPage AllGenerations;
        private System.Windows.Forms.TabPage LastSeveralGenerations;
        protected System.Windows.Forms.DataVisualization.Charting.Chart RestrictionFitnessChart;
    }
}