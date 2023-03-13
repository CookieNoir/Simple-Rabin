using System;
using System.Numerics;

namespace PrimalNumbers
{
    public static class RandomBigInteger
    {
        public static BigInteger RandomIntegerBelow(BigInteger N)
        {
            byte[] bytes = N.ToByteArray();
            BigInteger R;
            Random random = new Random();
            do
            {
                random.NextBytes(bytes);
                bytes[bytes.Length - 1] &= 0x7F; // negative sign bit
                R = new BigInteger(bytes);
            } while (R >= N);

            return R;
        }
    }
}
