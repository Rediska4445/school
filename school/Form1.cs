using school.Controllers;
using school.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using static school.Controllers.HomeworkController;

namespace school
{
    // Be kidda, To be will murdaaaaaaaaaaaaaaaaa!!!!!!!!!
    // Dis fuckin lunatic, however cannot to change it!

    /// <summary>
    /// Основная форма для отображения всей этой хуйни.
    /// </summary>
    public partial class Form1 : Form
    {
        // Ебанный список из классов для ебанного расписания
        private ComboBox directorComboBox; 

        // Хуйня для подключения к БД
        public static string CONNECTION_STRING = "Server=(localdb)\\MSSQLLocalDB;Database=SchoolSystemTest;Integrated Security=true;";

        // Конструктор формы
        public Form1()
        {
            InitializeComponent();

            // Хандлеры таблицы с домашкой
            dataGridViewHomework.CellEndEdit += DataGridViewHomework_CellEndEdit;
            dataGridViewHomework.UserDeletedRow += DataGridViewHomework_UserDeletedRow;

            // Хандлеры таблицы с оценками
            dataGridViewGrades.CellEndEdit += DataGridViewGrades_CellEndEdit;
            dataGridViewGrades.UserDeletedRow += DataGridViewGrades_UserDeletedRow;

            // Хандлеры таблицы с расписанием
            sheduleGridView.CellEndEdit += DataGridViewSchedule_CellEndEdit;
            sheduleGridView.UserDeletedRow += DataGridViewSchedule_UserDeletedRow;

            // Текст с ролью и именем
            labelRole.Text = UserController.CurrentUser.PermissionName + " - " + UserController.CurrentUser.FullName;

            // Подготовка всякой хуйни для ролевой системы
            PrepareRole();
        }

        private void PrepareRole()
        {
            if (UserController.CurrentUser.PermissionID == 1) // Только ученик
            {
                // Статистика 
                // У ученика прав мало, обязанностей дохуя, поэтому удалить вкладку с статистикой класса
                tabControlStatistic.TabPages.RemoveByKey("tabPageClass");
                tabControlStatistic.SelectedTab = tabControlStatistic.TabPages["tabPersonalStats"];

                // Визуальная подготовка статистика
                SetupStatisticsGrid(dataGridViewPersonalStatistics);

                // Удалить отчеты нахуй
                tabControl.TabPages.RemoveByKey("tabPageReports");

                // Удалить предметы нахуй
                tabControl.TabPages.RemoveByKey("tabPageSubjects");
            }
            // Учитель может смотреть статистику своего класса.
            // Директор может менять класс с помощью comboBox.
            else if (UserController.CurrentUser.PermissionID >= 2)
            {
                tabControlStatistic.TabPages.RemoveByKey("tabPagePersonal");

                dataGridViewClassStatistics.SelectionChanged += dataGridViewClassStatistics_SelectionChanged;

                SetupStatisticsGrid(dataGridViewClassStatistics);

                if (UserController.CurrentUser.PermissionID >= 3)
                {
                    InitializeScheduleClassCombo();
                }
            }
        }


        // Слушатель выпадающего списка с классами для директора
        private void ComboBoxStatisticsClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl.SelectedIndex != 3)
                return;

