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
            this.exitBtnm = new System.Windows.Forms.Button();
            this.labelRole = new System.Windows.Forms.Label();
            this.panelMain = new System.Windows.Forms.Panel();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabHomework = new System.Windows.Forms.TabPage();
            this.panelHomework = new System.Windows.Forms.Panel();
            this.homeworkTabControl = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dateTimePickerHomework = new System.Windows.Forms.DateTimePicker();
            this.labelHomeworkPeriod = new System.Windows.Forms.Label();
            this.dataGridViewHomework = new System.Windows.Forms.DataGridView();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.labelHomeworkAll = new System.Windows.Forms.Label();
            this.datagridviewHomeworkAll = new System.Windows.Forms.DataGridView();
            this.tabGrades = new System.Windows.Forms.TabPage();
            this.panelGrades = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.dateTimePickerGrades = new System.Windows.Forms.DateTimePicker();
            this.labelGradesPeriod = new System.Windows.Forms.Label();
            this.dataGridViewGrades = new System.Windows.Forms.DataGridView();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.dataGridViewGradesYear = new System.Windows.Forms.DataGridView();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.dataGridViewGradesAll = new System.Windows.Forms.DataGridView();
            this.tabShedule = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.sheduleTabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.sheduleGridView = new System.Windows.Forms.DataGridView();
            this.sheduleLabel = new System.Windows.Forms.Label();
            this.tabStatistics = new System.Windows.Forms.TabPage();
            this.panelStatistics = new System.Windows.Forms.Panel();
            this.tabControlStatistic = new System.Windows.Forms.TabControl();
            this.tabPagePersonal = new System.Windows.Forms.TabPage();
            this.dateTimePickerPersonalStatisticsBefore = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerPersonalStatisticsAfter = new System.Windows.Forms.DateTimePicker();
            this.labelStatisticsSummary = new System.Windows.Forms.Label();
            this.dataGridViewPersonalStatistics = new System.Windows.Forms.DataGridView();
            this.tabPageClass = new System.Windows.Forms.TabPage();
            this.dataGridViewStatisticsClass2 = new System.Windows.Forms.DataGridView();
            this.dataGridViewClassStatistics1 = new System.Windows.Forms.DataGridView();
            this.dataGridViewClassStatistics = new System.Windows.Forms.DataGridView();
            this.tabPageEvents = new System.Windows.Forms.TabPage();
            this.dataGridViewEvents = new System.Windows.Forms.DataGridView();
            this.tabPageSubjects = new System.Windows.Forms.TabPage();
            this.dataGridViewSubjects = new System.Windows.Forms.DataGridView();
            this.tabPageTeachers = new System.Windows.Forms.TabPage();
            this.dataGridViewTeachers = new System.Windows.Forms.DataGridView();
            this.tabPageStudents = new System.Windows.Forms.TabPage();
            this.tabControl3 = new System.Windows.Forms.TabControl();
            this.tabPage9 = new System.Windows.Forms.TabPage();
            this.dataGridViewStudents = new System.Windows.Forms.DataGridView();
            this.tabPageAttendance = new System.Windows.Forms.TabPage();
            this.tabControlAttendance = new System.Windows.Forms.TabControl();
            this.tabPage8 = new System.Windows.Forms.TabPage();
            this.dataGridViewPersonalAttendance = new System.Windows.Forms.DataGridView();
            this.tabPage7 = new System.Windows.Forms.TabPage();
            this.dataGridViewClassAtterdance = new System.Windows.Forms.DataGridView();
            this.tabPageReports = new System.Windows.Forms.TabPage();
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.printDocumentDialog1 = new System.Windows.Forms.PrintDialog();
            this.printDocumentAtterdance = new System.Drawing.Printing.PrintDocument();
            this.buttonPrint = new System.Windows.Forms.Button();
            this.printDialog1 = new System.Windows.Forms.PrintDialog();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panelTop.SuspendLayout();
            this.panelMain.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabHomework.SuspendLayout();
            this.panelHomework.SuspendLayout();
            this.homeworkTabControl.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewHomework)).BeginInit();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.datagridviewHomeworkAll)).BeginInit();
            this.tabGrades.SuspendLayout();
            this.panelGrades.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewGrades)).BeginInit();
            this.tabPage6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewGradesYear)).BeginInit();
            this.tabPage5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewGradesAll)).BeginInit();
            this.tabShedule.SuspendLayout();
            this.panel1.SuspendLayout();
            this.sheduleTabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sheduleGridView)).BeginInit();
            this.tabStatistics.SuspendLayout();
            this.panelStatistics.SuspendLayout();
            this.tabControlStatistic.SuspendLayout();
            this.tabPagePersonal.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPersonalStatistics)).BeginInit();
            this.tabPageClass.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewStatisticsClass2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewClassStatistics1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewClassStatistics)).BeginInit();
            this.tabPageEvents.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewEvents)).BeginInit();
            this.tabPageSubjects.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSubjects)).BeginInit();
            this.tabPageTeachers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTeachers)).BeginInit();
            this.tabPageStudents.SuspendLayout();
            this.tabControl3.SuspendLayout();
            this.tabPage9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewStudents)).BeginInit();
            this.tabPageAttendance.SuspendLayout();
            this.tabControlAttendance.SuspendLayout();
            this.tabPage8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPersonalAttendance)).BeginInit();
            this.tabPage7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewClassAtterdance)).BeginInit();
            this.tabPageReports.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.Color.LightSteelBlue;
            this.panelTop.Controls.Add(this.buttonPrint);
            this.panelTop.Controls.Add(this.exitBtnm);
            this.panelTop.Controls.Add(this.labelRole);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(1205, 52);
            this.panelTop.TabIndex = 0;
            // 
            // exitBtnm
            // 
            this.exitBtnm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.exitBtnm.Location = new System.Drawing.Point(1099, 9);
            this.exitBtnm.Name = "exitBtnm";
            this.exitBtnm.Size = new System.Drawing.Size(93, 30);
            this.exitBtnm.TabIndex = 1;
            this.exitBtnm.Text = "Выйти";
            this.exitBtnm.UseVisualStyleBackColor = true;
            this.exitBtnm.Click += new System.EventHandler(this.exitBtnm_Click);
            // 
            // labelRole
            // 
            this.labelRole.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelRole.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.labelRole.Location = new System.Drawing.Point(12, 9);
            this.labelRole.Name = "labelRole";
            this.labelRole.Size = new System.Drawing.Size(1180, 30);
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
            this.panelMain.Size = new System.Drawing.Size(1205, 828);
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
            this.tabControl.Controls.Add(this.tabPageAttendance);
            this.tabControl.Controls.Add(this.tabPageReports);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Multiline = true;
            this.tabControl.Name = "tabControl";
            this.tabControl.Padding = new System.Drawing.Point(20, 3);
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1205, 828);
            this.tabControl.TabIndex = 0;
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
            // 
            // tabHomework
            // 
            this.tabHomework.Controls.Add(this.panelHomework);
            this.tabHomework.Location = new System.Drawing.Point(52, 4);
            this.tabHomework.Name = "tabHomework";
            this.tabHomework.Padding = new System.Windows.Forms.Padding(9);
            this.tabHomework.Size = new System.Drawing.Size(1149, 820);
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
            this.panelHomework.Size = new System.Drawing.Size(1131, 802);
            this.panelHomework.TabIndex = 0;
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
            this.homeworkTabControl.Size = new System.Drawing.Size(1116, 796);
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
            this.tabPage2.Size = new System.Drawing.Size(1108, 766);
            this.tabPage2.TabIndex = 0;
            this.tabPage2.Text = "Период";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // dateTimePickerHomework
            // 
            this.dateTimePickerHomework.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePickerHomework.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePickerHomework.Location = new System.Drawing.Point(6, 6);
            this.dateTimePickerHomework.Name = "dateTimePickerHomework";
            this.dateTimePickerHomework.Size = new System.Drawing.Size(1096, 25);
            this.dateTimePickerHomework.TabIndex = 0;
            this.dateTimePickerHomework.Value = new System.DateTime(2025, 12, 7, 14, 53, 7, 265);
            this.dateTimePickerHomework.ValueChanged += new System.EventHandler(this.dateTimePickerHomework_ValueChanged_1);
            // 
            // labelHomeworkPeriod
            // 
            this.labelHomeworkPeriod.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.labelHomeworkPeriod.AutoSize = true;
            this.labelHomeworkPeriod.Location = new System.Drawing.Point(533, 744);
            this.labelHomeworkPeriod.Name = "labelHomeworkPeriod";
            this.labelHomeworkPeriod.Size = new System.Drawing.Size(45, 19);
            this.labelHomeworkPeriod.TabIndex = 2;
            this.labelHomeworkPeriod.Text = "label1";
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
            this.dataGridViewHomework.Size = new System.Drawing.Size(1096, 704);
            this.dataGridViewHomework.TabIndex = 1;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.labelHomeworkAll);
            this.tabPage3.Controls.Add(this.datagridviewHomeworkAll);
            this.tabPage3.Location = new System.Drawing.Point(4, 26);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(1108, 766);
            this.tabPage3.TabIndex = 1;
            this.tabPage3.Text = "Все";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // labelHomeworkAll
            // 
            this.labelHomeworkAll.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelHomeworkAll.AutoSize = true;
            this.labelHomeworkAll.Location = new System.Drawing.Point(347, 761);
            this.labelHomeworkAll.Name = "labelHomeworkAll";
            this.labelHomeworkAll.Size = new System.Drawing.Size(45, 19);
            this.labelHomeworkAll.TabIndex = 3;
            this.labelHomeworkAll.Text = "label1";
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
            this.datagridviewHomeworkAll.Size = new System.Drawing.Size(1100, 755);
            this.datagridviewHomeworkAll.TabIndex = 2;
            // 
            // tabGrades
            // 
            this.tabGrades.Controls.Add(this.panelGrades);
            this.tabGrades.Location = new System.Drawing.Point(52, 4);
            this.tabGrades.Name = "tabGrades";
            this.tabGrades.Padding = new System.Windows.Forms.Padding(9);
            this.tabGrades.Size = new System.Drawing.Size(1149, 820);
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
            this.panelGrades.Size = new System.Drawing.Size(1131, 802);
            this.panelGrades.TabIndex = 0;
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
            this.tabControl1.Size = new System.Drawing.Size(1116, 796);
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
            this.tabPage4.Size = new System.Drawing.Size(1108, 766);
            this.tabPage4.TabIndex = 0;
            this.tabPage4.Text = "Период";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // dateTimePickerGrades
            // 
            this.dateTimePickerGrades.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePickerGrades.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePickerGrades.Location = new System.Drawing.Point(6, 6);
            this.dateTimePickerGrades.Name = "dateTimePickerGrades";
            this.dateTimePickerGrades.Size = new System.Drawing.Size(1096, 25);
            this.dateTimePickerGrades.TabIndex = 0;
            this.dateTimePickerGrades.Value = new System.DateTime(2025, 12, 7, 14, 53, 7, 277);
            this.dateTimePickerGrades.ValueChanged += new System.EventHandler(this.dateTimePickerGrades_ValueChanged_1);
            // 
            // labelGradesPeriod
            // 
            this.labelGradesPeriod.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.labelGradesPeriod.AutoSize = true;
            this.labelGradesPeriod.Location = new System.Drawing.Point(534, 744);
            this.labelGradesPeriod.Name = "labelGradesPeriod";
            this.labelGradesPeriod.Size = new System.Drawing.Size(45, 19);
            this.labelGradesPeriod.TabIndex = 2;
            this.labelGradesPeriod.Text = "label1";
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
            this.dataGridViewGrades.Size = new System.Drawing.Size(1096, 704);
            this.dataGridViewGrades.TabIndex = 1;
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.dataGridViewGradesYear);
            this.tabPage6.Location = new System.Drawing.Point(4, 26);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Size = new System.Drawing.Size(1108, 766);
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
            this.dataGridViewGradesYear.Size = new System.Drawing.Size(1100, 774);
            this.dataGridViewGradesYear.TabIndex = 2;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.dataGridViewGradesAll);
            this.tabPage5.Location = new System.Drawing.Point(4, 26);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(1108, 766);
            this.tabPage5.TabIndex = 1;
            this.tabPage5.Text = "Все";
            this.tabPage5.UseVisualStyleBackColor = true;
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
            this.dataGridViewGradesAll.Size = new System.Drawing.Size(1096, 770);
            this.dataGridViewGradesAll.TabIndex = 2;
            // 
            // tabShedule
            // 
            this.tabShedule.Controls.Add(this.panel1);
            this.tabShedule.Location = new System.Drawing.Point(52, 4);
            this.tabShedule.Name = "tabShedule";
            this.tabShedule.Size = new System.Drawing.Size(1149, 820);
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
            this.panel1.Size = new System.Drawing.Size(1149, 820);
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
            this.sheduleTabControl.Size = new System.Drawing.Size(1125, 777);
            this.sheduleTabControl.TabIndex = 4;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.sheduleGridView);
            this.tabPage1.Location = new System.Drawing.Point(4, 26);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1117, 747);
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
            this.sheduleGridView.Size = new System.Drawing.Size(1105, 735);
            this.sheduleGridView.TabIndex = 3;
            // 
            // sheduleLabel
            // 
            this.sheduleLabel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.sheduleLabel.AutoSize = true;
            this.sheduleLabel.Location = new System.Drawing.Point(557, 792);
            this.sheduleLabel.Name = "sheduleLabel";
            this.sheduleLabel.Size = new System.Drawing.Size(45, 19);
            this.sheduleLabel.TabIndex = 2;
            this.sheduleLabel.Text = "label1";
            // 
            // tabStatistics
            // 
            this.tabStatistics.Controls.Add(this.panelStatistics);
            this.tabStatistics.Location = new System.Drawing.Point(52, 4);
            this.tabStatistics.Name = "tabStatistics";
            this.tabStatistics.Padding = new System.Windows.Forms.Padding(9);
            this.tabStatistics.Size = new System.Drawing.Size(1149, 820);
            this.tabStatistics.TabIndex = 2;
            this.tabStatistics.Text = "Статистика";
            this.tabStatistics.UseVisualStyleBackColor = true;
            // 
            // panelStatistics
            // 
            this.panelStatistics.Controls.Add(this.tabControlStatistic);
            this.panelStatistics.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelStatistics.Location = new System.Drawing.Point(9, 9);
            this.panelStatistics.Name = "panelStatistics";
            this.panelStatistics.Padding = new System.Windows.Forms.Padding(9);
            this.panelStatistics.Size = new System.Drawing.Size(1131, 802);
            this.panelStatistics.TabIndex = 0;
            // 
            // tabControlStatistic
            // 
            this.tabControlStatistic.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlStatistic.Controls.Add(this.tabPagePersonal);
            this.tabControlStatistic.Controls.Add(this.tabPageClass);
            this.tabControlStatistic.Location = new System.Drawing.Point(7, 3);
            this.tabControlStatistic.Name = "tabControlStatistic";
            this.tabControlStatistic.SelectedIndex = 0;
            this.tabControlStatistic.Size = new System.Drawing.Size(1116, 796);
            this.tabControlStatistic.TabIndex = 4;
            // 
            // tabPagePersonal
            // 
            this.tabPagePersonal.Controls.Add(this.dateTimePickerPersonalStatisticsBefore);
            this.tabPagePersonal.Controls.Add(this.dateTimePickerPersonalStatisticsAfter);
            this.tabPagePersonal.Controls.Add(this.labelStatisticsSummary);
            this.tabPagePersonal.Controls.Add(this.dataGridViewPersonalStatistics);
            this.tabPagePersonal.Location = new System.Drawing.Point(4, 26);
            this.tabPagePersonal.Name = "tabPagePersonal";
            this.tabPagePersonal.Padding = new System.Windows.Forms.Padding(3);
            this.tabPagePersonal.Size = new System.Drawing.Size(1108, 766);
            this.tabPagePersonal.TabIndex = 0;
            this.tabPagePersonal.Text = "Личное";
            this.tabPagePersonal.UseVisualStyleBackColor = true;
            // 
            // dateTimePickerPersonalStatisticsBefore
            // 
            this.dateTimePickerPersonalStatisticsBefore.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.dateTimePickerPersonalStatisticsBefore.Location = new System.Drawing.Point(6, 8);
            this.dateTimePickerPersonalStatisticsBefore.Name = "dateTimePickerPersonalStatisticsBefore";
            this.dateTimePickerPersonalStatisticsBefore.Size = new System.Drawing.Size(536, 25);
            this.dateTimePickerPersonalStatisticsBefore.TabIndex = 4;
            // 
            // dateTimePickerPersonalStatisticsAfter
            // 
            this.dateTimePickerPersonalStatisticsAfter.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.dateTimePickerPersonalStatisticsAfter.Location = new System.Drawing.Point(548, 8);
            this.dateTimePickerPersonalStatisticsAfter.Name = "dateTimePickerPersonalStatisticsAfter";
            this.dateTimePickerPersonalStatisticsAfter.Size = new System.Drawing.Size(554, 25);
            this.dateTimePickerPersonalStatisticsAfter.TabIndex = 3;
            // 
            // labelStatisticsSummary
            // 
            this.labelStatisticsSummary.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.labelStatisticsSummary.AutoSize = true;
            this.labelStatisticsSummary.Location = new System.Drawing.Point(538, 730);
            this.labelStatisticsSummary.Name = "labelStatisticsSummary";
            this.labelStatisticsSummary.Size = new System.Drawing.Size(45, 19);
            this.labelStatisticsSummary.TabIndex = 2;
            this.labelStatisticsSummary.Text = "label1";
            // 
            // dataGridViewPersonalStatistics
            // 
            this.dataGridViewPersonalStatistics.AllowUserToAddRows = false;
            this.dataGridViewPersonalStatistics.AllowUserToDeleteRows = false;
            this.dataGridViewPersonalStatistics.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewPersonalStatistics.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewPersonalStatistics.Location = new System.Drawing.Point(6, 39);
            this.dataGridViewPersonalStatistics.Name = "dataGridViewPersonalStatistics";
            this.dataGridViewPersonalStatistics.ReadOnly = true;
            this.dataGridViewPersonalStatistics.RowHeadersVisible = false;
            this.dataGridViewPersonalStatistics.RowTemplate.Height = 25;
            this.dataGridViewPersonalStatistics.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewPersonalStatistics.Size = new System.Drawing.Size(1096, 688);
            this.dataGridViewPersonalStatistics.TabIndex = 1;
            // 
            // tabPageClass
            // 
            this.tabPageClass.Controls.Add(this.dataGridViewStatisticsClass2);
            this.tabPageClass.Controls.Add(this.dataGridViewClassStatistics1);
            this.tabPageClass.Controls.Add(this.dataGridViewClassStatistics);
            this.tabPageClass.Location = new System.Drawing.Point(4, 26);
            this.tabPageClass.Name = "tabPageClass";
            this.tabPageClass.Size = new System.Drawing.Size(1108, 766);
            this.tabPageClass.TabIndex = 2;
            this.tabPageClass.Text = "Класс";
            this.tabPageClass.UseVisualStyleBackColor = true;
            // 
            // dataGridViewStatisticsClass2
            // 
            this.dataGridViewStatisticsClass2.AllowUserToAddRows = false;
            this.dataGridViewStatisticsClass2.AllowUserToDeleteRows = false;
            this.dataGridViewStatisticsClass2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewStatisticsClass2.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewStatisticsClass2.Location = new System.Drawing.Point(5, 410);
            this.dataGridViewStatisticsClass2.Name = "dataGridViewStatisticsClass2";
            this.dataGridViewStatisticsClass2.ReadOnly = true;
            this.dataGridViewStatisticsClass2.RowHeadersVisible = false;
            this.dataGridViewStatisticsClass2.RowTemplate.Height = 25;
            this.dataGridViewStatisticsClass2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewStatisticsClass2.Size = new System.Drawing.Size(1100, 357);
            this.dataGridViewStatisticsClass2.TabIndex = 4;
            // 
            // dataGridViewClassStatistics1
            // 
            this.dataGridViewClassStatistics1.AllowUserToAddRows = false;
            this.dataGridViewClassStatistics1.AllowUserToDeleteRows = false;
            this.dataGridViewClassStatistics1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewClassStatistics1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewClassStatistics1.Location = new System.Drawing.Point(556, 5);
            this.dataGridViewClassStatistics1.Name = "dataGridViewClassStatistics1";
            this.dataGridViewClassStatistics1.ReadOnly = true;
            this.dataGridViewClassStatistics1.RowHeadersVisible = false;
            this.dataGridViewClassStatistics1.RowTemplate.Height = 25;
            this.dataGridViewClassStatistics1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewClassStatistics1.Size = new System.Drawing.Size(549, 399);
            this.dataGridViewClassStatistics1.TabIndex = 3;
            // 
            // dataGridViewClassStatistics
            // 
            this.dataGridViewClassStatistics.AllowUserToAddRows = false;
            this.dataGridViewClassStatistics.AllowUserToDeleteRows = false;
            this.dataGridViewClassStatistics.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewClassStatistics.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewClassStatistics.Location = new System.Drawing.Point(3, 5);
            this.dataGridViewClassStatistics.Name = "dataGridViewClassStatistics";
            this.dataGridViewClassStatistics.ReadOnly = true;
            this.dataGridViewClassStatistics.RowHeadersVisible = false;
            this.dataGridViewClassStatistics.RowTemplate.Height = 25;
            this.dataGridViewClassStatistics.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewClassStatistics.Size = new System.Drawing.Size(547, 399);
            this.dataGridViewClassStatistics.TabIndex = 2;
            // 
            // tabPageEvents
            // 
            this.tabPageEvents.Controls.Add(this.dataGridViewEvents);
            this.tabPageEvents.Location = new System.Drawing.Point(52, 4);
            this.tabPageEvents.Name = "tabPageEvents";
            this.tabPageEvents.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageEvents.Size = new System.Drawing.Size(1149, 820);
            this.tabPageEvents.TabIndex = 4;
            this.tabPageEvents.Text = "Мероприятия";
            this.tabPageEvents.UseVisualStyleBackColor = true;
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
            this.dataGridViewEvents.Size = new System.Drawing.Size(1117, 806);
            this.dataGridViewEvents.TabIndex = 2;
            // 
            // tabPageSubjects
            // 
            this.tabPageSubjects.Controls.Add(this.dataGridViewSubjects);
            this.tabPageSubjects.Location = new System.Drawing.Point(52, 4);
            this.tabPageSubjects.Name = "tabPageSubjects";
            this.tabPageSubjects.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSubjects.Size = new System.Drawing.Size(1149, 820);
            this.tabPageSubjects.TabIndex = 5;
            this.tabPageSubjects.Text = "Предметы";
            this.tabPageSubjects.UseVisualStyleBackColor = true;
            // 
            // dataGridViewSubjects
            // 
            this.dataGridViewSubjects.AllowUserToAddRows = false;
            this.dataGridViewSubjects.AllowUserToDeleteRows = false;
            this.dataGridViewSubjects.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewSubjects.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewSubjects.Location = new System.Drawing.Point(6, 6);
            this.dataGridViewSubjects.Name = "dataGridViewSubjects";
            this.dataGridViewSubjects.ReadOnly = true;
            this.dataGridViewSubjects.RowHeadersVisible = false;
            this.dataGridViewSubjects.RowTemplate.Height = 25;
            this.dataGridViewSubjects.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewSubjects.Size = new System.Drawing.Size(1134, 806);
            this.dataGridViewSubjects.TabIndex = 2;
            // 
            // tabPageTeachers
            // 
            this.tabPageTeachers.Controls.Add(this.dataGridViewTeachers);
            this.tabPageTeachers.Location = new System.Drawing.Point(52, 4);
            this.tabPageTeachers.Name = "tabPageTeachers";
            this.tabPageTeachers.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTeachers.Size = new System.Drawing.Size(1149, 820);
            this.tabPageTeachers.TabIndex = 6;
            this.tabPageTeachers.Text = "Сотрудники";
            this.tabPageTeachers.UseVisualStyleBackColor = true;
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
            // tabPageStudents
            // 
            this.tabPageStudents.Controls.Add(this.tabControl3);
            this.tabPageStudents.Location = new System.Drawing.Point(52, 4);
            this.tabPageStudents.Name = "tabPageStudents";
            this.tabPageStudents.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageStudents.Size = new System.Drawing.Size(1149, 820);
            this.tabPageStudents.TabIndex = 7;
            this.tabPageStudents.Text = "Ученики";
            this.tabPageStudents.UseVisualStyleBackColor = true;
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
            this.tabControl3.Size = new System.Drawing.Size(1158, 806);
            this.tabControl3.TabIndex = 5;
            // 
            // tabPage9
            // 
            this.tabPage9.Controls.Add(this.dataGridViewStudents);
            this.tabPage9.Location = new System.Drawing.Point(4, 26);
            this.tabPage9.Name = "tabPage9";
            this.tabPage9.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage9.Size = new System.Drawing.Size(1150, 776);
            this.tabPage9.TabIndex = 0;
            this.tabPage9.Text = "Класс";
            this.tabPage9.UseVisualStyleBackColor = true;
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
            this.dataGridViewStudents.Size = new System.Drawing.Size(1124, 764);
            this.dataGridViewStudents.TabIndex = 2;
            // 
            // tabPageAttendance
            // 
            this.tabPageAttendance.Controls.Add(this.tabControlAttendance);
            this.tabPageAttendance.Location = new System.Drawing.Point(52, 4);
            this.tabPageAttendance.Name = "tabPageAttendance";
            this.tabPageAttendance.Size = new System.Drawing.Size(1149, 820);
            this.tabPageAttendance.TabIndex = 8;
            this.tabPageAttendance.Text = "Посещаемость";
            this.tabPageAttendance.UseVisualStyleBackColor = true;
            // 
            // tabControlAttendance
            // 
            this.tabControlAttendance.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlAttendance.Controls.Add(this.tabPage8);
            this.tabControlAttendance.Controls.Add(this.tabPage7);
            this.tabControlAttendance.Location = new System.Drawing.Point(3, 3);
            this.tabControlAttendance.Name = "tabControlAttendance";
            this.tabControlAttendance.SelectedIndex = 0;
            this.tabControlAttendance.Size = new System.Drawing.Size(1143, 814);
            this.tabControlAttendance.TabIndex = 0;
            // 
            // tabPage8
            // 
            this.tabPage8.Controls.Add(this.dataGridViewPersonalAttendance);
            this.tabPage8.Location = new System.Drawing.Point(4, 26);
            this.tabPage8.Name = "tabPage8";
            this.tabPage8.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage8.Size = new System.Drawing.Size(1135, 784);
            this.tabPage8.TabIndex = 1;
            this.tabPage8.Text = "Личное";
            this.tabPage8.UseVisualStyleBackColor = true;
            // 
            // dataGridViewPersonalAttendance
            // 
            this.dataGridViewPersonalAttendance.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewPersonalAttendance.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewPersonalAttendance.Location = new System.Drawing.Point(6, 6);
            this.dataGridViewPersonalAttendance.Name = "dataGridViewPersonalAttendance";
            this.dataGridViewPersonalAttendance.Size = new System.Drawing.Size(1123, 772);
            this.dataGridViewPersonalAttendance.TabIndex = 0;
            // 
            // tabPage7
            // 
            this.tabPage7.Controls.Add(this.dataGridViewClassAtterdance);
            this.tabPage7.Location = new System.Drawing.Point(4, 26);
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.Size = new System.Drawing.Size(1135, 784);
            this.tabPage7.TabIndex = 2;
            this.tabPage7.Text = "Класс";
            this.tabPage7.UseVisualStyleBackColor = true;
            // 
            // dataGridViewClassAtterdance
            // 
            this.dataGridViewClassAtterdance.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewClassAtterdance.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewClassAtterdance.Location = new System.Drawing.Point(6, 3);
            this.dataGridViewClassAtterdance.Name = "dataGridViewClassAtterdance";
            this.dataGridViewClassAtterdance.Size = new System.Drawing.Size(1123, 779);
            this.dataGridViewClassAtterdance.TabIndex = 1;
            // 
            // tabPageReports
            // 
            this.tabPageReports.Controls.Add(this.label1);
            this.tabPageReports.Controls.Add(this.textBox1);
            this.tabPageReports.Controls.Add(this.panel2);
            this.tabPageReports.Controls.Add(this.comboBox1);
            this.tabPageReports.Location = new System.Drawing.Point(52, 4);
            this.tabPageReports.Name = "tabPageReports";
            this.tabPageReports.Size = new System.Drawing.Size(1149, 820);
            this.tabPageReports.TabIndex = 9;
            this.tabPageReports.Text = "Отчеты";
            this.tabPageReports.UseVisualStyleBackColor = true;
            // 
            // printDocumentDialog1
            // 
            this.printDocumentDialog1.UseEXDialog = true;
            // 
            // printDocumentAtterdance
            // 
            this.printDocumentAtterdance.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocumentAtterdance_PrintPage);
            // 
            // buttonPrint
            // 
            this.buttonPrint.Location = new System.Drawing.Point(1000, 9);
            this.buttonPrint.Name = "buttonPrint";
            this.buttonPrint.Size = new System.Drawing.Size(93, 30);
            this.buttonPrint.TabIndex = 2;
            this.buttonPrint.Text = "Печать";
            this.buttonPrint.UseVisualStyleBackColor = true;
            this.buttonPrint.Click += new System.EventHandler(this.buttonPrint_Click);
            // 
            // printDialog1
            // 
            this.printDialog1.UseEXDialog = true;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(601, 3);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(539, 25);
            this.comboBox1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Location = new System.Drawing.Point(3, 34);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1137, 778);
            this.panel2.TabIndex = 1;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(119, 3);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(476, 25);
            this.textBox1.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 19);
            this.label1.TabIndex = 3;
            this.label1.Text = "Путь к шаблону:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1205, 880);
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
            this.homeworkTabControl.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewHomework)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.datagridviewHomeworkAll)).EndInit();
            this.tabGrades.ResumeLayout(false);
            this.panelGrades.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewGrades)).EndInit();
            this.tabPage6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewGradesYear)).EndInit();
            this.tabPage5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewGradesAll)).EndInit();
            this.tabShedule.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.sheduleTabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sheduleGridView)).EndInit();
            this.tabStatistics.ResumeLayout(false);
            this.panelStatistics.ResumeLayout(false);
            this.tabControlStatistic.ResumeLayout(false);
            this.tabPagePersonal.ResumeLayout(false);
            this.tabPagePersonal.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPersonalStatistics)).EndInit();
            this.tabPageClass.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewStatisticsClass2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewClassStatistics1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewClassStatistics)).EndInit();
            this.tabPageEvents.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewEvents)).EndInit();
            this.tabPageSubjects.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSubjects)).EndInit();
            this.tabPageTeachers.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTeachers)).EndInit();
            this.tabPageStudents.ResumeLayout(false);
            this.tabControl3.ResumeLayout(false);
            this.tabPage9.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewStudents)).EndInit();
            this.tabPageAttendance.ResumeLayout(false);
            this.tabControlAttendance.ResumeLayout(false);
            this.tabPage8.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPersonalAttendance)).EndInit();
            this.tabPage7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewClassAtterdance)).EndInit();
            this.tabPageReports.ResumeLayout(false);
            this.tabPageReports.PerformLayout();
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
        private System.Windows.Forms.TabControl tabControlStatistic;
        private System.Windows.Forms.TabPage tabPagePersonal;
        private System.Windows.Forms.Label labelStatisticsSummary;
        private System.Windows.Forms.DataGridView dataGridViewPersonalStatistics;
        private System.Windows.Forms.TabPage tabPageClass;
        private System.Windows.Forms.DataGridView dataGridViewClassStatistics;
        private System.Windows.Forms.DataGridView dataGridViewEvents;
        private System.Windows.Forms.DataGridView dataGridViewSubjects;
        private System.Windows.Forms.DataGridView dataGridViewTeachers;
        private System.Windows.Forms.DataGridView dataGridViewStudents;
        private System.Windows.Forms.TabControl tabControl3;
        private System.Windows.Forms.TabPage tabPage9;
        private System.Windows.Forms.TabPage tabPageAttendance;
        private System.Windows.Forms.TabPage tabPageReports;
        private System.Windows.Forms.DateTimePicker dateTimePickerPersonalStatisticsBefore;
        private System.Windows.Forms.DateTimePicker dateTimePickerPersonalStatisticsAfter;
        private System.Windows.Forms.DataGridView dataGridViewClassStatistics1;
        private System.Windows.Forms.DataGridView dataGridViewStatisticsClass2;
        private System.Windows.Forms.Button exitBtnm;
        private System.Windows.Forms.TabControl tabControlAttendance;
        private System.Windows.Forms.TabPage tabPage8;
        private System.Windows.Forms.DataGridView dataGridViewPersonalAttendance;
        private System.Drawing.Printing.PrintDocument printDocument1;
        private System.Windows.Forms.PrintDialog printDocumentDialog1;
        private System.Windows.Forms.TabPage tabPage7;
        private System.Windows.Forms.DataGridView dataGridViewClassAtterdance;
        private System.Drawing.Printing.PrintDocument printDocumentAtterdance;
        private System.Windows.Forms.Button buttonPrint;
        private System.Windows.Forms.PrintDialog printDialog1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ComboBox comboBox1;
    }
}

