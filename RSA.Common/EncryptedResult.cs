using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RSA.Common
{
    public class EncryptedResult
    {
        public string? result { get; set; }

        public string? privateKey { get; set; }

        public EncryptedResult(string? result, string? privateKey)
        {
            this.result = result;
            this.privateKey = privateKey;
        }
    }
}
