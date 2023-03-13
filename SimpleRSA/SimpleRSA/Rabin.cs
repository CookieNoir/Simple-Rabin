using System;
using System.Numerics;
using PrimalNumbers;

namespace SimpleRabin
{
    public class Rabin
    {
        BigInteger _p;
        BigInteger _q;
        BigInteger _n;

        public Rabin(int bitSize = 256)
        {
            if (bitSize < 0) bitSize = 256;
            do { _p = PrimalBigInteger.GetPrime(bitSize); } while (_p % 4 != 3);
            do { _q = PrimalBigInteger.GetPrime(bitSize); } while ((_p != _q) && (_q % 4 != 3));
            _n = _p * _q;
        }

        public byte[] GetPublicKey()
        {
            return _n.ToByteArray(isUnsigned: true);
        }

        public (byte[], byte[]) GetPrivateKey()
        {
            return (_p.ToByteArray(isUnsigned: true), _q.ToByteArray(isUnsigned: true));
        }

        public static byte[] Encrypt(byte[] message, byte[] publicKeyBytes)
        {
            BigInteger messageValue = new BigInteger(message, isUnsigned: true);
            BigInteger publicKeyValue = new BigInteger(publicKeyBytes, isUnsigned: true);

            BigInteger resultValue = (messageValue * messageValue) % publicKeyValue;

            return resultValue.ToByteArray(isUnsigned: true);
        }

        public static (byte[], byte[], byte[], byte[]) Decrypt(byte[] encryptedMessage, byte[] pBytes, byte[] qBytes)
        {
            BigInteger c = new BigInteger(encryptedMessage, isUnsigned: true);
            BigInteger p = new BigInteger(pBytes, isUnsigned: true);
            BigInteger q = new BigInteger(qBytes, isUnsigned: true);

            BigInteger mp = BigInteger.ModPow(c, (p + 1) / 4, p);
            BigInteger mq = BigInteger.ModPow(c, (q + 1) / 4, q);

            var (yp, yq) = _GcdExtendedCoefficients(p, q);

            BigInteger n = p * q;
            BigInteger yp_p_mq = yp * p * mq;
            BigInteger yq_q_mp = yq * q * mp;

            BigInteger r1 = ((yp_p_mq + yq_q_mp) % n + n) % n;
            BigInteger r2 = n - r1;
            BigInteger r3 = ((yp_p_mq - yq_q_mp) % n + n) % n;
            BigInteger r4 = n - r3;

            return (r1.ToByteArray(isUnsigned: true),
                    r2.ToByteArray(isUnsigned: true),
                    r3.ToByteArray(isUnsigned: true),
                    r4.ToByteArray(isUnsigned: true));
        }

        private static (BigInteger, BigInteger) _GcdExtendedCoefficients(BigInteger a, BigInteger b)
        {
            BigInteger x = 1, y = 0;
            BigInteger x1 = 0, y1 = 1, a1 = a, b1 = b;
            while (b1 != 0)
            {
                BigInteger q = a1 / b1;

                BigInteger temp = x;
                x = x1;
                x1 = temp - q * x1;

                temp = y;
                y = y1;
                y1 = temp - q * y1;

                temp = a1;
                a1 = b1;
                b1 = temp - q * b1;
            }
            return (x, y);
        }
    }
}
