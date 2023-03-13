using System;
using System.Text;
using System.Numerics;

namespace SimpleRabin
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Set bit size of p and q:");
            int bitSize = Convert.ToInt32(Console.ReadLine());

            Rabin rabin = new Rabin(bitSize);
            byte[] publicKeyBytes = rabin.GetPublicKey();
            Console.WriteLine($"Public key is: {new BigInteger(publicKeyBytes, isUnsigned: true)}");

            var (pBytes, qBytes) = rabin.GetPrivateKey();
            Console.WriteLine($"p is: {new BigInteger(pBytes, isUnsigned: true)}");
            Console.WriteLine($"q is: {new BigInteger(qBytes, isUnsigned: true)}");
            Console.WriteLine();

            Console.WriteLine("Set message:");
            string message = Console.ReadLine();
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);

            byte[] encryptedBytes = Rabin.Encrypt(messageBytes, publicKeyBytes);
            Console.WriteLine($"Encrypted message is: {Encoding.UTF8.GetString(encryptedBytes)}");

            var (decrypted1, decrypted2, decrypted3, decrypted4) = Rabin.Decrypt(encryptedBytes, pBytes, qBytes);
            Console.WriteLine($"Decrypted messages:\n" +
                $"  {Encoding.UTF8.GetString(decrypted1)},\n"+
                $"  {Encoding.UTF8.GetString(decrypted2)},\n"+
                $"  {Encoding.UTF8.GetString(decrypted3)},\n"+
                $"  {Encoding.UTF8.GetString(decrypted4)}\n");
        }
    }
}
