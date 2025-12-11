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
            this.tabShedule = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.sheduleTabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.sheduleGridView = new System.Windows.Forms.DataGridView();
            this.sheduleLabel = new System.Windows.Forms.Label();
            this.homeworkTabControl = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.datagridviewHomeworkAll = new System.Windows.Forms.DataGridView();
            this.labelHomeworkAll = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.dataGridViewGradesYear = new System.Windows.Forms.DataGridView();
            this.dataGridViewGradesAll = new System.Windows.Forms.DataGridView();
            this.tabPageEvents = new System.Windows.Forms.TabPage();
            this.tabPageSubjects = new System.Windows.Forms.TabPage();
            this.tabPageTeachers = new System.Windows.Forms.TabPage();
            this.tabPageStudents = new System.Windows.Forms.TabPage();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage7 = new System.Windows.Forms.TabPage();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.tabPage8 = new System.Windows.Forms.TabPage();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.dataGridViewEvents = new System.Windows.Forms.DataGridView();
            this.dataGridViewSubjects = new System.Windows.Forms.DataGridView();
            this.dataGridViewTeachers = new System.Windows.Forms.DataGridView();
            this.dataGridViewStudents = new System.Windows.Forms.DataGridView();
            this.tabControl3 = new System.Windows.Forms.TabControl();
            this.tabPage9 = new System.Windows.Forms.TabPage();
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
            this.tabShedule.SuspendLayout();
            this.panel1.SuspendLayout();
            this.sheduleTabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sheduleGridView)).BeginInit();
            this.homeworkTabControl.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.datagridviewHomeworkAll)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tabPage6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewGradesYear)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewGradesAll)).BeginInit();
            this.tabPageEvents.SuspendLayout();
            this.tabPageSubjects.SuspendLayout();
            this.tabPageTeachers.SuspendLayout();
            this.tabPageStudents.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tabPage8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewEvents)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSubjects)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTeachers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewStudents)).BeginInit();
            this.tabControl3.SuspendLayout();
            this.tabPage9.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.Color.LightSteelBlue;
            this.panelTop.Controls.Add(this.labelRole);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(1189, 52);
            this.panelTop.TabIndex = 0;
            // 
            // labelRole
            // 
            this.labelRole.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelRole.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.labelRole.Location = new System.Drawing.Point(12, 9);
            this.labelRole.Name = "labelRole";
            this.labelRole.Size = new System.Drawing.Size(1164, 30);
            this.labelRole.TabIndex = 0;
            this.labelRole.Text = "Образование";
            this.labelRole.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelMain
            // 
            this.panelMain.Controls.Add(this.tabControl);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 52);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(1189, 828);
            this.panelMain.TabIndex = 1;
            // 
            // tabControl
            // 
            this.tabControl.Alignment = System.Windows.Forms.TabAlignment.Left;
            this.tabControl.Controls.Add(this.tabHomework);
            this.tabControl.Controls.Add(this.tabGrades);
            this.tabControl.Controls.Add(this.tabShedule);
            this.tabControl.Controls.Add(this.tabStatistics);
            this.tabControl.Controls.Add(this.tabPageEvents);
            this.tabControl.Controls.Add(this.tabPageSubjects);
            this.tabControl.Controls.Add(this.tabPageTeachers);
            this.tabControl.Controls.Add(this.tabPageStudents);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Multiline = true;
            this.tabControl.Name = "tabControl";
            this.tabControl.Padding = new System.Drawing.Point(20, 3);
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1189, 828);
            this.tabControl.TabIndex = 0;
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
            // 
            // tabHomework
            // 
            this.tabHomework.Controls.Add(this.panelHomework);
            this.tabHomework.Location = new System.Drawing.Point(28, 4);
            this.tabHomework.Name = "tabHomework";
            this.tabHomework.Padding = new System.Windows.Forms.Padding(9);
            this.tabHomework.Size = new System.Drawing.Size(1157, 820);
            this.tabHomework.TabIndex = 0;
            this.tabHomework.Text = "Д/З";
            this.tabHomework.UseVisualStyleBackColor = true;
            // 
            // panelHomework
            // 
            this.panelHomework.Controls.Add(this.homeworkTabControl);
            this.panelHomework.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelHomework.Location = new System.Drawing.Point(9, 9);
            this.panelHomework.Name = "panelHomework";
            this.panelHomework.Padding = new System.Windows.Forms.Padding(9);
            this.panelHomework.Size = new System.Drawing.Size(1139, 802);
            this.panelHomework.TabIndex = 0;
            // 
            // labelHomeworkPeriod
            // 
            this.labelHomeworkPeriod.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.labelHomeworkPeriod.AutoSize = true;
            this.labelHomeworkPeriod.Location = new System.Drawing.Point(537, 744);
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
            this.dateTimePickerHomework.Location = new System.Drawing.Point(6, 6);
            this.dateTimePickerHomework.Name = "dateTimePickerHomework";
            this.dateTimePickerHomework.Size = new System.Drawing.Size(1104, 25);
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
            this.dataGridViewHomework.Location = new System.Drawing.Point(6, 37);
            this.dataGridViewHomework.Name = "dataGridViewHomework";
            this.dataGridViewHomework.ReadOnly = true;
            this.dataGridViewHomework.RowHeadersVisible = false;
            this.dataGridViewHomework.RowTemplate.Height = 25;
            this.dataGridViewHomework.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewHomework.Size = new System.Drawing.Size(1104, 704);
            this.dataGridViewHomework.TabIndex = 1;
            // 
            // tabGrades
            // 
            this.tabGrades.Controls.Add(this.panelGrades);
            this.tabGrades.Location = new System.Drawing.Point(28, 4);
            this.tabGrades.Name = "tabGrades";
            this.tabGrades.Padding = new System.Windows.Forms.Padding(9);
            this.tabGrades.Size = new System.Drawing.Size(1157, 820);
            this.tabGrades.TabIndex = 1;
            this.tabGrades.Text = "Оценки";
            this.tabGrades.UseVisualStyleBackColor = true;
            // 
            // panelGrades
            // 
            this.panelGrades.Controls.Add(this.tabControl1);
            this.panelGrades.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelGrades.Location = new System.Drawing.Point(9, 9);
            this.panelGrades.Name = "panelGrades";
            this.panelGrades.Padding = new System.Windows.Forms.Padding(9);
            this.panelGrades.Size = new System.Drawing.Size(1139, 802);
            this.panelGrades.TabIndex = 0;
            // 
            // labelGradesPeriod
            // 
            this.labelGradesPeriod.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.labelGradesPeriod.AutoSize = true;
            this.labelGradesPeriod.Location = new System.Drawing.Point(538, 744);
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
            this.dateTimePickerGrades.Location = new System.Drawing.Point(6, 6);
            this.dateTimePickerGrades.Name = "dateTimePickerGrades";
            this.dateTimePickerGrades.Size = new System.Drawing.Size(1104, 25);
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
            this.dataGridViewGrades.Location = new System.Drawing.Point(6, 37);
            this.dataGridViewGrades.Name = "dataGridViewGrades";
            this.dataGridViewGrades.ReadOnly = true;
            this.dataGridViewGrades.RowHeadersVisible = false;
            this.dataGridViewGrades.RowTemplate.Height = 25;
            this.dataGridViewGrades.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewGrades.Size = new System.Drawing.Size(1104, 704);
            this.dataGridViewGrades.TabIndex = 1;
            // 
            // tabStatistics
            // 
            this.tabStatistics.Controls.Add(this.panelStatistics);
            this.tabStatistics.Location = new System.Drawing.Point(28, 4);
            this.tabStatistics.Name = "tabStatistics";
            this.tabStatistics.Padding = new System.Windows.Forms.Padding(9);
            this.tabStatistics.Size = new System.Drawing.Size(1157, 820);
            this.tabStatistics.TabIndex = 2;
            this.tabStatistics.Text = "Статистика";
            this.tabStatistics.UseVisualStyleBackColor = true;
            // 
            // panelStatistics
            // 
            this.panelStatistics.Controls.Add(this.tabControl2);
            this.panelStatistics.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelStatistics.Location = new System.Drawing.Point(9, 9);
            this.panelStatistics.Name = "panelStatistics";
            this.panelStatistics.Padding = new System.Windows.Forms.Padding(9);
            this.panelStatistics.Size = new System.Drawing.Size(1139, 802);
            this.panelStatistics.TabIndex = 0;
            // 
            // tabShedule
            // 
            this.tabShedule.Controls.Add(this.panel1);
            this.tabShedule.Location = new System.Drawing.Point(28, 4);
            this.tabShedule.Name = "tabShedule";
            this.tabShedule.Size = new System.Drawing.Size(1157, 820);
            this.tabShedule.TabIndex = 3;
            this.tabShedule.Text = "Расписание";
            this.tabShedule.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.sheduleTabControl);
            this.panel1.Controls.Add(this.sheduleLabel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(9);
            this.panel1.Size = new System.Drawing.Size(1157, 820);
            this.panel1.TabIndex = 1;
            // 
            // sheduleTabControl
            // 
            this.sheduleTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sheduleTabControl.Controls.Add(this.tabPage1);
            this.sheduleTabControl.Location = new System.Drawing.Point(12, 12);
            this.sheduleTabControl.Name = "sheduleTabControl";
            this.sheduleTabControl.SelectedIndex = 0;
            this.sheduleTabControl.Size = new System.Drawing.Size(1133, 777);
            this.sheduleTabControl.TabIndex = 4;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.sheduleGridView);
            this.tabPage1.Location = new System.Drawing.Point(4, 26);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1125, 747);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Класс";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // sheduleGridView
            // 
            this.sheduleGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sheduleGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.sheduleGridView.Location = new System.Drawing.Point(6, 6);
            this.sheduleGridView.Name = "sheduleGridView";
            this.sheduleGridView.RowHeadersVisible = false;
            this.sheduleGridView.RowTemplate.Height = 25;
            this.sheduleGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.sheduleGridView.Size = new System.Drawing.Size(1113, 735);
            this.sheduleGridView.TabIndex = 3;
            // 
            // sheduleLabel
            // 
            this.sheduleLabel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.sheduleLabel.AutoSize = true;
            this.sheduleLabel.Location = new System.Drawing.Point(561, 792);
            this.sheduleLabel.Name = "sheduleLabel";
            this.sheduleLabel.Size = new System.Drawing.Size(45, 19);
            this.sheduleLabel.TabIndex = 2;
            this.sheduleLabel.Text = "label1";
            // 
            // homeworkTabControl
            // 
            this.homeworkTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.homeworkTabControl.Controls.Add(this.tabPage2);
            this.homeworkTabControl.Controls.Add(this.tabPage3);
            this.homeworkTabControl.Location = new System.Drawing.Point(3, 3);
            this.homeworkTabControl.Name = "homeworkTabControl";
            this.homeworkTabControl.SelectedIndex = 0;
            this.homeworkTabControl.Size = new System.Drawing.Size(1124, 796);
            this.homeworkTabControl.TabIndex = 3;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.dateTimePickerHomework);
            this.tabPage2.Controls.Add(this.labelHomeworkPeriod);
            this.tabPage2.Controls.Add(this.dataGridViewHomework);
            this.tabPage2.Location = new System.Drawing.Point(4, 26);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1116, 766);
            this.tabPage2.TabIndex = 0;
            this.tabPage2.Text = "Период";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.labelHomeworkAll);
            this.tabPage3.Controls.Add(this.datagridviewHomeworkAll);
            this.tabPage3.Location = new System.Drawing.Point(4, 26);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(731, 433);
            this.tabPage3.TabIndex = 1;
            this.tabPage3.Text = "Все";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // datagridviewHomeworkAll
            // 
            this.datagridviewHomeworkAll.AllowUserToAddRows = false;
            this.datagridviewHomeworkAll.AllowUserToDeleteRows = false;
            this.datagridviewHomeworkAll.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.datagridviewHomeworkAll.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.datagridviewHomeworkAll.Location = new System.Drawing.Point(5, 3);
            this.datagridviewHomeworkAll.Name = "datagridviewHomeworkAll";
            this.datagridviewHomeworkAll.ReadOnly = true;
            this.datagridviewHomeworkAll.RowHeadersVisible = false;
            this.datagridviewHomeworkAll.RowTemplate.Height = 25;
            this.datagridviewHomeworkAll.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.datagridviewHomeworkAll.Size = new System.Drawing.Size(723, 406);
            this.datagridviewHomeworkAll.TabIndex = 2;
            // 
            // labelHomeworkAll
            // 
            this.labelHomeworkAll.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelHomeworkAll.AutoSize = true;
            this.labelHomeworkAll.Location = new System.Drawing.Point(347, 412);
            this.labelHomeworkAll.Name = "labelHomeworkAll";
            this.labelHomeworkAll.Size = new System.Drawing.Size(45, 19);
            this.labelHomeworkAll.TabIndex = 3;
            this.labelHomeworkAll.Text = "label1";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage6);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Location = new System.Drawing.Point(3, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1124, 796);
            this.tabControl1.TabIndex = 3;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.dateTimePickerGrades);
            this.tabPage4.Controls.Add(this.labelGradesPeriod);
            this.tabPage4.Controls.Add(this.dataGridViewGrades);
            this.tabPage4.Location = new System.Drawing.Point(4, 26);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(1116, 766);
            this.tabPage4.TabIndex = 0;
            this.tabPage4.Text = "Период";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.dataGridViewGradesAll);
            this.tabPage5.Location = new System.Drawing.Point(4, 26);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(1014, 585);
            this.tabPage5.TabIndex = 1;
            this.tabPage5.Text = "Все";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.dataGridViewGradesYear);
            this.tabPage6.Location = new System.Drawing.Point(4, 26);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Size = new System.Drawing.Size(731, 433);
            this.tabPage6.TabIndex = 2;
            this.tabPage6.Text = "Год";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // dataGridViewGradesYear
            // 
            this.dataGridViewGradesYear.AllowUserToAddRows = false;
            this.dataGridViewGradesYear.AllowUserToDeleteRows = false;
            this.dataGridViewGradesYear.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewGradesYear.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewGradesYear.Location = new System.Drawing.Point(5, 3);
            this.dataGridViewGradesYear.Name = "dataGridViewGradesYear";
            this.dataGridViewGradesYear.ReadOnly = true;
            this.dataGridViewGradesYear.RowHeadersVisible = false;
            this.dataGridViewGradesYear.RowTemplate.Height = 25;
            this.dataGridViewGradesYear.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewGradesYear.Size = new System.Drawing.Size(723, 425);
            this.dataGridViewGradesYear.TabIndex = 2;
            // 
            // dataGridViewGradesAll
            // 
            this.dataGridViewGradesAll.AllowUserToAddRows = false;
            this.dataGridViewGradesAll.AllowUserToDeleteRows = false;
            this.dataGridViewGradesAll.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewGradesAll.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewGradesAll.Location = new System.Drawing.Point(6, 6);
            this.dataGridViewGradesAll.Name = "dataGridViewGradesAll";
            this.dataGridViewGradesAll.ReadOnly = true;
            this.dataGridViewGradesAll.RowHeadersVisible = false;
            this.dataGridViewGradesAll.RowTemplate.Height = 25;
            this.dataGridViewGradesAll.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewGradesAll.Size = new System.Drawing.Size(1002, 573);
            this.dataGridViewGradesAll.TabIndex = 2;
            // 
            // tabPageEvents
            // 
            this.tabPageEvents.Controls.Add(this.dataGridViewEvents);
            this.tabPageEvents.Location = new System.Drawing.Point(28, 4);
            this.tabPageEvents.Name = "tabPageEvents";
            this.tabPageEvents.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageEvents.Size = new System.Drawing.Size(1157, 820);
            this.tabPageEvents.TabIndex = 4;
            this.tabPageEvents.Text = "Мероприятия";
            this.tabPageEvents.UseVisualStyleBackColor = true;
            // 
            // tabPageSubjects
            // 
            this.tabPageSubjects.Controls.Add(this.dataGridViewSubjects);
            this.tabPageSubjects.Location = new System.Drawing.Point(28, 4);
            this.tabPageSubjects.Name = "tabPageSubjects";
            this.tabPageSubjects.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSubjects.Size = new System.Drawing.Size(1157, 820);
            this.tabPageSubjects.TabIndex = 5;
            this.tabPageSubjects.Text = "Предметы";
            this.tabPageSubjects.UseVisualStyleBackColor = true;
            // 
            // tabPageTeachers
            // 
            this.tabPageTeachers.Controls.Add(this.dataGridViewTeachers);
            this.tabPageTeachers.Location = new System.Drawing.Point(28, 4);
            this.tabPageTeachers.Name = "tabPageTeachers";
            this.tabPageTeachers.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTeachers.Size = new System.Drawing.Size(1157, 820);
            this.tabPageTeachers.TabIndex = 6;
            this.tabPageTeachers.Text = "Сотрудники";
            this.tabPageTeachers.UseVisualStyleBackColor = true;
            // 
            // tabPageStudents
            // 
            this.tabPageStudents.Controls.Add(this.tabControl3);
            this.tabPageStudents.Location = new System.Drawing.Point(28, 4);
            this.tabPageStudents.Name = "tabPageStudents";
            this.tabPageStudents.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageStudents.Size = new System.Drawing.Size(1157, 820);
            this.tabPageStudents.TabIndex = 7;
            this.tabPageStudents.Text = "Ученики";
            this.tabPageStudents.UseVisualStyleBackColor = true;
            // 
            // tabControl2
            // 
            this.tabControl2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl2.Controls.Add(this.tabPage7);
            this.tabControl2.Controls.Add(this.tabPage8);
            this.tabControl2.Location = new System.Drawing.Point(7, 3);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(1124, 796);
            this.tabControl2.TabIndex = 4;
            // 
            // tabPage7
            // 
            this.tabPage7.Controls.Add(this.dateTimePicker1);
            this.tabPage7.Controls.Add(this.label1);
            this.tabPage7.Controls.Add(this.dataGridView1);
            this.tabPage7.Location = new System.Drawing.Point(4, 26);
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage7.Size = new System.Drawing.Size(1116, 766);
            this.tabPage7.TabIndex = 0;
            this.tabPage7.Text = "Личное";
            this.tabPage7.UseVisualStyleBackColor = true;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker1.Location = new System.Drawing.Point(6, 6);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(1104, 25);
            this.dateTimePicker1.TabIndex = 0;
            this.dateTimePicker1.Value = new System.DateTime(2025, 12, 7, 14, 53, 7, 277);
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(538, 744);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 19);
            this.label1.TabIndex = 2;
            this.label1.Text = "label1";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(6, 37);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 25;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1104, 704);
            this.dataGridView1.TabIndex = 1;
            // 
            // tabPage8
            // 
            this.tabPage8.Controls.Add(this.dataGridView2);
            this.tabPage8.Location = new System.Drawing.Point(4, 26);
            this.tabPage8.Name = "tabPage8";
            this.tabPage8.Size = new System.Drawing.Size(1116, 766);
            this.tabPage8.TabIndex = 2;
            this.tabPage8.Text = "Класс";
            this.tabPage8.UseVisualStyleBackColor = true;
            // 
            // dataGridView2
            // 
            this.dataGridView2.AllowUserToAddRows = false;
            this.dataGridView2.AllowUserToDeleteRows = false;
            this.dataGridView2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView2.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView2.Location = new System.Drawing.Point(5, 3);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.ReadOnly = true;
            this.dataGridView2.RowHeadersVisible = false;
            this.dataGridView2.RowTemplate.Height = 25;
            this.dataGridView2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView2.Size = new System.Drawing.Size(1108, 758);
            this.dataGridView2.TabIndex = 2;
            // 
            // dataGridViewEvents
            // 
            this.dataGridViewEvents.AllowUserToAddRows = false;
            this.dataGridViewEvents.AllowUserToDeleteRows = false;
            this.dataGridViewEvents.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewEvents.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewEvents.Location = new System.Drawing.Point(6, 6);
            this.dataGridViewEvents.Name = "dataGridViewEvents";
            this.dataGridViewEvents.ReadOnly = true;
            this.dataGridViewEvents.RowHeadersVisible = false;
            this.dataGridViewEvents.RowTemplate.Height = 25;
            this.dataGridViewEvents.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewEvents.Size = new System.Drawing.Size(1142, 806);
            this.dataGridViewEvents.TabIndex = 2;
            // 
            // dataGridViewSubjects
            // 
            this.dataGridViewSubjects.AllowUserToAddRows = false;
            this.dataGridViewSubjects.AllowUserToDeleteRows = false;
            this.dataGridViewSubjects.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewSubjects.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewSubjects.Location = new System.Drawing.Point(3, 3);
            this.dataGridViewSubjects.Name = "dataGridViewSubjects";
            this.dataGridViewSubjects.ReadOnly = true;
            this.dataGridViewSubjects.RowHeadersVisible = false;
            this.dataGridViewSubjects.RowTemplate.Height = 25;
            this.dataGridViewSubjects.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewSubjects.Size = new System.Drawing.Size(1146, 809);
            this.dataGridViewSubjects.TabIndex = 2;
            // 
            // dataGridViewTeachers
            // 
            this.dataGridViewTeachers.AllowUserToAddRows = false;
            this.dataGridViewTeachers.AllowUserToDeleteRows = false;
            this.dataGridViewTeachers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewTeachers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewTeachers.Location = new System.Drawing.Point(6, 6);
            this.dataGridViewTeachers.Name = "dataGridViewTeachers";
            this.dataGridViewTeachers.ReadOnly = true;
            this.dataGridViewTeachers.RowHeadersVisible = false;
            this.dataGridViewTeachers.RowTemplate.Height = 25;
            this.dataGridViewTeachers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewTeachers.Size = new System.Drawing.Size(1142, 806);
            this.dataGridViewTeachers.TabIndex = 2;
            // 
            // dataGridViewStudents
            // 
            this.dataGridViewStudents.AllowUserToAddRows = false;
            this.dataGridViewStudents.AllowUserToDeleteRows = false;
            this.dataGridViewStudents.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewStudents.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewStudents.Location = new System.Drawing.Point(6, 6);
            this.dataGridViewStudents.Name = "dataGridViewStudents";
            this.dataGridViewStudents.ReadOnly = true;
            this.dataGridViewStudents.RowHeadersVisible = false;
            this.dataGridViewStudents.RowTemplate.Height = 25;
            this.dataGridViewStudents.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewStudents.Size = new System.Drawing.Size(1122, 764);
            this.dataGridViewStudents.TabIndex = 2;
            // 
            // tabControl3
            // 
            this.tabControl3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl3.Controls.Add(this.tabPage9);
            this.tabControl3.Location = new System.Drawing.Point(6, 6);
            this.tabControl3.Name = "tabControl3";
            this.tabControl3.SelectedIndex = 0;
            this.tabControl3.Size = new System.Drawing.Size(1142, 806);
            this.tabControl3.TabIndex = 5;
            // 
            // tabPage9
            // 
            this.tabPage9.Controls.Add(this.dataGridViewStudents);
            this.tabPage9.Location = new System.Drawing.Point(4, 26);
            this.tabPage9.Name = "tabPage9";
            this.tabPage9.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage9.Size = new System.Drawing.Size(1134, 776);
            this.tabPage9.TabIndex = 0;
            this.tabPage9.Text = "Класс";
            this.tabPage9.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1189, 880);
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
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewHomework)).EndInit();
            this.tabGrades.ResumeLayout(false);
            this.panelGrades.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewGrades)).EndInit();
            this.tabStatistics.ResumeLayout(false);
            this.panelStatistics.ResumeLayout(false);
            this.tabShedule.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.sheduleTabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sheduleGridView)).EndInit();
            this.homeworkTabControl.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.datagridviewHomeworkAll)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.tabPage5.ResumeLayout(false);
            this.tabPage6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewGradesYear)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewGradesAll)).EndInit();
            this.tabPageEvents.ResumeLayout(false);
            this.tabPageSubjects.ResumeLayout(false);
            this.tabPageTeachers.ResumeLayout(false);
            this.tabPageStudents.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage7.ResumeLayout(false);
            this.tabPage7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tabPage8.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewEvents)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSubjects)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTeachers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewStudents)).EndInit();
            this.tabControl3.ResumeLayout(false);
            this.tabPage9.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Panel panelTop, panelMain, panelHomework, panelGrades, panelStatistics;
        private System.Windows.Forms.Label labelRole;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabHomework, tabGrades, tabStatistics;
        private System.Windows.Forms.DateTimePicker dateTimePickerHomework, dateTimePickerGrades;
        private System.Windows.Forms.DataGridView dataGridViewHomework, dataGridViewGrades;

        #endregion

        private System.Windows.Forms.DataGridView sheduleGridView;
        private System.Windows.Forms.Label labelHomeworkPeriod;
        private System.Windows.Forms.TabPage tabShedule;
        private System.Windows.Forms.Label labelGradesPeriod;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label sheduleLabel;
        private System.Windows.Forms.TabControl sheduleTabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabControl homeworkTabControl;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Label labelHomeworkAll;
        private System.Windows.Forms.DataGridView datagridviewHomeworkAll;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TabPage tabPage6;
        private System.Windows.Forms.DataGridView dataGridViewGradesYear;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.DataGridView dataGridViewGradesAll;
        private System.Windows.Forms.TabPage tabPageEvents;
        private System.Windows.Forms.TabPage tabPageSubjects;
        private System.Windows.Forms.TabPage tabPageTeachers;
        private System.Windows.Forms.TabPage tabPageStudents;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage7;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TabPage tabPage8;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.DataGridView dataGridViewEvents;
        private System.Windows.Forms.DataGridView dataGridViewSubjects;
        private System.Windows.Forms.DataGridView dataGridViewTeachers;
        private System.Windows.Forms.DataGridView dataGridViewStudents;
        private System.Windows.Forms.TabControl tabControl3;
        private System.Windows.Forms.TabPage tabPage9;
    }
}

