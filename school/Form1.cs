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

        public Form1()
        {
            InitializeComponent();

            dataGridViewHomework.CellEndEdit += DataGridViewHomework_CellEndEdit;
            dataGridViewHomework.UserDeletedRow += DataGridViewHomework_UserDeletedRow;

            dataGridViewGrades.CellEndEdit += DataGridViewGrades_CellEndEdit;
            dataGridViewGrades.UserDeletedRow += DataGridViewGrades_UserDeletedRow;

            labelRole.Text = UserController.CurrentUser.PermissionName + " - " + UserController.CurrentUser.FullName;
        }

        private bool IsGradeRowComplete(DataGridViewRow row)
        {
            string date = row.Cells[1].Value?.ToString()?.Trim();      // colDate
            string subject = row.Cells[2].Value?.ToString()?.Trim();   // colSubject
            string grade = row.Cells[3].Value?.ToString()?.Trim();     // colGrade
            string person = row.Cells[4].Value?.ToString()?.Trim();    // colPerson

            // ✅ Все поля НЕ пустые
            bool allFilled = !string.IsNullOrEmpty(date) &&
                             !string.IsNullOrEmpty(subject) &&
                             !string.IsNullOrEmpty(grade) &&
                             !string.IsNullOrEmpty(person);

            return allFilled;
        }

        // ✅ Редактирование оценки
        private void DataGridViewGrades_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (UserController.CurrentUser.PermissionID <= 1) return;

            var row = dataGridViewGrades.Rows[e.RowIndex];

            // ✅ ПРОВЕРКА: все поля заполнены?
            if (!IsGradeRowComplete(row))
            {
                return;
            }

            // ✅ ИНДЕКСЫ вместо имен (БЕЗОПАСНО!)
            int gradeId = row.Cells.Count > 0 ? int.Parse(row.Cells[0].Value?.ToString() ?? "0") : 0;

            var grade = new Grade
            {
                GradeID = gradeId,
                GradeDate = DateTime.Parse(row.Cells[1].Value.ToString()), // colDate
                SubjectID = SubjectController._controller.GetSubjectIdByName(row.Cells[2].Value.ToString()), // colSubject
                GradeValue = byte.Parse(row.Cells[3].Value.ToString()), // colGrade
                TeacherID = UserController.CurrentUser.UserID
            };

            // Для учителя - StudentID из colPerson (индекс 4)
            if (UserController.CurrentUser.PermissionID > 1)
            {
                grade.StudentID = UserController._userController.GetStudentIdByName(row.Cells[4].Value.ToString());
            }

            // ✅ Проверка: ID найдены
            if (grade.SubjectID == 0)
            {
                MessageBox.Show("❌ Предмет не найден в БД!");
                return;
            }
            if (UserController.CurrentUser.PermissionID > 1 && grade.StudentID == 0)
            {
                MessageBox.Show("❌ Ученик не найден в БД!");
                return;
            }

            string action = gradeId > 0 ? "EDIT" : "ADD";
            GradesController._controller.AddGradeChange(action, grade);

            MessageBox.Show($"✅ {action} оценка #{gradeId}");
        }

        // ✅ Удаление оценки
        private void DataGridViewGrades_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            if (UserController.CurrentUser.PermissionID <= 1) return;

            var row = e.Row;
            int gradeId = int.Parse(row.Cells[0].Value?.ToString() ?? "0"); // Индекс 0 = GradeID

            if (gradeId > 0)
            {
                var grade = new Grade { GradeID = gradeId };
                GradesController._controller.AddGradeChange("DELETE", grade);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            int previousTab = tabControl.SelectedIndex; // Текущая ДО смены

            // ✅ СОХРАНИТЬ при уходе с ДОМАШКИ (0)
            if (previousTab == 0)
            {
                int saved = _homeworkController.CommitHomeworkChanges();
                if (saved > 0)
                    MessageBox.Show($"✅ Сохранено {saved} ДЗ!");
            }

            // ✅ СОХРАНИТЬ при уходе с ОЦЕНОК (1)
            if (previousTab == 1)
            {
                int saved = GradesController._controller.CommitGradeChanges();
                if (saved > 0)
                    MessageBox.Show($"✅ Сохранено {saved} оценок!");
            }

            switch (tabControl.SelectedIndex)
            {
                case 0: LoadHomeworkGrid(); break;
                case 1: LoadGradesGrid(); break;
                case 2: LoadStatisticsGrid(); break;
                case 3: LoadScheduleGrid(); break;
            }
        }

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

        private bool IsHomeworkRowComplete(DataGridViewRow row)
        {
            string date = row.Cells["colDate"].Value?.ToString()?.Trim();
            string cls = row.Cells["colClass"].Value?.ToString()?.Trim();
            string subject = row.Cells["colSubject"].Value?.ToString()?.Trim();
            string desc = row.Cells["colDescription"].Value?.ToString()?.Trim();

            // ✅ Все поля НЕ пустые И числовые поля корректны
            return !string.IsNullOrEmpty(date) &&
                   !string.IsNullOrEmpty(cls) &&
                   !string.IsNullOrEmpty(subject) &&
                   !string.IsNullOrEmpty(desc);
        }

        private void DataGridViewHomework_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (UserController.CurrentUser.PermissionID <= 1) return;

            var row = dataGridViewHomework.Rows[e.RowIndex];

            // ✅ ПРОВЕРКА: все поля заполнены?
            if (IsHomeworkRowComplete(row))
            {
                // ✅ ПОЛУЧАЕМ ID через методы контроллера
                var classObj = ClassController._controller.GetClassByName(row.Cells["colClass"].Value.ToString());
                var subjectObj = SubjectController._controller.GetSubjectByName(row.Cells["colSubject"].Value.ToString());

                var homework = new Homework
                {
                    AssignmentDate = DateTime.Parse(row.Cells["colDate"].Value.ToString()),
                    ClassID = classObj?.ClassID ?? 0,                           // ✅ ID из объекта!
                    SubjectID = subjectObj?.SubjectID ?? 0,                      // ✅ ID из объекта!
                    Description = row.Cells["colDescription"].Value.ToString(),
                    TeacherID = UserController.CurrentUser.UserID
                };

                // ✅ Проверка: предмет и класс найдены
                if (homework.ClassID == 0 || homework.SubjectID == 0)
                {
                    MessageBox.Show("❌ Класс или предмет не найден в БД!");
                    return;
                }

                _homeworkController.AddHomeworkChange("EDIT", homework);
                MessageBox.Show("✅ Добавлено в очередь");
            }
        }

        private void DataGridViewHomework_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            if (UserController.CurrentUser.PermissionID <= 1) return;

            var row = e.Row;

            // ✅ ИНДЕКС 0 = colHomeworkID (даже если колонка не создана!)
            if (row.Cells.Count > 0 && row.Cells[0].Value != null)
            {
                int homeworkId = int.Parse(row.Cells[0].Value.ToString());
                if (homeworkId > 0)
                {
                    var homework = new Homework { HomeworkID = homeworkId };
                    _homeworkController.AddHomeworkChange("DELETE", homework);

                    MessageBox.Show($"✅ Удаление ДЗ #{homeworkId} в очереди");
                }
            }
        }

        private void LoadHomeworkGrid()
        {
            try
            {
                DateTime startDate = dateTimePickerHomework.Value.AddDays(-7);
                DateTime endDate = dateTimePickerHomework.Value;

                List<Homework> homeworkList;
                if (UserController.CurrentUser.PermissionID > 1)
                    homeworkList = TeacherController._controller.GetTeacherHomework(startDate, endDate, UserController.CurrentUser);
                else
                    homeworkList = _homeworkController.GetHomeworkForClassPeriod((int)UserController.CurrentUser.ClassID, startDate, endDate);

                // ✅ 1. ПОЛНАЯ ОЧИСТКА
                dataGridViewHomework.DataSource = null;
                dataGridViewHomework.Columns.Clear();
                dataGridViewHomework.Rows.Clear();

                // ✅ 2. СНАЧАЛА СОЗДАЕМ СТОЛБЦЫ
                SetupHomeworkGrid();

                // ✅ 3. ПОТОМ добавляем СТРОКИ
                foreach (var hw in homeworkList)
                {
                    dataGridViewHomework.Rows.Add(
                        hw.HomeworkID,
                        hw.AssignmentDate.ToShortDateString(),
                        hw.ClassNameDisplay ?? hw.ClassID.ToString(),
                        hw.SubjectNameDisplay ?? hw.SubjectID.ToString(),
                        hw.TeacherNameDisplay ?? hw.TeacherID.ToString(),
                        hw.Description ?? ""
                    );
                }

                labelHomeworkPeriod.Text = $"{(UserController.CurrentUser.PermissionID > 1 ? "Мои Д/З" : "ДЗ класса")}: {homeworkList.Count} шт.";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private void SetupHomeworkGrid()
        {
            dataGridViewHomework.Columns.Clear();

            bool isTeacher = UserController.CurrentUser.PermissionID > 1;

            // ✅ 0. СКРЫТАЯ колонка ID (ПЕРВАЯ!)
            dataGridViewHomework.Columns.Add("colHomeworkID", "ID");
            dataGridViewHomework.Columns["colHomeworkID"].Visible = false;  // ✅ СКРЫТЬ!

            // ✅ 1-5. Видимые колонки
            dataGridViewHomework.Columns.Add("colDate", "Дата");
            dataGridViewHomework.Columns.Add("colClass", "Класс");
            dataGridViewHomework.Columns.Add("colSubject", "Предмет");
            dataGridViewHomework.Columns.Add("colTeacher", "Учитель");
            dataGridViewHomework.Columns.Add("colDescription", "Задание");

            dataGridViewHomework.ReadOnly = !isTeacher;
            dataGridViewHomework.AllowUserToAddRows = isTeacher;
            dataGridViewHomework.AllowUserToDeleteRows = isTeacher;
            dataGridViewHomework.EditMode = DataGridViewEditMode.EditOnKeystroke;
            dataGridViewHomework.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewHomework.RowHeadersVisible = false;
        }

        private void LoadGradesGrid()
        {
            try
            {
                dataGridViewGrades.DataSource = null;
                dataGridViewGrades.Columns.Clear();
                dataGridViewGrades.Rows.Clear();

                DateTime startDate = dateTimePickerGrades.Value.AddDays(-7);
                DateTime endDate = dateTimePickerGrades.Value;

                List<Grade> gradesList;
                if (UserController.CurrentUser.PermissionID > 1)
                {
                    gradesList = TeacherController._controller.GetTeacherGrades(startDate, endDate, UserController.CurrentUser);
                }
                else
                {
                    gradesList = GradesController._controller.GetGradesForStudentPeriod(UserController.CurrentUser.UserID, startDate, endDate);
                }

                // ✅ СНАЧАЛА СОЗДАЕМ КОЛОНКИ
                SetupGradesGrid();

                // ✅ ПОТОМ заполняем РУЧНОЙ строки
                foreach (var grade in gradesList)
                {
                    dataGridViewGrades.Rows.Add(
                        grade.GradeID,                                    // 0. СКРЫТЫЙ ID
                        grade.GradeDate.ToShortDateString(),              // 1. Дата
                        grade.SubjectNameDisplay ?? "",                   // 2. Предмет
                        grade.GradeValue,                                 // 3. Оценка
                        UserController.CurrentUser.PermissionID > 1 ?     // 4. Ученик/Учитель
                            grade.StudentNameDisplay ?? "" :
                            grade.TeacherNameDisplay ?? ""
                    );
                }

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
            dataGridViewGrades.Columns.Clear();
            bool isTeacher = UserController.CurrentUser.PermissionID > 1;

            // ✅ 0. СКРЫТАЯ колонка GradeID
            dataGridViewGrades.Columns.Add("colGradeID", "ID");
            dataGridViewGrades.Columns["colGradeID"].Visible = false;

            // ✅ 1. Дата
            dataGridViewGrades.Columns.Add("colDate", "Дата");

            // ✅ 2. Предмет
            dataGridViewGrades.Columns.Add("colSubject", "Предмет");

            // ✅ 3. Оценка (число 1-5)
            var colGrade = new DataGridViewTextBoxColumn();
            colGrade.Name = "colGrade";
            colGrade.HeaderText = "Оценка";
            colGrade.Width = 70;
            dataGridViewGrades.Columns.Add(colGrade);

            // ✅ 4. Ученик (учитель) / Учитель (ученик)
            string personHeader = isTeacher ? "Ученик" : "Учитель";
            dataGridViewGrades.Columns.Add("colPerson", personHeader);

            // ✅ НАСТРОЙКИ редактирования
            dataGridViewGrades.ReadOnly = !isTeacher;
            dataGridViewGrades.AllowUserToAddRows = isTeacher;
            dataGridViewGrades.AllowUserToDeleteRows = isTeacher;
            dataGridViewGrades.EditMode = DataGridViewEditMode.EditOnKeystroke;
            dataGridViewGrades.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewGrades.RowHeadersVisible = false;
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