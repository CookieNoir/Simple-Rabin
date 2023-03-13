using System.Collections;
using System.Numerics;

namespace PrimalNumbers
{
    public static class PrimalBigInteger
    {
        private static bool _IsPrime(BigInteger value)
        {
            return LowLevelPrimality.IsLowLevelPrime(value) && RabinMillerPrimality.IsMillerRabinPassed(value);
        }

        public static BigInteger GetPrime(int bitSize)
        {
            int extendedBitSize = bitSize + (8 - bitSize % 8);
            BitArray bits = new BitArray(extendedBitSize);
            bits[bitSize - 1] = true;
            byte[] bytes = new byte[extendedBitSize / 8];
            bits.CopyTo(bytes, 0);
            BigInteger minValue = new BigInteger(bytes);

            BigInteger result;
            do
            {
                result = minValue + RandomBigInteger.RandomIntegerBelow(minValue);
            } while (!_IsPrime(result));
            return result;
        }
    }
}
