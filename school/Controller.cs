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
                  UNIQUE (AttendanceDate, UserID),
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
        }

        public void PrepareDatabase(string connectionString, string dbName)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    foreach (var script in DatabaseScripts.GenerateDatabaseScripts(dbName))
                    {
                        using (var command = new SqlCommand(script, connection))
                        {
                            command.CommandTimeout = 30;
                            command.ExecuteNonQuery();
                            FileLogger.logger.Info($"Выполнен скрипт: {script.Split('\n')[0].Trim()}");
                        }
                    }
                }

                FileLogger.logger.Info("База данных полностью инициализирована!");
            }
            catch (Exception ex)
            {
                FileLogger.logger.Error($"Ошибка инициализации БД: {ex.Message}");
            }
        }
    }
}
