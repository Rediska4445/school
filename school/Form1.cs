using school.Controllers;
using school.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using static school.Controllers.HomeworkController;

namespace school
{
    public partial class Form1 : Form
    {
        public static string CONNECTION_STRING = "Server=(localdb)\\MSSQLLocalDB;Database=SchoolSystemTest;Integrated Security=true;";

        private readonly HomeworkController _homeworkController;

        public Form1()
        {
            InitializeComponent();
            _homeworkController = HomeworkController._homeworkController;

            labelRole.Text = UserController.CurrentUser.PermissionID + " - " + UserController.CurrentUser.FullName;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabControl.SelectedIndex)
            {
                case 0: // Д/З
                    LoadHomeworkGrid();
                    break;
                case 1: // Оценки
                    LoadGradesGrid();
                    break;
                case 2: // Статистика
                    LoadStatisticsGrid();
                    break;
                case 3: // Расписание - sheduleGridView
                    LoadScheduleGrid();
                    break;
            }
        }

        /// <summary>
        /// [translate:Загрузка расписания класса (полная неделя)]
        /// </summary>
        private void LoadScheduleGrid()
        {
            try
            {
                sheduleGridView.DataSource = null;

                var scheduleList = SheduleController._controller.GetScheduleForClass(
                    (int)UserController.CurrentUser.ClassID); // ✅ Без периода

                sheduleGridView.DataSource = scheduleList;
                SetupScheduleGrid();

                sheduleLabel.Text = $"Расписание класса ({scheduleList.Count} уроков)"; // ✅ Без дат
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки расписания: {ex.Message}");
            }
        }

