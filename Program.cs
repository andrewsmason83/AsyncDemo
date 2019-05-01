using System;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AsyncDemo.Clients;
using AsyncDemo.Primes;

namespace AsyncDemo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            ProgramArguments programArgs = new ProgramArguments(args);

            if (programArgs.ArgsValid)
            {
                if (programArgs.IsHelp)
                {
                    DisplayHelpPage();
                }
                else if (programArgs.IsAsync)
                {
                    await RunAsyncDemo(programArgs.Iterations);
                }
                else if (programArgs.IsSync)
                {
                    RunSyncDemo(programArgs.Iterations);
                }
                else
                {
                    RunMultithreadDemo();
                }
            }
            else
            {
                Console.WriteLine(programArgs.ErrorMessage);
            }
        }

        private static void DisplayHelpPage()
        {
            Console.WriteLine("This application shows the differences between calling 3 external REST api");
            Console.WriteLine("endpoints synchronously and asynchronously." + Environment.NewLine);
            Console.WriteLine("It accepts 2 arguments:" + Environment.NewLine);
            Console.WriteLine("-a or -s : Run the asynchronous/synchronous calls demonstration, respectively.");
            Console.WriteLine("-i [n]: Number of iterations (e.g. -i 10 for 10 iterations).");
            Console.WriteLine("Alternatively run with no arguments for multithreading demo (generate all primes upto 100,000).");
            Console.WriteLine(Environment.NewLine);
        }

        private static async Task RunAsyncDemo(UInt16 iterations)
        {
            Console.WriteLine($"Asynchronously retrieving comments, albums and photos. Iterations: {iterations}.");

            var placeholderClient = new PhotoAlbumsClient();

            List<long> timings = new List<long>();

            for (int i = 0; i < iterations; i++)
            {
                Console.WriteLine($"Iteration number: {i + 1}");
                var sw = new Stopwatch();
                sw.Start();
                var photosTask = placeholderClient.GetPhotosAsync();
                var commentsTask = placeholderClient.GetCommentsAsync();
                var albumsTask = placeholderClient.GetAlbumsAsync();

                await Task.WhenAll(photosTask, commentsTask, albumsTask);

                sw.Stop();

                timings.Add(sw.ElapsedMilliseconds);
                Console.WriteLine($"Total time to retrieve: {sw.ElapsedMilliseconds}ms");
            }
            Console.WriteLine($"Process completed, average time to retrieve {timings.Average()}");
        }

        private static void RunSyncDemo(UInt16 iterations)
        {
            Console.WriteLine("Synchronously retrieving comments, albums and photos. Iterations: {iterations}.");

            var placeholderClient = new PhotoAlbumsClient();

            List<long> timings = new List<long>();

            for (int i = 0; i < iterations; i++)
            {
                Console.WriteLine($"Iteration number: {i + 1}");
                var sw = new Stopwatch();
                sw.Start();
                var photos = placeholderClient.GetPhotos();
                var comments = placeholderClient.GetComments();
                var albums = placeholderClient.GetAlbums();
                sw.Stop();

                timings.Add(sw.ElapsedMilliseconds);
                Console.WriteLine($"Total time to retrieve: {sw.ElapsedMilliseconds}ms");
            }
            Console.WriteLine($"Process completed, average time to retrieve {timings.Average()}");
        }

        private static void RunMultithreadDemo()
        {
            List<uint> primesOneThread = new List<uint>();
            var sw = new Stopwatch();
            sw.Start();
            PrimeFinder.GeneratePrimes(primesOneThread, 1, 100000);
            sw.Stop();
            Console.WriteLine($"With a single thread, there were {primesOneThread.Count} primes found upto 100,000 in {sw.ElapsedMilliseconds}ms{Environment.NewLine}");

            sw.Reset();
            //Multithreaded part
            sw.Start();

            List<uint> primesMultiThreads = new List<uint>();
            var doneEvents = new ManualResetEvent[4];
            doneEvents[0] = new ManualResetEvent(false);
            doneEvents[1] = new ManualResetEvent(false);
            doneEvents[2] = new ManualResetEvent(false);

            var primesThread1 = new PrimeFinder(1, 40000, doneEvents[0]);
            var primesThread2 = new PrimeFinder(40001, 75000, doneEvents[1]);
            var primesThread3 = new PrimeFinder(75001, 100000, doneEvents[2]);

            ThreadPool.QueueUserWorkItem(primesThread1.ThreadPoolCallback, 1);
            ThreadPool.QueueUserWorkItem(primesThread2.ThreadPoolCallback, 2);
            ThreadPool.QueueUserWorkItem(primesThread3.ThreadPoolCallback, 3);

            WaitHandle.WaitAll(doneEvents);

            primesMultiThreads.AddRange(primesThread1.primes);
            primesMultiThreads.AddRange(primesThread2.primes);
            primesMultiThreads.AddRange(primesThread3.primes);

            sw.Stop();

            Console.WriteLine($"With 3 threads, there were {primesMultiThreads.Count} primes found upto 100,000 in {sw.ElapsedMilliseconds}ms{Environment.NewLine}");

            Console.WriteLine($"Would you like to display the last 100 primes? (y/n)");
            if (Console.ReadLine().ToLower() == "y")
            {
                for (int i = primesMultiThreads.Count - 100; i < primesMultiThreads.Count; i++)
                {
                    Console.Write($"{primesMultiThreads[i]},");
                }
            }
        }
    }
}