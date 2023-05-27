using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSA.Common
{
    public class DecryptInput
    {
        public string? textToDecrypt { get; set; }

        public string? privateKeyString { get; set; }
    }
}
