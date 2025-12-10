using System;
using System.ComponentModel.DataAnnotations;

namespace school.Models
{
    /// <summary>
    /// Таблица классов
    /// </summary>
    public class Class
    {
        public int ClassID { get; set; }

        [Required] // @NotNull
        [StringLength(10)]
        public string ClassName { get; set; } = ""; // Getter/Setter
    }

    /// <summary>
    /// Таблица предметов
    /// </summary>
    public class Subject
    {
        public int SubjectID { get; set; }

        [Required]
        [StringLength(50)]
        public string SubjectName { get; set; } = "";
    }

    /// <summary>
    /// Таблица пользователей
    /// </summary>
    public class User
    {
        public int UserID { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = "";

        [Required]
        public int PermissionID { get; set; }

        [StringLength(50)]
        public string PermissionName { get; set; } = ""; // "Обычный учитель", "Ученик" и т.д.

        public int? ClassID { get; set; }  // Nullable int для учителей
        public Class Class { get; set; }   // Navigation property
    }

    /// <summary>
    /// Таблица домашних заданий
    /// </summary>
    public class Homework
    {
        public int HomeworkID { get; set; }

        [Required]
        public DateTime AssignmentDate { get; set; }

        [Required]
        public int ClassID { get; set; }
        public Class Class { get; set; }

        [Required]
        public int SubjectID { get; set; }
        public Subject Subject { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; } = "";

        [Required]
        public int TeacherID { get; set; }
        public User Teacher { get; set; }

        public string ClassNameDisplay => Class?.ClassName ?? "";
        public string SubjectNameDisplay => Subject?.SubjectName ?? "";
        public string TeacherNameDisplay => Teacher?.FullName ?? "";
    }

    /// <summary>
    /// [translate:Таблица оценок]
    /// </summary>
    public class Grade
    {
        public int GradeID { get; set; }

        [Required]
        public DateTime GradeDate { get; set; }

        [Required]
        public int StudentID { get; set; }
        public User Student { get; set; }

        [Required]
        public int SubjectID { get; set; }
        public Subject Subject { get; set; }

        [Required]
        [Range(1, 5)]
        public byte GradeValue { get; set; } // TINYINT 1-5

        [Required]
        public int TeacherID { get; set; }
        public User Teacher { get; set; }

        public string SubjectNameDisplay => Subject?.SubjectName ?? "";
        public string TeacherNameDisplay => Teacher?.FullName ?? "";
        public string StudentNameDisplay => Student?.FullName ?? "";
    }

    public class ScheduleItem
    {
        public int ScheduleID { get; set; }
        public byte DayOfWeek { get; set; }
        public byte LessonNumber { get; set; }
        public TimeSpan? LessonTime { get; set; }
        public int ClassID { get; set; }
        public string ClassName { get; set; }
        public int SubjectID { get; set; }
        public string SubjectName { get; set; }
        public int TeacherID { get; set; }
        public string TeacherName { get; set; }

        // ✅ Свойства для DataGridView (C# 7.3)
        public string DayOfWeekDisplay => GetDayName(DayOfWeek);
        public string LessonTimeDisplay => LessonTime?.ToString(@"hh\:mm") ?? "";

        private static string GetDayName(byte dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case 1: return "Понедельник";
                case 2: return "Вторник";
                case 3: return "Среда";
                case 4: return "Четверг";
                case 5: return "Пятница";
                case 6: return "Суббота";
                case 7: return "Воскресенье";
                default: return "Неизвестно";
            }
        }
    }
}
