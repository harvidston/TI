using System;
using System.Numerics;
using System.Security.Cryptography;

namespace TI_3task
{
    public static class Helper
    {
        public static BigInteger GCD(BigInteger a, BigInteger b)
        {
            while (b > 0 && a > 0)
            {
                if (a > b)
                {
                    a %= b;
                }
                else
                {
                    b %= a;
                }
            }

            return a + b;
        }

        public static bool IsPrime(int number)
        {
            int divBorder = Convert.ToInt32(Math.Round(Math.Sqrt(number), MidpointRounding.AwayFromZero));
            for (int i = 2; i <= divBorder; i++)
            {
                if (number % i == 0)
                {
                    return false;
                }
            }

            return true;
        }

        public static BigInteger CalcEulerFunc(BigInteger p, BigInteger q)
        {
            return (p - 1) * (q - 1);
        }

        public static BigInteger CalcMultiplicativeInverseKey(BigInteger a, BigInteger b)
        {
            BigInteger d0 = a;
            BigInteger d1 = b;
            BigInteger x0 = BigInteger.One;
            BigInteger x1 = BigInteger.Zero;
            BigInteger y0 = BigInteger.Zero;
            BigInteger mulInverse = BigInteger.One;

            while (d1 > 1)
            {
                BigInteger q;
                BigInteger d2;
                BigInteger x2;
                BigInteger y2;

                q = d0 / d1;
                d2 = d0 % d1;
                x2 = x0 - q * x1;
                y2 = y0 - q * mulInverse;
                d0 = d1;
                d1 = d2;
                x0 = x1;
                x1 = x2;
                y0 = mulInverse;
                mulInverse = y2;
            }

            return (mulInverse.Sign != -1) ? mulInverse : (mulInverse + a);
        }

        public static bool MillerRabinTest(BigInteger n, int k)
        {
            if (n == 2 || n == 3)
            {
                return true;
            }

            if (n % 2 == 0)
            {
                return false;
            }

            BigInteger t = n - 1;
            int s = 0;
            while (t % 2 == 0)
            {
                t /= 2;
                s += 1;
            }

            for (int i = 0; i < k; i++)
            {
                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                byte[] arr = new byte[n.ToByteArray().LongLength];
                BigInteger a;

                do
                {
                    rng.GetBytes(arr);
                    a = new BigInteger(arr);
                }
                while (a < 2 || a >= n - 2);

                BigInteger x = BigInteger.ModPow(a, t, n);
                if (x == 1 || x == n - 1)
                    continue;

                for (int r = 1; r < s; r++)
                {
                    x = BigInteger.ModPow(x, 2, n);
                    if (x == 1)
                        return false;

                    if (x == n - 1)
                        break;
                }

                if (x != n - 1)
                {
                    return false;
                }
            }

            return true;
        }

        public static BigInteger FastExp(BigInteger number, BigInteger power, BigInteger mod)
        {
            BigInteger x = 1;
            while (power != 0)
            {
                while (power % 2 == 0)
                {
                    power /= 2;
                    number = (number * number) % mod;
                }

                power--;
                x = (x * number) % mod;
            }

            return x;
        }

        public static byte[] ToByteArray(ushort[] message)
        {
            byte[] result = new byte[message.Length * 2];
            int j = 0;
            for (int i = 0; i < message.Length; i++)
            {
                string res = GetByteString(GetBits(message[i]), 16);
                string firstByte = res.Substring(0, 8);
                string secondByte = res.Substring(8, 8);
                result[j] = Convert.ToByte(firstByte, 2);
                result[j + 1] = Convert.ToByte(secondByte, 2);
                j += 2;
            }

            return result;
        }

        public static ushort[] ToUShortArray(byte[] message)
        {
            ushort[] result = new ushort[message.Length / 2];
            int j = 0;
            for (int i = 0; i < result.Length; i++)
            {
                string res = GetByteString(Convert.ToString(message[j], 2), 8) + 
                    GetByteString(Convert.ToString(message[j + 1], 2), 8);
                result[i] = Convert.ToUInt16(res, 2);
                j += 2;
            }

            return result; 
        }

        private static string GetByteString(string str, int length)
        {
            if(str.Length < length)
            {
                int delta = length - str.Length;
                while(delta > 0)
                {
                    str = '0' + str;
                    delta--;
                }

                return str;
            }
            else
            {
                return str;
            }
        }

        private static string GetBits(BigInteger r)
        {
            string binaryStr = "";
            while (r > 0)
            {
                int temp = (int)(r % 2);
                binaryStr = temp.ToString() + binaryStr;
                r /= 2;
            }

            return binaryStr;
        }
    }
}
