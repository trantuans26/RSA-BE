using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSA.Common
{
    public class KetQuaMaHoa
    {
        public string? ketQua { get; set; }

        public RSAKey? khoa { get; set; }

        public KetQuaMaHoa(string? ketQua, RSAKey? khoa)
        {
            this.ketQua = ketQua;
            this.khoa = khoa;
        }
    }
}
