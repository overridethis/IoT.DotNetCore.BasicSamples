using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace IoTBasicSamples
{
    class Program
    {
        private const string RunModeError = "[RUN] Must provide a valid mode to run.";

        private const int RGB_LED_PIN_RED = 18;
        private const int RGB_LED_PIN_GREEN = 17;
        private const int RGB_LED_PIN_BLUE = 22;
        private const int LED_PIN = 18;
        private const int BUTTON_PIN = 23;
        private const int BUZZER_PIN = 12;
        private const int DHT11_PIN = 26;
        
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
                        case DemoType.Button:
                            await new ButtonRunner(BUTTON_PIN).RunAsync(tokenSource.Token);
                            break;
                        case DemoType.Buzzer:
                            await new BuzzerRunner(BUZZER_PIN).RunAsync(tokenSource.Token);
                            break;
                        case DemoType.DHT11:
                            await new DHT11Runner(DHT11_PIN).RunAsync(tokenSource.Token);
                            break;
                        case DemoType.LCD1602:
                            await new LcdRunner().RunAsync(tokenSource.Token);
                            break;
                        case DemoType.Led:
                            await new LedRunner(LED_PIN).RunAsync(tokenSource.Token);
                            break;
                        case DemoType.RgbLed:
                            await new RgbLedRunner(RGB_LED_PIN_RED, RGB_LED_PIN_GREEN, RGB_LED_PIN_BLUE)
                                .RunAsync(tokenSource.Token);
                            break;
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
