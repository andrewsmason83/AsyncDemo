using System;
using System.Collections.Generic;
using AsyncDemo.Extensions;

namespace AsyncDemo.Primes
{
    public static class PrimeFinder
    {
        public static void GeneratePrimes(List<uint> primes, int firstNumber, int lastNumber)
        {
            Console.WriteLine($"Generating primes from {firstNumber} to {lastNumber}");
            for (uint i = (uint)firstNumber; i < lastNumber + 1; i++)
            {
                if (i.IsPrime())
                {
                    primes.Add(i);
                }
            }
        }
    }
}