        /// <summary>
        /// [translate:Настройка колонок таблицы расписания]
        /// </summary>
        private void SetupScheduleGrid()
        {
            if (sheduleGridView.DataSource == null) return;

            sheduleGridView.AutoGenerateColumns = false;
            sheduleGridView.Columns.Clear();

            sheduleGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Day",
                DataPropertyName = "DayOfWeekDisplay",
                HeaderText = "День",
                Width = 80
            });

            sheduleGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Lesson",
                DataPropertyName = "LessonNumber",
                HeaderText = "Урок",
                Width = 60
            });

            sheduleGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Time",
                DataPropertyName = "LessonTimeDisplay",
                HeaderText = "Время",
                Width = 80
            });

            sheduleGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Subject",
                DataPropertyName = "SubjectName",
                HeaderText = "Предмет",
                Width = 120
            });

            sheduleGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Teacher",
                DataPropertyName = "TeacherName",
                HeaderText = "Учитель",
                Width = 150
            });

            sheduleGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            sheduleGridView.AllowUserToAddRows = false;
            sheduleGridView.ReadOnly = true;
            sheduleGridView.RowHeadersVisible = false;
            sheduleGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void LoadHomeworkGrid()
        {
            try
            {
                dataGridViewHomework.DataSource = null;

                DateTime startDate = dateTimePickerHomework.Value.AddDays(-7);
                DateTime endDate = dateTimePickerHomework.Value;

                List<Homework> homeworkList;

                if (UserController.CurrentUser.PermissionID > 1)
                {
                    // ✅ УЧИТЕЛЬ: только свои ДЗ по своим предметам из TeacherSubjects
                    homeworkList = TeacherController._controller.GetTeacherHomework(
                        startDate, endDate, UserController.CurrentUser
                    );
                }
                else
                {
                    // ✅ УЧЕНИК: ДЗ своего класса (все предметы)
                    homeworkList = _homeworkController.GetHomeworkForClassPeriod(
                        (int)UserController.CurrentUser.ClassID, startDate, endDate);
                }

                dataGridViewHomework.DataSource = homeworkList;
                SetupHomeworkGrid();

                string labelText = UserController.CurrentUser.PermissionID > 1
                    ? $"Мои Д/З: {startDate:dd.MM} - {endDate:dd.MM}"
                    : $"Домашние задания {UserController.CurrentUser.ClassID}: {startDate:dd.MM} - {endDate:dd.MM}";

                labelHomeworkPeriod.Text = $"{labelText} ({homeworkList.Count} шт.)";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки ДЗ: {ex.Message}");
            }
        }

        private void SetupHomeworkGrid()
        {
            if (dataGridViewHomework.DataSource == null) 
                return;

            //dataGridViewHomework.DataSource = null;

            dataGridViewHomework.Columns.Clear();

            dataGridViewHomework.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Date",
                DataPropertyName = "AssignmentDate",
                HeaderText = "Дата",
                Width = 80
            });

            dataGridViewHomework.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Class",
                DataPropertyName = "ClassNameDisplay",
                HeaderText = "Класс",
                Width = 60
            });

            dataGridViewHomework.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Subject",
                DataPropertyName = "SubjectNameDisplay",
                HeaderText = "Предмет",
                Width = 100
            });

            dataGridViewHomework.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Teacher",
                DataPropertyName = "TeacherNameDisplay",
                HeaderText = "Учитель",
                Width = 150
            });

            dataGridViewHomework.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Description",
                DataPropertyName = "Description",
                HeaderText = "Задание",
                Width = 300,
                DefaultCellStyle = new DataGridViewCellStyle { WrapMode = DataGridViewTriState.True } // ✅ Правильный синтаксис
            });

            bool isTeacher = UserController.CurrentUser.PermissionID > 1;
            dataGridViewHomework.ReadOnly = !isTeacher;
            dataGridViewHomework.AllowUserToAddRows = isTeacher;
            dataGridViewHomework.AllowUserToDeleteRows = isTeacher;

            dataGridViewHomework.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewHomework.RowHeadersVisible = false;
        }

        private void LoadGradesGrid()
        {
            try
            {
                dataGridViewGrades.DataSource = null;

                DateTime startDate = dateTimePickerGrades.Value.AddDays(-7);
                DateTime endDate = dateTimePickerGrades.Value;

                List<Grade> gradesList;

                if (UserController.CurrentUser.PermissionID > 1)
                {
                    // ✅ УЧИТЕЛЬ: только свои оценки по своим предметам из TeacherSubjects
                    gradesList = TeacherController._controller.GetTeacherGrades(
                        startDate, endDate, UserController.CurrentUser);
                }
                else
                {
                    // ✅ УЧЕНИК: свои оценки (все предметы, все учителя)
                    gradesList = GradesController._controller.GetGradesForStudentPeriod(
                        UserController.CurrentUser.UserID, startDate, endDate);
                }

                dataGridViewGrades.DataSource = gradesList;
                SetupGradesGrid();

                string labelText = UserController.CurrentUser.PermissionID > 1
                    ? $"Мои оценки: {startDate:dd.MM} - {endDate:dd.MM}"
                    : $"Оценки: {startDate:dd.MM} - {endDate:dd.MM}";

                labelGradesPeriod.Text = $"{labelText} ({gradesList.Count} шт.)";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки оценок: {ex.Message}");
            }
        }

        private void SetupGradesGrid()
        {
            if (dataGridViewGrades.DataSource == null) return;

            dataGridViewGrades.AutoGenerateColumns = false;
            dataGridViewGrades.Columns.Clear();

            // ✅ Общие колонки
            dataGridViewGrades.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Date",
                DataPropertyName = "GradeDate",
                HeaderText = "Дата",
                Width = 80
            });

            dataGridViewGrades.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Subject",
                DataPropertyName = "SubjectNameDisplay",
                HeaderText = "Предмет",
                Width = 120
            });

            dataGridViewGrades.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Grade",
                DataPropertyName = "GradeValue",
                HeaderText = "Оценка",
                Width = 70
            });

            // ✅ РАЗВИЛКА ПО РОЛИ
            if (UserController.CurrentUser.PermissionID > 1)
            {
                // УЧИТЕЛЬ: показывает УЧЕНИКА вместо Учителя
                dataGridViewGrades.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Student",
                    DataPropertyName = "StudentNameDisplay",
                    HeaderText = "Ученик",
                    Width = 150
                });
            }
            else
            {
                // УЧЕНИК: показывает УЧИТЕЛЯ
                dataGridViewGrades.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Teacher",
                    DataPropertyName = "TeacherNameDisplay",
                    HeaderText = "Учитель",
                    Width = 150
                });
            }

            labelRole.Text = !(UserController.CurrentUser.PermissionID > 1) + "";

            // ✅ РЕДАКТИРОВАНИЕ ТОЛЬКО ДЛЯ УЧИТЕЛЯ
            dataGridViewGrades.ReadOnly = !(UserController.CurrentUser.PermissionID > 1);
            dataGridViewGrades.AllowUserToAddRows = UserController.CurrentUser.PermissionID > 1;
            dataGridViewGrades.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewGrades.RowHeadersVisible = false;
            dataGridViewGrades.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void LoadStatisticsGrid()
        {
            dataGridViewStatistics.DataSource = null;
            // TODO: Statistics
        }

        private void dateTimePickerHomework_ValueChanged(object sender, EventArgs e)
        {
            if (tabControl.SelectedIndex == 0)
                LoadHomeworkGrid();
        }

        private void dateTimePickerStatistics_ValueChanged(object sender, EventArgs e)
        {
            if (tabControl.SelectedIndex == 2)
                LoadStatisticsGrid();
        }

        private void dateTimePickerHomework_ValueChanged_1(object sender, EventArgs e)
        {
            LoadHomeworkGrid();
        }

        private void dateTimePickerGrades_ValueChanged_1(object sender, EventArgs e)
        {
            LoadGradesGrid();
        }

        private void sheduleDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            LoadScheduleGrid();
        }
    }
}