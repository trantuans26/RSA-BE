using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSA.Common
{
    public class RSAKey
    {
        public ulong RSA_n { get; set; }

        public int RSA_b { get; set; }

        public ulong RSA_a { get; set; }

        public int[]? banMa { get; set; }
    }
}
