using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTM
{
    public class TestCase
    {
        public string TestName { get; set; }
        public string ParentFolder { get; set; }
        // Pass/Fail
        public bool status { get; set; }
        public bool selected { get; set; }
    }
}
