using System;
using System.Threading.Tasks;

namespace AsyncDemo.MouseRace
{
    public class Mouse
    {
        public string name { get; set; }
        public int consoleRow { get; set; }
    }

    public class MouseRace
    {
        private const string MOUSE = "~~(__^·>";
        private const int START_POSITION = 15;
        private const int FINISH_POSITION = 100;

        private readonly object runLock = new object();

        public void PrintTrack(params Mouse[] mice)
        {
            foreach (var mouse in mice)
            {
                Console.SetCursorPosition(0, mouse.consoleRow);
                Console.Write(mouse.name.ToUpper());

                Console.SetCursorPosition(START_POSITION, mouse.consoleRow);
                Console.Write(MOUSE);

                Console.SetCursorPosition(FINISH_POSITION + 8, mouse.consoleRow);
                Console.Write("|");
            }

            Console.Write(Environment.NewLine);
            Console.Write(Environment.NewLine);
        }

        public async Task<string> RunAsync(Mouse mouse)
        {
            var rand = new Random();

            for (int i = START_POSITION - 1; i <= FINISH_POSITION; i++)
            {
                await Task.Delay(rand.Next(50, 150));
                lock (runLock)
                {
                    Console.CursorVisible = false;
                    Console.SetCursorPosition(i + 1, mouse.consoleRow);
                    Console.Write($" {MOUSE}");
                }
            }

            return mouse.name;
        }
    }
}
