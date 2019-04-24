using System;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AsyncDemo.Clients;
using AsyncDemo.DataContracts;

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
            Console.WriteLine("Alternatively run with no arguments for multithreading demo.");
            Console.WriteLine(Environment.NewLine);
        }

        private static async Task RunAsyncDemo(UInt16 iterations)
        {
            Console.WriteLine($"Asynchronously retrieving comments, albums and photos. Iterations: {iterations}.");

            var placeholderClient = new JsonPlaceholderClient();

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

            var placeholderClient = new JsonPlaceholderClient();

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
            ThreadStart job = new ThreadStart(ThreadJob);
            Thread thread = new Thread(job);
            thread.Start();

            Random rand = new Random();

            for (int i = 0; i < 50; i++)
            {
                Console.WriteLine($"Main thread: {i}");
                Thread.Sleep(rand.Next(200));
            }
        }

        private static void ThreadJob()
        {
            Random rand = new Random();

            for (int i = 0; i < 50; i++)
            {
                Console.WriteLine($"Other thread: {i}");
                Thread.Sleep(rand.Next(200));
            }
        }
    }
}