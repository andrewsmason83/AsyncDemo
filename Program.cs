using System;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AsyncDemo.Clients;
using AsyncDemo.Primes;
using AsyncDemo.MouseRace;

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
                    DisplayHelp();
                }
                else if (programArgs.IsAsync)
                {
                    await RunAsyncDemo(programArgs.Iterations);
                }
                else if (programArgs.IsSync)
                {
                    RunSyncDemo(programArgs.Iterations);
                }
                else if (programArgs.IsMice)
                {
                    RunMouseRaceDemo();
                }
                else
                {
                    await RunMultithreadDemo();
                }
            }
            else
            {
                Console.WriteLine(programArgs.ErrorMessage);
            }
        }

        private static void DisplayHelp()
        {
            Console.WriteLine("This application shows the differences between calling 3 external REST api");
            Console.WriteLine("endpoints synchronously and asynchronously." + Environment.NewLine);
            Console.WriteLine("It accepts up to 2 arguments:" + Environment.NewLine);
            Console.WriteLine("-a or -s : Run the asynchronous/synchronous calls demonstration, respectively.");
            Console.WriteLine("-i [n]: Number of iterations (e.g. -i 10 for 10 iterations). Only works with -a and -s.");
            Console.WriteLine("-m : Run Mouse race demo (async demo using when any to await 1 response only).");
            Console.WriteLine("Alternatively run with no arguments for multithreading demo (generate all primes up to 100,000).");
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

        private static void RunMouseRaceDemo()
        {
            Console.WriteLine($"Asynchronous 3 mouse race.");
            
            var firstTrackRow = Console.CursorTop + 1;

            var mice = new MouseRace.MouseRace();
            var mouse1 = new Mouse {name = "Red Rum", consoleRow = firstTrackRow };
            var mouse2 = new Mouse { name = "Tiger Roll", consoleRow = firstTrackRow + 1 };
            var mouse3 = new Mouse { name = "Secretariat", consoleRow = firstTrackRow + 2 };

            mice.PrintTrack(mouse1, mouse2, mouse3);

            for (int i = 3; i > 0; i--)
            {
                Console.Write($"{i}");
                for (int j = 2; j > 0; j--)
                {
                    Thread.Sleep(500);
                    Console.Write($".");
                }
            }
            Console.WriteLine($"And they're off!");
                        
            var mouse1Run = mice.RunAsync(mouse1);
            var mouse2Run = mice.RunAsync(mouse2);
            var mouse3Run = mice.RunAsync(mouse3);

            var winner = Task.WhenAny(mouse1Run, mouse2Run, mouse3Run).Result;

            Console.CursorVisible = true;
            Console.SetCursorPosition(0, firstTrackRow + 5);

            Console.WriteLine($"{winner.Result} wins!");
        }

        private static async Task RunMultithreadDemo()
        {
            List<uint> primesOneThread = new List<uint>();
            var sw = new Stopwatch();
            sw.Start();
            PrimeFinder.GeneratePrimes(primesOneThread, 1, 10000000);
            sw.Stop();
            Console.WriteLine($"With a single thread, there were {primesOneThread.Count} primes found up to 10,000,000 in {sw.ElapsedMilliseconds/1000}s{Environment.NewLine}");

            sw.Reset();
            //Multithreaded part
            sw.Start();

            var primesThread1 = new List<uint>();
            var primesThread2 = new List<uint>();
            var primesThread3 = new List<uint>();
            var primesThread4 = new List<uint>();

            List<uint> primesMultiThreads = new List<uint>();
            var primeThreads = new Task[4];
            primeThreads[0] = Task.Run(() => PrimeFinder.GeneratePrimes(primesThread1, 1, 3000000));
            primeThreads[1] = Task.Run(() => PrimeFinder.GeneratePrimes(primesThread2, 3000001, 6000000));
            primeThreads[2] = Task.Run(() => PrimeFinder.GeneratePrimes(primesThread3, 6000001, 8000000));
            primeThreads[3] = Task.Run(() => PrimeFinder.GeneratePrimes(primesThread4, 8000001, 10000000));

            await Task.WhenAll(primeThreads);

            primesMultiThreads.AddRange(primesThread1);
            primesMultiThreads.AddRange(primesThread2);
            primesMultiThreads.AddRange(primesThread3);
            primesMultiThreads.AddRange(primesThread4);

            sw.Stop();

            Console.WriteLine($"With 4 threads, there were {primesMultiThreads.Count} primes found up to 10,000,000 in {sw.ElapsedMilliseconds/1000}s{Environment.NewLine}");

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