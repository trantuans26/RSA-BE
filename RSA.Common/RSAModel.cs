using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSA.Common
{
    public class RSAModel
    {
        int GCD(int a, int b)
        {
            int r = a % b;
            if (r == 0)
                return b;
            else
                return GCD(b, r);
        }

        ulong EuCoLit(int a, int n) // Tính a^-1 mod n
        {
            int n0 = n, m;
            int t0 = 0, t1 = 1, q, t = 0;
            if (GCD(a, n) != 1)
                return 0;
            else
            {
                while (n % a != 0)
                {
                    q = n / a;
                    m = n;
                    n = a; a = m % a;
                    t = t0 - q * t1;
                    t0 = t1; t1 = t;
                }
                if (t < 0)
                {
                    t += n0;
                }

                return (ulong) t;
            }
        }

        ulong BinhPhuongVaNhan(ulong x, ulong m, ulong n) // Tính x^m mod n
        {
            if (m == 0)
                return 1;
            else if (m == 1)
                return (ulong) (x % n);
            else
            {
                ulong kq = BinhPhuongVaNhan(x, m / 2, n);
                kq = (kq * kq) % (ulong) n;
                if (m % 2 == 0)
                    return kq;
                else
                    return ((ulong) x * kq) % (ulong) n;
            }
        }

        int GetRandom(int Min, int Max)
        {
            Random rnd = new Random();
            int computer = rnd.Next() % (Max - Min + 1) + Min;
            return computer;
        }

        // Hàm kiểm tra xem x có phải là số nguyên tố hay không
        bool isPrime(ulong x, int iter = 5)
        {
            Random rnd = new Random();

            if (x < 2) return false;
            if (x != 2 && x % 2 == 0) return false;
            ulong d = x - 1;
            while (d % 2 == 0)
            {
                d >>= 1;
            }
            for (int i = 0; i < iter; i++)
            {
                ulong a = (ulong) rnd.Next() % (x - 1) + 1;
                ulong tmp = d;
                ulong mod = BinhPhuongVaNhan(a, tmp, x);
                while (tmp != x - 1 && mod != 1 && mod != x - 1)
                {
                    mod = (mod * mod) % x;
                    tmp <<= 1;
                }
                if (mod != x - 1 && tmp % 2 == 0)
                {
                    return false;
                }
            }
            return true;
        }

        public RSAKey taoKhoa()
        {
            RSAKey rsaKey = new RSAKey();
            rsaKey.banMa = new int[100];

            ulong p = 0, q = 0;

            while (p == q)
            {
                do
                {
                    p = (ulong) GetRandom(1000, 5000);
                } while (isPrime(p) == false);

                do
                {
                    q = (ulong) GetRandom(1001, 5000);
                } while (isPrime(q) == false);
            }

            //Tạo public key
            rsaKey.RSA_n = (ulong) (p * q);
            int fi = (int) ((p - 1) * (q - 1));

            do
            {
                rsaKey.RSA_b = GetRandom(2, fi);
            } while (GCD(rsaKey.RSA_b, fi) != 1);

            //Tạo private key
            rsaKey.RSA_a = EuCoLit(rsaKey.RSA_b, fi);

            return rsaKey;
        }

        public string MaHoa(string s, RSAKey rsaKey)
        {
            string bm = "";
            ulong k;
            for (int i = 0; i < s.Length; i++)
            {
                k = BinhPhuongVaNhan(s[i], (ulong) rsaKey.RSA_b, (ulong) rsaKey.RSA_n);
                rsaKey.banMa[i] = (int) k / 256;
                k = k % 256;
                bm += (char) k;
            }

            return bm;
        }

        public string GiaiMa(string s, RSAKey rsaKey)
        {
            ulong k, tam, n = (ulong) s.Length;
            string br = ""; 
            for (int i = 0; i < (int)n; i++)
            {
                int m = (int) s[i];
                if (m < 0)
                    m += 256;
                tam = (ulong) (rsaKey.banMa[i] * 256 + m);
                k = BinhPhuongVaNhan(tam, rsaKey.RSA_a, rsaKey.RSA_n);
                br += (char) k;
            }
            return br;
        }
    }
}
