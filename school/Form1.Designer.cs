using System;
using System.Drawing;
using System.Windows.Forms;

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
            this.directorComboBox = new System.Windows.Forms.ComboBox();
            this.textBoxSearch = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.buttonPrint = new System.Windows.Forms.Button();
            this.exitBtnm = new System.Windows.Forms.Button();
            this.labelRole = new System.Windows.Forms.Label();
            this.panelMain = new System.Windows.Forms.Panel();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabHomework = new System.Windows.Forms.TabPage();
            this.panelHomework = new System.Windows.Forms.Panel();
            this.homeworkTabControl = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.dateTimePickerHomework = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerHomework1 = new System.Windows.Forms.DateTimePicker();
            this.buttonHomeworkPresentation = new System.Windows.Forms.Button();
            this.labelHomeworkPeriod = new System.Windows.Forms.Label();
            this.dataGridViewHomework = new System.Windows.Forms.DataGridView();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.labelHomeworkAll = new System.Windows.Forms.Label();
            this.datagridviewHomeworkAll = new System.Windows.Forms.DataGridView();
            this.tabGrades = new System.Windows.Forms.TabPage();
            this.panelGrades = new System.Windows.Forms.Panel();
            this.dateTimePickerGrades1 = new System.Windows.Forms.DateTimePicker();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.labelGradesPeriod = new System.Windows.Forms.Label();
            this.dataGridViewGrades = new System.Windows.Forms.DataGridView();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.dataGridViewGradesAll = new System.Windows.Forms.DataGridView();
            this.tabPage10 = new System.Windows.Forms.TabPage();
            this.comboBoxQuaterGradesStudent = new System.Windows.Forms.ComboBox();
            this.buttonCalculateQuaterGrades = new System.Windows.Forms.Button();
            this.dataGridViewQuaterGrades = new System.Windows.Forms.DataGridView();
            this.SubjectName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Quarter1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Quarter2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Quarter3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Quarter4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dateTimePickerGrades = new System.Windows.Forms.DateTimePicker();
            this.tabShedule = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.sheduleTabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.sheduleGridView = new System.Windows.Forms.DataGridView();
            this.tabPageSheduleAll = new System.Windows.Forms.TabPage();
            this.dataGridViewSheduleAll = new System.Windows.Forms.DataGridView();
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
            this.buttonCreateSheduleFromSubjects = new System.Windows.Forms.Button();
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
            this.label2 = new System.Windows.Forms.Label();
            this.dateTimePickerAttendanceEnd = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerAttendanceStart = new System.Windows.Forms.DateTimePicker();
            this.dataGridViewClassAtterdance = new System.Windows.Forms.DataGridView();
            this.tabPageClasses = new System.Windows.Forms.TabPage();
            this.dataGridViewClasses = new System.Windows.Forms.DataGridView();
            this.tabPageReports = new System.Windows.Forms.TabPage();
            this.buttonReportsToWord = new System.Windows.Forms.Button();
            this.dateTimePickerGradesReports1 = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerGradesReports2 = new System.Windows.Forms.DateTimePicker();
            this.buttonExcelReport = new System.Windows.Forms.Button();
            this.tabControlReports = new System.Windows.Forms.TabControl();
            this.tabPageGradesReports = new System.Windows.Forms.TabPage();
            this.dataGridViewGradesReports = new System.Windows.Forms.DataGridView();
            this.tabPageAtterdanceReports = new System.Windows.Forms.TabPage();
            this.dataGridViewAtterdanceReports = new System.Windows.Forms.DataGridView();
            this.tabPageRegisters = new System.Windows.Forms.TabPage();
            this.button4 = new System.Windows.Forms.Button();
            this.dataGridViewRegisters = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumnApplicationID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumnFullName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumnPermissionID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumnClassID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumnAge = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumnTelephone = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumnApplicationDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumnIsApproved = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.printDocumentDialog1 = new System.Windows.Forms.PrintDialog();
            this.printDocumentAtterdance = new System.Drawing.Printing.PrintDocument();
            this.printDialog1 = new System.Windows.Forms.PrintDialog();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn13 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn14 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn15 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn16 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn17 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn18 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelTop.SuspendLayout();
            this.panelMain.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabHomework.SuspendLayout();
            this.panelHomework.SuspendLayout();
            this.homeworkTabControl.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewHomework)).BeginInit();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.datagridviewHomeworkAll)).BeginInit();
            this.tabGrades.SuspendLayout();
            this.panelGrades.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewGrades)).BeginInit();
            this.tabPage5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewGradesAll)).BeginInit();
            this.tabPage10.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewQuaterGrades)).BeginInit();
            this.tabShedule.SuspendLayout();
            this.panel1.SuspendLayout();
            this.sheduleTabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sheduleGridView)).BeginInit();
            this.tabPageSheduleAll.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSheduleAll)).BeginInit();
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
            this.tabPageClasses.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewClasses)).BeginInit();
            this.tabPageReports.SuspendLayout();
            this.tabControlReports.SuspendLayout();
            this.tabPageGradesReports.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewGradesReports)).BeginInit();
            this.tabPageAtterdanceReports.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAtterdanceReports)).BeginInit();
            this.tabPageRegisters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRegisters)).BeginInit();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.Color.LightSteelBlue;
            this.panelTop.Controls.Add(this.directorComboBox);
            this.panelTop.Controls.Add(this.textBoxSearch);
            this.panelTop.Controls.Add(this.button3);
            this.panelTop.Controls.Add(this.button2);
            this.panelTop.Controls.Add(this.button1);
            this.panelTop.Controls.Add(this.buttonPrint);
            this.panelTop.Controls.Add(this.exitBtnm);
            this.panelTop.Controls.Add(this.labelRole);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(1205, 71);
            this.panelTop.TabIndex = 0;
            // 
            // directorComboBox
            // 
            this.directorComboBox.FormattingEnabled = true;
            this.directorComboBox.Location = new System.Drawing.Point(12, 19);
            this.directorComboBox.Name = "directorComboBox";
            this.directorComboBox.Size = new System.Drawing.Size(163, 21);
            this.directorComboBox.TabIndex = 7;
            this.directorComboBox.SelectedIndexChanged += new System.EventHandler(this.directorComboBox_SelectedIndexChanged);
            // 
            // textBoxSearch
            // 
            this.textBoxSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxSearch.Location = new System.Drawing.Point(12, 45);
            this.textBoxSearch.Name = "textBoxSearch";
            this.textBoxSearch.Size = new System.Drawing.Size(1180, 20);
            this.textBoxSearch.TabIndex = 6;
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button3.Location = new System.Drawing.Point(901, 9);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(93, 30);
            this.button3.TabIndex = 5;
            this.button3.Text = "Превью";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.buttonPreview_Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(827, 10);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(31, 30);
            this.button2.TabIndex = 4;
            this.button2.Text = "+";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.addButton_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(864, 10);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(31, 30);
            this.button1.TabIndex = 3;
            this.button1.Text = "-";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.deleteButton_Click);
            // 
            // buttonPrint
            // 
            this.buttonPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonPrint.Location = new System.Drawing.Point(1000, 9);
            this.buttonPrint.Name = "buttonPrint";
            this.buttonPrint.Size = new System.Drawing.Size(93, 30);
            this.buttonPrint.TabIndex = 2;
            this.buttonPrint.Text = "Печать";
            this.buttonPrint.UseVisualStyleBackColor = true;
            this.buttonPrint.Click += new System.EventHandler(this.buttonPrint_Click);
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
            this.panelMain.Location = new System.Drawing.Point(0, 71);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(1205, 809);
            this.panelMain.TabIndex = 1;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabHomework);
            this.tabControl.Controls.Add(this.tabGrades);
            this.tabControl.Controls.Add(this.tabShedule);
            this.tabControl.Controls.Add(this.tabStatistics);
            this.tabControl.Controls.Add(this.tabPageEvents);
            this.tabControl.Controls.Add(this.tabPageSubjects);
            this.tabControl.Controls.Add(this.tabPageTeachers);
            this.tabControl.Controls.Add(this.tabPageStudents);
            this.tabControl.Controls.Add(this.tabPageAttendance);
            this.tabControl.Controls.Add(this.tabPageClasses);
            this.tabControl.Controls.Add(this.tabPageReports);
            this.tabControl.Controls.Add(this.tabPageRegisters);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Multiline = true;
            this.tabControl.Name = "tabControl";
            this.tabControl.Padding = new System.Drawing.Point(20, 5);
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1205, 809);
            this.tabControl.TabIndex = 0;
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
            // 
            // tabHomework
            // 
            this.tabHomework.Controls.Add(this.panelHomework);
            this.tabHomework.Location = new System.Drawing.Point(4, 56);
            this.tabHomework.Name = "tabHomework";
            this.tabHomework.Padding = new System.Windows.Forms.Padding(9);
            this.tabHomework.Size = new System.Drawing.Size(1197, 749);
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
            this.panelHomework.Size = new System.Drawing.Size(1179, 731);
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
            this.homeworkTabControl.Size = new System.Drawing.Size(1164, 725);
            this.homeworkTabControl.TabIndex = 3;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.tableLayoutPanel1);
            this.tabPage2.Controls.Add(this.buttonHomeworkPresentation);
            this.tabPage2.Controls.Add(this.labelHomeworkPeriod);
            this.tabPage2.Controls.Add(this.dataGridViewHomework);
            this.tabPage2.Location = new System.Drawing.Point(4, 26);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1156, 695);
            this.tabPage2.TabIndex = 0;
            this.tabPage2.Text = "Период";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.dateTimePickerHomework, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.dateTimePickerHomework1, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(6, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1144, 31);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // dateTimePickerHomework
            // 
            this.dateTimePickerHomework.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePickerHomework.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePickerHomework.Location = new System.Drawing.Point(3, 3);
            this.dateTimePickerHomework.Name = "dateTimePickerHomework";
            this.dateTimePickerHomework.Size = new System.Drawing.Size(566, 25);
            this.dateTimePickerHomework.TabIndex = 0;
            this.dateTimePickerHomework.Value = new System.DateTime(2025, 12, 7, 14, 53, 7, 265);
            this.dateTimePickerHomework.ValueChanged += new System.EventHandler(this.dateTimePickerHomework_ValueChanged_1);
            // 
            // dateTimePickerHomework1
            // 
            this.dateTimePickerHomework1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePickerHomework1.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePickerHomework1.Location = new System.Drawing.Point(575, 3);
            this.dateTimePickerHomework1.Name = "dateTimePickerHomework1";
            this.dateTimePickerHomework1.Size = new System.Drawing.Size(566, 25);
            this.dateTimePickerHomework1.TabIndex = 3;
            this.dateTimePickerHomework1.Value = new System.DateTime(2025, 12, 7, 14, 53, 7, 265);
            this.dateTimePickerHomework1.ValueChanged += new System.EventHandler(this.dateTimePickerHomework1_ValueChanged);
            // 
            // buttonHomeworkPresentation
            // 
            this.buttonHomeworkPresentation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonHomeworkPresentation.Location = new System.Drawing.Point(6, 648);
            this.buttonHomeworkPresentation.Name = "buttonHomeworkPresentation";
            this.buttonHomeworkPresentation.Size = new System.Drawing.Size(110, 33);
            this.buttonHomeworkPresentation.TabIndex = 4;
            this.buttonHomeworkPresentation.Text = "Презентация";
            this.buttonHomeworkPresentation.UseVisualStyleBackColor = true;
            this.buttonHomeworkPresentation.Click += new System.EventHandler(this.buttonHomeworkPresentation_Click);
            // 
            // labelHomeworkPeriod
            // 
            this.labelHomeworkPeriod.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.labelHomeworkPeriod.AutoSize = true;
            this.labelHomeworkPeriod.Location = new System.Drawing.Point(521, 656);
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
            this.dataGridViewHomework.Size = new System.Drawing.Size(1144, 605);
            this.dataGridViewHomework.TabIndex = 1;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.labelHomeworkAll);
            this.tabPage3.Controls.Add(this.datagridviewHomeworkAll);
            this.tabPage3.Location = new System.Drawing.Point(4, 26);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(1156, 695);
            this.tabPage3.TabIndex = 1;
            this.tabPage3.Text = "Все";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // labelHomeworkAll
            // 
            this.labelHomeworkAll.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelHomeworkAll.AutoSize = true;
            this.labelHomeworkAll.Location = new System.Drawing.Point(347, 766);
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
            this.datagridviewHomeworkAll.Size = new System.Drawing.Size(1148, 760);
            this.datagridviewHomeworkAll.TabIndex = 2;
            // 
            // tabGrades
            // 
            this.tabGrades.Controls.Add(this.panelGrades);
            this.tabGrades.Location = new System.Drawing.Point(4, 30);
            this.tabGrades.Name = "tabGrades";
            this.tabGrades.Padding = new System.Windows.Forms.Padding(9);
            this.tabGrades.Size = new System.Drawing.Size(1197, 775);
            this.tabGrades.TabIndex = 1;
            this.tabGrades.Text = "Оценки";
            this.tabGrades.UseVisualStyleBackColor = true;
            // 
            // panelGrades
            // 
            this.panelGrades.Controls.Add(this.dateTimePickerGrades1);
            this.panelGrades.Controls.Add(this.tabControl1);
            this.panelGrades.Controls.Add(this.dateTimePickerGrades);
            this.panelGrades.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelGrades.Location = new System.Drawing.Point(9, 9);
            this.panelGrades.Name = "panelGrades";
            this.panelGrades.Padding = new System.Windows.Forms.Padding(9);
            this.panelGrades.Size = new System.Drawing.Size(1179, 757);
            this.panelGrades.TabIndex = 0;
            // 
            // dateTimePickerGrades1
            // 
            this.dateTimePickerGrades1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.dateTimePickerGrades1.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePickerGrades1.Location = new System.Drawing.Point(557, 12);
            this.dateTimePickerGrades1.Name = "dateTimePickerGrades1";
            this.dateTimePickerGrades1.Size = new System.Drawing.Size(606, 25);
            this.dateTimePickerGrades1.TabIndex = 3;
            this.dateTimePickerGrades1.Value = new System.DateTime(2025, 12, 7, 14, 53, 7, 277);
            this.dateTimePickerGrades1.ValueChanged += new System.EventHandler(this.dateTimePickerGrades1_ValueChanged);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Controls.Add(this.tabPage10);
            this.tabControl1.Location = new System.Drawing.Point(3, 43);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1164, 685);
            this.tabControl1.TabIndex = 3;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.labelGradesPeriod);
            this.tabPage4.Controls.Add(this.dataGridViewGrades);
            this.tabPage4.Location = new System.Drawing.Point(4, 26);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(1156, 655);
            this.tabPage4.TabIndex = 0;
            this.tabPage4.Text = "Период";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // labelGradesPeriod
            // 
            this.labelGradesPeriod.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.labelGradesPeriod.AutoSize = true;
            this.labelGradesPeriod.Location = new System.Drawing.Point(558, 625);
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
            this.dataGridViewGrades.Location = new System.Drawing.Point(6, 6);
            this.dataGridViewGrades.Name = "dataGridViewGrades";
            this.dataGridViewGrades.ReadOnly = true;
            this.dataGridViewGrades.RowHeadersVisible = false;
            this.dataGridViewGrades.RowTemplate.Height = 25;
            this.dataGridViewGrades.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewGrades.Size = new System.Drawing.Size(1144, 616);
            this.dataGridViewGrades.TabIndex = 1;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.dataGridViewGradesAll);
            this.tabPage5.Location = new System.Drawing.Point(4, 26);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(1156, 655);
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
            this.dataGridViewGradesAll.Size = new System.Drawing.Size(1144, 709);
            this.dataGridViewGradesAll.TabIndex = 2;
            // 
            // tabPage10
            // 
            this.tabPage10.Controls.Add(this.comboBoxQuaterGradesStudent);
            this.tabPage10.Controls.Add(this.buttonCalculateQuaterGrades);
            this.tabPage10.Controls.Add(this.dataGridViewQuaterGrades);
            this.tabPage10.Location = new System.Drawing.Point(4, 26);
            this.tabPage10.Name = "tabPage10";
            this.tabPage10.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage10.Size = new System.Drawing.Size(1156, 655);
            this.tabPage10.TabIndex = 2;
            this.tabPage10.Text = "Итоговые";
            this.tabPage10.UseVisualStyleBackColor = true;
            // 
            // comboBoxQuaterGradesStudent
            // 
            this.comboBoxQuaterGradesStudent.FormattingEnabled = true;
            this.comboBoxQuaterGradesStudent.Location = new System.Drawing.Point(6, 6);
            this.comboBoxQuaterGradesStudent.Name = "comboBoxQuaterGradesStudent";
            this.comboBoxQuaterGradesStudent.Size = new System.Drawing.Size(1144, 25);
            this.comboBoxQuaterGradesStudent.TabIndex = 4;
            this.comboBoxQuaterGradesStudent.SelectedIndexChanged += new System.EventHandler(this.comboBoxQuaterGradesStudent_SelectedIndexChanged);
            // 
            // buttonCalculateQuaterGrades
            // 
            this.buttonCalculateQuaterGrades.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCalculateQuaterGrades.Location = new System.Drawing.Point(6, 601);
            this.buttonCalculateQuaterGrades.Name = "buttonCalculateQuaterGrades";
            this.buttonCalculateQuaterGrades.Size = new System.Drawing.Size(1144, 37);
            this.buttonCalculateQuaterGrades.TabIndex = 3;
            this.buttonCalculateQuaterGrades.Text = "Сформировать оценки за четверти автоматически";
            this.buttonCalculateQuaterGrades.UseVisualStyleBackColor = true;
            this.buttonCalculateQuaterGrades.Click += new System.EventHandler(this.buttonCalculateQuaterGrades_Click);
            // 
            // dataGridViewQuaterGrades
            // 
            this.dataGridViewQuaterGrades.AllowUserToAddRows = false;
            this.dataGridViewQuaterGrades.AllowUserToDeleteRows = false;
            this.dataGridViewQuaterGrades.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewQuaterGrades.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewQuaterGrades.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
            this.dataGridViewQuaterGrades.ColumnHeadersHeight = 30;
            this.dataGridViewQuaterGrades.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SubjectName,
            this.Quarter1,
            this.Quarter2,
            this.Quarter3,
            this.Quarter4});
            this.dataGridViewQuaterGrades.Location = new System.Drawing.Point(6, 39);
            this.dataGridViewQuaterGrades.Name = "dataGridViewQuaterGrades";
            this.dataGridViewQuaterGrades.ReadOnly = true;
            this.dataGridViewQuaterGrades.RowHeadersVisible = false;
            this.dataGridViewQuaterGrades.RowTemplate.Height = 25;
            this.dataGridViewQuaterGrades.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewQuaterGrades.Size = new System.Drawing.Size(1144, 560);
            this.dataGridViewQuaterGrades.TabIndex = 2;
            // 
            // SubjectName
            // 
            this.SubjectName.HeaderText = "Предмет";
            this.SubjectName.Name = "SubjectName";
            this.SubjectName.ReadOnly = true;
            // 
            // Quarter1
            // 
            this.Quarter1.HeaderText = "1-я четверть";
            this.Quarter1.Name = "Quarter1";
            this.Quarter1.ReadOnly = true;
            // 
            // Quarter2
            // 
            this.Quarter2.HeaderText = "2-я четверть";
            this.Quarter2.Name = "Quarter2";
            this.Quarter2.ReadOnly = true;
            // 
            // Quarter3
            // 
            this.Quarter3.HeaderText = "3-я четверть";
            this.Quarter3.Name = "Quarter3";
            this.Quarter3.ReadOnly = true;
            // 
            // Quarter4
            // 
            this.Quarter4.HeaderText = "4-я четверть";
            this.Quarter4.Name = "Quarter4";
            this.Quarter4.ReadOnly = true;
            // 
            // dateTimePickerGrades
            // 
            this.dateTimePickerGrades.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.dateTimePickerGrades.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePickerGrades.Location = new System.Drawing.Point(7, 12);
            this.dateTimePickerGrades.Name = "dateTimePickerGrades";
            this.dateTimePickerGrades.Size = new System.Drawing.Size(544, 25);
            this.dateTimePickerGrades.TabIndex = 0;
            this.dateTimePickerGrades.Value = new System.DateTime(2025, 12, 7, 14, 53, 7, 277);
            this.dateTimePickerGrades.ValueChanged += new System.EventHandler(this.dateTimePickerGrades_ValueChanged_1);
            // 
            // tabShedule
            // 
            this.tabShedule.Controls.Add(this.panel1);
            this.tabShedule.Location = new System.Drawing.Point(4, 30);
            this.tabShedule.Name = "tabShedule";
            this.tabShedule.Size = new System.Drawing.Size(1197, 775);
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
            this.panel1.Size = new System.Drawing.Size(1197, 775);
            this.panel1.TabIndex = 1;
            // 
            // sheduleTabControl
            // 
            this.sheduleTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sheduleTabControl.Controls.Add(this.tabPage1);
            this.sheduleTabControl.Controls.Add(this.tabPageSheduleAll);
            this.sheduleTabControl.Location = new System.Drawing.Point(12, 12);
            this.sheduleTabControl.Name = "sheduleTabControl";
            this.sheduleTabControl.SelectedIndex = 0;
            this.sheduleTabControl.Size = new System.Drawing.Size(1173, 700);
            this.sheduleTabControl.TabIndex = 4;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.sheduleGridView);
            this.tabPage1.Location = new System.Drawing.Point(4, 26);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1165, 670);
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
            this.sheduleGridView.Size = new System.Drawing.Size(1153, 650);
            this.sheduleGridView.TabIndex = 3;
            // 
            // tabPageSheduleAll
            // 
            this.tabPageSheduleAll.Controls.Add(this.dataGridViewSheduleAll);
            this.tabPageSheduleAll.Location = new System.Drawing.Point(4, 26);
            this.tabPageSheduleAll.Name = "tabPageSheduleAll";
            this.tabPageSheduleAll.Size = new System.Drawing.Size(1165, 670);
            this.tabPageSheduleAll.TabIndex = 1;
            this.tabPageSheduleAll.Text = "Все";
            this.tabPageSheduleAll.UseVisualStyleBackColor = true;
            // 
            // dataGridViewSheduleAll
            // 
            this.dataGridViewSheduleAll.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewSheduleAll.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewSheduleAll.Location = new System.Drawing.Point(6, 6);
            this.dataGridViewSheduleAll.Name = "dataGridViewSheduleAll";
            this.dataGridViewSheduleAll.RowHeadersVisible = false;
            this.dataGridViewSheduleAll.RowTemplate.Height = 25;
            this.dataGridViewSheduleAll.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridViewSheduleAll.Size = new System.Drawing.Size(1153, 710);
            this.dataGridViewSheduleAll.TabIndex = 4;
            // 
            // sheduleLabel
            // 
            this.sheduleLabel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.sheduleLabel.AutoSize = true;
            this.sheduleLabel.Location = new System.Drawing.Point(573, 721);
            this.sheduleLabel.Name = "sheduleLabel";
            this.sheduleLabel.Size = new System.Drawing.Size(45, 19);
            this.sheduleLabel.TabIndex = 2;
            this.sheduleLabel.Text = "label1";
            // 
            // tabStatistics
            // 
            this.tabStatistics.Controls.Add(this.panelStatistics);
            this.tabStatistics.Location = new System.Drawing.Point(4, 30);
            this.tabStatistics.Name = "tabStatistics";
            this.tabStatistics.Padding = new System.Windows.Forms.Padding(9);
            this.tabStatistics.Size = new System.Drawing.Size(1197, 775);
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
            this.panelStatistics.Size = new System.Drawing.Size(1179, 757);
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
            this.tabControlStatistic.Size = new System.Drawing.Size(1164, 725);
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
            this.tabPagePersonal.Size = new System.Drawing.Size(1156, 695);
            this.tabPagePersonal.TabIndex = 0;
            this.tabPagePersonal.Text = "Личное";
            this.tabPagePersonal.UseVisualStyleBackColor = true;
            // 
            // dateTimePickerPersonalStatisticsBefore
            // 
            this.dateTimePickerPersonalStatisticsBefore.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.dateTimePickerPersonalStatisticsBefore.Location = new System.Drawing.Point(6, 8);
            this.dateTimePickerPersonalStatisticsBefore.Name = "dateTimePickerPersonalStatisticsBefore";
            this.dateTimePickerPersonalStatisticsBefore.Size = new System.Drawing.Size(560, 25);
            this.dateTimePickerPersonalStatisticsBefore.TabIndex = 4;
            // 
            // dateTimePickerPersonalStatisticsAfter
            // 
            this.dateTimePickerPersonalStatisticsAfter.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.dateTimePickerPersonalStatisticsAfter.Location = new System.Drawing.Point(572, 8);
            this.dateTimePickerPersonalStatisticsAfter.Name = "dateTimePickerPersonalStatisticsAfter";
            this.dateTimePickerPersonalStatisticsAfter.Size = new System.Drawing.Size(540, 25);
            this.dateTimePickerPersonalStatisticsAfter.TabIndex = 3;
            // 
            // labelStatisticsSummary
            // 
            this.labelStatisticsSummary.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.labelStatisticsSummary.AutoSize = true;
            this.labelStatisticsSummary.Location = new System.Drawing.Point(562, 651);
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
            this.dataGridViewPersonalStatistics.Size = new System.Drawing.Size(1144, 609);
            this.dataGridViewPersonalStatistics.TabIndex = 1;
            // 
            // tabPageClass
            // 
            this.tabPageClass.Controls.Add(this.dataGridViewStatisticsClass2);
            this.tabPageClass.Controls.Add(this.dataGridViewClassStatistics1);
            this.tabPageClass.Controls.Add(this.dataGridViewClassStatistics);
            this.tabPageClass.Location = new System.Drawing.Point(4, 26);
            this.tabPageClass.Name = "tabPageClass";
            this.tabPageClass.Size = new System.Drawing.Size(1156, 695);
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
            this.dataGridViewStatisticsClass2.Location = new System.Drawing.Point(5, 389);
            this.dataGridViewStatisticsClass2.Name = "dataGridViewStatisticsClass2";
            this.dataGridViewStatisticsClass2.ReadOnly = true;
            this.dataGridViewStatisticsClass2.RowHeadersVisible = false;
            this.dataGridViewStatisticsClass2.RowTemplate.Height = 25;
            this.dataGridViewStatisticsClass2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewStatisticsClass2.Size = new System.Drawing.Size(1148, 357);
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
            this.dataGridViewClassStatistics1.Location = new System.Drawing.Point(571, 5);
            this.dataGridViewClassStatistics1.Name = "dataGridViewClassStatistics1";
            this.dataGridViewClassStatistics1.ReadOnly = true;
            this.dataGridViewClassStatistics1.RowHeadersVisible = false;
            this.dataGridViewClassStatistics1.RowTemplate.Height = 25;
            this.dataGridViewClassStatistics1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewClassStatistics1.Size = new System.Drawing.Size(582, 378);
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
            this.dataGridViewClassStatistics.Size = new System.Drawing.Size(562, 378);
            this.dataGridViewClassStatistics.TabIndex = 2;
            // 
            // tabPageEvents
            // 
            this.tabPageEvents.Controls.Add(this.dataGridViewEvents);
            this.tabPageEvents.Location = new System.Drawing.Point(4, 30);
            this.tabPageEvents.Name = "tabPageEvents";
            this.tabPageEvents.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageEvents.Size = new System.Drawing.Size(1197, 775);
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
            this.dataGridViewEvents.Size = new System.Drawing.Size(1165, 838);
            this.dataGridViewEvents.TabIndex = 2;
            // 
            // tabPageSubjects
            // 
            this.tabPageSubjects.Controls.Add(this.buttonCreateSheduleFromSubjects);
            this.tabPageSubjects.Controls.Add(this.dataGridViewSubjects);
            this.tabPageSubjects.Location = new System.Drawing.Point(4, 30);
            this.tabPageSubjects.Name = "tabPageSubjects";
            this.tabPageSubjects.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSubjects.Size = new System.Drawing.Size(1197, 775);
            this.tabPageSubjects.TabIndex = 5;
            this.tabPageSubjects.Text = "Предметы";
            this.tabPageSubjects.UseVisualStyleBackColor = true;
            // 
            // buttonCreateSheduleFromSubjects
            // 
            this.buttonCreateSheduleFromSubjects.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCreateSheduleFromSubjects.Location = new System.Drawing.Point(8, 759);
            this.buttonCreateSheduleFromSubjects.Name = "buttonCreateSheduleFromSubjects";
            this.buttonCreateSheduleFromSubjects.Size = new System.Drawing.Size(1175, 34);
            this.buttonCreateSheduleFromSubjects.TabIndex = 3;
            this.buttonCreateSheduleFromSubjects.Text = "Сформировать расписание автоматически";
            this.buttonCreateSheduleFromSubjects.UseVisualStyleBackColor = true;
            this.buttonCreateSheduleFromSubjects.Click += new System.EventHandler(this.buttonCreateSheduleFromSubjects_Click);
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
            this.dataGridViewSubjects.Size = new System.Drawing.Size(1182, 747);
            this.dataGridViewSubjects.TabIndex = 2;
            // 
            // tabPageTeachers
            // 
            this.tabPageTeachers.Controls.Add(this.dataGridViewTeachers);
            this.tabPageTeachers.Location = new System.Drawing.Point(4, 30);
            this.tabPageTeachers.Name = "tabPageTeachers";
            this.tabPageTeachers.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTeachers.Size = new System.Drawing.Size(1197, 775);
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
            this.dataGridViewTeachers.Size = new System.Drawing.Size(1185, 858);
            this.dataGridViewTeachers.TabIndex = 2;
            // 
            // tabPageStudents
            // 
            this.tabPageStudents.Controls.Add(this.tabControl3);
            this.tabPageStudents.Location = new System.Drawing.Point(4, 56);
            this.tabPageStudents.Name = "tabPageStudents";
            this.tabPageStudents.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageStudents.Size = new System.Drawing.Size(1197, 749);
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
            this.tabControl3.Size = new System.Drawing.Size(1182, 737);
            this.tabControl3.TabIndex = 5;
            // 
            // tabPage9
            // 
            this.tabPage9.Controls.Add(this.dataGridViewStudents);
            this.tabPage9.Location = new System.Drawing.Point(4, 26);
            this.tabPage9.Name = "tabPage9";
            this.tabPage9.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage9.Size = new System.Drawing.Size(1174, 707);
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
            this.dataGridViewStudents.Location = new System.Drawing.Point(12, 6);
            this.dataGridViewStudents.Name = "dataGridViewStudents";
            this.dataGridViewStudents.ReadOnly = true;
            this.dataGridViewStudents.RowHeadersVisible = false;
            this.dataGridViewStudents.RowTemplate.Height = 25;
            this.dataGridViewStudents.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewStudents.Size = new System.Drawing.Size(1156, 695);
            this.dataGridViewStudents.TabIndex = 2;
            // 
            // tabPageAttendance
            // 
            this.tabPageAttendance.Controls.Add(this.tabControlAttendance);
            this.tabPageAttendance.Location = new System.Drawing.Point(4, 30);
            this.tabPageAttendance.Name = "tabPageAttendance";
            this.tabPageAttendance.Size = new System.Drawing.Size(1197, 775);
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
            this.tabControlAttendance.Size = new System.Drawing.Size(1191, 866);
            this.tabControlAttendance.TabIndex = 0;
            // 
            // tabPage8
            // 
            this.tabPage8.Controls.Add(this.dataGridViewPersonalAttendance);
            this.tabPage8.Location = new System.Drawing.Point(4, 26);
            this.tabPage8.Name = "tabPage8";
            this.tabPage8.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage8.Size = new System.Drawing.Size(1183, 836);
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
            this.dataGridViewPersonalAttendance.Size = new System.Drawing.Size(1171, 816);
            this.dataGridViewPersonalAttendance.TabIndex = 0;
            // 
            // tabPage7
            // 
            this.tabPage7.Controls.Add(this.label2);
            this.tabPage7.Controls.Add(this.dateTimePickerAttendanceEnd);
            this.tabPage7.Controls.Add(this.dateTimePickerAttendanceStart);
            this.tabPage7.Controls.Add(this.dataGridViewClassAtterdance);
            this.tabPage7.Location = new System.Drawing.Point(4, 26);
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.Size = new System.Drawing.Size(1183, 836);
            this.tabPage7.TabIndex = 2;
            this.tabPage7.Text = "Класс";
            this.tabPage7.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(559, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(15, 19);
            this.label2.TabIndex = 4;
            this.label2.Text = "-";
            // 
            // dateTimePickerAttendanceEnd
            // 
            this.dateTimePickerAttendanceEnd.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.dateTimePickerAttendanceEnd.Location = new System.Drawing.Point(575, 4);
            this.dateTimePickerAttendanceEnd.Name = "dateTimePickerAttendanceEnd";
            this.dateTimePickerAttendanceEnd.Size = new System.Drawing.Size(602, 25);
            this.dateTimePickerAttendanceEnd.TabIndex = 3;
            this.dateTimePickerAttendanceEnd.ValueChanged += new System.EventHandler(this.dateTimePickerAttendanceEnd_ValueChanged);
            // 
            // dateTimePickerAttendanceStart
            // 
            this.dateTimePickerAttendanceStart.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.dateTimePickerAttendanceStart.Location = new System.Drawing.Point(6, 4);
            this.dateTimePickerAttendanceStart.Name = "dateTimePickerAttendanceStart";
            this.dateTimePickerAttendanceStart.Size = new System.Drawing.Size(547, 25);
            this.dateTimePickerAttendanceStart.TabIndex = 2;
            this.dateTimePickerAttendanceStart.ValueChanged += new System.EventHandler(this.dateTimePickerAttendanceStart_ValueChanged);
            // 
            // dataGridViewClassAtterdance
            // 
            this.dataGridViewClassAtterdance.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewClassAtterdance.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewClassAtterdance.Location = new System.Drawing.Point(6, 34);
            this.dataGridViewClassAtterdance.Name = "dataGridViewClassAtterdance";
            this.dataGridViewClassAtterdance.Size = new System.Drawing.Size(1171, 824);
            this.dataGridViewClassAtterdance.TabIndex = 1;
            // 
            // tabPageClasses
            // 
            this.tabPageClasses.Controls.Add(this.dataGridViewClasses);
            this.tabPageClasses.Location = new System.Drawing.Point(4, 30);
            this.tabPageClasses.Name = "tabPageClasses";
            this.tabPageClasses.Size = new System.Drawing.Size(1197, 775);
            this.tabPageClasses.TabIndex = 10;
            this.tabPageClasses.Text = "Классы";
            this.tabPageClasses.UseVisualStyleBackColor = true;
            // 
            // dataGridViewClasses
            // 
            this.dataGridViewClasses.AllowUserToAddRows = false;
            this.dataGridViewClasses.AllowUserToDeleteRows = false;
            this.dataGridViewClasses.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewClasses.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewClasses.Location = new System.Drawing.Point(3, 3);
            this.dataGridViewClasses.Name = "dataGridViewClasses";
            this.dataGridViewClasses.ReadOnly = true;
            this.dataGridViewClasses.RowHeadersVisible = false;
            this.dataGridViewClasses.RowTemplate.Height = 25;
            this.dataGridViewClasses.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewClasses.Size = new System.Drawing.Size(1186, 861);
            this.dataGridViewClasses.TabIndex = 2;
            // 
            // tabPageReports
            // 
            this.tabPageReports.Controls.Add(this.buttonReportsToWord);
            this.tabPageReports.Controls.Add(this.dateTimePickerGradesReports1);
            this.tabPageReports.Controls.Add(this.dateTimePickerGradesReports2);
            this.tabPageReports.Controls.Add(this.buttonExcelReport);
            this.tabPageReports.Controls.Add(this.tabControlReports);
            this.tabPageReports.Location = new System.Drawing.Point(4, 30);
            this.tabPageReports.Name = "tabPageReports";
            this.tabPageReports.Size = new System.Drawing.Size(1197, 775);
            this.tabPageReports.TabIndex = 11;
            this.tabPageReports.Text = "Отчетность";
            this.tabPageReports.UseVisualStyleBackColor = true;
            // 
            // buttonReportsToWord
            // 
            this.buttonReportsToWord.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonReportsToWord.Location = new System.Drawing.Point(644, 817);
            this.buttonReportsToWord.Name = "buttonReportsToWord";
            this.buttonReportsToWord.Size = new System.Drawing.Size(267, 28);
            this.buttonReportsToWord.TabIndex = 2;
            this.buttonReportsToWord.Text = "docx";
            this.buttonReportsToWord.UseVisualStyleBackColor = true;
            this.buttonReportsToWord.Click += new System.EventHandler(this.buttonReportsToWord_Click);
            // 
            // dateTimePickerGradesReports1
            // 
            this.dateTimePickerGradesReports1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.dateTimePickerGradesReports1.Location = new System.Drawing.Point(8, 3);
            this.dateTimePickerGradesReports1.Name = "dateTimePickerGradesReports1";
            this.dateTimePickerGradesReports1.Size = new System.Drawing.Size(592, 25);
            this.dateTimePickerGradesReports1.TabIndex = 0;
            this.dateTimePickerGradesReports1.ValueChanged += new System.EventHandler(this.dateTimePickerGradesReports1_ValueChanged);
            // 
            // dateTimePickerGradesReports2
            // 
            this.dateTimePickerGradesReports2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.dateTimePickerGradesReports2.Location = new System.Drawing.Point(606, 3);
            this.dateTimePickerGradesReports2.Name = "dateTimePickerGradesReports2";
            this.dateTimePickerGradesReports2.Size = new System.Drawing.Size(579, 25);
            this.dateTimePickerGradesReports2.TabIndex = 1;
            this.dateTimePickerGradesReports2.ValueChanged += new System.EventHandler(this.dateTimePickerGradesReports2_ValueChanged);
            // 
            // buttonExcelReport
            // 
            this.buttonExcelReport.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonExcelReport.Location = new System.Drawing.Point(371, 817);
            this.buttonExcelReport.Name = "buttonExcelReport";
            this.buttonExcelReport.Size = new System.Drawing.Size(267, 28);
            this.buttonExcelReport.TabIndex = 1;
            this.buttonExcelReport.Text = "xlsx";
            this.buttonExcelReport.UseVisualStyleBackColor = true;
            this.buttonExcelReport.Click += new System.EventHandler(this.buttonExcelReport_Click);
            // 
            // tabControlReports
            // 
            this.tabControlReports.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlReports.Controls.Add(this.tabPageGradesReports);
            this.tabControlReports.Controls.Add(this.tabPageAtterdanceReports);
            this.tabControlReports.Location = new System.Drawing.Point(8, 34);
            this.tabControlReports.Name = "tabControlReports";
            this.tabControlReports.SelectedIndex = 0;
            this.tabControlReports.Size = new System.Drawing.Size(1181, 781);
            this.tabControlReports.TabIndex = 0;
            // 
            // tabPageGradesReports
            // 
            this.tabPageGradesReports.Controls.Add(this.dataGridViewGradesReports);
            this.tabPageGradesReports.Location = new System.Drawing.Point(4, 26);
            this.tabPageGradesReports.Name = "tabPageGradesReports";
            this.tabPageGradesReports.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGradesReports.Size = new System.Drawing.Size(1173, 751);
            this.tabPageGradesReports.TabIndex = 0;
            this.tabPageGradesReports.Text = "Успеваемость";
            this.tabPageGradesReports.UseVisualStyleBackColor = true;
            // 
            // dataGridViewGradesReports
            // 
            this.dataGridViewGradesReports.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.dataGridViewGradesReports.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewGradesReports.Location = new System.Drawing.Point(6, 31);
            this.dataGridViewGradesReports.Name = "dataGridViewGradesReports";
            this.dataGridViewGradesReports.Size = new System.Drawing.Size(1161, 589);
            this.dataGridViewGradesReports.TabIndex = 2;
            // 
            // tabPageAtterdanceReports
            // 
            this.tabPageAtterdanceReports.Controls.Add(this.dataGridViewAtterdanceReports);
            this.tabPageAtterdanceReports.Location = new System.Drawing.Point(4, 26);
            this.tabPageAtterdanceReports.Name = "tabPageAtterdanceReports";
            this.tabPageAtterdanceReports.Size = new System.Drawing.Size(1173, 751);
            this.tabPageAtterdanceReports.TabIndex = 1;
            this.tabPageAtterdanceReports.Text = "Посещаемость";
            this.tabPageAtterdanceReports.UseVisualStyleBackColor = true;
            // 
            // dataGridViewAtterdanceReports
            // 
            this.dataGridViewAtterdanceReports.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.dataGridViewAtterdanceReports.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewAtterdanceReports.Location = new System.Drawing.Point(6, 36);
            this.dataGridViewAtterdanceReports.Name = "dataGridViewAtterdanceReports";
            this.dataGridViewAtterdanceReports.Size = new System.Drawing.Size(1161, 667);
            this.dataGridViewAtterdanceReports.TabIndex = 3;
            // 
            // tabPageRegisters
            // 
            this.tabPageRegisters.Controls.Add(this.button4);
            this.tabPageRegisters.Controls.Add(this.dataGridViewRegisters);
            this.tabPageRegisters.Location = new System.Drawing.Point(4, 56);
            this.tabPageRegisters.Name = "tabPageRegisters";
            this.tabPageRegisters.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageRegisters.Size = new System.Drawing.Size(1197, 749);
            this.tabPageRegisters.TabIndex = 12;
            this.tabPageRegisters.Text = "Заявления";
            this.tabPageRegisters.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button4.Location = new System.Drawing.Point(8, 707);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(1180, 36);
            this.button4.TabIndex = 1;
            this.button4.Text = "Распределить автоматически";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // dataGridViewRegisters
            // 
            this.dataGridViewRegisters.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewRegisters.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewRegisters.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumnApplicationID,
            this.dataGridViewTextBoxColumnFullName,
            this.dataGridViewTextBoxColumnPermissionID,
            this.dataGridViewTextBoxColumnClassID,
            this.dataGridViewTextBoxColumnAge,
            this.dataGridViewTextBoxColumnTelephone,
            this.dataGridViewTextBoxColumnApplicationDate,
            this.dataGridViewTextBoxColumnIsApproved});
            this.dataGridViewRegisters.Location = new System.Drawing.Point(6, 6);
            this.dataGridViewRegisters.Name = "dataGridViewRegisters";
            this.dataGridViewRegisters.Size = new System.Drawing.Size(1182, 695);
            this.dataGridViewRegisters.TabIndex = 0;
            // 
            // dataGridViewTextBoxColumnApplicationID
            // 
            this.dataGridViewTextBoxColumnApplicationID.HeaderText = "ID";
            this.dataGridViewTextBoxColumnApplicationID.Name = "dataGridViewTextBoxColumnApplicationID";
            // 
            // dataGridViewTextBoxColumnFullName
            // 
            this.dataGridViewTextBoxColumnFullName.HeaderText = "ФИО";
            this.dataGridViewTextBoxColumnFullName.Name = "dataGridViewTextBoxColumnFullName";
            // 
            // dataGridViewTextBoxColumnPermissionID
            // 
            this.dataGridViewTextBoxColumnPermissionID.HeaderText = "Роль";
            this.dataGridViewTextBoxColumnPermissionID.Name = "dataGridViewTextBoxColumnPermissionID";
            // 
            // dataGridViewTextBoxColumnClassID
            // 
            this.dataGridViewTextBoxColumnClassID.HeaderText = "Класс";
            this.dataGridViewTextBoxColumnClassID.Name = "dataGridViewTextBoxColumnClassID";
            // 
            // dataGridViewTextBoxColumnAge
            // 
            this.dataGridViewTextBoxColumnAge.HeaderText = "Возраст";
            this.dataGridViewTextBoxColumnAge.Name = "dataGridViewTextBoxColumnAge";
            // 
            // dataGridViewTextBoxColumnTelephone
            // 
            this.dataGridViewTextBoxColumnTelephone.HeaderText = "Телефон";
            this.dataGridViewTextBoxColumnTelephone.Name = "dataGridViewTextBoxColumnTelephone";
            // 
            // dataGridViewTextBoxColumnApplicationDate
            // 
            this.dataGridViewTextBoxColumnApplicationDate.HeaderText = "Дата подачи";
            this.dataGridViewTextBoxColumnApplicationDate.Name = "dataGridViewTextBoxColumnApplicationDate";
            // 
            // dataGridViewTextBoxColumnIsApproved
            // 
            this.dataGridViewTextBoxColumnIsApproved.HeaderText = "Одобрено";
            this.dataGridViewTextBoxColumnIsApproved.Name = "dataGridViewTextBoxColumnIsApproved";
            // 
            // printDocumentDialog1
            // 
            this.printDocumentDialog1.UseEXDialog = true;
            // 
            // printDocumentAtterdance
            // 
            this.printDocumentAtterdance.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocumentAtterdance_PrintPage);
            // 
            // printDialog1
            // 
            this.printDialog1.UseEXDialog = true;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.HeaderText = "Предмет";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            this.dataGridViewTextBoxColumn6.Width = 228;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.HeaderText = "1-я четверть";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            this.dataGridViewTextBoxColumn7.Width = 229;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.HeaderText = "2-я четверть";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.ReadOnly = true;
            this.dataGridViewTextBoxColumn8.Width = 228;
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.HeaderText = "3-я четверть";
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            this.dataGridViewTextBoxColumn9.ReadOnly = true;
            this.dataGridViewTextBoxColumn9.Width = 229;
            // 
            // dataGridViewTextBoxColumn10
            // 
            this.dataGridViewTextBoxColumn10.HeaderText = "4-я четверть";
            this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            this.dataGridViewTextBoxColumn10.ReadOnly = true;
            this.dataGridViewTextBoxColumn10.Width = 228;
            // 
            // dataGridViewTextBoxColumn11
            // 
            this.dataGridViewTextBoxColumn11.HeaderText = "ID";
            this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
            // 
            // dataGridViewTextBoxColumn12
            // 
            this.dataGridViewTextBoxColumn12.HeaderText = "FullName";
            this.dataGridViewTextBoxColumn12.Name = "dataGridViewTextBoxColumn12";
            // 
            // dataGridViewTextBoxColumn13
            // 
            this.dataGridViewTextBoxColumn13.HeaderText = "Роль";
            this.dataGridViewTextBoxColumn13.Name = "dataGridViewTextBoxColumn13";
            // 
            // dataGridViewTextBoxColumn14
            // 
            this.dataGridViewTextBoxColumn14.HeaderText = "Класс";
            this.dataGridViewTextBoxColumn14.Name = "dataGridViewTextBoxColumn14";
            // 
            // dataGridViewTextBoxColumn15
            // 
            this.dataGridViewTextBoxColumn15.HeaderText = "Age";
            this.dataGridViewTextBoxColumn15.Name = "dataGridViewTextBoxColumn15";
            // 
            // dataGridViewTextBoxColumn16
            // 
            this.dataGridViewTextBoxColumn16.HeaderText = "Телефон";
            this.dataGridViewTextBoxColumn16.Name = "dataGridViewTextBoxColumn16";
            // 
            // dataGridViewTextBoxColumn17
            // 
            this.dataGridViewTextBoxColumn17.HeaderText = "Дата подачи";
            this.dataGridViewTextBoxColumn17.Name = "dataGridViewTextBoxColumn17";
            // 
            // dataGridViewTextBoxColumn18
            // 
            this.dataGridViewTextBoxColumn18.HeaderText = "Одобрено";
            this.dataGridViewTextBoxColumn18.Name = "dataGridViewTextBoxColumn18";
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
            this.panelTop.PerformLayout();
            this.panelMain.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.tabHomework.ResumeLayout(false);
            this.panelHomework.ResumeLayout(false);
            this.homeworkTabControl.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
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
            this.tabPage5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewGradesAll)).EndInit();
            this.tabPage10.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewQuaterGrades)).EndInit();
            this.tabShedule.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.sheduleTabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sheduleGridView)).EndInit();
            this.tabPageSheduleAll.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSheduleAll)).EndInit();
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
            this.tabPage7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewClassAtterdance)).EndInit();
            this.tabPageClasses.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewClasses)).EndInit();
            this.tabPageReports.ResumeLayout(false);
            this.tabControlReports.ResumeLayout(false);
            this.tabPageGradesReports.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewGradesReports)).EndInit();
            this.tabPageAtterdanceReports.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAtterdanceReports)).EndInit();
            this.tabPageRegisters.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRegisters)).EndInit();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Panel panelTop, panelMain, panelHomework, panelGrades, panelStatistics;
        private System.Windows.Forms.Label labelRole;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabHomework, tabGrades, tabStatistics;
        private System.Windows.Forms.DateTimePicker dateTimePickerHomework, dateTimePickerGrades;
        private System.Windows.Forms.DataGridView dataGridViewHomework, dataGridViewGrades;

        #endregion
        private System.Windows.Forms.Label labelHomeworkPeriod;
        private System.Windows.Forms.TabPage tabShedule;
        private System.Windows.Forms.Label labelGradesPeriod;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label sheduleLabel;
        private System.Windows.Forms.TabControl homeworkTabControl;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Label labelHomeworkAll;
        private System.Windows.Forms.DataGridView datagridviewHomeworkAll;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage4;
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
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dateTimePickerAttendanceEnd;
        private System.Windows.Forms.DateTimePicker dateTimePickerAttendanceStart;
        private System.Windows.Forms.TabPage tabPageClasses;
        private System.Windows.Forms.DataGridView dataGridViewClasses;
        private System.Windows.Forms.DateTimePicker dateTimePickerHomework1;
        private System.Windows.Forms.DateTimePicker dateTimePickerGrades1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TabPage tabPageReports;
        private System.Windows.Forms.TabControl tabControlReports;
        private System.Windows.Forms.TabPage tabPageGradesReports;
        private System.Windows.Forms.DataGridView dataGridViewGradesReports;
        private System.Windows.Forms.DateTimePicker dateTimePickerGradesReports2;
        private System.Windows.Forms.DateTimePicker dateTimePickerGradesReports1;
        private System.Windows.Forms.Button buttonExcelReport;
        private System.Windows.Forms.Button buttonReportsToWord;
        private System.Windows.Forms.Button buttonHomeworkPresentation;
        private System.Windows.Forms.TabPage tabPageAtterdanceReports;
        private System.Windows.Forms.DataGridView dataGridViewAtterdanceReports;
        private System.Windows.Forms.TextBox textBoxSearch;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ComboBox directorComboBox;
        private System.Windows.Forms.TabPage tabPage10;
        private System.Windows.Forms.Button buttonCalculateQuaterGrades;
        private System.Windows.Forms.DataGridView dataGridViewQuaterGrades;
        private System.Windows.Forms.ComboBox comboBoxQuaterGradesStudent;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn SubjectName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Quarter1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Quarter2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Quarter3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Quarter4;
        private System.Windows.Forms.Button buttonCreateSheduleFromSubjects;
        private System.Windows.Forms.TabPage tabPageRegisters;
        private System.Windows.Forms.TabControl sheduleTabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.DataGridView sheduleGridView;
        private System.Windows.Forms.TabPage tabPageSheduleAll;
        private System.Windows.Forms.DataGridView dataGridViewSheduleAll;
        private System.Windows.Forms.DataGridView dataGridViewRegisters;
        private System.Windows.Forms.Button button4;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn12;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn13;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn14;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn15;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn16;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn17;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn18;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumnApplicationID;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumnFullName;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumnPermissionID;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumnClassID;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumnAge;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumnTelephone;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumnApplicationDate;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumnIsApproved;
    }
}

