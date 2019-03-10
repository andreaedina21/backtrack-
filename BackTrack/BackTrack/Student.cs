using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackTrack
{
    class Student
    {
        public List<int> Neighbours { get; set; }
        public List<int> Variants { get; set; }
        public int SelectedVariant { get; set; }
    }
}
