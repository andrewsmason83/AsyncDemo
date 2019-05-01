using System;
using System.Collections.Generic;
using System.Threading;
using AsyncDemo.Extensions;

namespace AsyncDemo.Primes
{
    public class PrimeFinder
    {
        private ManualResetEvent _doneEvent;

        private readonly int _startNumber;
        private readonly int _endNumber;

        public List<uint> primes { get; }

        public PrimeFinder(int startNumber, int endNumber, ManualResetEvent doneEvent)
        {
            primes = new List<uint>();
            _startNumber = startNumber;
            _endNumber = endNumber;
            _doneEvent = doneEvent;
        }

        public void ThreadPoolCallback(Object threadContext)
        {
            int threadIndex = (int)threadContext;
            GeneratePrimes(primes, _startNumber, _endNumber);
            Console.WriteLine($"Thread {threadIndex} result calculated...");
            _doneEvent.Set();
        }

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
