namespace ShiftScheduleVisualiser
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.dataTypeComboBox = new System.Windows.Forms.ComboBox();
            this.showButton = new System.Windows.Forms.Button();
            this.dayComboBox = new System.Windows.Forms.ComboBox();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.personComboBox = new System.Windows.Forms.ComboBox();
            this.testDataDirectoryComboBox = new System.Windows.Forms.ComboBox();
            this.dataSetComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.personIdLabel = new System.Windows.Forms.Label();
            this.formPanel = new System.Windows.Forms.Panel();
            this.backButton = new System.Windows.Forms.Button();
            this.personScheduleGridView = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.formPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.personScheduleGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // dataTypeComboBox
            // 
            this.dataTypeComboBox.Enabled = false;
            this.dataTypeComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.dataTypeComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.dataTypeComboBox.FormattingEnabled = true;
            this.dataTypeComboBox.Items.AddRange(new object[] {
            "Requirements, resulting schedule",
            "Person"});
            this.dataTypeComboBox.Location = new System.Drawing.Point(181, 95);
            this.dataTypeComboBox.Name = "dataTypeComboBox";
            this.dataTypeComboBox.Size = new System.Drawing.Size(265, 28);
            this.dataTypeComboBox.TabIndex = 1;
            // 
            // showButton
            // 
            this.showButton.BackColor = System.Drawing.SystemColors.Control;
            this.showButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.showButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.showButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.showButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.showButton.Location = new System.Drawing.Point(344, 221);
            this.showButton.Name = "showButton";
            this.showButton.Size = new System.Drawing.Size(102, 28);
            this.showButton.TabIndex = 2;
            this.showButton.Text = "Show";
            this.showButton.UseVisualStyleBackColor = false;
            this.showButton.Click += new System.EventHandler(this.showButton_Click);
            // 
            // dayComboBox
            // 
            this.dayComboBox.Enabled = false;
            this.dayComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.dayComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.dayComboBox.FormattingEnabled = true;
            this.dayComboBox.Location = new System.Drawing.Point(181, 135);
            this.dayComboBox.Name = "dayComboBox";
            this.dayComboBox.Size = new System.Drawing.Size(265, 28);
            this.dayComboBox.TabIndex = 3;
            // 
            // chart1
            // 
            this.chart1.BackColor = System.Drawing.Color.Transparent;
            this.chart1.BorderlineColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.chart1.BorderlineWidth = 10;
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            this.chart1.Enabled = false;
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(12, 57);
            this.chart1.Name = "chart1";
            series1.BorderWidth = 5;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Legend = "Legend1";
            series1.Name = "Requirements";
            series2.BorderWidth = 5;
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series2.Legend = "Legend1";
            series2.Name = "Available workers";
            this.chart1.Series.Add(series1);
            this.chart1.Series.Add(series2);
            this.chart1.Size = new System.Drawing.Size(1100, 598);
            this.chart1.TabIndex = 4;
            this.chart1.Text = "chart1";
            this.chart1.Visible = false;
            // 
            // personComboBox
            // 
            this.personComboBox.Enabled = false;
            this.personComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.personComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.personComboBox.FormattingEnabled = true;
            this.personComboBox.Location = new System.Drawing.Point(181, 175);
            this.personComboBox.Name = "personComboBox";
            this.personComboBox.Size = new System.Drawing.Size(265, 28);
            this.personComboBox.TabIndex = 5;
            // 
            // testDataDirectoryComboBox
            // 
            this.testDataDirectoryComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.testDataDirectoryComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.testDataDirectoryComboBox.FormattingEnabled = true;
            this.testDataDirectoryComboBox.Location = new System.Drawing.Point(181, 15);
            this.testDataDirectoryComboBox.Name = "testDataDirectoryComboBox";
            this.testDataDirectoryComboBox.Size = new System.Drawing.Size(265, 28);
            this.testDataDirectoryComboBox.TabIndex = 6;
            // 
            // dataSetComboBox
            // 
            this.dataSetComboBox.Enabled = false;
            this.dataSetComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.dataSetComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.dataSetComboBox.FormattingEnabled = true;
            this.dataSetComboBox.Location = new System.Drawing.Point(181, 55);
            this.dataSetComboBox.Name = "dataSetComboBox";
            this.dataSetComboBox.Size = new System.Drawing.Size(265, 28);
            this.dataSetComboBox.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.Location = new System.Drawing.Point(21, 95);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 20);
            this.label1.TabIndex = 8;
            this.label1.Text = "Data type:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label2.Location = new System.Drawing.Point(21, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(144, 20);
            this.label2.TabIndex = 9;
            this.label2.Text = "Test data directory:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label3.Location = new System.Drawing.Point(21, 55);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 20);
            this.label3.TabIndex = 10;
            this.label3.Text = "Data set:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label4.Location = new System.Drawing.Point(21, 135);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 20);
            this.label4.TabIndex = 11;
            this.label4.Text = "Day:";
            // 
            // personIdLabel
            // 
            this.personIdLabel.AutoSize = true;
            this.personIdLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.personIdLabel.Location = new System.Drawing.Point(21, 175);
            this.personIdLabel.Name = "personIdLabel";
            this.personIdLabel.Size = new System.Drawing.Size(79, 20);
            this.personIdLabel.TabIndex = 12;
            this.personIdLabel.Text = "Person id:";
            // 
            // formPanel
            // 
            this.formPanel.Controls.Add(this.dataTypeComboBox);
            this.formPanel.Controls.Add(this.showButton);
            this.formPanel.Controls.Add(this.personIdLabel);
            this.formPanel.Controls.Add(this.label1);
            this.formPanel.Controls.Add(this.label4);
            this.formPanel.Controls.Add(this.label2);
            this.formPanel.Controls.Add(this.personComboBox);
            this.formPanel.Controls.Add(this.dataSetComboBox);
            this.formPanel.Controls.Add(this.label3);
            this.formPanel.Controls.Add(this.dayComboBox);
            this.formPanel.Controls.Add(this.testDataDirectoryComboBox);
            this.formPanel.Location = new System.Drawing.Point(370, 169);
            this.formPanel.Name = "formPanel";
            this.formPanel.Size = new System.Drawing.Size(458, 308);
            this.formPanel.TabIndex = 13;
            // 
            // backButton
            // 
            this.backButton.BackColor = System.Drawing.Color.Transparent;
            this.backButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.backButton.Enabled = false;
            this.backButton.FlatAppearance.BorderSize = 0;
            this.backButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gray;
            this.backButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.backButton.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.backButton.ForeColor = System.Drawing.Color.Coral;
            this.backButton.Location = new System.Drawing.Point(12, 12);
            this.backButton.Name = "backButton";
            this.backButton.Size = new System.Drawing.Size(80, 45);
            this.backButton.TabIndex = 14;
            this.backButton.Text = "Back";
            this.backButton.UseVisualStyleBackColor = false;
            this.backButton.Visible = false;
            this.backButton.Click += new System.EventHandler(this.backButton_Click);
            // 
            // personScheduleGridView
            // 
            this.personScheduleGridView.AllowUserToAddRows = false;
            this.personScheduleGridView.AllowUserToDeleteRows = false;
            this.personScheduleGridView.AllowUserToResizeColumns = false;
            this.personScheduleGridView.AllowUserToResizeRows = false;
            this.personScheduleGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.personScheduleGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5,
            this.Column6,
            this.Column7,
            this.Column8});
            this.personScheduleGridView.Enabled = false;
            this.personScheduleGridView.Location = new System.Drawing.Point(12, 63);
            this.personScheduleGridView.Name = "personScheduleGridView";
            this.personScheduleGridView.Size = new System.Drawing.Size(1100, 592);
            this.personScheduleGridView.TabIndex = 15;
            this.personScheduleGridView.Visible = false;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "0. ";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 132;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "1.";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 132;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "2.";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Width = 132;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "3.";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.Width = 132;
            // 
            // Column5
            // 
            this.Column5.HeaderText = "4.";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            this.Column5.Width = 132;
            // 
            // Column6
            // 
            this.Column6.HeaderText = "5.";
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            this.Column6.Width = 132;
            // 
            // Column7
            // 
            this.Column7.HeaderText = "6.";
            this.Column7.Name = "Column7";
            this.Column7.ReadOnly = true;
            this.Column7.Width = 132;
            // 
            // Column8
            // 
            this.Column8.HeaderText = "7.";
            this.Column8.Name = "Column8";
            this.Column8.ReadOnly = true;
            this.Column8.Width = 132;
            // 
            // Form1
            // 
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(1124, 667);
            this.Controls.Add(this.personScheduleGridView);
            this.Controls.Add(this.backButton);
            this.Controls.Add(this.formPanel);
            this.Controls.Add(this.chart1);
            this.Name = "Form1";
            this.Text = "Shift schedule visualiser";
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.formPanel.ResumeLayout(false);
            this.formPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.personScheduleGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ComboBox dataTypeComboBox;
        private System.Windows.Forms.Button showButton;
        private System.Windows.Forms.ComboBox dayComboBox;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.ComboBox personComboBox;
        private System.Windows.Forms.ComboBox testDataDirectoryComboBox;
        private System.Windows.Forms.ComboBox dataSetComboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label personIdLabel;
        private System.Windows.Forms.Panel formPanel;
        private System.Windows.Forms.Button backButton;
        private System.Windows.Forms.DataGridView personScheduleGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
    }
}

