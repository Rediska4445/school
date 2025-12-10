using school.Controllers;
using school.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using static school.Controllers.HomeworkController;

namespace school
{
    /// <summary>
    /// Основная форма для отображения всей этой хуйни.
    /// </summary>
    public partial class Form1 : Form
    {
        // Переменная-флаг для сохранения расписания ебанного
        private bool _scheduleChanged = false;

        // Ебанный список из классов для ебанного расписания
        private ComboBox comboBoxScheduleClass; 

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

        // Метод смены вкладок.
        // Он также сохраняет изменения, при смене вкладки.
        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Предыдущая хуйня
            // Может сохранить только если предыдущая хуйня не текущая (здесь - расписание)
            int previousTab = tabControl.SelectedIndex;

            // Сохранение расписания
            if (previousTab != 3 && UserController.CurrentUser.PermissionID >= 3 && _scheduleChanged)
            {
                SaveScheduleGridChanges();
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

            // Загрузка таблиц
            switch (tabControl.SelectedIndex)
            {
                // Домашка
                case 0: LoadHomeworkGrid(); break;

                // Оценки
                case 1: LoadGradesGrid(); break;

                // Статистика
                case 2: LoadStatisticsGrid(); break;

                // Расписание
                case 3: LoadScheduleGrid(); break;
            }
        }

