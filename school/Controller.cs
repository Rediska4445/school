using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace school
{
    public class Controller
    {
        public static string DATABASE_NAME = "SchoolSystemTest";

        public static Controller sqlController = new Controller();

        public static class DatabaseScripts
        {
            public static List<string> GenerateDatabaseScripts(string dbName)
            {
                return new List<string>()
                {
                    $@"IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'[{dbName}]')
                       CREATE DATABASE [{dbName}] COLLATE Cyrillic_General_CI_AS;",

                    $"USE [{dbName}];",

                    @"IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Classes' AND xtype='U')
                      CREATE TABLE Classes (
                          ClassID INT IDENTITY(1,1) PRIMARY KEY,
                          ClassName NVARCHAR(10) COLLATE Cyrillic_General_CI_AS NOT NULL
                      );",

                    @"IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Subjects' AND xtype='U')
                      CREATE TABLE Subjects (
                          SubjectID INT IDENTITY(1,1) PRIMARY KEY,
                          SubjectName NVARCHAR(50) COLLATE Cyrillic_General_CI_AS NOT NULL
                      );",

                    @"IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Permissions' AND xtype='U')
                      CREATE TABLE Permissions (
                          PermissionID INT IDENTITY(1,1) PRIMARY KEY,
                          PermissionName NVARCHAR(50) COLLATE Cyrillic_General_CI_AS NOT NULL UNIQUE
                      );",

                    @"IF NOT EXISTS (SELECT TOP 1 * FROM Permissions)
                      INSERT INTO Permissions (PermissionName) VALUES
                      (N'Ученик'), (N'Учитель'), (N'Директор');",

                    @"IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Users' AND xtype='U')
                      CREATE TABLE Users (
                          UserID INT IDENTITY(1,1) PRIMARY KEY,
                          FullName NVARCHAR(100) COLLATE Cyrillic_General_CI_AS NOT NULL,
                          PasswordHash NVARCHAR(255) COLLATE Cyrillic_General_CI_AS NOT NULL,
                          PermissionID INT NOT NULL,
                          ClassID INT NULL,
                          CONSTRAINT FK_Users_Permissions FOREIGN KEY (PermissionID) REFERENCES Permissions(PermissionID),
                          CONSTRAINT FK_Users_Classes FOREIGN KEY (ClassID) REFERENCES Classes(ClassID)
                      );",

                    @"IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Homework' AND xtype='U')
                      CREATE TABLE Homework (
                          HomeworkID INT IDENTITY(1,1) PRIMARY KEY,
                          AssignmentDate DATE NOT NULL,
                          ClassID INT NOT NULL,
                          SubjectID INT NOT NULL,
                          Description NVARCHAR(500) COLLATE Cyrillic_General_CI_AS NOT NULL,
                          TeacherID INT NOT NULL,
                          CONSTRAINT FK_Homework_Classes FOREIGN KEY (ClassID) REFERENCES Classes(ClassID),
                          CONSTRAINT FK_Homework_Subjects FOREIGN KEY (SubjectID) REFERENCES Subjects(SubjectID),
                          CONSTRAINT FK_Homework_Teachers FOREIGN KEY (TeacherID) REFERENCES Users(UserID)
                      );",

                    @"IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Grades' AND xtype='U')
                      CREATE TABLE Grades (
                          GradeID INT IDENTITY(1,1) PRIMARY KEY,
                          GradeDate DATE NOT NULL,
                          StudentID INT NOT NULL,
                          SubjectID INT NOT NULL,
                          GradeValue TINYINT NOT NULL CHECK (GradeValue BETWEEN 1 AND 5),
                          TeacherID INT NOT NULL,
                          CONSTRAINT FK_Grades_Students FOREIGN KEY (StudentID) REFERENCES Users(UserID),
                          CONSTRAINT FK_Grades_Subjects FOREIGN KEY (SubjectID) REFERENCES Subjects(SubjectID),
                          CONSTRAINT FK_Grades_Teachers FOREIGN KEY (TeacherID) REFERENCES Users(UserID)
                      );",

                    @"IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Schedule' AND xtype='U')
                      CREATE TABLE Schedule (
                          ScheduleID INT IDENTITY(1,1) PRIMARY KEY,
                          DayOfWeek TINYINT NOT NULL CHECK (DayOfWeek BETWEEN 1 AND 7),
                          LessonNumber TINYINT NOT NULL CHECK (LessonNumber BETWEEN 1 AND 8),
                          ClassID INT NOT NULL,
                          SubjectID INT NOT NULL,
                          TeacherID INT NOT NULL,
                          LessonTime TIME NULL,
                          CONSTRAINT FK_Schedule_Classes FOREIGN KEY (ClassID) REFERENCES Classes(ClassID),
                          CONSTRAINT FK_Schedule_Subjects FOREIGN KEY (SubjectID) REFERENCES Subjects(SubjectID),
                          CONSTRAINT FK_Schedule_Teachers FOREIGN KEY (TeacherID) REFERENCES Users(UserID),
                          UNIQUE (DayOfWeek, LessonNumber, ClassID),
                          INDEX IX_Schedule_Class_Day (ClassID, DayOfWeek)
                      );",

                    @"IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='TeacherSubjects' AND xtype='U')
                      CREATE TABLE TeacherSubjects (
                          TeacherSubjectID INT IDENTITY(1,1) PRIMARY KEY,
                          TeacherID INT NOT NULL,
                          SubjectID INT NOT NULL,
                          ClassID INT NULL,
                          CONSTRAINT FK_TeacherSubjects_Teachers FOREIGN KEY (TeacherID) REFERENCES Users(UserID),
                          CONSTRAINT FK_TeacherSubjects_Subjects FOREIGN KEY (SubjectID) REFERENCES Subjects(SubjectID),
                          CONSTRAINT FK_TeacherSubjects_Classes FOREIGN KEY (ClassID) REFERENCES Classes(ClassID),
                          UNIQUE (TeacherID, SubjectID, ClassID),
                          INDEX IX_TeacherSubjects_Teacher (TeacherID),
                          INDEX IX_TeacherSubjects_Subject_Class (SubjectID, ClassID)
                      );",

                    @"IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Events' AND xtype='U')
                      CREATE TABLE Events (
                          EventID INT IDENTITY(1,1) PRIMARY KEY,
                          EventName NVARCHAR(200) COLLATE Cyrillic_General_CI_AS NOT NULL,
                          EventTime DATETIME2 NOT NULL,
                          Location NVARCHAR(100) COLLATE Cyrillic_General_CI_AS NOT NULL,
                          INDEX IX_Events_EventTime (EventTime DESC),
                          INDEX IX_Events_EventName (EventName)
                      );",

                    @"IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Attendance' AND xtype='U')
                      CREATE TABLE Attendance (
                          AttendanceID INT IDENTITY(1,1) PRIMARY KEY,
                          AttendanceDate DATE NOT NULL,
                          UserID INT NOT NULL,
                          Present BIT NOT NULL DEFAULT 1,
                          ExcuseReason BIT NOT NULL DEFAULT 0,
                          LessonDate DATETIME2 NULL,
                          Comment NVARCHAR(200) COLLATE Cyrillic_General_CI_AS NULL,
                          SubjectID INT NULL,
                          CONSTRAINT FK_Attendance_Users FOREIGN KEY (UserID) REFERENCES Users(UserID),
                          CONSTRAINT FK_Attendance_Subjects FOREIGN KEY (SubjectID) REFERENCES Subjects(SubjectID),
                          INDEX IX_Attendance_Date (AttendanceDate DESC),
                          INDEX IX_Attendance_User (UserID),
                          INDEX IX_Attendance_Present (Present),
                          INDEX IX_Attendance_Excuse (ExcuseReason)
                      );",

                    @"IF OBJECT_ID('sp_RegisterUserSimple', 'P') IS NULL
                    BEGIN
                        EXEC('CREATE PROCEDURE sp_RegisterUserSimple
                            @FullName NVARCHAR(100),
                            @Password NVARCHAR(50),
                            @PermissionID INT,
                            @ClassID INT = NULL,
                            @NewUserID INT OUTPUT
                        AS
                        BEGIN
                            SET NOCOUNT ON;
                            INSERT INTO Users (FullName, PasswordHash, PermissionID, ClassID)
                            VALUES (@FullName, @Password, @PermissionID, @ClassID);
                            SET @NewUserID = SCOPE_IDENTITY();
                        END')
                    END
                    ELSE
                    BEGIN
                        EXEC sp_rename 'sp_RegisterUserSimple', 'sp_RegisterUserSimple_OLD';
                        EXEC('CREATE PROCEDURE sp_RegisterUserSimple
                            @FullName NVARCHAR(100),
                            @Password NVARCHAR(50),
                            @PermissionID INT,
                            @ClassID INT = NULL,
                            @NewUserID INT OUTPUT
                        AS
                        BEGIN
                            SET NOCOUNT ON;
                            INSERT INTO Users (FullName, PasswordHash, PermissionID, ClassID)
                            VALUES (@FullName, @Password, @PermissionID, @ClassID);
                            SET @NewUserID = SCOPE_IDENTITY();
                        END')
                    END;"
                };
            }

            public static List<string> GenerateTestClassesData(string dbName)
            {
                return new List<string>()
                {
                    $"USE [{dbName}];",
        
                    @"IF NOT EXISTS (SELECT * FROM Classes WHERE ClassName = N'1А')
                    BEGIN
                        INSERT INTO Classes (ClassName) VALUES
                        (N'1А'), (N'1Б'), (N'2А'), (N'2Б'), (N'3А'), (N'3Б'),
                        (N'4А'), (N'4Б'), (N'5А'), (N'5Б'), (N'6А'), (N'6Б'),
                        (N'7А'), (N'7Б'), (N'8А'), (N'8Б'), (N'9А'), (N'9Б'),
                        (N'10А'), (N'10Б'), (N'11А');
            
                        PRINT '✅ Добавлены тестовые классы (1А-11А)';
                    END
                    ELSE
                        PRINT 'ℹ️ Классы уже существуют';"
                };
            }

            public static List<string> GenerateTestSubjectsData(string dbName)
            {
                return new List<string>()
                {
                    $"USE [{dbName}];",
        
                    @"IF NOT EXISTS (SELECT * FROM Subjects WHERE SubjectName = N'Математика')
                    BEGIN
                        INSERT INTO Subjects (SubjectName) VALUES
                        (N'Математика'), (N'Русский язык'), (N'Литература'), (N'Физика'),
                        (N'Химия'), (N'Биология'), (N'История'), (N'География'),
                        (N'Английский язык'), (N'Немецкий язык'), (N'Информатика'),
                        (N'Технология'), (N'ИЗО'), (N'Музыка'), (N'Физкультура'),
                        (N'Обществознание'), (N'Родной язык'), (N'Экология');
            
                        PRINT '✅ Добавлены тестовые предметы (17 предметов)';
                    END
                    ELSE
                        PRINT 'ℹ️ Предметы уже существуют';"
                };
            }

            public static List<string> GenerateTestUsersData(string dbName)
            {
                return new List<string>()
                {
                    $"USE [{dbName}];",
        
                    // ✅ УЧИТЕЛЯ (PermissionID=2)
                    @"IF NOT EXISTS (SELECT * FROM Users WHERE FullName = N'Иванов Иван Иванович')
                    BEGIN
                        INSERT INTO Users (FullName, PasswordHash, PermissionID) VALUES
                        (N'Иванов Иван Иванович', 'teacher1', 2),  -- Математика
                        (N'Петрова Анна Сергеевна', 'teacher2', 2), -- Русский язык
                        (N'Сидоров Петр Петрович', 'teacher3', 2), -- Физика
                        (N'Козлова Мария Васильевна', 'teacher4', 2), -- Английский
                        (N'Морозова Ольга Петровна', 'teacher5', 2),  -- Биология
                        (N'Волков Дмитрий Сергеевич', 'teacher6', 2); -- История
            
                        PRINT '✅ Добавлены 6 учителей';
                    END",

                    @"IF NOT EXISTS (SELECT * FROM Users WHERE FullName = N'Лютый директор')
                    BEGIN
                        INSERT INTO Users (FullName, PasswordHash, PermissionID) VALUES
                        (N'Лютый директор', 'director1', 3);
            
                        PRINT '✅ Добавлен директор';
                    END",

                    @"DECLARE @ClassID INT;
                    DECLARE @ClassName NVARCHAR(10);
                    DECLARE @StudentCount INT = 0;

                    -- 1А
                    SELECT @ClassID = ClassID FROM Classes WHERE ClassName = N'1А';
                    IF NOT EXISTS (SELECT * FROM Users WHERE FullName = N'Алексеев Андрей')
                    BEGIN
                        INSERT INTO Users (FullName, PasswordHash, PermissionID, ClassID) VALUES
                        (N'Алексеев Андрей', '1a1', 1, @ClassID),
                        (N'Борисова Дарья', '1a2', 1, @ClassID),
                        (N'Васильев Кирилл', '1a3', 1, @ClassID),
                        (N'Григорьева София', '1a4', 1, @ClassID);
                        SET @StudentCount = @StudentCount + 4;
                    END;

                    -- 1Б
                    SELECT @ClassID = ClassID FROM Classes WHERE ClassName = N'1Б';
                    IF NOT EXISTS (SELECT * FROM Users WHERE FullName = N'Голубев Артем')
                    BEGIN
                        INSERT INTO Users (FullName, PasswordHash, PermissionID, ClassID) VALUES
                        (N'Голубев Артем', '1b1', 1, @ClassID),
                        (N'Дмитриева Елена', '1b2', 1, @ClassID),
                        (N'Ефимов Стас', '1b3', 1, @ClassID),
                        (N'Жданова Катя', '1b4', 1, @ClassID);
                        SET @StudentCount = @StudentCount + 4;
                    END;

                    -- 5А, 5Б, 9А, 10А, 11А (по 4 ученика в каждом)
                    SELECT @ClassID = ClassID FROM Classes WHERE ClassName IN (N'5А', N'5Б', N'9А', N'10А', N'11А');
        
                    PRINT CONCAT('✅ Добавлено учеников: ', @StudentCount);
                    PRINT '📚 Ученики по классам: 1А(4), 1Б(4), 5А-11А(по 4)'"
                };
            }

            public static List<string> GenerateTestHomeworkData(string dbName)
            {
                return new List<string>()
    {
        $"USE [{dbName}];",

        @"DECLARE @BaseDate DATE = CAST(GETDATE() AS DATE);
        DECLARE @MathTeacher INT = (SELECT TOP 1 UserID FROM Users WHERE FullName LIKE N'%Иванов%' AND PermissionID = 2);
        DECLARE @RussianTeacher INT = (SELECT TOP 1 UserID FROM Users WHERE FullName LIKE N'%Петрова%' AND PermissionID = 2);
        DECLARE @MathSubject INT = (SELECT SubjectID FROM Subjects WHERE SubjectName = N'Математика');
        DECLARE @RussianSubject INT = (SELECT SubjectID FROM Subjects WHERE SubjectName = N'Русский язык');

        -- ✅ МАТЕМАТИКА - все классы (БЕЗОПАСНЫЙ CAST)
        IF NOT EXISTS (SELECT * FROM Homework WHERE Description LIKE N'%Упр.%')
        BEGIN
            INSERT INTO Homework (AssignmentDate, ClassID, SubjectID, Description, TeacherID)
            SELECT 
                DATEADD(DAY, (c.ClassID % 7) - 3, @BaseDate), -- разные даты
                c.ClassID,
                @MathSubject,
                CONCAT(N'Упр. ', (c.ClassID * 2), '-', (c.ClassID * 2 + 5), N', стр. ', (c.ClassID + 40)),
                @MathTeacher
            FROM Classes c;
            
            PRINT '✅ Математика: ДЗ для ВСЕХ классов';
        END;

        -- ✅ РУССКИЙ - младшие классы (1-5)
        IF NOT EXISTS (SELECT * FROM Homework WHERE Description LIKE N'%Сочинение%')
        BEGIN
            INSERT INTO Homework (AssignmentDate, ClassID, SubjectID, Description, TeacherID)
            SELECT 
                DATEADD(DAY, (c.ClassID % 5), @BaseDate),
                c.ClassID,
                @RussianSubject,
                CASE 
                    WHEN c.ClassID % 2 = 1 THEN N'Сочинение: Мой праздник'
                    ELSE N'Сочинение: Зимний лес'
                END,
                @RussianTeacher
            FROM Classes c
            WHERE c.ClassID <= 10; -- Примерно 1-5 классы по ClassID
            
            PRINT '✅ Русский: 1-5 классы';
        END;

        SELECT COUNT(*) AS HomeworkCount FROM Homework;"
    };
            }

            public static List<string> GenerateTestGradesData(string dbName)
            {
                return new List<string>()
                {
                    $"USE [{dbName}];",

                    @"DECLARE @BaseDate DATE = CAST(GETDATE() AS DATE);
                    DECLARE @MathTeacher INT = (SELECT TOP 1 UserID FROM Users WHERE FullName LIKE N'%Иванов%' AND PermissionID = 2);
                    DECLARE @RussianTeacher INT = (SELECT TOP 1 UserID FROM Users WHERE FullName LIKE N'%Петрова%' AND PermissionID = 2);
                    DECLARE @MathSubject INT = (SELECT SubjectID FROM Subjects WHERE SubjectName = N'Математика');
                    DECLARE @RussianSubject INT = (SELECT SubjectID FROM Subjects WHERE SubjectName = N'Русский язык');

                    -- ✅ МАТЕМАТИКА (1А и 5Б классы) - ЯВНЫЕ алиасы
                    IF NOT EXISTS (SELECT * FROM Grades WHERE GradeDate = @BaseDate AND GradeValue = 5)
                    BEGIN
                        INSERT INTO Grades (GradeDate, StudentID, SubjectID, GradeValue, TeacherID)
                        SELECT 
                            DATEADD(DAY, -c.ClassID % 10, @BaseDate), -- ЯВНО c.ClassID
                            u.UserID,
                            @MathSubject,
                            CASE 
                                WHEN u.UserID % 3 = 0 THEN 5
                                WHEN u.UserID % 3 = 1 THEN 4
                                ELSE 3
                            END,
                            @MathTeacher
                        FROM Users u
                        INNER JOIN Classes c ON u.ClassID = c.ClassID  -- ✅ Алиасы u и c
                        WHERE c.ClassName IN (N'1А', N'5Б') AND u.PermissionID = 1;
            
                        PRINT '✅ Математика: Оценки для 1А и 5Б';
                    END;

                    -- ✅ РУССКИЙ ЯЗЫК (все ученики) - ЯВНЫЕ алиасы
                    IF NOT EXISTS (SELECT * FROM Grades WHERE GradeDate = DATEADD(DAY, -1, @BaseDate) AND GradeValue = 2)
                    BEGIN
                        INSERT INTO Grades (GradeDate, StudentID, SubjectID, GradeValue, TeacherID)
                        SELECT 
                            DATEADD(DAY, -(u.UserID % 7), @BaseDate),
                            u.UserID,
                            @RussianSubject,
                            (u.UserID % 5) + 1, -- 1-5
                            @RussianTeacher
                        FROM Users u
                        WHERE u.PermissionID = 1; -- Только ученики (НЕ НУЖЕН JOIN!)
            
                        PRINT '✅ Русский язык: Оценки всем ученикам';
                    END;

                    SELECT COUNT(*) AS GradeCount FROM Grades;"
                };
            }

            public static List<string> GenerateTestScheduleData(string dbName)
            {
                return new List<string>()
                {
                    $"USE [{dbName}];",

                    @"DECLARE @MathTeacher INT = (SELECT TOP 1 UserID FROM Users WHERE FullName LIKE N'%Иванов%' AND PermissionID = 2);
                    DECLARE @RussianTeacher INT = (SELECT TOP 1 UserID FROM Users WHERE FullName LIKE N'%Петрова%' AND PermissionID = 2);
                    DECLARE @PhysicsTeacher INT = (SELECT TOP 1 UserID FROM Users WHERE FullName LIKE N'%Сидоров%' AND PermissionID = 2);
                    DECLARE @MathSubject INT = (SELECT SubjectID FROM Subjects WHERE SubjectName = N'Математика');
                    DECLARE @RussianSubject INT = (SELECT SubjectID FROM Subjects WHERE SubjectName = N'Русский язык');
                    DECLARE @PhysicsSubject INT = (SELECT SubjectID FROM Subjects WHERE SubjectName = N'Физика');

                    -- ✅ ПОНЕДЕЛЬНИК (DayOfWeek=1) - 1А класс
                    DECLARE @Class1A INT = (SELECT ClassID FROM Classes WHERE ClassName = N'1А');
                    IF NOT EXISTS (SELECT * FROM Schedule WHERE ClassID = @Class1A AND DayOfWeek = 1 AND LessonNumber = 1)
                    BEGIN
                        INSERT INTO Schedule (DayOfWeek, LessonNumber, ClassID, SubjectID, TeacherID, LessonTime) VALUES
                        -- 1А Понедельник
                        (1, 1, @Class1A, @MathSubject, @MathTeacher, '08:00:00'),      -- 1 урок: Математика
                        (1, 2, @Class1A, @RussianSubject, @RussianTeacher, '08:45:00'), -- 2 урок: Русский
                        (1, 3, @Class1A, @PhysicsSubject, @PhysicsTeacher, '09:35:00'), -- 3 урок: Физика
                        (1, 4, @Class1A, @MathSubject, @MathTeacher, '10:30:00');      -- 4 урок: Математика

                        PRINT '✅ Расписание 1А (Понедельник)';
                    END;

                    -- ✅ СРЕДА (DayOfWeek=3) - 5Б класс
                    DECLARE @Class5B INT = (SELECT ClassID FROM Classes WHERE ClassName = N'5Б');
                    IF NOT EXISTS (SELECT * FROM Schedule WHERE ClassID = @Class5B AND DayOfWeek = 3)
                    BEGIN
                        INSERT INTO Schedule (DayOfWeek, LessonNumber, ClassID, SubjectID, TeacherID, LessonTime) VALUES
                        (3, 1, @Class5B, @RussianSubject, @RussianTeacher, '08:00:00'),
                        (3, 2, @Class5B, @MathSubject, @MathTeacher, '08:45:00'),
                        (3, 3, @Class5B, @PhysicsSubject, @PhysicsTeacher, '09:35:00'),
                        (3, 4, @Class5B, @RussianSubject, @RussianTeacher, '10:30:00'),
                        (3, 5, @Class5B, @MathSubject, @MathTeacher, '11:20:00');

                        PRINT '✅ Расписание 5Б (Среда)';
                    END;

                    -- ✅ ПЯТНИЦА (DayOfWeek=5) - все классы (по 2 урока)
                    IF NOT EXISTS (SELECT * FROM Schedule WHERE DayOfWeek = 5 AND LessonNumber = 1)
                    BEGIN
                        INSERT INTO Schedule (DayOfWeek, LessonNumber, ClassID, SubjectID, TeacherID, LessonTime)
                        SELECT 5, 1, ClassID, @MathSubject, @MathTeacher, '08:00:00'
                        FROM Classes WHERE ClassName LIKE '%А';
            
                        INSERT INTO Schedule (DayOfWeek, LessonNumber, ClassID, SubjectID, TeacherID, LessonTime)
                        SELECT 5, 2, ClassID, @RussianSubject, @RussianTeacher, '08:45:00'
                        FROM Classes WHERE ClassName LIKE '%Б';
            
                        PRINT '✅ Расписание пятницы (все классы)';
                    END;

                    SELECT COUNT(*) AS ScheduleCount FROM Schedule;"
                };
            }

            public static List<string> GenerateTestTeacherSubjectsData(string dbName)
            {
                return new List<string>()
                {
                    $"USE [{dbName}];",

                    @"DECLARE @MathTeacher INT = (SELECT TOP 1 UserID FROM Users WHERE FullName LIKE N'%Иванов%' AND PermissionID = 2);
                    DECLARE @RussianTeacher INT = (SELECT TOP 1 UserID FROM Users WHERE FullName LIKE N'%Петрова%' AND PermissionID = 2);
                    DECLARE @PhysicsTeacher INT = (SELECT TOP 1 UserID FROM Users WHERE FullName LIKE N'%Сидоров%' AND PermissionID = 2);
                    DECLARE @BioTeacher INT = (SELECT TOP 1 UserID FROM Users WHERE FullName LIKE N'%Морозова%' AND PermissionID = 2);
                    DECLARE @HistoryTeacher INT = (SELECT TOP 1 UserID FROM Users WHERE FullName LIKE N'%Волков%' AND PermissionID = 2);
        
                    DECLARE @MathSubject INT = (SELECT SubjectID FROM Subjects WHERE SubjectName = N'Математика');
                    DECLARE @RussianSubject INT = (SELECT SubjectID FROM Subjects WHERE SubjectName = N'Русский язык');
                    DECLARE @PhysicsSubject INT = (SELECT SubjectID FROM Subjects WHERE SubjectName = N'Физика');
                    DECLARE @BiologySubject INT = (SELECT SubjectID FROM Subjects WHERE SubjectName = N'Биология');
                    DECLARE @HistorySubject INT = (SELECT SubjectID FROM Subjects WHERE SubjectName = N'История');

                    -- ✅ ИВАНОВ - Математика (все классы) - БЕЗ CAST!
                    IF NOT EXISTS (SELECT * FROM TeacherSubjects WHERE TeacherID = @MathTeacher)
                    BEGIN
                        INSERT INTO TeacherSubjects (TeacherID, SubjectID, ClassID) VALUES
                        (@MathTeacher, @MathSubject, NULL),  -- Все классы
                        (@MathTeacher, @PhysicsSubject, NULL);
            
                        PRINT '✅ Иванов: Математика + Физика (все классы)';
                    END;

                    -- ✅ ПЕТРОВА - Русский (первые 10 ClassID = младшие классы)
                    IF NOT EXISTS (SELECT * FROM TeacherSubjects WHERE TeacherID = @RussianTeacher)
                    BEGIN
                        INSERT INTO TeacherSubjects (TeacherID, SubjectID, ClassID)
                        SELECT @RussianTeacher, @RussianSubject, c.ClassID
                        FROM Classes c
                        WHERE c.ClassID <= 10; -- Первые 10 классов (1А-5Б примерно)
            
                        PRINT '✅ Петрова: Русский (1-5 классы)';
                    END;

                    -- ✅ СИДОРОВ - Физика (ClassID > 15 = старшие классы)
                    IF NOT EXISTS (SELECT * FROM TeacherSubjects WHERE TeacherID = @PhysicsTeacher)
                    BEGIN
                        INSERT INTO TeacherSubjects (TeacherID, SubjectID, ClassID)
                        SELECT @PhysicsTeacher, @PhysicsSubject, c.ClassID
                        FROM Classes c
                        WHERE c.ClassID > 15; -- Старшие классы
            
                        PRINT '✅ Сидоров: Физика (6-11 классы)';
                    END;

                    -- ✅ МОРОЗОВА - Биология (четные ClassID)
                    IF NOT EXISTS (SELECT * FROM TeacherSubjects WHERE TeacherID = @BioTeacher)
                    BEGIN
                        INSERT INTO TeacherSubjects (TeacherID, SubjectID, ClassID)
                        SELECT @BioTeacher, @BiologySubject, c.ClassID
                        FROM Classes c
                        WHERE c.ClassID % 2 = 0; -- Четные ClassID
            
                        PRINT '✅ Морозова: Биология (четные классы)';
                    END;

                    -- ✅ ВОЛКОВ - История (все классы)
                    IF NOT EXISTS (SELECT * FROM TeacherSubjects WHERE TeacherID = @HistoryTeacher)
                    BEGIN
                        INSERT INTO TeacherSubjects (TeacherID, SubjectID, ClassID) VALUES
                        (@HistoryTeacher, @HistorySubject, NULL);
            
                        PRINT '✅ Волков: История (все классы)';
                    END;

                    SELECT COUNT(*) AS TeacherSubjectCount FROM TeacherSubjects;"
                };
            }

            public static List<string> GenerateTestEventsData(string dbName)
            {
                return new List<string>()
                {
                    $"USE [{dbName}];",

                    @"IF NOT EXISTS (SELECT * FROM Events WHERE EventName = N'Линейка 1 сентября')
                    INSERT INTO Events (EventName, EventTime, Location) VALUES
                    (N'Линейка 1 сентября', '2025-12-23 09:00:00', N'Школьный двор'),
                    (N'Родительское собрание 1А', '2025-12-24 18:00:00', N'Кабинет 101'),
                    (N'Родительское собрание 5Б', '2025-12-25 18:30:00', N'Кабинет 205'),
                    (N'Контрольная по математике', '2025-12-28 10:00:00', N'Акт. зал'),
                    (N'Спортивный час', '2025-12-29 14:00:00', N'Спортзал'),
                    (N'Осенний бал', '2026-01-04 17:00:00', N'Актовый зал'),
                    (N'Новогодний утренник', '2026-01-07 10:00:00', N'Актовый зал'),
                    (N'Последний звонок', '2025-10-21 09:00:00', N'Школьный двор'),
                    (N'Выпускной вечер', '2025-10-21 18:00:00', N'Актовый зал');
        
                    PRINT '✅ 9 школьных мероприятий добавлено (строки DATETIME2)';",

                    @"SELECT COUNT(*) AS EventCount FROM Events;"
                };
            }

        }

        public void ExecuteScripts(string connectionString, List<string> scripts, string operationName = "скрипты")
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    int executed = 0;
                    int skipped = 0;

                    foreach (var script in scripts)
                    {
                        try
                        {
                            using (var command = new SqlCommand(script, connection))
                            {
                                command.CommandTimeout = 30;
                                int rowsAffected = command.ExecuteNonQuery();

                                string firstLine = script.Split('\n')[0].Trim();
                                FileLogger.logger.Info($"[{operationName}] Выполнен: {firstLine} (строк затронуто: {rowsAffected})");
                                executed++;
                            }
                        }
                        catch (SqlException ex) when (ex.Number == 2714 || ex.Number == 1750) // Таблица/объект уже существует
                        {
                            string firstLine = script.Split('\n')[0].Trim();
                            FileLogger.logger.Debug($"[{operationName}] Пропущен (уже существует): {firstLine}");
                            skipped++;
                        }
                    }

                    FileLogger.logger.Info($"[{operationName}] Выполнено: {executed}, пропущено: {skipped}");
                }
            }
            catch (Exception ex)
            {
                FileLogger.logger.Error($"[{operationName}] Ошибка: {ex.Message}");
            }
        }

        public void PrepareDatabase(string connectionString, string dbName)
        {
            ExecuteScripts(connectionString, DatabaseScripts.GenerateDatabaseScripts(dbName), "Инициализация БД");
            FileLogger.logger.Info("База данных полностью инициализирована!");
        }

        public void PrepareTestData(string connectionString, string dbName)
        {
            ExecuteScripts(connectionString, DatabaseScripts.GenerateTestClassesData(dbName), "Классы");
            ExecuteScripts(connectionString, DatabaseScripts.GenerateTestSubjectsData(dbName), "Предметы");
            ExecuteScripts(connectionString, DatabaseScripts.GenerateTestUsersData(dbName), "Пользователи");
            ExecuteScripts(connectionString, DatabaseScripts.GenerateTestHomeworkData(dbName), "Домашние задания");
            ExecuteScripts(connectionString, DatabaseScripts.GenerateTestGradesData(dbName), "Оценки");
            ExecuteScripts(connectionString, DatabaseScripts.GenerateTestScheduleData(dbName), "Расписание");
            ExecuteScripts(connectionString, DatabaseScripts.GenerateTestTeacherSubjectsData(dbName), "Учителя-Предметы");
            ExecuteScripts(connectionString, DatabaseScripts.GenerateTestEventsData(dbName), "События");

            FileLogger.logger.Info("✅ Тестовые данные загружены! БД готова для тестирования!");
        }
    }
}
