using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace school
{
    public class StatisticsController
    {
        public static StatisticsController _controller = new StatisticsController();

        
    }

    public class ClassStatistics
    {
        public double AverageGrade { get; set; }
        public int TotalDays { get; set; }
        public int PresentDays { get; set; }
        public int ExcusedDays { get; set; }
        public int TruancyDays { get; set; }
    }

}