        // Загрузка расписания с БД.
        // Вызывается при переключении на вкладку "Расписание".
        // Вызывается при смене класса в списке (доступно только при директоре).
        private void LoadScheduleGrid()
        {
            // Если директор и прочая лабуда для проверки
            if (UserController.CurrentUser.PermissionID >= 3 &&
                comboBoxScheduleClass != null &&
                comboBoxScheduleClass.SelectedItem is ComboBoxItem selectedItem)
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
            // Проверяем, есть ли уже вкладка "Личный"
            TabPage personalTab = null;
            foreach (TabPage tab in sheduleTabControl.TabPages)
            {
                if (tab.Text == "Личный")
                {
                    personalTab = tab;
                    break;
                }
            }

            // Если вкладка не существует - создаём
            if (personalTab == null)
            {
                personalTab = new TabPage("Личный");

                // ✅ Создаём DataGridView для личного расписания
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
                if(comboBoxScheduleClass == null)
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

                // Текст для класса расписания - под таблицей
                sheduleLabel.Text = $"Расписание класса {GetClassName(classId)} ({scheduleList.Count} уроков)";

                // Смена состояния флага для сохранения расписания
                _scheduleChanged = false;
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
                    if (comboBoxScheduleClass == null)
                    {
                        FileLogger.logger.Warn(
                            "GetCurrentClassId: PermissionID=3 (директор), но comboBoxScheduleClass == null. " +
                            "Возвращаем ClassID пользователя.");
                    }
                    else
                    {
                        if (comboBoxScheduleClass.SelectedItem is ComboBoxItem selectedItem)
                        {
                            FileLogger.logger.Info(
                                $"GetCurrentClassId: выбран класс из комбобокса: ClassID={selectedItem.ClassID}, Text='{selectedItem.Text}'");
                            return selectedItem.ClassID;
                        }
                        else
                        {
                            FileLogger.logger.Warn(
                                "GetCurrentClassId: comboBoxScheduleClass.SelectedItem НЕ ComboBoxItem или null. " +
                                "Text='" + comboBoxScheduleClass.Text + "'. Возвращаем ClassID пользователя.");
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

        // Название класса
        private string GetClassName(int classId)
        {
            return classId == 1 ? "5А" : $"Класс {classId}";
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

        // Инициализация выпадающего списка с классами для расписания.
        private void InitializeScheduleClassCombo()
        {
            bool isDirector = UserController.CurrentUser.PermissionID == 3;

            if (comboBoxScheduleClass != null)
            {
                comboBoxScheduleClass.Dispose();
                comboBoxScheduleClass = null;
            }

            if (isDirector)
            {
                comboBoxScheduleClass = new ComboBox
                {
                    Location = new Point(10, 10),
                    Size = new Size(120, 25),
                    DropDownStyle = ComboBoxStyle.DropDownList
                };

                var classes = ClassController._controller.GetAllClasses();
                comboBoxScheduleClass.Items.Clear();

                foreach (var cls in classes)
                {
                    var item = new ComboBoxItem { Text = cls.ClassName, ClassID = cls.ClassID };
                    comboBoxScheduleClass.Items.Add(item);
                }

                if (comboBoxScheduleClass.Items.Count > 0)
                    comboBoxScheduleClass.SelectedIndex = 0;

                comboBoxScheduleClass.SelectedIndexChanged += ComboBoxScheduleClass_SelectedIndexChanged;
                this.Controls.Add(comboBoxScheduleClass);
                comboBoxScheduleClass.BringToFront();
                sheduleGridView.Top = 45;
            }
            else
            {
                sheduleGridView.Top = 10;
            }
        }

        // Слушатель выпадающего списка с классами для директора
        private void ComboBoxScheduleClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxScheduleClass.SelectedItem is ComboBoxItem selectedItem)
            {
                LoadScheduleGrid(selectedItem.ClassID);
            }
        }

        // Сохранение расписания (в БД)
        private void SaveScheduleGridChanges()
        {
            FileLogger.logger.Info(
                $"SaveScheduleGridChanges START. UserID={UserController.CurrentUser.UserID}, " +
                $"PermissionID={UserController.CurrentUser.PermissionID}, " +
                $"Role={UserController.CurrentUser.PermissionName}");

            if (UserController.CurrentUser.PermissionID < 3)
            {
                FileLogger.logger.Warn(
                    $"SaveScheduleGridChanges: доступ запрещён. PermissionID={UserController.CurrentUser.PermissionID}");
                return;
            }

            int classId = GetCurrentClassId();
            FileLogger.logger.Info(
                $"SaveScheduleGridChanges: текущий класс ClassID={classId}, ClassName={GetClassName(classId)}");

            int savedCount = 0;
            int totalRows = sheduleGridView.Rows.Count;
            FileLogger.logger.Info(
                $"SaveScheduleGridChanges: начало обхода строк расписания. Всего строк={totalRows}");

            int rowIndex = -1;

            try
            {
                foreach (DataGridViewRow row in sheduleGridView.Rows)
                {
                    rowIndex = row.Index;

                    if (row.IsNewRow)
                    {
                        FileLogger.logger.Debug($"Row {rowIndex}: пропущена (IsNewRow=true).");
                        continue;
                    }

                    string rawId = row.Cells[0].Value?.ToString(); // может быть null/пусто для новых
                    string rawDay = row.Cells[1].Value?.ToString();
                    string rawLesson = row.Cells[2].Value?.ToString();
                    string rawTime = row.Cells[3].Value?.ToString();
                    string rawSubject = row.Cells[4].Value?.ToString();
                    string rawTeacher = row.Cells[5].Value?.ToString();

                    FileLogger.logger.Debug(
                        $"Row {rowIndex}: RAW: ID='{rawId}', Day='{rawDay}', Lesson='{rawLesson}', " +
                        $"Time='{rawTime}', Subject='{rawSubject}', Teacher='{rawTeacher}'");

                    // --- Парсинг времени
                    TimeSpan? lessonTime = null;
                    if (!string.IsNullOrWhiteSpace(rawTime))
                    {
                        TimeSpan time;
                        if (TimeSpan.TryParse(rawTime, out time))
                        {
                            lessonTime = time;
                            FileLogger.logger.Debug(
                                $"Row {rowIndex}: LessonTime успешно распарсен: {time}");
                        }
                        else
                        {
                            FileLogger.logger.Warn(
                                $"Row {rowIndex}: неверное время '{rawTime}', TimeSpan.TryParse вернул false.");
                            MessageBox.Show($"❌ Ошибка {rawTime}: Время неверно!");
                            continue;
                        }
                    }
                    else
                    {
                        FileLogger.logger.Debug(
                            $"Row {rowIndex}: поле времени пустое, LessonTime будет NULL.");
                    }

                    // --- Парсинг ID / Day / LessonNumber
                    int scheduleId = 0;      // 0 = новая строка
                    byte dayOfWeek;
                    byte lessonNumber;
                    try
                    {
                        // Если rawId null/пустой/мусор — TryParse даст 0, это ОК
                        int.TryParse(rawId, out scheduleId);
                        dayOfWeek = GetDayNumber(rawDay ?? "Пн");
                        lessonNumber = byte.Parse(string.IsNullOrWhiteSpace(rawLesson) ? "1" : rawLesson);
                    }
                    catch (Exception exParse)
                    {
                        FileLogger.logger.Error(
                            $"Row {rowIndex}: ошибка парсинга DayOfWeek/LessonNumber. " +
                            $"rawId='{rawId}', rawDay='{rawDay}', rawLesson='{rawLesson}'", exParse);
                        MessageBox.Show($"❌ Строка {rowIndex + 1}: ошибка парсинга данных (День/Урок)!");
                        continue;
                    }

                    // --- Получаем SubjectID и TeacherID
                    int subjectId = 0;
                    int teacherId = 0;

                    try
                    {
                        subjectId = SubjectController._controller
                            .GetSubjectIdByName(rawSubject ?? "");
                        FileLogger.logger.Debug(
                            $"Row {rowIndex}: SubjectName='{rawSubject}', SubjectID={subjectId}");
                    }
                    catch (Exception exSubject)
                    {
                        FileLogger.logger.Error(
                            $"Row {rowIndex}: исключение при получении SubjectID для '{rawSubject}'", exSubject);
                    }

                    try
                    {
                        var teacher = TeacherController._controller
                            .GetTeacherOrDirectorByName(rawTeacher ?? "");
                        teacherId = teacher?.UserID ?? 0;
                        FileLogger.logger.Debug(
                            $"Row {rowIndex}: TeacherName='{rawTeacher}', TeacherID={teacherId}");
                    }
                    catch (Exception exTeacher)
                    {
                        FileLogger.logger.Error(
                            $"Row {rowIndex}: исключение при получении TeacherID для '{rawTeacher}'", exTeacher);
                    }

                    var schedule = new ScheduleItem
                    {
                        ScheduleID = scheduleId,   // 0 для новых
                        DayOfWeek = dayOfWeek,
                        LessonNumber = lessonNumber,
                        ClassID = classId,
                        SubjectID = subjectId,
                        TeacherID = teacherId,
                        LessonTime = lessonTime
                    };

                    FileLogger.logger.Debug(
                        $"Row {rowIndex}: ScheduleItem: " +
                        $"ScheduleID={schedule.ScheduleID}, DayOfWeek={schedule.DayOfWeek}, " +
                        $"LessonNumber={schedule.LessonNumber}, ClassID={schedule.ClassID}, " +
                        $"SubjectID={schedule.SubjectID}, TeacherID={schedule.TeacherID}, " +
                        $"LessonTime={(schedule.LessonTime.HasValue ? schedule.LessonTime.Value.ToString() : "NULL")}");

                    if (schedule.SubjectID == 0 || schedule.TeacherID == 0)
                    {
                        FileLogger.logger.Warn(
                            $"Row {rowIndex}: SubjectID={schedule.SubjectID}, TeacherID={schedule.TeacherID} — " +
                            "одно из значений равно 0, строка пропускается.");
                        MessageBox.Show(
                            $"❌ Строка {rowIndex + 1}: Предмет ({schedule.SubjectID})/учитель ({schedule.TeacherID}) не найдены!");
                        continue;
                    }

                    try
                    {
                        // контроллер должен вернуть ID:
                        // - существующий (при обновлении)
                        // - новый уникальный (при вставке, если ScheduleID == 0)
                        int resultId = SheduleController._controller.InsertOrUpdateSchedulePart(schedule);

                        // обязательно прописываем ID обратно в грид
                        row.Cells[0].Value = resultId;

                        savedCount++;
                        FileLogger.logger.Info(
                            $"Row {rowIndex}: успешно сохранён/обновлён. OldScheduleID={scheduleId}, NewScheduleID={resultId}");
                    }
                    catch (Exception exSave)
                    {
                        FileLogger.logger.Error(
                            $"Row {rowIndex}: ошибка при сохранении ScheduleItem в БД. ScheduleID={schedule.ScheduleID}", exSave);
                        MessageBox.Show(
                            $"❌ Строка {rowIndex + 1}: ошибка сохранения урока в БД: {exSave.Message}");
                    }
                }

                sheduleLabel.Text =
                    $"Расписание класса {GetClassName(classId)} ({savedCount} уроков сохранено)";
                _scheduleChanged = false;

                FileLogger.logger.Info(
                    $"SaveScheduleGridChanges END. ClassID={classId}, SavedCount={savedCount}");

                MessageBox.Show(
                    $"✅ Сохранено {savedCount} уроков для класса {GetClassName(classId)}!");
            }
            catch (Exception ex)
            {
                FileLogger.logger.Error(
                    "SaveScheduleGridChanges: необработанное исключение на уровне метода.", ex);
                MessageBox.Show(
                    $"❌ Критическая ошибка сохранения расписания: {ex.Message}");
            }
        }

        // Слушатель изменений (вставка) в расписании.
        // Меняет только флаг.
        private void DataGridViewSchedule_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (UserController.CurrentUser.PermissionID != 3) return; 

            _scheduleChanged = true; 
        }

        // Слушатель изменений (удаление) в расписании.
        // Меняет только флаг.
        private void DataGridViewSchedule_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            FileLogger.logger.Info("DataGridViewSchedule_UserDeletedRow ВЫЗВАН!"); // ← ДОБАВЬ

            if (UserController.CurrentUser.PermissionID != 3)
            {
                FileLogger.logger.Warn("Удаление заблокировано: PermissionID != 3");
                return;
            }

            int scheduleId = int.Parse(e.Row.Cells[0].Value?.ToString() ?? "0");
            FileLogger.logger.Info($"Удаляем ScheduleID={scheduleId}");

            if (scheduleId > 0)
            {
                SheduleController._controller.DeleteSchedulePartById(scheduleId);
                _scheduleChanged = true;
                FileLogger.logger.Info($"ScheduleID={scheduleId} удалён из БД");
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

        // Подготовка статистики
        private void LoadStatisticsGrid()
        {
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