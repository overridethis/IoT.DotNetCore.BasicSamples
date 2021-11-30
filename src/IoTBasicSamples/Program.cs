using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace IoTBasicSamples
{
    static class Program
    {
        private const string RunModeError = "[RUN] Must provide a valid mode to run.";

        static void Main(string[] args)
        {
            if (!args.Any() && !string.IsNullOrWhiteSpace(args[0]))
            {
                Console.WriteLine(RunModeError);
                return;
            }

            if (!Enum.TryParse<DemoType>(args[0].Trim(), true, out var demoType))
            {
                Console.WriteLine(RunModeError);
                return;
            }

            var tokenSource = new CancellationTokenSource();

            Console.WriteLine("[RUN]: Demo");
            Console.WriteLine($"[RUN:{demoType.ToString()}] Running");

            var runTask = Task.Run(async () =>
                {
                    switch (demoType)
                    {
                        default:
                            Console.WriteLine($"[RUN:{demoType.ToString()}] Not Implemented!");
                            break;
                    }
                }, tokenSource.Token);


            Console.WriteLine($"[RUN:{demoType.ToString()}]: Press any key to exit.");
            Console.ReadKey();

            tokenSource.Cancel();
            runTask.Wait();

            Console.WriteLine("[RUN]: End");
        }
    }
}
