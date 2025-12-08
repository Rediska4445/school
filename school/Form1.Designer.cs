using System;
using System.Drawing;

namespace school
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        private void InitializeComponent()
        {
            this.panelTop = new System.Windows.Forms.Panel();
            this.labelRole = new System.Windows.Forms.Label();
            this.panelMain = new System.Windows.Forms.Panel();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabHomework = new System.Windows.Forms.TabPage();
            this.panelHomework = new System.Windows.Forms.Panel();
            this.labelHomeworkPeriod = new System.Windows.Forms.Label();
            this.dateTimePickerHomework = new System.Windows.Forms.DateTimePicker();
            this.dataGridViewHomework = new System.Windows.Forms.DataGridView();
            this.tabGrades = new System.Windows.Forms.TabPage();
            this.panelGrades = new System.Windows.Forms.Panel();
            this.labelGradesPeriod = new System.Windows.Forms.Label();
            this.dateTimePickerGrades = new System.Windows.Forms.DateTimePicker();
            this.dataGridViewGrades = new System.Windows.Forms.DataGridView();
            this.tabStatistics = new System.Windows.Forms.TabPage();
            this.panelStatistics = new System.Windows.Forms.Panel();
            this.dateTimePickerStatistics = new System.Windows.Forms.DateTimePicker();
            this.dataGridViewStatistics = new System.Windows.Forms.DataGridView();
            this.tabShedule = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.sheduleLabel = new System.Windows.Forms.Label();
            this.sheduleDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.sheduleGridView = new System.Windows.Forms.DataGridView();
            this.panelTop.SuspendLayout();
            this.panelMain.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabHomework.SuspendLayout();
            this.panelHomework.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewHomework)).BeginInit();
            this.tabGrades.SuspendLayout();
            this.panelGrades.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewGrades)).BeginInit();
            this.tabStatistics.SuspendLayout();
            this.panelStatistics.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewStatistics)).BeginInit();
            this.tabShedule.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sheduleGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.Color.LightSteelBlue;
            this.panelTop.Controls.Add(this.labelRole);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(686, 52);
            this.panelTop.TabIndex = 0;
            // 
            // labelRole
            // 
            this.labelRole.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelRole.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.labelRole.Location = new System.Drawing.Point(12, 13);
            this.labelRole.Name = "labelRole";
            this.labelRole.Size = new System.Drawing.Size(661, 26);
            this.labelRole.TabIndex = 0;
            this.labelRole.Text = "УЧИТЕЛЬ";
            this.labelRole.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelMain
            // 
            this.panelMain.Controls.Add(this.tabControl);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 52);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(686, 408);
            this.panelMain.TabIndex = 1;
            // 
            // tabControl
            // 
            this.tabControl.Alignment = System.Windows.Forms.TabAlignment.Left;
            this.tabControl.Controls.Add(this.tabHomework);
            this.tabControl.Controls.Add(this.tabGrades);
            this.tabControl.Controls.Add(this.tabStatistics);
            this.tabControl.Controls.Add(this.tabShedule);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Multiline = true;
            this.tabControl.Name = "tabControl";
            this.tabControl.Padding = new System.Drawing.Point(20, 3);
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(686, 408);
            this.tabControl.TabIndex = 0;
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
            // 
            // tabHomework
            // 
            this.tabHomework.Controls.Add(this.panelHomework);
            this.tabHomework.Location = new System.Drawing.Point(28, 4);
            this.tabHomework.Name = "tabHomework";
            this.tabHomework.Padding = new System.Windows.Forms.Padding(9);
            this.tabHomework.Size = new System.Drawing.Size(654, 400);
            this.tabHomework.TabIndex = 0;
            this.tabHomework.Text = "Д/З";
            this.tabHomework.UseVisualStyleBackColor = true;
            // 
            // panelHomework
            // 
            this.panelHomework.Controls.Add(this.labelHomeworkPeriod);
            this.panelHomework.Controls.Add(this.dateTimePickerHomework);
            this.panelHomework.Controls.Add(this.dataGridViewHomework);
            this.panelHomework.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelHomework.Location = new System.Drawing.Point(9, 9);
            this.panelHomework.Name = "panelHomework";
            this.panelHomework.Padding = new System.Windows.Forms.Padding(9);
            this.panelHomework.Size = new System.Drawing.Size(636, 382);
            this.panelHomework.TabIndex = 0;
            // 
            // labelHomeworkPeriod
            // 
            this.labelHomeworkPeriod.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelHomeworkPeriod.AutoSize = true;
            this.labelHomeworkPeriod.Location = new System.Drawing.Point(281, 354);
            this.labelHomeworkPeriod.Name = "labelHomeworkPeriod";
            this.labelHomeworkPeriod.Size = new System.Drawing.Size(45, 19);
            this.labelHomeworkPeriod.TabIndex = 2;
            this.labelHomeworkPeriod.Text = "label1";
            // 
            // dateTimePickerHomework
            // 
            this.dateTimePickerHomework.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePickerHomework.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePickerHomework.Location = new System.Drawing.Point(11, 12);
            this.dateTimePickerHomework.Name = "dateTimePickerHomework";
            this.dateTimePickerHomework.Size = new System.Drawing.Size(613, 25);
            this.dateTimePickerHomework.TabIndex = 0;
            this.dateTimePickerHomework.Value = new System.DateTime(2025, 12, 7, 14, 53, 7, 265);
            this.dateTimePickerHomework.ValueChanged += new System.EventHandler(this.dateTimePickerHomework_ValueChanged_1);
            // 
            // dataGridViewHomework
            // 
            this.dataGridViewHomework.AllowUserToAddRows = false;
            this.dataGridViewHomework.AllowUserToDeleteRows = false;
            this.dataGridViewHomework.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewHomework.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewHomework.Location = new System.Drawing.Point(11, 43);
            this.dataGridViewHomework.Name = "dataGridViewHomework";
            this.dataGridViewHomework.ReadOnly = true;
            this.dataGridViewHomework.RowHeadersVisible = false;
            this.dataGridViewHomework.RowTemplate.Height = 25;
            this.dataGridViewHomework.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewHomework.Size = new System.Drawing.Size(613, 308);
            this.dataGridViewHomework.TabIndex = 1;
            // 
            // tabGrades
            // 
            this.tabGrades.Controls.Add(this.panelGrades);
            this.tabGrades.Location = new System.Drawing.Point(28, 4);
            this.tabGrades.Name = "tabGrades";
            this.tabGrades.Padding = new System.Windows.Forms.Padding(9);
            this.tabGrades.Size = new System.Drawing.Size(654, 400);
            this.tabGrades.TabIndex = 1;
            this.tabGrades.Text = "Оценки";
            this.tabGrades.UseVisualStyleBackColor = true;
            // 
            // panelGrades
            // 
            this.panelGrades.Controls.Add(this.labelGradesPeriod);
            this.panelGrades.Controls.Add(this.dateTimePickerGrades);
            this.panelGrades.Controls.Add(this.dataGridViewGrades);
            this.panelGrades.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelGrades.Location = new System.Drawing.Point(9, 9);
            this.panelGrades.Name = "panelGrades";
            this.panelGrades.Padding = new System.Windows.Forms.Padding(9);
            this.panelGrades.Size = new System.Drawing.Size(636, 382);
            this.panelGrades.TabIndex = 0;
            // 
            // labelGradesPeriod
            // 
            this.labelGradesPeriod.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelGradesPeriod.AutoSize = true;
            this.labelGradesPeriod.Location = new System.Drawing.Point(286, 354);
            this.labelGradesPeriod.Name = "labelGradesPeriod";
            this.labelGradesPeriod.Size = new System.Drawing.Size(45, 19);
            this.labelGradesPeriod.TabIndex = 2;
            this.labelGradesPeriod.Text = "label1";
            // 
            // dateTimePickerGrades
            // 
            this.dateTimePickerGrades.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePickerGrades.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePickerGrades.Location = new System.Drawing.Point(12, 12);
            this.dateTimePickerGrades.Name = "dateTimePickerGrades";
            this.dateTimePickerGrades.Size = new System.Drawing.Size(612, 25);
            this.dateTimePickerGrades.TabIndex = 0;
            this.dateTimePickerGrades.Value = new System.DateTime(2025, 12, 7, 14, 53, 7, 277);
            this.dateTimePickerGrades.ValueChanged += new System.EventHandler(this.dateTimePickerGrades_ValueChanged_1);
            // 
            // dataGridViewGrades
            // 
            this.dataGridViewGrades.AllowUserToAddRows = false;
            this.dataGridViewGrades.AllowUserToDeleteRows = false;
            this.dataGridViewGrades.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewGrades.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewGrades.Location = new System.Drawing.Point(11, 43);
            this.dataGridViewGrades.Name = "dataGridViewGrades";
            this.dataGridViewGrades.ReadOnly = true;
            this.dataGridViewGrades.RowHeadersVisible = false;
            this.dataGridViewGrades.RowTemplate.Height = 25;
            this.dataGridViewGrades.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewGrades.Size = new System.Drawing.Size(613, 299);
            this.dataGridViewGrades.TabIndex = 1;
            // 
            // tabStatistics
            // 
            this.tabStatistics.Controls.Add(this.panelStatistics);
            this.tabStatistics.Location = new System.Drawing.Point(28, 4);
            this.tabStatistics.Name = "tabStatistics";
            this.tabStatistics.Padding = new System.Windows.Forms.Padding(9);
            this.tabStatistics.Size = new System.Drawing.Size(654, 400);
            this.tabStatistics.TabIndex = 2;
            this.tabStatistics.Text = "Статистика";
            this.tabStatistics.UseVisualStyleBackColor = true;
            // 
            // panelStatistics
            // 
            this.panelStatistics.Controls.Add(this.dateTimePickerStatistics);
            this.panelStatistics.Controls.Add(this.dataGridViewStatistics);
            this.panelStatistics.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelStatistics.Location = new System.Drawing.Point(9, 9);
            this.panelStatistics.Name = "panelStatistics";
            this.panelStatistics.Padding = new System.Windows.Forms.Padding(9);
            this.panelStatistics.Size = new System.Drawing.Size(636, 382);
            this.panelStatistics.TabIndex = 0;
            // 
            // dateTimePickerStatistics
            // 
            this.dateTimePickerStatistics.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePickerStatistics.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePickerStatistics.Location = new System.Drawing.Point(11, 11);
            this.dateTimePickerStatistics.Name = "dateTimePickerStatistics";
            this.dateTimePickerStatistics.Size = new System.Drawing.Size(613, 25);
            this.dateTimePickerStatistics.TabIndex = 0;
            this.dateTimePickerStatistics.Value = new System.DateTime(2025, 12, 7, 14, 53, 7, 286);
            // 
            // dataGridViewStatistics
            // 
            this.dataGridViewStatistics.AllowUserToAddRows = false;
            this.dataGridViewStatistics.AllowUserToDeleteRows = false;
            this.dataGridViewStatistics.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewStatistics.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewStatistics.Location = new System.Drawing.Point(11, 43);
            this.dataGridViewStatistics.Name = "dataGridViewStatistics";
            this.dataGridViewStatistics.ReadOnly = true;
            this.dataGridViewStatistics.RowHeadersVisible = false;
            this.dataGridViewStatistics.RowTemplate.Height = 25;
            this.dataGridViewStatistics.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewStatistics.Size = new System.Drawing.Size(613, 327);
            this.dataGridViewStatistics.TabIndex = 1;
            // 
            // tabShedule
            // 
            this.tabShedule.Controls.Add(this.panel1);
            this.tabShedule.Location = new System.Drawing.Point(28, 4);
            this.tabShedule.Name = "tabShedule";
            this.tabShedule.Size = new System.Drawing.Size(654, 400);
            this.tabShedule.TabIndex = 3;
            this.tabShedule.Text = "Расписание";
            this.tabShedule.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.sheduleLabel);
            this.panel1.Controls.Add(this.sheduleDateTimePicker);
            this.panel1.Controls.Add(this.sheduleGridView);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(9);
            this.panel1.Size = new System.Drawing.Size(654, 400);
            this.panel1.TabIndex = 1;
            // 
            // sheduleLabel
            // 
            this.sheduleLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sheduleLabel.AutoSize = true;
            this.sheduleLabel.Location = new System.Drawing.Point(286, 372);
            this.sheduleLabel.Name = "sheduleLabel";
            this.sheduleLabel.Size = new System.Drawing.Size(45, 19);
            this.sheduleLabel.TabIndex = 2;
            this.sheduleLabel.Text = "label1";
            // 
            // sheduleDateTimePicker
            // 
            this.sheduleDateTimePicker.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sheduleDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.sheduleDateTimePicker.Location = new System.Drawing.Point(12, 12);
            this.sheduleDateTimePicker.Name = "sheduleDateTimePicker";
            this.sheduleDateTimePicker.Size = new System.Drawing.Size(630, 25);
            this.sheduleDateTimePicker.TabIndex = 0;
            this.sheduleDateTimePicker.Value = new System.DateTime(2025, 12, 7, 14, 53, 7, 277);
            this.sheduleDateTimePicker.ValueChanged += new System.EventHandler(this.sheduleDateTimePicker_ValueChanged);
            // 
            // sheduleGridView
            // 
            this.sheduleGridView.AllowUserToAddRows = false;
            this.sheduleGridView.AllowUserToDeleteRows = false;
            this.sheduleGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sheduleGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.sheduleGridView.Location = new System.Drawing.Point(11, 43);
            this.sheduleGridView.Name = "sheduleGridView";
            this.sheduleGridView.ReadOnly = true;
            this.sheduleGridView.RowHeadersVisible = false;
            this.sheduleGridView.RowTemplate.Height = 25;
            this.sheduleGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.sheduleGridView.Size = new System.Drawing.Size(631, 317);
            this.sheduleGridView.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(686, 460);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.panelTop);
            this.Name = "Form1";
            this.Text = "Школьная информационная система";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.panelTop.ResumeLayout(false);
            this.panelMain.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.tabHomework.ResumeLayout(false);
            this.panelHomework.ResumeLayout(false);
            this.panelHomework.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewHomework)).EndInit();
            this.tabGrades.ResumeLayout(false);
            this.panelGrades.ResumeLayout(false);
            this.panelGrades.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewGrades)).EndInit();
            this.tabStatistics.ResumeLayout(false);
            this.panelStatistics.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewStatistics)).EndInit();
            this.tabShedule.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sheduleGridView)).EndInit();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Panel panelTop, panelMain, panelHomework, panelGrades, panelStatistics;
        private System.Windows.Forms.Label labelRole;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabHomework, tabGrades, tabStatistics;
        private System.Windows.Forms.DateTimePicker dateTimePickerHomework, dateTimePickerGrades, dateTimePickerStatistics;
        private System.Windows.Forms.DataGridView dataGridViewHomework, dataGridViewGrades, dataGridViewStatistics;

        #endregion

        private System.Windows.Forms.Label labelHomeworkPeriod;
        private System.Windows.Forms.TabPage tabShedule;
        private System.Windows.Forms.Label labelGradesPeriod;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label sheduleLabel;
        private System.Windows.Forms.DateTimePicker sheduleDateTimePicker;
        private System.Windows.Forms.DataGridView sheduleGridView;
    }
}

