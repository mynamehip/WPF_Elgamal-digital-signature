using System;
using System.Numerics;

namespace ElGamalSignature
{
    class Program
    {
        static void Main(string[] args)
        {
            BigInteger p = 17; // prime number
            BigInteger g = 3; // primitive root modulo p
            BigInteger x = 5; // private key
            BigInteger y = BigInteger.ModPow(g, x, p); // public key

            BigInteger k = 7; // random number between 1 and p-2
            BigInteger r = BigInteger.ModPow(g, k, p);
            BigInteger m = 10; // message to be signed
            BigInteger s = (m - x * r) * BigInteger.ModPow(k, p - 2, p - 1) % (p - 1);

            Console.WriteLine("Public key: ({0}, {1})", p, g);
            Console.WriteLine("Private key: {0}", x);
            Console.WriteLine("Signature: ({0}, {1})", r, s);

            bool valid = (BigInteger.ModPow(y, r, p) * BigInteger.ModPow(r, s, p)) % p == BigInteger.ModPow(g, m, p);
            Console.WriteLine("Valid signature? {0}", valid);
        }
    }
}
