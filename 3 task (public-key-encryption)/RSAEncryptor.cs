using System;
using System.Numerics;

namespace TI_3task
{
    public static class RSAEncryptor
    {
        public static ushort[] Encrypt(byte[] message, BigInteger key, BigInteger r)
        { 
            ushort[] result = new ushort[message.Length];
            for (int i = 0; i < message.Length; i++)
            {
                BigInteger res = Helper.FastExp(message[i], key, r);
                if (res > ushort.MaxValue)
                {
                    throw new ArithmeticException($"Cipher value is more than {ushort.MaxValue}. " +
                                                  $"Please, fix input parameters");
                }

                result[i] = (ushort)res;
            }

            return result;
        }

        public static byte[] Decrypt(byte[] message, BigInteger key, BigInteger r)
        {
            byte[] result = new byte[message.Length / 2];
            ushort[] ushortArrMessage = Helper.ToUShortArray(message);
            for (int i = 0; i < ushortArrMessage.Length; i++)
            {
                BigInteger res = Helper.FastExp(ushortArrMessage[i], key, r);
                if (res > ushort.MaxValue)
                {
                    throw new ArithmeticException($"Decipher value is more than {byte.MaxValue}. " +
                                                  $"Please, fix input parameters");
                }

                result[i] = (byte)res;
            }

            return result;
        }
    }
}