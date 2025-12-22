using school.Controllers;
using school.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;
using static school.Controllers.HomeworkController;
using User = school.Models.User;

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
        // Может меняться для тестов
        public static string CONNECTION_STRING
                          = "Server=(localdb)\\MSSQLLocalDB;Database=" + Controller.DATABASE_NAME + ";Integrated Security=true;";
        //            = "Server=localhost\\SQLEXPRESS;Database=SchoolSystemTest3;Integrated Security=true;Encrypt=false;TrustServerCertificate=true;";

        // Конструктор формы
        public Form1()
        {
            // Понятно, инициализация элементов
            InitializeComponent();

            // Текст с ролью и именем
            labelRole.Text = UserController.CurrentUser.PermissionName + " - " + UserController.CurrentUser.FullName;

            // Подготовка всякой хуйни для ролевой системы
            PrepareRole();
        }

        // Подговтока ролевых ограничений
        private void PrepareRole()
        {
            // Роль ученика
            if (UserController.CurrentUser.PermissionID == 1)
            {
                // Статистика 
                // У ученика удалить вкладку со статистикой класса
                tabControlStatistic.TabPages.RemoveByKey("tabPageClass");
                tabControlStatistic.SelectedTab = tabControlStatistic.TabPages["tabPersonalStats"];

                // Визуальная подготовка статистика
                SetupStatisticsGrid(dataGridViewPersonalStatistics);

                // Удалить отчеты нахуй
                tabControl.TabPages.RemoveByKey("tabPageReports");

                // Удалить предметы нахуй
                tabControl.TabPages.RemoveByKey("tabPageSubjects");

                // Посещаемость класса - нахуй
                tabControlAttendance.TabPages.RemoveByKey("tabPage7");
            }
            // Учитель может смотреть статистику своего класса.
            // Директор может менять класс с помощью comboBox.
            else if (UserController.CurrentUser.PermissionID >= 2)
            {
                // У учителя не может быть личной статистики
                tabControlStatistic.TabPages.RemoveByKey("tabPagePersonal");

                // Слушатель на отображение статистики определённого ученика
                dataGridViewClassStatistics.SelectionChanged += dataGridViewClassStatistics_SelectionChanged;

                // Подготовка внешнего вида ебанной таблицы со статистикой
                SetupStatisticsGrid(dataGridViewClassStatistics);

                // есть всплывающий список, который позволяет менять класс
                InitializeScheduleClassCombo();

                // Роль: Директор
                if (UserController.CurrentUser.PermissionID >= 3)
                {
                    // Слушатели на мероприятия
                    // Только у директора - но это с-с-порно!
                    dataGridViewEvents.CellEndEdit += DataGridViewEvents_CellEndEdit;
                    dataGridViewEvents.UserDeletingRow += DataGridViewEvents_UserDeletingRow;
                }

                // Личная посещаемость есть только у ученика
                tabControlAttendance.TabPages.RemoveByKey("tabPage8");

                // Хандлеры таблицы с домашкой
                dataGridViewHomework.CellEndEdit += DataGridViewHomework_CellEndEdit;
                dataGridViewHomework.UserDeletedRow += DataGridViewHomework_UserDeletedRow;

                // Хандлеры таблицы с оценками
                dataGridViewGrades.CellEndEdit += DataGridViewGrades_CellEndEdit;
                dataGridViewGrades.UserDeletedRow += DataGridViewGrades_UserDeletedRow;

                // Хандлеры таблицы с расписанием
                sheduleGridView.CellEndEdit += DataGridViewSchedule_CellEndEdit;
                sheduleGridView.UserDeletedRow += DataGridViewSchedule_UserDeletedRow;

                // Хандлеры таблицы с учениками
                dataGridViewStudents.CellEndEdit += DataGridViewStudents_CellEndEdit;
                dataGridViewStudents.UserDeletingRow += DataGridViewStudents_UserDeletingRow;

                // Хандлеры таблицы с учителями
                dataGridViewTeachers.CellEndEdit += DataGridViewTeachers_CellEndEdit;
                dataGridViewTeachers.UserDeletingRow += DataGridViewTeachers_UserDeletingRow;

                dataGridViewClassAtterdance.UserDeletingRow += DataGridViewClassAtterdance_UserDeletingRow;
            }
        }

        private void DataGridViewClassAtterdance_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            var row = e.Row;

            string attIdStr = row.Cells["AttendanceID"].Value?.ToString()?.Trim();
            string studentName = row.Cells["StudentName"].Value?.ToString()?.Trim();
            string subjectName = row.Cells["SubjectName"].Value?.ToString()?.Trim();
            string lessonDateStr = row.Cells["AttendanceDate"].Value?.ToString()?.Trim();
            string lessonTimeStr = row.Cells["LessonDate"].Value?.ToString()?.Trim();
            string statusStr = row.Cells["Status"].Value?.ToString()?.Trim();
            string commentStr = row.Cells["Comment"].Value?.ToString()?.Trim();

            if (string.IsNullOrEmpty(studentName) || string.IsNullOrEmpty(subjectName) ||
                string.IsNullOrEmpty(lessonDateStr) || string.IsNullOrEmpty(statusStr))
            {
                FileLogger.logger.Warn($"dataGridViewClassAttendance_CellValueChanged - Пропускаем - пустые обязательные поля (Row={e.Row.Index}): Student={studentName?.Length ?? 0}, Subject={subjectName?.Length ?? 0}, Date={lessonDateStr?.Length ?? 0}, Status={statusStr?.Length ?? 0}");
                return;
            }

            int attendanceId = -1;
            if (!string.IsNullOrEmpty(attIdStr) && int.TryParse(attIdStr, out int parsedId))
            {
                attendanceId = parsedId;
                FileLogger.logger.Debug($"dataGridViewClassAttendance_CellValueChanged - ID посещения распарсен: {attendanceId}");
            }
            else
            {
                FileLogger.logger.Debug($"dataGridViewClassAttendance_CellValueChanged - Новый запись посещения (Row={e.Row.Index})");
            }

            Attendance attendance;
            try
            {
                attendance = new Attendance
                {
                    AttendanceID = attendanceId,
                    SubjectID = SubjectController._controller.GetSubjectIdByName(subjectName),
                    LessonDate = DateTime.Parse(lessonTimeStr),
                    AttendanceDate = DateTime.Parse(lessonDateStr).Date,

                    UserID = UserController._userController.GetStudentIdByName(studentName),
                    Present = statusStr.Contains("Присут"),
                    ExcuseReason = statusStr?.Contains("оправдание") ?? false,
                    Comment = commentStr ?? ""
                };

                FileLogger.logger.Info($"dataGridViewClassAttendance_CellValueChanged - Сформирован объект Attendance: ID={attendance.AttendanceID}, StudentID={attendance.UserID}, SubjectID={attendance.SubjectID}, Present={attendance.Present}, Date={attendance.LessonDate:yyyy-MM-dd}");
            }
            catch (Exception ex)
            {
                FileLogger.logger.Error($"dataGridViewClassAttendance_CellValueChanged - Ошибка создания Attendance (Row={e.Row.Index}): {ex.Message}\nStudent='{studentName}', Date='{lessonDateStr}', Status='{statusStr}'");
                return;
            }

            try
            {
                FileLogger.logger.Info($"dataGridViewClassAttendance_CellValueChanged - Очередь ДО добавления: {AtterdanceController.Instance.PendingChangesCount} изменений");

                AtterdanceController.Instance.AddAttendanceChange("DELETE", attendance);

                FileLogger.logger.Info($"dataGridViewClassAttendance_CellValueChanged - Очередь ПОСЛЕ добавления: {AtterdanceController.Instance.PendingChangesCount} изменений");
                FileLogger.logger.Info($"dataGridViewClassAttendance_CellValueChanged - Изменения сохранены: {attendance.AttendanceID} ({(attendanceId == -1 ? "NEW" : "EDIT")})");
            }
            catch (Exception ex)
            {
                FileLogger.logger.Error($"dataGridViewClassAttendance_CellValueChanged - Ошибка сохранения (Row={e.Row.Index}): {ex.Message}");
            }
        }

        private void DataGridViewStudents_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (e.Row.Index < 0 || e.Row.Index >= dataGridViewStudents.Rows.Count) return;

            var row = dataGridViewStudents.Rows[e.Row.Index];

            var userIdCell = row.Cells["UserID"];
            if (userIdCell.Value == null || string.IsNullOrWhiteSpace(userIdCell.Value.ToString()))
            {
                userIdCell.Value = -1;
            }

            if (HasEmptyVisibleCellsInRow(row))
            {
                return;
            }

            foreach (DataGridViewColumn col in dataGridViewStudents.Columns)
            {
                FileLogger.logger.Debug($"Колонка: {col.Name}, Индекс: {col.Index}, Ширина: {col.Width}");
            }

            int userId = int.Parse(userIdCell.Value.ToString());
            string fullName = row.Cells["FullName"].Value?.ToString() ?? "";
            int permissionId = 1;
            Class cl = ClassController._controller.GetClassById(int.Parse(row.Cells["ClassID"].Value.ToString()));

            var userModel = new User
            {
                UserID = userId,
                FullName = fullName,
                Password = row.Cells["Password"].Value.ToString(),
                PermissionID = permissionId,
                ClassID = cl.ClassID,
                Class = cl
            };

            string action = "DELETE";
            UserController._userController.AddUserChange(action, userModel);

            FileLogger.logger.Info($"СТУДЕНТЫ: {action} - {userModel.FullName} (ID: {userModel.UserID})");
        }

        private void DataGridViewTeachers_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            var eventIdCell = e.Row.Cells["EventID"];

            if (eventIdCell.Value == null || !int.TryParse(eventIdCell.Value.ToString(), out int eventId))
            {
                e.Cancel = true;
                return;
            }

            if (eventId <= 0)
            {
                e.Cancel = true;
                FileLogger.logger.Warn("Попытка удалить новую строку - отменено");
                return;
            }

            var eventModel = new Event { EventID = eventId };
            EventsController._controller.AddEventChange("DELETE", eventModel);

            FileLogger.logger.Info($"Очередь: DELETE событие ID={eventId}");

            var result = MessageBox.Show(
                $"Удалить событие ID {eventId}?\nЭто добавит удаление в очередь изменений.",
                "Подтверждение удаления",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        private void DataGridViewTeachers_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            FileLogger.logger.Debug($"DataGridViewTeachers_CellEndEdit - Событие: Row={e.RowIndex}, Col={e.ColumnIndex}");

            if (e.RowIndex < 0 || e.RowIndex >= dataGridViewTeachers.Rows.Count)
            {
                FileLogger.logger.Debug($"DataGridViewTeachers_CellEndEdit - Игнорируем невалидный индекс (Row={e.RowIndex})");
                return;
            }

            var row = dataGridViewTeachers.Rows[e.RowIndex];
            FileLogger.logger.Debug($"DataGridViewTeachers_CellEndEdit - Обрабатываем строку {e.RowIndex}");

            var userIdCell = row.Cells["UserID"];
            FileLogger.logger.Debug($"DataGridViewTeachers_CellEndEdit - UserID cell value: '{userIdCell.Value}'");

            if (userIdCell.Value == null || string.IsNullOrWhiteSpace(userIdCell.Value.ToString()))
            {
                userIdCell.Value = -1;
                FileLogger.logger.Debug("DataGridViewTeachers_CellEndEdit - UserID установлен в -1 (новая запись)");
            }

            if (HasEmptyVisibleCellsInRow(row))
            {
                FileLogger.logger.Warn($"DataGridViewTeachers_CellEndEdit - Пропускаем - пустые видимые ячейки (Row={e.RowIndex})");
                return;
            }

            FileLogger.logger.Debug("DataGridViewTeachers_CellEndEdit - Логируем колонки:");
            foreach (DataGridViewColumn col in dataGridViewTeachers.Columns)
            {
                FileLogger.logger.Debug($"  Колонка: {col.Name}, Индекс: {col.Index}, Ширина: {col.Width}");
            }

            try
            {
                int userId = int.Parse(userIdCell.Value.ToString());
                FileLogger.logger.Debug($"DataGridViewTeachers_CellEndEdit - UserID распарсен: {userId}");

                string fullName = row.Cells["FullName"].Value?.ToString() ?? "";
                FileLogger.logger.Debug($"DataGridViewTeachers_CellEndEdit - FullName: '{fullName}'");

                int permissionId = UserController._userController.GetPermission(row.Cells["PermissionName"].Value.ToString());
                FileLogger.logger.Debug($"DataGridViewTeachers_CellEndEdit - PermissionID: {permissionId}");

                string classNameStr = row.Cells["ClassName"].Value?.ToString();
                FileLogger.logger.Debug($"DataGridViewTeachers_CellEndEdit - ClassName строка: '{classNameStr}'");

                if (string.IsNullOrWhiteSpace(classNameStr))
                {
                    FileLogger.logger.Warn("DataGridViewTeachers_CellEndEdit - ClassName пустой - пропускаем");
                    return;
                }

                Class cl = ClassController._controller.GetClassByName(classNameStr);

                if (cl == null)
                {
                    FileLogger.logger.Error($"DataGridViewTeachers_CellEndEdit - Класс '{classNameStr}' НЕ НАЙДЕН!");
                    return;
                }

                FileLogger.logger.Debug($"DataGridViewTeachers_CellEndEdit - Класс найден: {cl.ClassName} (ID={cl.ClassID})");

                var userModel = new User
                {
                    UserID = userId,
                    FullName = fullName,
                    Password = row.Cells["Password"].Value?.ToString() ?? "",
                    PermissionID = permissionId,
                    ClassID = cl.ClassID,
                    Class = cl
                };

                FileLogger.logger.Debug($"DataGridViewTeachers_CellEndEdit - 📦 userModel СОДЕРЖИМОЕ:");
                FileLogger.logger.Debug($"  UserID: {userModel.UserID} ({(userModel.UserID > 0 ? "СУЩЕСТВУЕТ" : "НОВЫЙ")})");
                FileLogger.logger.Debug($"  FullName: '{userModel.FullName}' (длина: {userModel.FullName?.Length ?? 0})");
                FileLogger.logger.Debug($"  Password: '{(string.IsNullOrEmpty(userModel.Password) ? "<ПУСТОЙ>" : userModel.Password)}'");
                FileLogger.logger.Debug($"  PermissionID: {userModel.PermissionID}");
                FileLogger.logger.Debug($"  ClassID: {userModel.ClassID ?? 0} {(userModel.ClassID.HasValue ? "" : "(NULL)")}");
                FileLogger.logger.Debug($"  Class: {(userModel.Class != null ? $"{userModel.Class.ClassName} (ID={userModel.Class.ClassID})" : "NULL")}");

                FileLogger.logger.Debug($"DataGridViewTeachers_CellEndEdit - Создан UserModel: ID={userModel.UserID}, Name='{userModel.FullName}', ClassID={userModel.ClassID}");

                string action = userModel.UserID > 0 ? "EDIT" : "ADD";
                UserController._userController.AddUserChange(action, userModel);

                FileLogger.logger.Info($"Teachers: {action} - {userModel.FullName} (ID: {userModel.UserID}, Class: {cl.ClassName})");
            }
            catch (Exception ex)
            {
                FileLogger.logger.Error($"DataGridViewTeachers_CellEndEdit - ОШИБКА (Row={e.RowIndex}): {ex.Message}\n{ex.StackTrace}");
            }
        }

        private void DataGridViewStudents_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dataGridViewStudents.Rows.Count) return;

            var row = dataGridViewStudents.Rows[e.RowIndex];

            var userIdCell = row.Cells["UserID"];
            if (userIdCell.Value == null || string.IsNullOrWhiteSpace(userIdCell.Value.ToString()))
            {
                userIdCell.Value = -1;
            }

            if (HasEmptyVisibleCellsInRow(row))
            {
                return;
            }

            foreach (DataGridViewColumn col in dataGridViewStudents.Columns)
            {
                FileLogger.logger.Debug($"Колонка: {col.Name}, Индекс: {col.Index}, Ширина: {col.Width}");
            }

            int userId = int.Parse(userIdCell.Value.ToString());
            string fullName = row.Cells["FullName"].Value?.ToString() ?? "";
            int permissionId = 1;
            Class cl = ClassController._controller.GetClassByName(row.Cells["ClassName"].Value.ToString());

            var userModel = new User
            {
                UserID = userId,
                FullName = fullName,
                Password = row.Cells["Password"].Value.ToString(),
                PermissionID = permissionId,
                ClassID = cl.ClassID,
                Class = cl
            };

            string action = userModel.UserID > 0 ? "EDIT" : "ADD";
            UserController._userController.AddUserChange(action, userModel);

            FileLogger.logger.Info($"СТУДЕНТЫ: {action} - {userModel.FullName} (ID: {userModel.UserID})");
        }

        private void DataGridViewEvents_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            var eventIdCell = e.Row.Cells["EventID"];

            if (eventIdCell.Value == null || !int.TryParse(eventIdCell.Value.ToString(), out int eventId))
            {
                e.Cancel = true;
                return;
            }

            if (eventId <= 0)
            {
                e.Cancel = true;
                FileLogger.logger.Warn("Попытка удалить новую строку - отменено");
                return;
            }

            var eventModel = new Event { EventID = eventId };
            EventsController._controller.AddEventChange("DELETE", eventModel);

            FileLogger.logger.Info($"Очередь: DELETE событие ID={eventId}");

            var result = MessageBox.Show(
                $"Удалить событие ID {eventId}?\nЭто добавит удаление в очередь изменений.",
                "Подтверждение удаления",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        private void DataGridViewEvents_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dataGridViewEvents.Rows.Count) return;

            var row = dataGridViewEvents.Rows[e.RowIndex];

            if (HasEmptyVisibleCellsInRow(row))
            {
                return;
            }

            var eventIdCell = row.Cells["EventID"];
            if (eventIdCell.Value == null || string.IsNullOrWhiteSpace(eventIdCell.Value.ToString()))
            {
                eventIdCell.Value = -1;
                FileLogger.logger.Debug("EventID пустой → установлен -1 (новое событие)");
            }

            if (!int.TryParse(eventIdCell.Value.ToString(), out int eventId))
                return;

            var eventModel = new Event
            {
                EventID = eventId,
                EventName = row.Cells["EventName"].Value?.ToString() ?? "",
                EventTime = DateTime.TryParse(row.Cells["EventTime"].Value?.ToString(), out var time)
                    ? time : DateTime.Now,
                Location = row.Cells["Location"].Value?.ToString() ?? ""
            };

            EventsController._controller.AddEventChange("EDIT", eventModel);
            FileLogger.logger.Info($"Очередь: EDIT событие ID={eventId}, \"{eventModel.EventName}\"");
        }

        /// <summary>
        /// Вспомогательный метод для проверки пустых ячеек в строке
        /// </summary>
        private bool HasEmptyVisibleCellsInRow(DataGridViewRow row)
        {
            foreach (DataGridViewCell cell in row.Cells)
            {
                if (cell.OwningColumn.Visible && cell.OwningColumn.Name != "EventID")
                {
                    var value = cell.Value;
                    if (value == null || value == DBNull.Value || string.IsNullOrWhiteSpace(value.ToString()))
                        return true;
                }
            }
            return false;
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

            FileLogger.logger.Info("Таблица статистики Ключ|Значение подготовлена");
        }

        private void LoadStatisticsGrid()
        {
            if (UserController.CurrentUser.PermissionID == 1)
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
                Class clzz = new Class();
                clzz.ClassID = ((ComboBoxItem)directorComboBox.SelectedItem).ClassID;

                LoadStatisticsGrid(clzz);
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

            if (classId == null) return;

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
            User selectedStudent = (User)selectedRow.Tag;

            if (selectedStudent == null)
                return;

            FileLogger.logger.Info($"Выбран ученик: {selectedStudent.FullName} (ID: {selectedStudent.UserID})");

            var statistics = StatisticsController._controller.GetStudentStatistics(selectedStudent);

            FillStatisticsGrid(statistics);
        }

        private void FillStatisticsGrid(Dictionary<string, string> statistics)
        {
            dataGridViewClassStatistics1.Rows.Clear();

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
            int columnIndex = e.ColumnIndex;

            if (columnIndex < 1 || !IsGradeRowComplete(row)) return;

            try
            {
                int gradeId = row.Cells["colGrade"].Value?.ToString() == "" ? 0 : int.Parse(row.Cells["colGrade"].Value.ToString());

                var grade = new Grade
                {
                    GradeID = gradeId,
                    GradeDate = DateTime.Parse(row.Cells["colDate"].Value?.ToString() ?? DateTime.Now.ToShortDateString()),
                    TeacherID = UserController.CurrentUser.UserID
                };

                var subjectValue = row.Cells["colSubject"].Value;
                if (subjectValue is Subject subjectObj)
                {
                    grade.SubjectID = subjectObj.SubjectID;
                }
                else
                {
                    grade.SubjectID = SubjectController._controller.GetSubjectIdByName(subjectValue?.ToString() ?? "");
                }

                var gradeValue = row.Cells["colGrade"].Value;
                if (gradeValue is Grade gradeObj)
                {
                    grade.GradeValue = gradeObj.GradeValue;
                }
                else
                {
                    grade.GradeValue = byte.Parse(gradeValue?.ToString() ?? "0");
                }

                var studentValue = row.Cells["colPerson"].Value;
                if (studentValue is User studentObj)
                {
                    grade.StudentID = studentObj.UserID;
                }
                else
                {
                    grade.StudentID = UserController._userController.GetStudentIdByName(studentValue?.ToString() ?? "");
                }

                if (grade.SubjectID == 0)
                {
                    MessageBox.Show("Предмет не найден в БД!");
                    return;
                }
                if (grade.StudentID == 0)
                {
                    MessageBox.Show("Ученик не найден в БД!");
                    return;
                }

                string action = gradeId > 0 ? "EDIT" : "ADD";
                GradesController._controller.AddGradeChange(action, grade);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}\nStack: " + ex.StackTrace);
            }
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

        public void CommitsAll(string previousTabName)
        {
            // Сохранение расписания (tabShedule)
            if (previousTabName != "tabShedule" && UserController.CurrentUser.PermissionID == 3)
            {
                int saved = SheduleController._controller.CommitScheduleChanges();
                if (saved > 0)
                    MessageBox.Show($"✅ Выполнено {saved} изменений расписания!");
            }

            // Сохранение домашки (tabHomework)
            if (previousTabName != "tabHomework")
            {
                int saved = _homeworkController.CommitHomeworkChanges();
                if (saved > 0)
                    MessageBox.Show($"✅ Сохранено {saved} ДЗ!");
            }

            // Сохранение оценок (tabGrades)
            if (previousTabName != "tabGrades")
            {
                int saved = GradesController._controller.CommitGradeChanges();
                if (saved > 0)
                    MessageBox.Show($"✅ Сохранено {saved} оценок!");
            }

            // Сохранение предметов (tabPageSubjects)
            if (previousTabName != "tabPageSubjects")
            {
                int saved = SubjectController._controller.CommitSubjectChanges();
                if (saved > 0)
                    MessageBox.Show($"✅ Сохранено {saved} предметов!");
            }

            // Сохранение посещаемость (tabPageAttendance)
            if (previousTabName != "tabPageAttendance")
            {
                int saved = AtterdanceController.Instance.CommitAttendanceChanges();
                if (saved > 0)
                    MessageBox.Show($"✅ Сохранено {saved} посещаемости!");
            }

            // Сохранение классов
            if (previousTabName != "tabPageClasses")
            {
                int saved = ClassController._controller.CommitClassChanges();
                if (saved > 0)
                    MessageBox.Show($"✅ Сохранено {saved} классов!");
            }

            // Создание учеников
            if (previousTabName != "tabPageStudents")
            {
                int saved = UserController._userController.CommitUserChanges();
                if (saved > 0)
                    MessageBox.Show($"✅ Сохранено {saved} учеников!");
            }

            // Создание сотрудников
            if (previousTabName != "tabPageTeachers")
            {
                int saved = UserController._userController.CommitUserChanges();
                if (saved > 0)
                    MessageBox.Show($"✅ Сохранено {saved} сотрудников!");
            }

            // Сохранение классов
            if (previousTabName != "tabPageEvents")
            {
                int saved = EventsController._controller.CommitEventChanges();
                if (saved > 0)
                    MessageBox.Show($"✅ Сохранено {saved} мероприятий!");
            }
        }

        // Метод смены вкладок.
        // Он также сохраняет изменения, при смене вкладки.
        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Предыдущая хуйня
            // Может сохранить только если предыдущая хуйня не текущая (здесь - расписание)
            string previousTabName = tabControl.SelectedTab?.Name ?? "tabShedule";

            CommitsAll(previousTabName);

            FileLogger.logger.Info("Индексы панелей: " + tabControl.TabCount + " Selected: " + tabControl.SelectedIndex);

            // Загрузка таблиц
            switch (tabControl.SelectedTab.Name)
            {
                // Домашка
                case "tabHomework": LoadHomeworkGrid(); break;

                // Оценки
                case "tabGrades": LoadGradesGrid(); break;

                // Статистика
                case "tabStatistics": LoadStatisticsGrid(); break;

                // Расписание
                case "tabShedule": LoadScheduleGrid(); break;

                // Вкладка "Мероприятия"
                case "tabPageEvents": LoadEventsGrid(); break;

                // Предметы 
                case "tabPageSubjects": LoadSubjects(); break;

                // Ученики
                case "tabPageStudents": LoadStudentsGrid(); break;

                // Сотрудники
                case "tabPageTeachers": LoadTeachersGrid(); break;

                // Вкладка посещаемости
                case "tabPageAttendance": LoadAttendance(); break;

                // Вкладка классов
                case "tabPageClasses": LoadClasses(dataGridViewClasses); break;
            }
        }

        public void SetupAttendance(DataGridView dataGridView)
        {
            dataGridView.Rows.Clear();
            dataGridView.Columns.Clear();

            dataGridView.Columns.Add("AttendanceID", "AttendanceID");
            dataGridView.Columns["AttendanceID"].Visible = false;

            if (UserController.CurrentUser.PermissionID <= 1)
            {
                dataGridView.Columns.Add("AttendanceDate", "Дата");
                dataGridView.Columns.Add("SubjectName", "Предмет");
                dataGridView.Columns.Add("LessonDate", "Время");
                dataGridView.Columns.Add("Status", "Статус");
                dataGridView.Columns.Add("Comment", "Комментарий");

                dataGridView.Columns["AttendanceDate"].Width = 90;
                dataGridView.Columns["SubjectName"].Width = 150;
                dataGridView.Columns["LessonDate"].Width = 70;
                dataGridView.Columns["Status"].Width = 120;
                dataGridView.Columns["Comment"].Width = 200;

                dataGridView.ReadOnly = true;
                dataGridView.AllowUserToAddRows = false;
                dataGridView.AllowUserToDeleteRows = false;
            }
            else if (UserController.CurrentUser.PermissionID >= 2)
            {
                dataGridView.Columns.Add("StudentName", "Ученик");

                var dateCol = new CalendarColumn();
                dateCol.Name = "AttendanceDate";
                dateCol.HeaderText = "Дата";
                dateCol.Width = 100;
                dataGridView.Columns.Add(dateCol);

                DataGridViewComboBoxColumn comboSubjectCol = new DataGridViewComboBoxColumn();
                comboSubjectCol.Name = "SubjectName";
                comboSubjectCol.HeaderText = "Предмет";
                comboSubjectCol.DisplayMember = "SubjectName";
                comboSubjectCol.ValueMember = "SubjectName";
                comboSubjectCol.ValueType = typeof(string);
                comboSubjectCol.Width = 120;

                foreach (Subject sub in SubjectController._controller.GetAllSubjects())
                {
                    comboSubjectCol.Items.Add(sub);
                }
                comboSubjectCol.Items.Insert(0, "");
                dataGridView.Columns.Add(comboSubjectCol);

                dataGridView.Columns.Add("LessonDate", "Время");

                DataGridViewComboBoxColumn comboStatusCol = new DataGridViewComboBoxColumn();
                comboStatusCol.Name = "Status";
                comboStatusCol.HeaderText = "Статус";
                comboStatusCol.ValueType = typeof(string);
                comboStatusCol.Width = 120;
                comboStatusCol.Items.AddRange(new string[] { "", "Присутствовал", "Отсутствовал" });
                dataGridView.Columns.Add(comboStatusCol);

                dataGridView.Columns.Add("Comment", "Комментарий");

                dataGridView.Columns["StudentName"].Width = 200;
                dataGridView.Columns["AttendanceDate"].Width = 90;
                dataGridView.Columns["SubjectName"].Width = 120;
                dataGridView.Columns["LessonDate"].Width = 70;
                dataGridView.Columns["Status"].Width = 120;
                dataGridView.Columns["Comment"].Width = 150;

                dataGridView.ReadOnly = false;
                dataGridView.AllowUserToAddRows = true;
                dataGridView.AllowUserToDeleteRows = true;

                dataGridView.CellEndEdit += dataGridViewClassAttendance_CellValueChanged;
                dataGridView.UserDeletedRow += DataGridViewClassAttendance_UserDeletedRow;

                dataGridView.ReadOnly = false;
                dataGridView.AllowUserToAddRows = true;
                dataGridView.AllowUserToDeleteRows = true;
            }

            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView.MultiSelect = false;

            FileLogger.logger.Debug($"SetupAttendance для {dataGridView.Name}");
        }

        private void dataGridViewClassAttendance_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                FileLogger.logger.Debug($"dataGridViewClassAttendance_CellValueChanged - Игнорируем заголовок/невалидные индексы (Row={e.RowIndex}, Col={e.ColumnIndex})");
                return;
            }

            var row = dataGridViewClassAtterdance.Rows[e.RowIndex];

            string attIdStr = row.Cells["AttendanceID"].Value?.ToString()?.Trim();
            string studentName = row.Cells["StudentName"].Value?.ToString()?.Trim();
            string subjectName = row.Cells["SubjectName"].Value?.ToString()?.Trim();
            string lessonDateStr = row.Cells["AttendanceDate"].Value?.ToString()?.Trim();
            string lessonTimeStr = row.Cells["LessonDate"].Value?.ToString()?.Trim();
            string statusStr = row.Cells["Status"].Value?.ToString()?.Trim();
            string commentStr = row.Cells["Comment"].Value?.ToString()?.Trim();

            FileLogger.logger.Debug($"dataGridViewClassAttendance_CellValueChanged - Изменена ячейка (Row={e.RowIndex}, Col={e.ColumnIndex}, Val={row.Cells[e.ColumnIndex].Value}): Student='{studentName}', Status='{statusStr}'");

            if (string.IsNullOrEmpty(studentName) || string.IsNullOrEmpty(subjectName) ||
                string.IsNullOrEmpty(lessonDateStr) || string.IsNullOrEmpty(statusStr))
            {
                FileLogger.logger.Warn($"dataGridViewClassAttendance_CellValueChanged - Пропускаем - пустые обязательные поля (Row={e.RowIndex}): Student={studentName?.Length ?? 0}, Subject={subjectName?.Length ?? 0}, Date={lessonDateStr?.Length ?? 0}, Status={statusStr?.Length ?? 0}");
                return;
            }

            int attendanceId = -1;
            if (!string.IsNullOrEmpty(attIdStr) && int.TryParse(attIdStr, out int parsedId))
            {
                attendanceId = parsedId;
                FileLogger.logger.Debug($"dataGridViewClassAttendance_CellValueChanged - ID посещения распарсен: {attendanceId}");
            }
            else
            {
                FileLogger.logger.Debug($"dataGridViewClassAttendance_CellValueChanged - Новый запись посещения (Row={e.RowIndex})");
            }

            Attendance attendance;
            try
            {
                attendance = new Attendance
                {
                    AttendanceID = attendanceId,
                    SubjectID = SubjectController._controller.GetSubjectIdByName(subjectName),
                    LessonDate = DateTime.Parse(lessonTimeStr),
                    AttendanceDate = DateTime.Parse(lessonDateStr).Date,

                    UserID = UserController._userController.GetStudentIdByName(studentName),
                    Present = statusStr.Contains("Присут"),
                    ExcuseReason = statusStr?.Contains("оправдание") ?? false,
                    Comment = commentStr ?? ""
                };

                FileLogger.logger.Info($"dataGridViewClassAttendance_CellValueChanged - Сформирован объект Attendance: ID={attendance.AttendanceID}, StudentID={attendance.UserID}, SubjectID={attendance.SubjectID}, Present={attendance.Present}, Date={attendance.LessonDate:yyyy-MM-dd}");
            }
            catch (Exception ex)
            {
                FileLogger.logger.Error($"dataGridViewClassAttendance_CellValueChanged - Ошибка создания Attendance (Row={e.RowIndex}): {ex.Message}\nStudent='{studentName}', Date='{lessonDateStr}', Status='{statusStr}'");
                return;
            }

            try
            {
                FileLogger.logger.Info($"dataGridViewClassAttendance_CellValueChanged - Очередь ДО добавления: {AtterdanceController.Instance.PendingChangesCount} изменений");
                AtterdanceController.Instance.AddAttendanceChange("EDIT", attendance);
                FileLogger.logger.Info($"dataGridViewClassAttendance_CellValueChanged - Очередь ПОСЛЕ добавления: {AtterdanceController.Instance.PendingChangesCount} изменений");
                FileLogger.logger.Info($"dataGridViewClassAttendance_CellValueChanged - Изменения сохранены: {attendance.AttendanceID} ({(attendanceId == -1 ? "NEW" : "EDIT")})");
            }
            catch (Exception ex)
            {
                FileLogger.logger.Error($"dataGridViewClassAttendance_CellValueChanged - Ошибка сохранения (Row={e.RowIndex}): {ex.Message}");
            }
        }

        private void DataGridViewClassAttendance_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {

        }

        private void LoadAttendance()
        {
            if (UserController.CurrentUser.PermissionID <= 1)
            {
                if (dataGridViewPersonalAttendance.Columns.Count == 0)
                {
                    SetupAttendance(dataGridViewPersonalAttendance);
                }

                dataGridViewPersonalAttendance.Rows.Clear();

                int userId = UserController.CurrentUser.UserID;

                try
                {
                    var attendances = AtterdanceController.Instance.GetStudentAttendance(userId);

                    foreach (var attendance in attendances)
                    {
                        dataGridViewPersonalAttendance.Rows.Add(
                            string.Format("{0:dd.MM.yyyy}", attendance.AttendanceDate),
                            attendance.SubjectName,
                            attendance.LessonDate != DateTime.MinValue ?
                                string.Format("{0:HH:mm}", attendance.LessonDate) : "",
                            attendance.StatusDisplay,
                            attendance.Comment
                        );
                    }
                }
                catch (Exception ex)
                {
                    FileLogger.logger.Error($"Ошибка LoadAttendance: {ex.Message}");
                }
            }
            else if (UserController.CurrentUser.PermissionID >= 2)
            {
                if (dataGridViewClassAtterdance.Columns.Count == 0)
                {
                    SetupAttendance(dataGridViewClassAtterdance);
                }

                dataGridViewClassAtterdance.Rows.Clear();

                int classId = -1;

                if (UserController.CurrentUser.PermissionID == 3)
                {
                    classId = ((ComboBoxItem)directorComboBox.SelectedItem).ClassID;
                }
                else
                {
                    classId = UserController.CurrentUser.ClassID.Value;
                }

                if (classId == -1)
                {
                    throw new ArgumentNullException("Класса не существует");
                }

                DateTime startDate = dateTimePickerAttendanceStart.Value.Date;
                DateTime endDate = dateTimePickerAttendanceEnd.Value.Date;

                try
                {
                    var attendances = AtterdanceController.Instance.GetClassAttendance(classId, startDate, endDate);

                    dataGridViewClassAtterdance.Rows.Clear();

                    foreach (var attendance in attendances)
                    {
                        int rowIndex = dataGridViewClassAtterdance.Rows.Add();
                        var newRow = dataGridViewClassAtterdance.Rows[rowIndex];

                        newRow.Cells["AttendanceID"].Value = attendance.AttendanceID;
                        newRow.Cells["StudentName"].Value = attendance.StudentNameDisplay;
                        newRow.Cells["AttendanceDate"].Value = attendance.AttendanceDate;
                        newRow.Cells["SubjectName"].Value = attendance.SubjectName ?? "";
                        newRow.Cells["LessonDate"].Value = attendance.LessonDate != DateTime.MinValue ?
                            string.Format("{0:HH:mm}", attendance.LessonDate) : "";
                        newRow.Cells["Status"].Value = attendance.StatusDisplay ?? "";
                        newRow.Cells["Comment"].Value = attendance.Comment ?? "";
                    }

                }
                catch (Exception ex)
                {
                    FileLogger.logger.Error($"Ошибка загрузки посещаемости класса: {ex.Message}");
                }
            }
        }

        private void SetupDataGridViewClasses(DataGridView dgv)
        {
            dgv.Columns.Clear();
            dgv.AutoGenerateColumns = false;

            dgv.DataSource = null;

            bool isDirector = UserController.CurrentUser.PermissionID == 3;

            dgv.Columns.Add("ClassID", "ID");
            dgv.Columns["ClassID"].DataPropertyName = "ClassID";
            dgv.Columns["ClassID"].Width = 50;
            dgv.Columns["ClassID"].Visible = isDirector;

            dgv.Columns.Add("ClassName", "Название класса");
            dgv.Columns["ClassName"].DataPropertyName = "ClassName";
            dgv.Columns["ClassName"].Width = 150;

            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;
            dgv.AllowUserToAddRows = isDirector;
            dgv.AllowUserToDeleteRows = isDirector;
            dgv.ReadOnly = !isDirector;

            dgv.CellValueChanged += DataGridViewClasses_CellValueChanged;
            dgv.UserDeletingRow += DataGridViewClasses_UserDeletedRow;
        }

        private void DataGridViewClasses_UserDeletedRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            var row = e.Row;
            var classIdValue = row.Cells["ClassID"].Value;

            var classItem = new Class
            {
                ClassID = (int)classIdValue
            };

            ClassController._controller.AddClassChange("DELETE", classItem);
        }

        private void DataGridViewClasses_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return; // Заголовки

            var dgv = sender as DataGridView;
            var row = dgv.Rows[e.RowIndex];

            string className = row.Cells["ClassName"].Value?.ToString()?.Trim();
            if (string.IsNullOrEmpty(className))
            {
                return;
            }

            int classId;
            if (row.Cells["ClassID"].Value is int idValue)
            {
                classId = idValue;
            }
            else
            {
                classId = -1;
            }

            var classItem = new Class
            {
                ClassID = classId,
                ClassName = className
            };

            ClassController._controller.AddClassChange("EDIT", classItem);
        }

        private void LoadClasses(DataGridView dgv)
        {
            if (dgv.Columns.Count == 0)
            {
                SetupDataGridViewClasses(dgv);
            }

            try
            {
                dgv.Rows.Clear();

                var classes = ClassController._controller.GetAllClasses();
                foreach (var cls in classes)
                {
                    dgv.Rows.Add(cls.ClassID, cls.ClassName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки классов: {ex.Message}", "Ошибка",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            if (isDirector)
            {
                dataGridViewSubjects.CellValueChanged += dataGridViewSubjects_CellValueChanged;
                dataGridViewSubjects.UserDeletingRow += dataGridViewSubjects_UserDeletedRow;
            }

            FileLogger.logger.Info("Обработчики событий добавлены");

            FileLogger.logger.Info("=== SetupSubject КОНЕЦ ===");
        }

        private void dataGridViewSubjects_UserDeletedRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            FileLogger.logger.Info($"UserDeletedRow: RowIndex={e.Row.Index}");

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

                if (UserController.CurrentUser.PermissionID == 2)
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

                SetupScheduleGridColumns(personalGrid);

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
                sheduleGridView.DataSource = null;
                sheduleGridView.Rows.Clear();

                var scheduleList = SheduleController._controller.GetScheduleForClass(classId);

                if (sheduleGridView.Columns.Count == 0)
                {
                    SetupScheduleGrid();
                }

                foreach (var schedule in scheduleList)
                {
                    sheduleGridView.Rows.Add(
                        schedule.ScheduleID,
                        schedule.DayOfWeekDisplay,
                        schedule.LessonNumber,
                        schedule.LessonTimeDisplay ?? "",
                        schedule.SubjectName,
                        schedule.TeacherName
                    );
                }

                string className = classId == 1 ? "5А" : $"Класс {classId}";

                sheduleLabel.Text = $"Расписание класса {className} ({scheduleList.Count} уроков)";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private void NumberTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Только цифры 0-9, Backspace, Delete
            if (!char.IsDigit(e.KeyChar) &&
                e.KeyChar != (char)Keys.Back &&
                e.KeyChar != (char)Keys.Delete)
            {
                e.Handled = true;  // Запрет ввода
            }
        }

        // Подготовка (визуально) таблицы расписания.
        private void SetupScheduleGrid()
        {
            sheduleGridView.Columns.Clear();
            bool isStudent = UserController.CurrentUser.PermissionID <= 1;
            bool isDirectorOrTeacher = UserController.CurrentUser.PermissionID >= 2;

            sheduleGridView.Columns.Add("colScheduleID", "ID");
            sheduleGridView.Columns["colScheduleID"].Visible = false;

            if (isStudent)
            {
                // Ученик - текстовые колонки (только просмотр)
                sheduleGridView.Columns.Add("colDay", "День");
                sheduleGridView.Columns.Add("colLesson", "Урок");
                sheduleGridView.Columns.Add("colTime", "Время");
                sheduleGridView.Columns.Add("colSubject", "Предмет");
                sheduleGridView.Columns.Add("colTeacher", "Учитель");
            }
            else if (isDirectorOrTeacher)
            {
                // Учитель/Директор - ComboBox списки

                // День - ComboBox
                DataGridViewComboBoxColumn comboDayCol = new DataGridViewComboBoxColumn();
                comboDayCol.Name = "colDay";
                comboDayCol.HeaderText = "День";
                comboDayCol.ValueType = typeof(string);
                comboDayCol.Items.AddRange(new string[] {
                    "Понедельник", "Вторник", "Среда", "Четверг",
                    "Пятница", "Суббота"
                });
                sheduleGridView.Columns.Add(comboDayCol);

                sheduleGridView.Columns.Add("colLesson", "Урок");

                // Время - пока текстовая (или ComboBox с временами)
                sheduleGridView.Columns.Add("colTime", "Время");

                // Предмет - ComboBox
                DataGridViewComboBoxColumn comboSubjectCol = new DataGridViewComboBoxColumn();
                comboSubjectCol.Name = "colSubject";
                comboSubjectCol.HeaderText = "Предмет";
                comboSubjectCol.DisplayMember = "SubjectName";
                comboSubjectCol.ValueMember = "SubjectName";
                comboSubjectCol.ValueType = typeof(string);

                foreach (Subject sub in SubjectController._controller.GetAllSubjects())
                {
                    comboSubjectCol.Items.Add(sub);
                }
                sheduleGridView.Columns.Add(comboSubjectCol);

                // Учитель - ComboBox
                DataGridViewComboBoxColumn comboTeacherCol = new DataGridViewComboBoxColumn();
                comboTeacherCol.Name = "colTeacher";
                comboTeacherCol.HeaderText = "Учитель";
                comboTeacherCol.DisplayMember = "FullName";
                comboTeacherCol.ValueMember = "FullName";
                comboTeacherCol.ValueType = typeof(string);

                foreach (User teacher in UserController._userController.GetAllOfPredicate("u.PermissionID >= 2"))
                {
                    comboTeacherCol.Items.Add(teacher);
                }
                sheduleGridView.Columns.Add(comboTeacherCol);
            }

            sheduleGridView.ReadOnly = isStudent;  // Только просмотр для ученика
            sheduleGridView.AllowUserToAddRows = !isStudent;
            sheduleGridView.AllowUserToDeleteRows = !isStudent;
            sheduleGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            sheduleGridView.EditMode = DataGridViewEditMode.EditOnKeystroke;
            sheduleGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            sheduleGridView.RowHeadersVisible = false;
        }

        // Инициализация выпадающего списка с классами.
        private void InitializeScheduleClassCombo()
        {
            bool isDirector = UserController.CurrentUser.PermissionID >= 2;

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
                    directorComboBox.SelectedIndex = (int)(UserController.CurrentUser.ClassID == null ? 0 : UserController.CurrentUser.ClassID);

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
            if (UserController.CurrentUser.PermissionID < 3) return;

            DataGridView grid = sender as DataGridView;
            int rowIndex = e.RowIndex;

            if (grid[1, rowIndex].Value == null || grid[1, rowIndex].Value.ToString().Trim() == "" ||  // День
                grid[2, rowIndex].Value == null || grid[2, rowIndex].Value.ToString().Trim() == "" ||  // Урок  
                grid[4, rowIndex].Value == null || grid[4, rowIndex].Value.ToString().Trim() == "" ||  // Предмет
                grid[5, rowIndex].Value == null || grid[5, rowIndex].Value.ToString().Trim() == "")    // Учитель
                return;

            TimeSpan? lessonTime = null;
            string timeText = grid[3, rowIndex]?.Value?.ToString()?.Trim();
            FileLogger.logger.Debug($"Time text from grid: '{timeText}'");

            if (!string.IsNullOrWhiteSpace(timeText))
            {
                TimeSpan tempTime;
                string[] formats = { "hh:mm", "h:mm", "hh:mm:ss", "H:mm", "HH:mm" };
                if (TimeSpan.TryParseExact(timeText, formats, null, out tempTime))
                    lessonTime = tempTime;
                else if (TimeSpan.TryParse(timeText, out tempTime))
                    lessonTime = tempTime;
            }

            int scheduleID = grid[0, rowIndex].Value?.ToString() == "" ? -1 : Convert.ToInt32(grid[0, rowIndex].Value);
            byte dayOfWeek = ScheduleItem.GetDayNumber(grid[1, rowIndex].Value.ToString());
            byte lessonNumber = Convert.ToByte(grid[2, rowIndex].Value);

            int classID = ((ComboBoxItem)directorComboBox.SelectedItem).ClassID;
            int subjectID = SubjectController._controller.GetSubjectIdByName(grid[4, rowIndex].Value.ToString());
            int teacherID = TeacherController._controller.GetUserByNameAndPermissions(grid[5, rowIndex].Value.ToString(), new int[] { 2, 3 }).UserID;

            FileLogger.logger.Debug($"DEBUG: ID={scheduleID}, Day={dayOfWeek}, Lesson={lessonNumber}, Class={classID}, Subject={subjectID}, Teacher={teacherID}, LessonTime={lessonTime}");

            ScheduleItem schedule = new ScheduleItem
            {
                ScheduleID = scheduleID,
                DayOfWeek = dayOfWeek,
                LessonNumber = lessonNumber,
                LessonTime = lessonTime,
                ClassID = classID,
                SubjectID = subjectID,
                TeacherID = teacherID
            };

            // Добавляем в очередь
            string action = schedule.ScheduleID > 0 ? "EDIT" : "ADD";
            SheduleController._controller.AddScheduleChange(action, schedule);
        }

        // Слушатель изменений (удаление) в расписании.
        // Слушатель изменений (удаление) в расписании
        private void DataGridViewSchedule_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            if (UserController.CurrentUser.PermissionID != 3) return;

            DataGridView grid = sender as DataGridView;

            int scheduleID = Convert.ToInt32(e.Row.Cells[0].Value);

            if (scheduleID > 0)
            {
                ScheduleItem scheduleToDelete = new ScheduleItem { ScheduleID = scheduleID };

                SheduleController._controller.AddScheduleChange("DELETE", scheduleToDelete);

                FileLogger.logger.Info($"DELETE queued: ScheduleID={scheduleID}");
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
            if (UserController.CurrentUser.PermissionID <= 1)
                return;

            var row = dataGridViewHomework.Rows[e.RowIndex];

            if (IsHomeworkRowComplete(row))
            {
                var classObj = ClassController._controller.GetClassByName(row.Cells["colClass"].Value.ToString());
                var subjectObj = SubjectController._controller.GetSubjectByName(row.Cells["colSubject"].Value.ToString());

                var homework = new Homework
                {
                    AssignmentDate = DateTime.Parse(row.Cells["colDate"].Value.ToString()),
                    ClassID = classObj?.ClassID ?? 0,
                    SubjectID = subjectObj?.SubjectID ?? 0,
                    Description = row.Cells["colDescription"].Value.ToString(),
                    TeacherID = UserController.CurrentUser.UserID
                };

                if (homework.ClassID == 0)
                {
                    MessageBox.Show("Класс не найден в БД!");
                    return;
                }
                else if (homework.SubjectID == 0)
                {
                    MessageBox.Show("Предмет не найден в БД!");
                    return;
                }

                _homeworkController.AddHomeworkChange("EDIT", homework);
                MessageBox.Show("Добавлено в очередь");
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
                DateTime startDate = dateTimePickerHomework.Value;
                DateTime endDate = dateTimePickerHomework1.Value;

                List<Homework> homeworkList;
                if (UserController.CurrentUser.PermissionID > 1)
                    homeworkList = TeacherController._controller.GetTeacherHomework(startDate, endDate, UserController.CurrentUser);
                else
                    homeworkList = _homeworkController.GetHomeworkForClassPeriod((int)UserController.CurrentUser.ClassID, startDate, endDate);

                dataGridViewHomework.DataSource = null;
                dataGridViewHomework.Rows.Clear();

                if (dataGridViewHomework.Columns.Count == 0)
                {
                    dataGridViewHomework.Columns.Clear();

                    SetupHomeworkGrid();
                }

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
                MessageBox.Show($"Ошибка: {ex.Message}.\nStack-Trace: \n" + ex.StackTrace);
            }
        }

        // Подготовка таблицы для домашнего задания
        private void SetupHomeworkGrid()
        {
            dataGridViewHomework.Columns.Clear();

            bool isTeacher = UserController.CurrentUser.PermissionID > 1;

            if(isTeacher)
            {
                dataGridViewHomework.Columns.Add("colHomeworkID", "ID");
                dataGridViewHomework.Columns["colHomeworkID"].Visible = false;

                var dateCol = new CalendarColumn();
                dateCol.Name = "colDate";
                dateCol.HeaderText = "Дата";
                dateCol.Width = 100;
                dataGridViewHomework.Columns.Add(dateCol);

                DataGridViewComboBoxColumn comboCol = new DataGridViewComboBoxColumn();
                comboCol.Name = "colClass";
                comboCol.HeaderText = "Класс";
                comboCol.DisplayMember = "ClassName";
                comboCol.ValueMember = "ClassName";

                foreach (Class clzz in ClassController._controller.GetAllClasses())
                {
                    comboCol.Items.Add(clzz);
                }

                dataGridViewHomework.Columns.Add(comboCol);

                DataGridViewComboBoxColumn comboSubjectCol = new DataGridViewComboBoxColumn();
                comboSubjectCol.Name = "colSubject";
                comboSubjectCol.HeaderText = "Предмет";
                comboSubjectCol.DisplayMember = "SubjectName";
                comboSubjectCol.ValueMember = "SubjectName";

                foreach (Subject sub in SubjectController._controller.GetAllSubjects())
                {
                    comboSubjectCol.Items.Add(sub);
                }

                dataGridViewHomework.Columns.Add(comboSubjectCol);

                DataGridViewComboBoxColumn comboTeacherCol = new DataGridViewComboBoxColumn();
                comboTeacherCol.Name = "colTeacher";
                comboTeacherCol.HeaderText = "Учитель";
                comboTeacherCol.DisplayMember = "FullName";
                comboTeacherCol.ValueMember = "FullName";

                foreach (User sub in UserController._userController.GetAllOfPredicate("u.PermissionID >= 2"))
                {
                    comboTeacherCol.Items.Add(sub);
                }

                dataGridViewHomework.Columns.Add(comboTeacherCol);

                dataGridViewHomework.Columns.Add("colDescription", "Задание");
            } 
            else
            {
                dataGridViewHomework.Columns.Clear();

                dataGridViewHomework.Columns.Add("colHomeworkID", "ID");
                dataGridViewHomework.Columns["colHomeworkID"].Visible = false;

                var dateCol = new CalendarColumn();
                dateCol.Name = "colDate";
                dateCol.HeaderText = "Дата";
                dateCol.Width = 100;
                dataGridViewHomework.Columns.Add(dateCol);

                dataGridViewHomework.Columns.Add("colClass", "Класс");
                dataGridViewHomework.Columns.Add("colSubject", "Предмет");
                dataGridViewHomework.Columns.Add("colTeacher", "Учитель");
                dataGridViewHomework.Columns.Add("colDescription", "Задание");
            }
            
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

                DateTime startDate = dateTimePickerGrades.Value;
                DateTime endDate = dateTimePickerGrades1.Value;

                List<Grade> gradesList;
                if (UserController.CurrentUser.PermissionID > 1)
                {
                    gradesList = TeacherController._controller.GetTeacherGrades(startDate, endDate, UserController.CurrentUser);
                }
                else
                {
                    gradesList = GradesController._controller.GetGradesForStudentPeriod(UserController.CurrentUser.UserID, startDate, endDate);
                }

                if(dataGridViewGrades.Columns.Count == 0)
                {
                    SetupGradesGrid();
                }

                foreach (var grade in gradesList)
                {
                    int rowIndex = dataGridViewGrades.Rows.Add();
                    var newRow = dataGridViewGrades.Rows[rowIndex];

                    newRow.Cells["colGradeID"].Value = grade.GradeID;
                    newRow.Cells["colDate"].Value = grade.GradeDate;
                    newRow.Cells["colSubject"].Value = grade.SubjectNameDisplay ?? "";
                    newRow.Cells["colGrade"].Value = grade.GradeValue.ToString();

                    if (UserController.CurrentUser.PermissionID > 1)
                    {
                        newRow.Cells["colPerson"].Value = grade.StudentNameDisplay ?? "👤";
                        newRow.Cells["colClass"].Value = grade.Student?.Class?.ClassName ?? "";
                    }
                    else
                    {
                        newRow.Cells["colPerson"].Value = grade.TeacherNameDisplay ?? "";
                    }

                    newRow.Tag = grade; 
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

            if (isTeacher)
            {
                dataGridViewGrades.Columns.Add("colGradeID", "ID");
                dataGridViewGrades.Columns["colGradeID"].Visible = false;

                var dateCol = new CalendarColumn();
                dateCol.Name = "colDate";
                dateCol.HeaderText = "Дата";
                dateCol.Width = 100;
                dataGridViewGrades.Columns.Add(dateCol);

                DataGridViewComboBoxColumn comboSubjectCol = new DataGridViewComboBoxColumn();
                comboSubjectCol.Name = "colSubject";
                comboSubjectCol.HeaderText = "Предмет";
                comboSubjectCol.DisplayMember = "SubjectName";
                comboSubjectCol.ValueMember = "SubjectName";
                comboSubjectCol.ValueType = typeof(string);

                foreach (Subject sub in SubjectController._controller.GetAllSubjects())
                {
                    comboSubjectCol.Items.Add(sub);
                }

                dataGridViewGrades.Columns.Add(comboSubjectCol);

                dataGridViewGrades.Columns.Add("colGrade", "Оценка");

                DataGridViewComboBoxColumn btnPersonCol = new DataGridViewComboBoxColumn();
                btnPersonCol.Name = "colPerson";
                btnPersonCol.HeaderText = "Ученик";
                btnPersonCol.DisplayMember = "FullName";
                btnPersonCol.ValueMember = "FullName";
                btnPersonCol.ValueType = typeof(string);

                foreach (User sub in UserController._userController.GetClassStudents(((ComboBoxItem) directorComboBox.SelectedItem).ClassID))
                {
                    btnPersonCol.Items.Add(sub.FullName);
                }

                dataGridViewGrades.Columns.Add(btnPersonCol);

                dataGridViewGrades.Columns.Add("colClass", "Класс");

                dataGridViewGrades.ReadOnly = !isTeacher;
                dataGridViewGrades.AllowUserToAddRows = isTeacher;  
                dataGridViewGrades.AllowUserToDeleteRows = isTeacher;
                dataGridViewGrades.EditMode = DataGridViewEditMode.EditOnKeystroke;
                dataGridViewGrades.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridViewGrades.RowHeadersVisible = false;
            }
            else
            {
                dataGridViewGrades.Columns.Clear();

                dataGridViewGrades.Columns.Add("colGradeID", "ID");
                dataGridViewGrades.Columns["colGradeID"].Visible = false;

                var dateCol = new CalendarColumn();
                dateCol.Name = "colDate";
                dateCol.HeaderText = "Дата";
                dateCol.Width = 100;
                dataGridViewGrades.Columns.Add(dateCol);

                dataGridViewGrades.Columns.Add("colSubject", "Предмет");
                dataGridViewGrades.Columns.Add("colGrade", "Оценка");
                dataGridViewGrades.Columns.Add("colPerson", "Учитель");

                dataGridViewHomework.ReadOnly = !isTeacher;
                dataGridViewHomework.AllowUserToAddRows = isTeacher;
                dataGridViewHomework.AllowUserToDeleteRows = isTeacher;
                dataGridViewHomework.EditMode = DataGridViewEditMode.EditOnKeystroke;
                dataGridViewHomework.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridViewHomework.RowHeadersVisible = false;
            }
        }

        private void dateTimePickerHomework_ValueChanged_1(object sender, EventArgs e)
        {
            LoadHomeworkGrid();
        }

        private void dateTimePickerGrades_ValueChanged_1(object sender, EventArgs e)
        {
            LoadGradesGrid();
        }

        /// <summary>
        /// Проверяет все видимые ячейки таблицы на пустоту.
        /// Возвращает true если есть пустые ячейки, false если все заполнены
        /// </summary>
        public bool HasEmptyVisibleCells(DataGridView dataGridView)
        {
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                if (row.IsNewRow) continue;

                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (!cell.OwningColumn.Visible) continue;

                    if (IsCellEmpty(cell))
                    {
                        dataGridView.CurrentCell = cell;
                        dataGridView.BeginEdit(true);
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Проверяет, пуста ли ячейка
        /// </summary>
        private bool IsCellEmpty(DataGridViewCell cell)
        {
            var value = cell.Value;

            if (value == null || value == DBNull.Value)
                return true;

            string stringValue = value.ToString().Trim();
            return string.IsNullOrEmpty(stringValue);
        }

        /* Сегмент с вкладкой "Мероприятия" */
        private void LoadEventsGrid()
        {
            FileLogger.logger.Info("LoadEventsGrid: загрузка мероприятий");

            try
            {
                dataGridViewEvents.Rows.Clear();

                if (dataGridViewEvents.Columns.Count == 0)
                {
                    CreateEventsColumns();
                    FileLogger.logger.Debug("LoadEventsGrid - Колонки мероприятий созданы");
                }

                var events = EventsController._controller.GetAllEvents();
                FileLogger.logger.Info($"LoadEventsGrid - Получено {events.Count} мероприятий");

                if (events.Count == 0)
                {
                    FileLogger.logger.Warn("LoadEventsGrid - Мероприятия не найдены");
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

                    FileLogger.logger.Debug($"LoadEventsGrid - {ev.EventName} | {ev.EventTime:dd.MM HH:mm} | {ev.Location}");
                }

                bool isDirector = UserController.CurrentUser.PermissionID >= 3;

                dataGridViewEvents.ReadOnly = !isDirector;
                dataGridViewEvents.AllowUserToAddRows = isDirector;
                dataGridViewEvents.AllowUserToDeleteRows = isDirector;
                dataGridViewEvents.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridViewEvents.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                FileLogger.logger.Info($"LoadEventsGrid - Загружено {dataGridViewEvents.Rows.Count} мероприятий");
            }
            catch (Exception ex)
            {
                FileLogger.logger.Error($"LoadEventsGrid - {ex.Message}");
                MessageBox.Show($"LoadEventsGrid - Ошибка загрузки мероприятий: {ex.Message}");
            }
        }

        private void CreateEventsColumns()
        {
            dataGridViewEvents.Columns.Clear();

            dataGridViewEvents.Columns.Add("EventID", "ID");
            dataGridViewEvents.Columns.Add("EventName", "Название");

            var dateCol = new CalendarColumn();
            dateCol.Name = "EventTime";
            dateCol.HeaderText = "Дата/Время";
            dateCol.Width = 100;
            dataGridViewEvents.Columns.Add(dateCol);

            dataGridViewEvents.Columns.Add("Location", "Место");

            dataGridViewEvents.Columns["EventID"].Visible = false;
            dataGridViewEvents.Columns["EventID"].ReadOnly = true;

            dataGridViewEvents.Columns["EventName"].FillWeight = 50;
            dataGridViewEvents.Columns["EventTime"].FillWeight = 30;
            dataGridViewEvents.Columns["Location"].FillWeight = 20;

            bool isDirector = UserController.CurrentUser.PermissionID >= 3;
            dataGridViewEvents.AllowUserToAddRows = isDirector;
            dataGridViewEvents.AllowUserToDeleteRows = isDirector;
            dataGridViewEvents.ReadOnly = !isDirector;
            dataGridViewEvents.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;

            FileLogger.logger.Debug("Form1.CreateEventsColumns - Колонки Events настроены");
        }

        /* Сегмент с вкладкой "Сотрудники" */
        private void LoadTeachersGrid()
        {
            FileLogger.logger.Info("Form1.LoadTeachersGrid - Начало загрузки сотрудников");

            try
            {
                dataGridViewTeachers.Rows.Clear();

                if (dataGridViewTeachers.Columns.Count == 0)
                {
                    FileLogger.logger.Debug("Form1.LoadTeachersGrid - Колонки отсутствуют, создаём...");

                    SetupTeachersGridColumns();

                    FileLogger.logger.Debug("Form1.LoadTeachersGrid - Колонки созданы");
                }

                var teachers = TeacherController._controller.GetAllTeachers();
                FileLogger.logger.Info($"Form1.LoadTeachersGrid - Получено {teachers.Count} сотрудников из БД");

                if (teachers.Count == 0)
                {
                    FileLogger.logger.Warn("Form1.LoadTeachersGrid - Список сотрудников пуст");
                    return;
                }

                foreach (var user in teachers)
                {
                    string classValue = user.Class?.ClassName ?? "";

                    int rowIndex = dataGridViewTeachers.Rows.Add();
                    var newRow = dataGridViewTeachers.Rows[rowIndex];

                    newRow.Cells["UserID"].Value = user.UserID;
                    newRow.Cells["FullName"].Value = user.FullName ?? "";
                    newRow.Cells["PermissionName"].Value = user.PermissionName;
                    newRow.Cells["Password"].Value = user.Password ?? "";
                    newRow.Cells["ClassID"].Value = user.ClassID?.ToString() ?? "";
                    newRow.Cells["ClassName"].Value = classValue;

                    FileLogger.logger.Debug($"Добавлена строка: {user.FullName} ({user.PermissionName})");
                }

                FileLogger.logger.Info($"Form1.LoadTeachersGrid - Zагружено {dataGridViewTeachers.Rows.Count} строк в грид");
                FileLogger.logger.Info("Form1.LoadTeachersGrid: ЗАВЕРШЕНО успешно");
            }
            catch (Exception ex)
            {
                FileLogger.logger.Error($"Form1.LoadTeachersGrid: ОШИБКА = {ex.Message}\n{ex.StackTrace}");
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
            dataGridViewTeachers.Columns[0].Visible = false;

            dataGridViewTeachers.Columns.Add("FullName", "ФИО");

            if (UserController.CurrentUser.PermissionID >= 3)
            {
                DataGridViewComboBoxColumn comboRoleCol = new DataGridViewComboBoxColumn();
                comboRoleCol.Name = "PermissionName";
                comboRoleCol.HeaderText = "Роль";
                comboRoleCol.ValueType = typeof(string);
                comboRoleCol.Items.AddRange(new string[] {
                    "", "Учитель", "Директор"
                });
                dataGridViewTeachers.Columns.Add(comboRoleCol);
            }
            else
            {
                dataGridViewTeachers.Columns.Add("PermissionName", "Роль");
            }

            dataGridViewTeachers.Columns.Add("Password", "Пароль");
            dataGridViewTeachers.Columns["Password"].Visible = UserController.CurrentUser.PermissionID >= 3;

            dataGridViewTeachers.Columns.Add("ClassID", "Класс ID");
            dataGridViewTeachers.Columns["ClassID"].Visible = false;

            if (UserController.CurrentUser.PermissionID >= 3)
            {
                DataGridViewComboBoxColumn comboClassCol = new DataGridViewComboBoxColumn();
                comboClassCol.Name = "ClassName";
                comboClassCol.HeaderText = "Руководство классом";

                foreach (Class clzz in ClassController._controller.GetAllClasses())
                {
                    comboClassCol.Items.Add(clzz.ClassName);
                }

                dataGridViewTeachers.Columns.Add(comboClassCol);
            }
            else
            {
                dataGridViewTeachers.Columns.Add("ClassName", "Руководство классом");
            }

            dataGridViewTeachers.ReadOnly = !(UserController.CurrentUser.PermissionID >= 3);
            dataGridViewTeachers.AllowUserToAddRows = UserController.CurrentUser.PermissionID >= 3;
            dataGridViewTeachers.AllowUserToDeleteRows = UserController.CurrentUser.PermissionID >= 3;

            FileLogger.logger.Debug($"Колонки: {dataGridViewTeachers.Columns.Count} шт.");
        }

        /* Сегмент с вкладкой "Ученики" */
        private void LoadStudentsGrid()
        {
            FileLogger.logger.Info("LoadStudentsGrid: загрузка учеников");

            try
            {
                dataGridViewStudents.Rows.Clear();

                if (dataGridViewStudents.Columns.Count == 0)
                {
                    SetupStudentsColumns();
                }

                var students = TeacherController._controller.GetAllStudents();
                FileLogger.logger.Info($"Получено {students.Count} учеников");

                foreach (var student in students)
                {
                    var row = dataGridViewStudents.Rows[dataGridViewStudents.Rows.Add()];

                    dataGridViewStudents.Rows[row.Index].Cells["UserID"].Value = student.UserID;
                    dataGridViewStudents.Rows[row.Index].Cells["FullName"].Value = student.FullName;
                    dataGridViewStudents.Rows[row.Index].Cells["PermissionName"].Value = student.PermissionName;
                    dataGridViewStudents.Rows[row.Index].Cells["Password"].Value = student.Password;
                    dataGridViewStudents.Rows[row.Index].Cells["ClassID"].Value = student.ClassID?.ToString() ?? "";

                    dataGridViewStudents.Rows[row.Index].Cells["ClassName"].Value =
                        student.Class?.ClassName ?? "-";

                    FileLogger.logger.Debug($"Form1.LoadStudentsGrid - {student.FullName} | Класс: {student.Class?.ClassName ?? "нет класса"}");
                }

                FileLogger.logger.Info($"Загружено {dataGridViewStudents.Rows.Count} учеников");
            }
            catch (Exception ex)
            {
                FileLogger.logger.Error($"LoadStudentsGrid: {ex.Message}\n{ex.StackTrace}");
            }
        }

        private void dataGridViewStudents_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.Cells["PermissionName"].Value = "Ученик";
        }

        private void SetupStudentsColumns()
        {
            if (dataGridViewStudents.Columns.Count > 0)
                dataGridViewStudents.Columns.Clear();

            dataGridViewStudents.AutoGenerateColumns = false;

            if (dataGridViewStudents.Columns.Count > 0)
                dataGridViewStudents.Columns.Clear();

            dataGridViewStudents.Columns.Add("UserID", "ID");
            dataGridViewStudents.Columns.Add("FullName", "ФИО");
            dataGridViewStudents.Columns.Add("PermissionName", "Роль");
            dataGridViewStudents.Columns.Add("Password", "Пароль");

            dataGridViewStudents.Columns["Password"].Visible = UserController.CurrentUser.PermissionID >= 3;

            dataGridViewStudents.Columns.Add("ClassID", "Класс ID");

            if (UserController.CurrentUser.PermissionID >= 3)
            {
                DataGridViewComboBoxColumn comboClassCol = new DataGridViewComboBoxColumn();
                comboClassCol.Name = "ClassName";
                comboClassCol.HeaderText = "Класс";
                comboClassCol.DisplayMember = "ClassName";
                comboClassCol.ValueMember = "ClassName";

                foreach (Class clzz in ClassController._controller.GetAllClasses())
                {
                    comboClassCol.Items.Add(clzz);
                }
                comboClassCol.Items.Insert(0, "");
                dataGridViewStudents.Columns.Add(comboClassCol);
            }
            else
            {
                dataGridViewStudents.Columns.Add("ClassName", "Класс");
            }

            dataGridViewStudents.Columns["UserID"].Visible = false;
            dataGridViewStudents.Columns["ClassID"].Visible = false;

            dataGridViewStudents.Columns["FullName"].FillWeight = 50;
            dataGridViewStudents.Columns["PermissionName"].FillWeight = 25;
            dataGridViewStudents.Columns["ClassName"].FillWeight = 25;

            dataGridViewStudents.ReadOnly = !(UserController.CurrentUser.PermissionID >= 3);
            dataGridViewStudents.AllowUserToAddRows = UserController.CurrentUser.PermissionID >= 3;
            dataGridViewStudents.AllowUserToDeleteRows = UserController.CurrentUser.PermissionID >= 3;

            dataGridViewStudents.Columns["PermissionName"].ReadOnly = true;

            dataGridViewStudents.DefaultValuesNeeded += dataGridViewStudents_DefaultValuesNeeded;
        }

        private void exitBtnm_Click(object sender, EventArgs e)
        {
            Hide();
            new LoginForm().Show();
        }

        private PrintPreviewDialog printPreviewDialog1;

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            DataGridViewPrinter.PrintDataGridView(sheduleGridView, "Расписание", e);
        }

        private void printDocumentAtterdance_PrintPage(object sender, PrintPageEventArgs e)
        {
            DataGridViewPrinter.PrintDataGridView(dataGridViewClassAtterdance, "Посещаемость", e);
        }

        private List<DataGridView> _gridsToPrint;
        private int _currentGridIndex;
        private string _currentPrintTitle;

        private void buttonPrint_Click(object sender, EventArgs e)
        {
            CommitsAll(tabControl.SelectedTab.Name);

            _gridsToPrint = GetCurrentGridViews();
            _currentGridIndex = 0;
            _currentPrintTitle = GetCurrentGridTitle();

            if (_gridsToPrint == null || _gridsToPrint.Count == 0)
            {
                MessageBox.Show("Нет таблиц для печати!", "Ошибка");
                return;
            }

            printDialog1.Document = printDocument1;
            if (printDialog1.ShowDialog() == DialogResult.OK)
            {
                printDocument1.PrintPage -= printDocument2_PrintPage;
                printDocument1.PrintPage += printDocument2_PrintPage;
                printDocument1.Print();
                printDocument1.PrintPage -= printDocument2_PrintPage;

                _gridsToPrint = null;
                _currentGridIndex = 0;
            }
        }

        private void buttonPreview_Click(object sender, EventArgs e)
        {
            _gridsToPrint = GetCurrentGridViews();
            _currentGridIndex = 0;
            _currentPrintTitle = GetCurrentGridTitle();

            if (_gridsToPrint != null && _gridsToPrint.Count > 0)
            {
                printPreviewDialog1 = new PrintPreviewDialog
                {
                    Document = printDocument1,
                    ShowIcon = false,
                    WindowState = FormWindowState.Maximized
                };

                printDocument1.PrintPage += printDocument2_PrintPage;
                printPreviewDialog1.ShowDialog();
                printDocument1.PrintPage -= printDocument2_PrintPage;
            }
        }

        private void printDocument2_PrintPage(object sender, PrintPageEventArgs e)
        {
            if (_gridsToPrint == null || _gridsToPrint.Count == 0)
            {
                e.HasMorePages = false;
                return;
            }

            var grid = _gridsToPrint[_currentGridIndex];

            DataGridViewPrinter.PrintDataGridView(grid, _currentPrintTitle, e);

            _currentGridIndex++;

            if (_currentGridIndex < _gridsToPrint.Count)
            {
                e.HasMorePages = true;
            }
            else
            {
                e.HasMorePages = false;
            }
        }

        private List<DataGridView> GetCurrentGridViews()
        {
            List<DataGridView> grids = new List<DataGridView>();
            Control currentTab = tabControl.SelectedTab;

            FileLogger.logger.Debug($"Печать вкладки: '{currentTab?.Name}' (Controls: {currentTab?.Controls.Count ?? 0})");

            FindDataGridViewsRecursive(currentTab, grids);

            FileLogger.logger.Debug($"ИТОГО таблиц для печати: {grids.Count}");
            return grids;
        }

        private void FindDataGridViewsRecursive(Control parent, List<DataGridView> grids)
        {
            if (parent == null) return;

            foreach (Control control in parent.Controls)
            {
                FileLogger.logger.Debug($"  Контрол: '{control.Name}' Type: {control.GetType().Name} Visible: {control.Visible} (Level: {GetControlLevel(control)})");

                if (control is DataGridView dgv)
                {
                    FileLogger.logger.Debug($"    НАЙДЕН DataGridView: '{dgv.Name}' Rows: {dgv.RowCount}");

                    if (dgv.RowCount > 0 && dgv.Visible)
                    {
                        grids.Add(dgv);
                        FileLogger.logger.Debug($"    ДОБАВЛЕН в печать: '{dgv.Name}'");
                    }
                    else
                    {
                        FileLogger.logger.Debug($"    ПРОПУЩЕН (пустой/невидимый): Rows={dgv.RowCount}, Visible={dgv.Visible}");
                    }
                }

                if (control.HasChildren)
                {
                    FileLogger.logger.Debug($"    ИДЕМ ВГЛУБЬ: {control.Name} (Children: {control.Controls.Count})");
                    FindDataGridViewsRecursive(control, grids);
                }
            }
        }

        private int GetControlLevel(Control control)
        {
            int level = 0;
            Control current = control.Parent;
            while (current != null)
            {
                level++;
                current = current.Parent;
            }
            return level;
        }

        private string GetCurrentGridTitle()
        {
            switch (tabControl.SelectedTab.Name)
            {
                case "tabShedule": return "РАСПИСАНИЕ УРОКОВ";
                case "tabHomework": return "ДОМАШНИЕ ЗАДАНИЯ";
                case "tabGrades": return "ОЦЕНКИ";
                case "tabPageEvents": return "МЕРОПРИЯТИЯ";
                case "tabPageStudents": return "УЧЕНИКИ";
                case "tabPageTeachers": return "СОТРУДНИКИ";
                case "tabPageAttendance": return "ПОСЕЩАЕМОСТЬ";
                default: return "ТАБЛИЦА";
            }
        }

        private void dateTimePickerAttendanceStart_ValueChanged(object sender, EventArgs e)
        {
            LoadAttendance();
        }

        private void dateTimePickerAttendanceEnd_ValueChanged(object sender, EventArgs e)
        {
            LoadAttendance();
        }

        private void dateTimePickerHomework1_ValueChanged(object sender, EventArgs e)
        {
            LoadHomeworkGrid();
        }

        private void dateTimePickerGrades1_ValueChanged(object sender, EventArgs e)
        {
            LoadGradesGrid();
        }

        private DataGridView GetFirstDataGridView()
        {
            return FindFirstDataGridView(this);
        }

        private DataGridView FindFirstDataGridView(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                if (control is DataGridView dgv && dgv.Visible && dgv.Enabled)
                    return dgv;

                if (control.HasChildren)
                {
                    DataGridView found = FindFirstDataGridView(control);
                    if (found != null)
                        return found;
                }
            }
            return null;
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            var dgv = GetFirstDataGridView();
            if (dgv == null) return;

            int rowIndex = dgv.Rows.Add();
            var newRow = dgv.Rows[rowIndex];

            DataGridViewCell firstVisibleCell = null;
            foreach (DataGridViewColumn col in dgv.Columns)
            {
                if (col.Visible && col.Index < dgv.Columns.Count)
                {
                    firstVisibleCell = dgv.Rows[rowIndex].Cells[col.Index];
                    break;
                }
            }

            if (firstVisibleCell != null)
            {
                dgv.CurrentCell = firstVisibleCell;
                dgv.Rows[rowIndex].Selected = true;
                dgv.FirstDisplayedScrollingRowIndex = rowIndex;
            }
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            var dgv = GetFirstDataGridView();
            if (dgv == null) return;

            if (dgv.CurrentRow != null && dgv.CurrentRow.Index >= 0)
            {
                if (MessageBox.Show("Удалить выбранную строку?", "Подтверждение",
                    MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    dgv.Rows.RemoveAt(dgv.CurrentRow.Index);
                }
            }
        }
    }
}