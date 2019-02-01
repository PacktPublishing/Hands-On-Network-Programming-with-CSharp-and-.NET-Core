using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessDataStore {
    public class FitnessRecord {
        public string title { get; set; }
        public string workoutType { get; set; }
        public string comments { get; set; }
        public DateTime workoutDate { get; set; }
    }
}
