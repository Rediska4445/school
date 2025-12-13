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
        public static Controller sqlController = new Controller();

        public List<string> Database = new List<string>()
        { 
            // 1. ✅ Создание БД
            @"IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'SchoolSystemTest')
              CREATE DATABASE SchoolSystemTest COLLATE Cyrillic_General_CI_AS;",

            // 2. ✅ USE БД
            "USE SchoolSystemTest;",

            // 3. ✅ Таблица Classes
            @"IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Classes' AND xtype='U')
              CREATE TABLE Classes (
                  ClassID INT IDENTITY(1,1) PRIMARY KEY,
                  ClassName NVARCHAR(10) COLLATE Cyrillic_General_CI_AS NOT NULL
              );",

            // 4. ✅ Таблица Subjects
            @"IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Subjects' AND xtype='U')
              CREATE TABLE Subjects (
                  SubjectID INT IDENTITY(1,1) PRIMARY KEY,
                  SubjectName NVARCHAR(50) COLLATE Cyrillic_General_CI_AS NOT NULL
              );",

            // 5. ✅ Таблица Permissions
            @"IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Permissions' AND xtype='U')
              CREATE TABLE Permissions (
                  PermissionID INT IDENTITY(1,1) PRIMARY KEY,
                  PermissionName NVARCHAR(50) COLLATE Cyrillic_General_CI_AS NOT NULL UNIQUE
              );",

            // 6. ✅ INSERT Permissions (если пусто)
            @"IF NOT EXISTS (SELECT TOP 1 * FROM Permissions)
              INSERT INTO Permissions (PermissionName) VALUES
              (N'Ученик'), (N'Учитель'), (N'Директор');",

            // 7. ✅ Таблица Users
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

            // 8. ✅ Таблица Homework
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

            // 9. ✅ Таблица Grades
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

            // 10. ✅ Таблица Schedule
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

            // 11. ✅ Таблица TeacherSubjects
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

            // 12. ✅ Таблица Events
            @"IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Events' AND xtype='U')
              CREATE TABLE Events (
                  EventID INT IDENTITY(1,1) PRIMARY KEY,
                  EventName NVARCHAR(200) COLLATE Cyrillic_General_CI_AS NOT NULL,
                  EventTime DATETIME2 NOT NULL,
                  Location NVARCHAR(100) COLLATE Cyrillic_General_CI_AS NOT NULL,
                  INDEX IX_Events_EventTime (EventTime DESC),
                  INDEX IX_Events_EventName (EventName)
              );"
        };

        public void PrepareDatabase(string connectionString)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    foreach (var script in Database)
                    {
                        using (var command = new SqlCommand(script, connection))
                        {
                            command.CommandTimeout = 30;
                            command.ExecuteNonQuery();
                            FileLogger.logger.Info($"✅ Выполнен скрипт: {script.Split('\n')[0].Trim()}");
                        }
                    }
                }

                FileLogger.logger.Info("🎉 База данных полностью инициализирована!");
            }
            catch (Exception ex)
            {
                FileLogger.logger.Error($"💥 Ошибка инициализации БД: {ex.Message}");
            }
        }
    }
}
