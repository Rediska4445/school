using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace school
{
    public class QuarterGrade
    {
        public int QuarterGradeID { get; set; }
        public int StudentID { get; set; }
        public int SubjectID { get; set; }
        public short Year { get; set; }
        public byte? Quarter1Grade { get; set; }
        public byte? Quarter2Grade { get; set; }
        public byte? Quarter3Grade { get; set; }
        public byte? Quarter4Grade { get; set; }
    }
}