            if (directorComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                LoadStatisticsGrid();
            }
        }

        private void SetupStatisticsGrid(DataGridView grid)
        {
            grid.Columns.Clear();
            grid.Rows.Clear();

            grid.Columns.Add("colKey", "Показатель");
            grid.Columns.Add("colValue", "Значение");

            grid.ReadOnly = true;
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.RowHeadersVisible = false;

            grid.Columns["colKey"].FillWeight = 60;
            grid.Columns["colValue"].FillWeight = 40;

            FileLogger.logger.Info("✅ Таблица статистики Ключ|Значение подготовлена");
        }

        private void LoadStatisticsGrid()
        {
            if(UserController.CurrentUser.PermissionID == 1)
            {
                try
                {
                    DateTime startDate = dateTimePickerPersonalStatisticsBefore.Value;
                    DateTime endDate = dateTimePickerPersonalStatisticsAfter.Value;

                    FileLogger.logger.Info($"Загрузка статистики для {UserController.CurrentUser.FullName}: {startDate:dd.MM} - {endDate:dd.MM}");

                    var stats = StatisticsController._controller.GetStudentStatistics(
                        UserController.CurrentUser);

                    dataGridViewPersonalStatistics.Rows.Clear();
                    foreach (var kvp in stats)
                    {
                        dataGridViewPersonalStatistics.Rows.Add(kvp.Key, kvp.Value);
                    }

                    labelStatisticsSummary.Text = $"Статистика за {startDate:dd.MM.yyyy} - {endDate:dd.MM.yyyy} ({stats.Count} показателей)";

                    FileLogger.logger.Info($"✅ Загружено {stats.Count} показателей статистики");
                }
                catch (Exception ex)
                {
                    FileLogger.logger.Error($"Ошибка загрузки статистики: {ex.Message}");
                    dataGridViewPersonalStatistics.Rows.Clear();
                    dataGridViewPersonalStatistics.Rows.Add("Ошибка", ex.Message);
                    labelStatisticsSummary.Text = "Ошибка загрузки статистики";
                }
            }
            else if (UserController.CurrentUser.PermissionID >= 2) // Учитель
            {
                LoadStatisticsGrid(UserController.CurrentUser.Class);

                if (UserController.CurrentUser.PermissionID >= 3)
                {
                    Class clzz = new Class();
                    clzz.ClassID = ((ComboBoxItem) directorComboBox.SelectedItem).ClassID;

                    LoadStatisticsGrid(clzz);
                }
            }
        }

        private void LoadStatisticsGrid(Class classId)
        {
            dataGridViewClassStatistics.Rows.Clear();
            dataGridViewClassStatistics1.Rows.Clear();
            dataGridViewStatisticsClass2.Rows.Clear();

            var students = TeacherController._controller.GetStudentsByClass(classId);

            foreach (User student in students)
            {
                int rowIndex = dataGridViewClassStatistics.Rows.Add();
                var row = dataGridViewClassStatistics.Rows[rowIndex];

                row.Cells["colKey"].Value = student.UserID;
                row.Cells["colValue"].Value = student.FullName;

                dataGridViewClassStatistics.Rows[rowIndex].Tag = student;
            }

            var stats = StatisticsController._controller.GetClassStatisticsFull(classId);

            dataGridViewStatisticsClass2.Rows.Clear();
            dataGridViewStatisticsClass2.Columns.Clear();

            dataGridViewStatisticsClass2.Columns.Add("StatKey", "Предмет");
            dataGridViewStatisticsClass2.Columns.Add("StudentsCount", "Учеников в классе");
            dataGridViewStatisticsClass2.Columns.Add("AverageGrade", "Средний");
            dataGridViewStatisticsClass2.Columns.Add("TotalGrades", "Оценок");
            dataGridViewStatisticsClass2.Columns.Add("MinGrade", "Мин");
            dataGridViewStatisticsClass2.Columns.Add("MaxGrade", "Макс");

            foreach (var kvp in stats)
            {
                dataGridViewStatisticsClass2.Rows.Add(
                    kvp.Key,
                    kvp.Value[0],
                    kvp.Value[1],
                    kvp.Value[2],
                    kvp.Value[3],
                    kvp.Value[4]
                );
            }
        }

        private void dataGridViewClassStatistics_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewClassStatistics.SelectedRows.Count == 0) return;

            DataGridViewRow selectedRow = dataGridViewClassStatistics.SelectedRows[0];
            User selectedStudent = (User) selectedRow.Tag; 

            if (selectedStudent == null)
                return;

            FileLogger.logger.Info($"Выбран ученик: {selectedStudent.FullName} (ID: {selectedStudent.UserID})");

            var statistics = StatisticsController._controller.GetStudentStatistics(selectedStudent);

            FillStatisticsGrid(statistics);
        }

        private void FillStatisticsGrid(Dictionary<string, string> statistics)
        {
            dataGridViewClassStatistics1.Rows.Clear();

            // Создаем колонки если нет
            if (dataGridViewClassStatistics1.Columns.Count == 0)
            {
                dataGridViewClassStatistics1.Columns.Add("Key", "Показатель");
                dataGridViewClassStatistics1.Columns.Add("Value", "Значение");
            }

            foreach (var stat in statistics)
            {
                dataGridViewClassStatistics1.Rows.Add(stat.Key, stat.Value);
            }
        }

        // Проверка на пустые ячейки в строке таблицы оценок.
        // Нужен для того, чтобы добавить строку в список изменений в контроллере.
        // Без метода, при каждой заполненной ячейке будут добавляться полу пустые строки.
        private bool IsGradeRowComplete(DataGridViewRow row)
        {
            string date = row.Cells[1].Value?.ToString()?.Trim();      // colDate
            string subject = row.Cells[2].Value?.ToString()?.Trim();   // colSubject
            string grade = row.Cells[3].Value?.ToString()?.Trim();     // colGrade
            string person = row.Cells[4].Value?.ToString()?.Trim();    // colPerson

            bool allFilled = !string.IsNullOrEmpty(date) &&
                             !string.IsNullOrEmpty(subject) &&
                             !string.IsNullOrEmpty(grade) &&
                             !string.IsNullOrEmpty(person);

            return allFilled;
        }

        // Слушатель изменения оценок.
        // Нужен для добавления ячеек на изменение (вставку) - отправку в БД.
        private void DataGridViewGrades_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (UserController.CurrentUser.PermissionID <= 1) return;

            var row = dataGridViewGrades.Rows[e.RowIndex];

            if (!IsGradeRowComplete(row))
            {
                return;
            }

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

        // Слушатель удаления оценок.
        // Нужен для добавления ячеек на изменение (удаление) - отправку в БД.
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

        // Слушатель закрытия формы.
        // Нужен, так как если не будет закрыта главная форма (которая блять скрыта), то процесс не завершится.
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        public void CommitsAll(int previousTab)
        {
            // Сохранение расписания
            if (previousTab != 3 && UserController.CurrentUser.PermissionID == 3)
            {
                int saved = SheduleController._controller.CommitScheduleChanges();
                if (saved > 0)
                {
                    MessageBox.Show($"✅ Выполнено {saved} изменений расписания!");
                }
            }

            // Сохранение домашки
            if (previousTab != 0)
            {
                int saved = _homeworkController.CommitHomeworkChanges();
                if (saved > 0)
                    MessageBox.Show($"✅ Сохранено {saved} ДЗ!");
            }

            // Сохранение оценок
            if (previousTab != 1)
            {
                int saved = GradesController._controller.CommitGradeChanges();
                if (saved > 0)
                    MessageBox.Show($"✅ Сохранено {saved} оценок!");
            }

            // Сохранение предметов (только директор может менять)
            if (previousTab != 5)
            {
                int saved = SubjectController._controller.CommitSubjectChanges();
                if (saved > 0)
                    MessageBox.Show($"✅ Сохранено {saved} предметов!");
            }
        }

        // Метод смены вкладок.
        // Он также сохраняет изменения, при смене вкладки.
        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Предыдущая хуйня
            // Может сохранить только если предыдущая хуйня не текущая (здесь - расписание)
            int previousTab = tabControl.SelectedIndex;

            CommitsAll(previousTab);

            // Загрузка таблиц
            switch (tabControl.SelectedIndex)
            {
                // Домашка
                case 0: LoadHomeworkGrid(); break;

                // Оценки
                case 1: LoadGradesGrid(); break;

                // Статистика
                case 3: LoadStatisticsGrid(); break;

                // Расписание
                case 2: LoadScheduleGrid(); break;

                // Предметы 
                case 5: LoadSubjects(); break;

                // Сотрудники
                case 6: LoadTeachersGrid(); break;

                // Ученики
                case 7: LoadStudentsGrid(); break;

                // Вкладка "Мероприятия"
                case 4: LoadEventsGrid(); break;
            }
        }

        private bool IsValidSubjectRow(DataGridViewRow row)
        {
            string nameValue = row.Cells["SubjectName"].Value?.ToString()?.Trim();
            return !string.IsNullOrWhiteSpace(nameValue);  // ✅ ТОЛЬКО Name!
        }

        private void SetupSubject()
        {
            FileLogger.logger.Info("=== SetupSubject НАЧАЛО ===");

            bool isDirector = UserController.CurrentUser.PermissionID == 3;
            FileLogger.logger.Info($"isDirector: {isDirector} (PermissionID: {UserController.CurrentUser.PermissionID})");

            dataGridViewSubjects.Columns.Add("SubjectID", "ID");
            dataGridViewSubjects.Columns.Add("SubjectName", "Предмет");
            FileLogger.logger.Info("Колонки добавлены");

            dataGridViewSubjects.ReadOnly = !isDirector;
            dataGridViewSubjects.AllowUserToAddRows = isDirector;
            dataGridViewSubjects.AllowUserToDeleteRows = isDirector;
            dataGridViewSubjects.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;
            dataGridViewSubjects.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewSubjects.MultiSelect = false;

            FileLogger.logger.Info($"ReadOnly: {!isDirector}, AddRows: {isDirector}, DeleteRows: {isDirector}");

            if(isDirector) {
                dataGridViewSubjects.CellValueChanged += dataGridViewSubjects_CellValueChanged;
                dataGridViewSubjects.UserDeletingRow += dataGridViewSubjects_UserDeletedRow;
            }

            FileLogger.logger.Info("Обработчики событий добавлены");

            FileLogger.logger.Info("=== SetupSubject КОНЕЦ ===");
        }

        private void dataGridViewSubjects_UserDeletedRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            FileLogger.logger.Info($"UserDeletedRow: RowIndex={e.Row.Index}");

            // Получаем ID удаленной строки ДО удаления (через контроллер)
            if (e.Row.Index >= 0 && e.Row.Cells["SubjectID"].Value != null)
            {
                if (int.TryParse(e.Row.Cells["SubjectID"].Value.ToString(), out int subjectId))
                {
                    if (subjectId > 0)
                    {
                        FileLogger.logger.Info($"ОТМЕЧАЕМ УДАЛЕНИЕ: ID={subjectId}");
                        SubjectController._controller.AddSubjectChange("DELETE", new Subject { SubjectID = subjectId });
                    }
                }
            }
        }

        private void dataGridViewSubjects_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            FileLogger.logger.Debug($"CellValueChanged: Row={e.RowIndex}, Col={e.ColumnIndex}");

            if (e.RowIndex < 0)
            {
                FileLogger.logger.Debug("RowIndex < 0 - выход");
                return;
            }

            DataGridViewRow row = dataGridViewSubjects.Rows[e.RowIndex];
            if (!IsValidSubjectRow(row))
            {
                FileLogger.logger.Debug($"Строка {e.RowIndex} не валидна");
                return;
            }

            int subjectId = 0;
            var idValue = row.Cells["SubjectID"].Value;
            string nameValue = row.Cells["SubjectName"].Value?.ToString();

            FileLogger.logger.Debug($"ID Value: '{idValue}' | Name: '{nameValue}'");

            // ✅ Если id пустая строка -> -1
            if (idValue == null || idValue.ToString().Trim() == "")
            {
                subjectId = -1;
                FileLogger.logger.Debug("ID пустая строка -> -1 (новый предмет)");
            }
            else if (int.TryParse(idValue.ToString(), out subjectId))
            {
                FileLogger.logger.Debug($"ID распарсен: {subjectId}");
            }
            else
            {
                subjectId = -1;
                FileLogger.logger.Debug("ID не распарсен -> -1 (новый предмет)");
            }

            var subject = new Subject
            {
                SubjectID = subjectId,
                SubjectName = nameValue.Trim()
            };

            FileLogger.logger.Info($"Добавляем изменение: ID={subject.SubjectID} Name='{subject.SubjectName}' (новый: {subject.SubjectID <= 0})");

            SubjectController._controller.AddSubjectChange("EDIT", subject);
        }

        private void LoadSubjects()
        {
            if (UserController.CurrentUser.PermissionID <= 1)
                return;

            FileLogger.logger.Info("=== LoadSubjects НАЧАЛО ===");

            if (dataGridViewSubjects.Columns.Count == 0)
            {
                FileLogger.logger.Info("Колонки отсутствуют - вызываем SetupSubject");
                SetupSubject();
            }
            else
            {
                FileLogger.logger.Info("Колонки уже есть");
            }

            dataGridViewSubjects.Rows.Clear();
            FileLogger.logger.Info("Строки очищены");

            var subjects = SubjectController._controller.GetAllSubjects();
            FileLogger.logger.Info($"Загружено предметов: {subjects.Count}");

            foreach (Subject subject in subjects)
            {
                dataGridViewSubjects.Rows.Add(subject.SubjectID, subject.SubjectName);
                FileLogger.logger.Debug($"Добавлена строка: {subject.SubjectID} - {subject.SubjectName}");
            }

            FileLogger.logger.Info("=== LoadSubjects КОНЕЦ ===");
        }

        // Загрузка расписания с БД.
        // Вызывается при переключении на вкладку "Расписание".
        // Вызывается при смене класса в списке (доступно только при директоре).
        private void LoadScheduleGrid()
        {
            // Если директор и прочая лабуда для проверки
            if (UserController.CurrentUser.PermissionID >= 3 &&
                directorComboBox != null &&
                directorComboBox.SelectedItem is ComboBoxItem selectedItem)
            {

                // Вызвать загрузку с тем классом что выбран в списке
                LoadScheduleGrid(selectedItem.ClassID);
            }
            else
            {
                // Вызвать если не директор, а лох.
                // Учителя также имеют класс - которым они руководствуются.
                // Если учитель не имеет класс. руководства, ему нахуй расписание класса не нужно
                // (будет расписание для конкретного учителя).
                LoadScheduleGrid((int)(UserController.CurrentUser.ClassID ?? 1));

                if(UserController.CurrentUser.PermissionID == 2)
                {
                    AddPersonalScheduleTab();
                }
            }
        }

        private void AddPersonalScheduleTab()
        {
            TabPage personalTab = null;
            foreach (TabPage tab in sheduleTabControl.TabPages)
            {
                if (tab.Text == "Личный")
                {
                    personalTab = tab;
                    break;
                }
            }

            if (personalTab == null)
            {
                personalTab = new TabPage("Личный");

                DataGridView personalGrid = new DataGridView
                {
                    Dock = DockStyle.Fill,
                    Name = "personalScheduleGrid",
                    ReadOnly = true,
                    AllowUserToAddRows = false,
                    AllowUserToDeleteRows = false,
                    SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                    AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                    RowHeadersVisible = false
                };

                // Настраиваем колонки как в основном гриде
                SetupScheduleGridColumns(personalGrid);

                // Загружаем личное расписание
                LoadPersonalSchedule(personalGrid);

                personalTab.Controls.Add(personalGrid);
                sheduleTabControl.TabPages.Add(personalTab);
            }
        }

        private void SetupScheduleGridColumns(DataGridView grid)
        {
            grid.Columns.Clear();
            grid.Columns.Add("colScheduleID", "ID");
            grid.Columns["colScheduleID"].Visible = false;
            grid.Columns.Add("colDay", "День");
            grid.Columns.Add("colLesson", "Урок");
            grid.Columns.Add("colTime", "Время");
            grid.Columns.Add("colSubject", "Предмет");
            grid.Columns.Add("colTeacher", "Учитель");
        }

        private void LoadPersonalSchedule(DataGridView grid)
        {
            try
            {
                var teacherSchedule = SheduleController._controller.GetScheduleForTeacher(UserController.CurrentUser.UserID);

                grid.Rows.Clear();
                foreach (var item in teacherSchedule)
                {
                    grid.Rows.Add(
                        item.ScheduleID,
                        item.DayOfWeekDisplay,
                        item.LessonNumber,
                        item.LessonTimeDisplay,
                        item.SubjectName,
                        item.TeacherName
                    );
                }

                sheduleLabel.Text = $"Личное расписание: {teacherSchedule.Count} уроков";
            }
            catch (Exception ex)
            {
                grid.Rows.Clear();
                grid.Rows.Add(0, "", "", "", "Ошибка загрузки", ex.Message);
                sheduleLabel.Text = "Ошибка загрузки личного расписания";
            }
        }

        // Загрузка расписания конкретного класса.
        private void LoadScheduleGrid(int classId)
        {
            try
            {
                // Отвязка данных, иначе нельзя будет редактировать
                sheduleGridView.DataSource = null;

                // Очистка от предущего мусора
                sheduleGridView.Columns.Clear();
                sheduleGridView.Rows.Clear();

                // Если список для директора ещё не появился
                if(directorComboBox == null)
                {

                    // Загрузить список с классами для расписания
                    InitializeScheduleClassCombo();
                }

                // Текущее расписание которое есть в БД.
                var scheduleList = SheduleController._controller.GetScheduleForClass(classId);

                // Подготовить таблицу для расписания (внешний вид).
                SetupScheduleGrid();

                // Ручное заполнение таблицы.
                foreach (var schedule in scheduleList)
                {
                    sheduleGridView.Rows.Add(
                        schedule.ScheduleID, // Идентификатор расписания
                        schedule.DayOfWeekDisplay, // День недели
                        schedule.LessonNumber, // Номер урока
                        schedule.LessonTimeDisplay ?? "", // Время начала
                        schedule.SubjectName, // Предмет
                        schedule.TeacherName // Учитель
                    );
                }

                string className = classId == 1 ? "5А" : $"Класс {classId}";

                // Текст для класса расписания - под таблицей
                sheduleLabel.Text = $"Расписание класса {className} ({scheduleList.Count} уроков)";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        // Подготовка (визуально) таблицы расписания.
        private void SetupScheduleGrid()
        {
            sheduleGridView.Columns.Clear();
            bool isDirector = UserController.CurrentUser.PermissionID == 3;

            sheduleGridView.Columns.Add("colScheduleID", "ID");
            sheduleGridView.Columns["colScheduleID"].Visible = false;
            sheduleGridView.Columns.Add("colDay", "День");
            sheduleGridView.Columns.Add("colLesson", "Урок");
            sheduleGridView.Columns.Add("colTime", "Время");
            sheduleGridView.Columns.Add("colSubject", "Предмет");
            sheduleGridView.Columns.Add("colTeacher", "Учитель");

            sheduleGridView.ReadOnly = !isDirector; 
            sheduleGridView.AllowUserToAddRows = isDirector;
            sheduleGridView.AllowUserToDeleteRows = isDirector;
            sheduleGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            sheduleGridView.EditMode = DataGridViewEditMode.EditOnKeystroke;
            sheduleGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            sheduleGridView.RowHeadersVisible = false;
        }

        // Получить текущий класс из выпадающего списка.
        private int GetCurrentClassId()
        {
            try
            {
                FileLogger.logger.Info(
                    $"GetCurrentClassId START. UserID={UserController.CurrentUser.UserID}, " +
                    $"PermissionID={UserController.CurrentUser.PermissionID}, " +
                    $"Role={UserController.CurrentUser.PermissionName}");

                if (UserController.CurrentUser.PermissionID == 3)
                {
                    if (directorComboBox == null)
                    {
                        FileLogger.logger.Warn(
                            "GetCurrentClassId: PermissionID=3 (директор), но comboBoxScheduleClass == null. " +
                            "Возвращаем ClassID пользователя.");
                    }
                    else
                    {
                        if (directorComboBox.SelectedItem is ComboBoxItem selectedItem)
                        {
                            FileLogger.logger.Info(
                                $"GetCurrentClassId: выбран класс из комбобокса: ClassID={selectedItem.ClassID}, Text='{selectedItem.Text}'");
                            return selectedItem.ClassID;
                        }
                        else
                        {
                            FileLogger.logger.Warn(
                                "GetCurrentClassId: comboBoxScheduleClass.SelectedItem НЕ ComboBoxItem или null. " +
                                "Text='" + directorComboBox.Text + "'. Возвращаем ClassID пользователя.");
                        }
                    }
                }

                int fallbackClassId = (int)(UserController.CurrentUser.ClassID ?? 1);
                FileLogger.logger.Info(
                    $"GetCurrentClassId: используем ClassID пользователя: {fallbackClassId}");
                return fallbackClassId;
            }
            catch (Exception ex)
            {
                FileLogger.logger.Error(
                    "GetCurrentClassId: необработанное исключение. Возвращаем ClassID=1 по умолчанию.", ex);
                return 1;
            }
        }

        // Парсинг для недели
        private byte GetDayNumber(string dayName)
        {
            var days = new Dictionary<string, byte>
            {
                ["Пн"] = 1,
                ["Вт"] = 2,
                ["Ср"] = 3,
                ["Чт"] = 4,
                ["Пт"] = 5,
                ["Сб"] = 6,
                ["Вс"] = 7
            };

            return days.TryGetValue(dayName, out byte num) ? num : (byte)1;
        }

        // Инициализация выпадающего списка с классами.
        private void InitializeScheduleClassCombo()
        {
            bool isDirector = UserController.CurrentUser.PermissionID >= 3;

            if (isDirector)
            {
                directorComboBox = new ComboBox
                {
                    Location = new Point(10, 10),
                    Size = new Size(120, 25),
                    DropDownStyle = ComboBoxStyle.DropDownList
                };

                var classes = ClassController._controller.GetAllClasses();
                directorComboBox.Items.Clear();

                foreach (var cls in classes)
                {
                    var item = new ComboBoxItem { Text = cls.ClassName, ClassID = cls.ClassID };
                    directorComboBox.Items.Add(item);
                }

                if (directorComboBox.Items.Count > 0)
                    directorComboBox.SelectedIndex = (int) (UserController.CurrentUser.ClassID == null ? 0 : UserController.CurrentUser.ClassID);

                directorComboBox.SelectedIndexChanged += ComboBoxScheduleClass_SelectedIndexChanged;
                directorComboBox.SelectedIndexChanged += ComboBoxStatisticsClass_SelectedIndexChanged;

                this.Controls.Add(directorComboBox);
                directorComboBox.BringToFront();
            }
        }

        // Слушатель выпадающего списка с классами для директора
        private void ComboBoxScheduleClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (directorComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                LoadScheduleGrid(selectedItem.ClassID);
            }
        }

        // Слушатель изменений (вставка) в расписании.
        private void DataGridViewSchedule_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            FileLogger.logger.Info($"🔍 CellEndEdit: Row={e.RowIndex}, Col={e.ColumnIndex}, Time={DateTime.Now:HH:mm:ss.fff}");

            if (UserController.CurrentUser.PermissionID != 3)
            {
                FileLogger.logger.Debug("👤 Нет прав (не директор)");
                return;
            }
            if (e.RowIndex < 0 || sheduleGridView.Rows[e.RowIndex].IsNewRow)
            {
                FileLogger.logger.Debug($"⏭️ Пропуск: RowIndex={e.RowIndex}, IsNewRow={sheduleGridView.Rows[e.RowIndex].IsNewRow}");
                return;
            }

            var row = sheduleGridView.Rows[e.RowIndex];
            FileLogger.logger.Info($"📊 Строка {e.RowIndex}: [{string.Join("|", row.Cells.Cast<DataGridViewCell>().Select(c => c.Value ?? "NULL"))}]");

            try
            {
                string dayText = row.Cells[1].Value?.ToString()?.Trim() ?? "";
                string lessonText = row.Cells[2].Value?.ToString()?.Trim() ?? "";
                string timeText = row.Cells[3].Value?.ToString()?.Trim() ?? "";
                string subjectName = row.Cells[4].Value?.ToString()?.Trim() ?? "";
                string teacherName = row.Cells[5].Value?.ToString()?.Trim() ?? "";

                FileLogger.logger.Debug($"📝 Поля: День='{dayText}', Урок='{lessonText}', Время='{timeText}', Предмет='{subjectName}', Учитель='{teacherName}'");

                if (string.IsNullOrWhiteSpace(dayText) ||
                    string.IsNullOrWhiteSpace(lessonText) ||
                    string.IsNullOrWhiteSpace(subjectName) ||
                    string.IsNullOrWhiteSpace(teacherName))
                {
                    FileLogger.logger.Warn("⚠️ Недостаточно данных — пропуск");
                    return;
                }

                int scheduleId = int.TryParse(row.Cells[0].Value?.ToString() ?? "0", out int id) ? id : 0;
                byte dayOfWeek = GetDayNumber(dayText);
                byte lessonNumber = byte.TryParse(lessonText, out byte ln) ? ln : (byte)1;

                TimeSpan? lessonTime = null;
                if (!string.IsNullOrWhiteSpace(timeText))
                {
                    if (TimeSpan.TryParse(timeText, out TimeSpan timeParsed))
                    {
                        lessonTime = timeParsed;
                    }
                }

                int classId = GetCurrentClassId();
                FileLogger.logger.Debug($"🔢 Парсинг: ID={scheduleId}, День={dayOfWeek}, Урок={lessonNumber}, Класс={classId}, Время={lessonTime}");

                int subjectId = SubjectController._controller.GetSubjectIdByName(subjectName);
                var teacher = TeacherController._controller.GetTeacherOrDirectorByName(teacherName);
                int teacherId = teacher?.UserID ?? 0;

                FileLogger.logger.Debug($"👨‍🏫‍ ПредметID={subjectId}, УчительID={teacherId}");

                if (subjectId == 0 || teacherId == 0)
                {
                    FileLogger.logger.Warn($"❌ Предмет/учитель не найдены: {subjectName}/{teacherName}");
                    MessageBox.Show($"❌ Предмет/учитель не найдены: {subjectName}/{teacherName}");
                    return;
                }

                var schedule = new ScheduleItem
                {
                    ScheduleID = scheduleId,
                    DayOfWeek = dayOfWeek,
                    LessonNumber = lessonNumber,
                    ClassID = classId,
                    SubjectID = subjectId,
                    TeacherID = teacherId,
                    LessonTime = lessonTime
                };

                FileLogger.logger.Info($"➕ ScheduleItem готов: {schedule.ScheduleID} | {schedule.DayOfWeek} | {schedule.LessonNumber} | {schedule.SubjectID}/{schedule.TeacherID}");

                string action = scheduleId > 0 ? "EDIT" : "ADD";

                FileLogger.logger.Info($"📤 ДО AddScheduleChange: PendingCount={SheduleController._controller.PendingChangesCount}");
                SheduleController._controller.AddScheduleChange(action, schedule);
                FileLogger.logger.Info($"📤 ПОСЛЕ AddScheduleChange: PendingCount={SheduleController._controller.PendingChangesCount}");

                MessageBox.Show($"✅ {action} урока в очереди ({SheduleController._controller.PendingChangesCount} всего)");
                FileLogger.logger.Info($"✅ {action} ДОБАВЛЕНО! Итого в очереди: {SheduleController._controller.PendingChangesCount}");
            }
            catch (Exception ex)
            {
                FileLogger.logger.Error($"💥 ОШИБКА CellEndEdit: {ex}");
                MessageBox.Show($"❌ Ошибка обработки строки: {ex.Message}");
            }
        }

        // Слушатель изменений (удаление) в расписании.
        // Меняет только флаг.
        private void DataGridViewSchedule_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            if (UserController.CurrentUser.PermissionID != 3) return;

            int scheduleId = int.TryParse(e.Row.Cells[0].Value?.ToString() ?? "0", out int id) ? id : 0;

            if (scheduleId > 0)
            {
                // ✅ Добавляем удаление в очередь контроллера
                var schedule = new ScheduleItem { ScheduleID = scheduleId };
                SheduleController._controller.AddScheduleChange("DELETE", schedule);

                MessageBox.Show($"✅ Удаление урока #{scheduleId} в очереди ({SheduleController._controller.PendingChangesCount} всего)");
            }
        }

        // Проверка на пустые ячейки в строке таблицы домашнего задания.
        // Нужен по той же причине что и с оценками.
        private bool IsHomeworkRowComplete(DataGridViewRow row)
        {
            string date = row.Cells["colDate"].Value?.ToString()?.Trim();
            string cls = row.Cells["colClass"].Value?.ToString()?.Trim();
            string subject = row.Cells["colSubject"].Value?.ToString()?.Trim();
            string desc = row.Cells["colDescription"].Value?.ToString()?.Trim();

            return !string.IsNullOrEmpty(date) &&
                   !string.IsNullOrEmpty(cls) &&
                   !string.IsNullOrEmpty(subject) &&
                   !string.IsNullOrEmpty(desc);
        }

        // Слушатель изменений (вставка) ячеек таблицы с домашкой
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

        // Слушатель изменений (удаление) ячеек таблицы с домашкой
        private void DataGridViewHomework_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            if (UserController.CurrentUser.PermissionID < 2) return;

            var row = e.Row;

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

        // Загрузка (из БД) таблицы домашнего задания.
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

                dataGridViewHomework.DataSource = null;
                dataGridViewHomework.Columns.Clear();
                dataGridViewHomework.Rows.Clear();

                SetupHomeworkGrid();

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

        // Подготовка таблицы для домашнего задания
        private void SetupHomeworkGrid()
        {
            dataGridViewHomework.Columns.Clear();

            bool isTeacher = UserController.CurrentUser.PermissionID > 1;

            // ✅ 0. СКРЫТАЯ колонка ID (ПЕРВАЯ!)
            dataGridViewHomework.Columns.Add("colHomeworkID", "ID");
            dataGridViewHomework.Columns["colHomeworkID"].Visible = false;  // ✅ СКРЫТЬ!

            // ✅ 1-5. Видимые колонки
            var dateCol = new CalendarColumn();
            dateCol.Name = "colDate";
            dateCol.HeaderText = "Дата";
            dateCol.Width = 100;
            dataGridViewHomework.Columns.Add(dateCol);

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

        // Загрузка оценок из БД
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

                SetupGradesGrid();

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

        // Подготовка таблицы для оценок
        private void SetupGradesGrid()
        {
            dataGridViewGrades.Columns.Clear();
            bool isTeacher = UserController.CurrentUser.PermissionID > 1;

            // ✅ 0. СКРЫТАЯ колонка GradeID
            dataGridViewGrades.Columns.Add("colGradeID", "ID");
            dataGridViewGrades.Columns["colGradeID"].Visible = false;

            // ✅ 1. Дата
            var dateCol = new CalendarColumn();
            dateCol.Name = "colDate";
            dateCol.HeaderText = "Дата";
            dateCol.Width = 100;
            dataGridViewGrades.Columns.Add(dateCol);

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

        private void dateTimePickerHomework_ValueChanged(object sender, EventArgs e)
        {
            if (tabControl.SelectedIndex == 0)
                LoadHomeworkGrid();
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

        /* Сегмент с вкладкой "Мероприятия" */
        private void LoadEventsGrid()
        {
            FileLogger.logger.Info("🔄 LoadEventsGrid: загрузка мероприятий");

            try
            {
                dataGridViewEvents.Rows.Clear();

                if (dataGridViewEvents.Columns.Count == 0)
                {
                    CreateEventsColumns();
                    FileLogger.logger.Debug("📐 Колонки мероприятий созданы");
                }

                var events = EventsController._controller.GetAllEvents(); 
                FileLogger.logger.Info($"📊 Получено {events.Count} мероприятий");

                if (events.Count == 0)
                {
                    FileLogger.logger.Warn("⚠️ Мероприятия не найдены");
                    return;
                }

                foreach (var ev in events)
                {
                    dataGridViewEvents.Rows.Add(
                        ev.EventID,
                        ev.EventName,
                        ev.EventTime.ToString("dd.MM.yyyy HH:mm"), 
                        ev.Location
                    );

                    FileLogger.logger.Debug($"➕ {ev.EventName} | {ev.EventTime:dd.MM HH:mm} | {ev.Location}");
                }

                bool isDirector = UserController.CurrentUser.PermissionID >= 3;

                dataGridViewEvents.ReadOnly = !isDirector;  
                dataGridViewEvents.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridViewEvents.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                FileLogger.logger.Info($"Загружено {dataGridViewEvents.Rows.Count} мероприятий");
            }
            catch (Exception ex)
            {
                FileLogger.logger.Error($"LoadEventsGrid: {ex.Message}");
                MessageBox.Show($"Ошибка загрузки мероприятий: {ex.Message}");
            }
        }

        private void CreateEventsColumns()
        {
            dataGridViewEvents.Columns.Clear();

            dataGridViewEvents.Columns.Add("EventID", "ID");
            dataGridViewEvents.Columns.Add("EventName", "Название");
            dataGridViewEvents.Columns.Add("EventTime", "Дата/Время");
            dataGridViewEvents.Columns.Add("Location", "Место");

            dataGridViewEvents.Columns["EventID"].Visible = false;
            dataGridViewEvents.Columns["EventID"].ReadOnly = true;

            dataGridViewEvents.Columns["EventName"].FillWeight = 50;
            dataGridViewEvents.Columns["EventTime"].FillWeight = 30;
            dataGridViewEvents.Columns["Location"].FillWeight = 20;

            FileLogger.logger.Debug("Колонки Events настроены");
        }

        /* Сегмент с вкладкой "Сотрудники" */

        private void LoadTeachersGrid()
        {
            FileLogger.logger.Info("🔄 LoadTeachersGrid: Начало загрузки сотрудников");

            try
            {
                // ✅ Очищаем грид
                dataGridViewTeachers.Rows.Clear();

                if (dataGridViewTeachers.Columns.Count == 0)
                {
                    FileLogger.logger.Debug("Колонки отсутствуют, создаём...");

                    SetupTeachersGridColumns();

                    FileLogger.logger.Debug("Колонки созданы");
                }

                // ✅ Получаем данные из контроллера/модели
                var teachers = TeacherController._controller.GetAllTeachers();  // Твой метод List<User>
                FileLogger.logger.Info($"📊 Получено {teachers.Count} сотрудников из БД");

                if (teachers.Count == 0)
                {
                    FileLogger.logger.Warn("⚠️ Список сотрудников пуст");
                    return;
                }

                // ✅ Заполняем строки
                foreach (var user in teachers)
                {
                    dataGridViewTeachers.Rows.Add(
                        user.UserID,
                        user.FullName,
                        user.PermissionName,
                        user.PermissionID,
                        user.ClassID?.ToString() ?? "",
                        user.Class?.ClassName ?? "-"  
                    );

                    FileLogger.logger.Debug($"➕ Добавлена строка: {user.FullName} ({user.PermissionName})");
                }

                FileLogger.logger.Info($"✅ Загружено {dataGridViewTeachers.Rows.Count} строк в грид");

                FileLogger.logger.Debug("⚙️ Колонки настроены");

                // ✅ Финальная настройка грида
                dataGridViewTeachers.ReadOnly = true;
                dataGridViewTeachers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridViewTeachers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                FileLogger.logger.Info("🎉 LoadTeachersGrid: ЗАВЕРШЕНО успешно");
            }
            catch (Exception ex)
            {
                FileLogger.logger.Error($"💥 LoadTeachersGrid: ОШИБКА = {ex.Message}\n{ex.StackTrace}");
                MessageBox.Show($"Ошибка загрузки сотрудников:\n{ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupTeachersGridColumns()
        {
            if (dataGridViewTeachers.Columns.Count != 0)
                return;

            if (dataGridViewTeachers.Columns.Count > 0)
                dataGridViewTeachers.Columns.Clear();

            dataGridViewTeachers.Columns.Add("UserID", "ID");
            dataGridViewTeachers.Columns.Add("FullName", "ФИО");
            dataGridViewTeachers.Columns.Add("PermissionName", "Роль");
            dataGridViewTeachers.Columns.Add("PermissionID", "Роль ID");
            dataGridViewTeachers.Columns.Add("ClassID", "Класс ID");
            dataGridViewTeachers.Columns.Add("ClassName", "Руководство классом");

            dataGridViewTeachers.Columns[0].HeaderText = "ID";
            dataGridViewTeachers.Columns[0].Visible = false;  

            dataGridViewTeachers.Columns[1].HeaderText = "ФИО";
            dataGridViewTeachers.Columns[1].FillWeight = 40;

            dataGridViewTeachers.Columns[2].HeaderText = "Роль";
            dataGridViewTeachers.Columns[2].FillWeight = 25;

            dataGridViewTeachers.Columns[3].HeaderText = "Роль ID";
            dataGridViewTeachers.Columns[3].Visible = false;

            dataGridViewTeachers.Columns[4].HeaderText = "Класс ID";
            dataGridViewTeachers.Columns[4].Visible = false;

            if (dataGridViewTeachers.Columns.Count > 5)
            {
                dataGridViewTeachers.Columns[5].HeaderText = "Класс";
                dataGridViewTeachers.Columns[5].FillWeight = 35;
            }

            FileLogger.logger.Debug($"📐 Колонки: {dataGridViewTeachers.Columns.Count} шт.");
        }

        /* Сегмент с вкладкой "Ученики" */

        private void LoadStudentsGrid()
        {
            FileLogger.logger.Info("🔄 LoadStudentsGrid: загрузка учеников");

            try
            {
                dataGridViewStudents.Rows.Clear();
                CreateStudentsColumns();

                var students = TeacherController._controller.GetAllStudents();
                FileLogger.logger.Info($"📊 Получено {students.Count} учеников");

                foreach (var student in students)
                {
                    var row = dataGridViewStudents.Rows[dataGridViewStudents.Rows.Add()];

                    dataGridViewStudents.Rows[row.Index].Cells["UserID"].Value = student.UserID;
                    dataGridViewStudents.Rows[row.Index].Cells["FullName"].Value = student.FullName;
                    dataGridViewStudents.Rows[row.Index].Cells["PermissionName"].Value = student.PermissionName;
                    dataGridViewStudents.Rows[row.Index].Cells["ClassID"].Value = student.ClassID?.ToString() ?? "";

                    dataGridViewStudents.Rows[row.Index].Cells["ClassName"].Value =
                        student.Class?.ClassName ?? "-";  

                    FileLogger.logger.Debug($"➕ {student.FullName} | Класс: {student.Class?.ClassName ?? "нет класса"}");
                }

                dataGridViewStudents.ReadOnly = true;
                dataGridViewStudents.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                FileLogger.logger.Info($"✅ Загружено {dataGridViewStudents.Rows.Count} учеников");
            }
            catch (Exception ex)
            {
                FileLogger.logger.Error($"💥 LoadStudentsGrid: {ex.Message}\n{ex.StackTrace}");
            }
        }

        private void CreateStudentsColumns()
        {
            if (dataGridViewStudents.Columns.Count > 0)
                dataGridViewStudents.Columns.Clear();

            dataGridViewStudents.Columns.Add("UserID", "ID");
            dataGridViewStudents.Columns.Add("FullName", "ФИО");
            dataGridViewStudents.Columns.Add("PermissionName", "Роль");
            dataGridViewStudents.Columns.Add("ClassID", "Класс ID");     
            dataGridViewStudents.Columns.Add("ClassName", "Класс");      

            dataGridViewStudents.Columns["UserID"].Visible = false;
            dataGridViewStudents.Columns["ClassID"].Visible = false;

            dataGridViewStudents.Columns["FullName"].FillWeight = 50;
            dataGridViewStudents.Columns["PermissionName"].FillWeight = 25;
            dataGridViewStudents.Columns["ClassName"].FillWeight = 25;  
        }

        private void exitBtnm_Click(object sender, EventArgs e)
        {
            Hide();
            new LoginForm().Show();
        }

        private void buttonPrintShedule_Click(object sender, EventArgs e)
        {
            printDocument1.PrintPage += printDocument1_PrintPage;
            printDocumentDialog1.Document = printDocument1;

            if (printDocumentDialog1.ShowDialog() == DialogResult.OK)
            {
                printDocument1.Print();
            }
        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            float x = 40f;
            float y = 80f;
            float pageWidth = 750f;

            // Заголовок
            e.Graphics.DrawString("📅 РАСПИСАНИЕ УРОКОВ", new Font("Arial", 16, FontStyle.Bold), Brushes.Black, x + (pageWidth - 200) / 2, y);
            y += 50;

            float colWidth = pageWidth / sheduleGridView.ColumnCount;

            // Заголовки (28px высота)
            for (int col = 0; col < sheduleGridView.ColumnCount; col++)
            {
                float colX = x + col * colWidth;
                e.Graphics.FillRectangle(Brushes.LightBlue, colX, y, colWidth, 28);
                e.Graphics.DrawRectangle(Pens.DarkBlue, colX, y, colWidth, 28);
                e.Graphics.DrawString(sheduleGridView.Columns[col].HeaderText, new Font("Arial", 10, FontStyle.Bold), Brushes.White, colX + 5, y + 5);
            }
            y += 32;

            // ✅ ДАННЫЕ С ДИНАМИЧЕСКОЙ ВЫСОТОЙ!
            for (int row = 0; row < sheduleGridView.RowCount && y < 1050; row++)
            {
                // ✅ ВЫЧИСЛЯЕМ НУЖНУЮ ВЫСОТУ для этой строки
                float neededHeight = GetRowHeight(e.Graphics, row, colWidth, x);

                // 1. ФОН
                for (int col = 0; col < sheduleGridView.ColumnCount; col++)
                {
                    float colX = x + col * colWidth;
                    e.Graphics.FillRectangle(Brushes.White, colX, y, colWidth, neededHeight);
                }

                // 2. РАМКИ (УВЕЛИЧЕННЫЕ!)
                for (int col = 0; col < sheduleGridView.ColumnCount; col++)
                {
                    float colX = x + col * colWidth;
                    e.Graphics.DrawRectangle(Pens.Gray, colX, y, colWidth, neededHeight);
                }

                // 3. ТЕКСТ С ПЕРЕНОСОМ
                for (int col = 0; col < sheduleGridView.ColumnCount; col++)
                {
                    float colX = x + col * colWidth;
                    string cellText = sheduleGridView.Rows[row].Cells[col].Value?.ToString() ?? "";
                    DrawTextWithWrap(e.Graphics, cellText, colX + 4, y + 4, colWidth - 8, new Font("Arial", 8));
                }

                y += neededHeight; // ✅ ДИНАМИЧЕСКИ!
            }
        }

        // ✅ ВЫЧИСЛЯЕМ ВЫСОТУ СТРОКИ ПО СОДЕРЖИМОМУ
        private float GetRowHeight(Graphics g, int rowIndex, float colWidth, float startX)
        {
            float maxHeight = 24f;
            Font font = new Font("Arial", 8);
            float lineHeight = g.MeasureString("А", font).Height + 3;

            for (int col = 0; col < sheduleGridView.ColumnCount; col++)
            {
                string text = sheduleGridView.Rows[rowIndex].Cells[col].Value?.ToString() ?? "";
                float colHeight = CalculateTextHeight(g, text, colWidth - 8, font);
                if (colHeight > maxHeight) maxHeight = colHeight;
            }

            return Math.Max(24f, maxHeight);
        }

        // ✅ ПЕРЕНОС ТЕКСТА
        private void DrawTextWithWrap(Graphics g, string text, float x, float y, float maxWidth, Font font)
        {
            if (string.IsNullOrEmpty(text)) return;

            string[] words = text.Split(' ');
            string currentLine = "";
            float lineHeight = g.MeasureString("А", font).Height + 3;

            foreach (string word in words)
            {
                string testLine = string.IsNullOrEmpty(currentLine) ? word : currentLine + " " + word;
                if (g.MeasureString(testLine, font).Width > maxWidth)
                {
                    g.DrawString(currentLine, font, Brushes.Black, x, y);
                    y += lineHeight;
                    currentLine = word;
                }
                else
                {
                    currentLine = testLine;
                }
            }
            g.DrawString(currentLine, font, Brushes.Black, x, y);
        }

        // ✅ ВЫЧИСЛЯЕМ ВЫСОТУ ТЕКСТА
        private float CalculateTextHeight(Graphics g, string text, float maxWidth, Font font)
        {
            if (string.IsNullOrEmpty(text)) return 0;

            string[] words = text.Split(' ');
            string currentLine = "";
            float lineHeight = g.MeasureString("А", font).Height + 3;
            int lines = 1;

            foreach (string word in words)
            {
                string testLine = string.IsNullOrEmpty(currentLine) ? word : currentLine + " " + word;
                if (g.MeasureString(testLine, font).Width > maxWidth)
                {
                    lines++;
                    currentLine = word;
                }
                else
                {
                    currentLine = testLine;
                }
            }

            return lines * lineHeight;
        }
    }
}