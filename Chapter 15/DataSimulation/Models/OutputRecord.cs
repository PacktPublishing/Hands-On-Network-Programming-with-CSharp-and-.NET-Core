using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSimulation {
    public class OutputRecord {
        public int Id { get; set; }
        public string SimpleString { get; set; }
        public List<string> StringList { get; set; } = new List<string>();
    }
}
