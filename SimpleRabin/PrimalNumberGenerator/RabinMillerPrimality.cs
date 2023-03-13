using System;
using System.Numerics;

namespace PrimalNumberGenerator
{
    // https://www.geeksforgeeks.org/how-to-generate-large-prime-numbers-for-rsa-algorithm/
    internal static class RabinMillerPrimality
    {
        // This function calculates (base ^ exp) % mod
        private static BigInteger _expMod(BigInteger basement, BigInteger exp, BigInteger mod)
        {
            if (exp == 0) return 1;
            if (exp % 2 == 0)
            {
                return (BigInteger.Pow(_expMod(basement, (exp / 2), mod),2)) % mod;
            }
            else
            {
                return (basement * _expMod(basement, (exp - 1), mod)) % mod;
            }
        }

        private static bool _isTrialComposite(in BigInteger roundTester, in BigInteger evenComponent,
                                   BigInteger candidate, int maxDivisionsByTwo)
        {
            if (_expMod(roundTester, evenComponent, candidate) == 1)
                return false;
            for (int i = 0; i < maxDivisionsByTwo; i++)
            {
                if (_expMod(roundTester, (1 << i) * evenComponent,
                           candidate) == candidate - 1)
                    return false;
            }
            return true;
        }

        internal static bool IsMillerRabinPassed(BigInteger candidate)
        {
            // Run 20 iterations of Rabin Miller Primality test

            int maxDivisionsByTwo = 0;
            BigInteger evenComponent = candidate - 1;

            while (evenComponent % 2 == 0)
            {
                evenComponent >>= 1;
                maxDivisionsByTwo += 1;
            }

            // Set number of trials here
            int numberOfRabinTrials = 20;
            for (int i = 0; i < (numberOfRabinTrials); i++)
            {
                BigInteger roundTester = RandomBigInteger.RandomIntegerBelow(candidate);
                if (roundTester < 2) roundTester = 2;
                if (_isTrialComposite(roundTester, evenComponent,
                                   candidate, maxDivisionsByTwo))
                    return false;
            }
            return true;
        }
    }
}
