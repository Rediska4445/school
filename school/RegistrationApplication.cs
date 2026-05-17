using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace school
{
    public class RegistrationApplication
    {
        public int ApplicationID { get; set; }
        public string FullName { get; set; }
        public string PasswordHash { get; set; }
        public int PermissionID { get; set; }
        public int? ClassID { get; set; }
        public byte? Age { get; set; }
        public string Telephone { get; set; }
        public DateTime ApplicationDate { get; set; }
        public bool IsApproved { get; set; }
    }
}
