using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace school
{
    public class ComboBoxItem
    {
        public string Text { get; set; }
        public int ClassID { get; set; }
        public override string ToString() => Text; 
    }
}
