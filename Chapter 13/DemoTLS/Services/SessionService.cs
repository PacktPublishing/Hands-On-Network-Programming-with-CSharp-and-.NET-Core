using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoTLS {
    public static class SessionService {
        public static string CurrentAlgorithm { get; set; }

        public static string GenerateSharedKeyWithPrivateKeyAndRandomSessionKey() {
            return "SHARED_SECRET";
        }
    }
}
