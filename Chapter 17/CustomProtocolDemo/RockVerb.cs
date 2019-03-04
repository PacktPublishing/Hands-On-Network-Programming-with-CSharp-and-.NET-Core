using System;
using System.Collections.Generic;
using System.Text;

namespace CustomProtocolDemo {
public enum RockVerb {
    Delete = 0b000000000000000000000000,
    Insert = 0b010000000000000000000000,
    Update = 0b100000000000000000000000
    }
}
