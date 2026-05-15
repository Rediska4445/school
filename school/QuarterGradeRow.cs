using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace school
{
    public class QuarterGradeRow
    {
        public int SubjectID { get; set; }
        public string SubjectName { get; set; }
        public int? Quarter1Grade { get; set; }
        public int? Quarter2Grade { get; set; }
        public int? Quarter3Grade { get; set; }
        public int? Quarter4Grade { get; set; }
    }
}
