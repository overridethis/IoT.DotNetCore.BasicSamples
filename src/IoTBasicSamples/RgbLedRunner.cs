using System.Device.Pwm.Drivers;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

namespace IoTBasicSamples
{
    public class RgbLedRunner
    {
        private readonly int _red;
        private readonly int _green;
        private readonly int _blue;

        public RgbLedRunner(
            int red,
            int green,
            int blue)
        {
            _red = red;
            _green = green;
            _blue = blue;
        }
        
        public Task RunAsync(CancellationToken token)
        {
            return Task.Run(() =>
            {
                var rgb = new
                {
                    Red = new SoftwarePwmChannel(_red, 2000),
                    Green = new SoftwarePwmChannel(_green, 2000),
                    Blue = new SoftwarePwmChannel(_blue, 2000),
                };
                
                rgb.Red.Start();
                rgb.Green.Start();
                rgb.Blue.Start();

                void ShutDown()
                {
                    rgb.Red.DutyCycle = 0;
                    rgb.Green.DutyCycle = 0;
                    rgb.Blue.DutyCycle = 0;
                }
                
                void SetColor(Color color)
                {
                    rgb.Red.DutyCycle = MapDutyCycle(color.R);
                    rgb.Green.DutyCycle = MapDutyCycle(color.G);
                    rgb.Blue.DutyCycle = MapDutyCycle(color.B);
                }
                
                double MapDutyCycle(double number, double in_min = 0, double in_max = 255, double out_min = 0, double out_max = 1)
                    => (number - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;

                while (true)
                {
                    SetColor(Color.Red);
                    Thread.Sleep(500);
                    
                    SetColor(Color.Green);
                    Thread.Sleep(500);
                    
                    SetColor(Color.Blue);
                    Thread.Sleep(500);
                    
                    SetColor(Color.Yellow);
                    Thread.Sleep(500);
                    
                    if (token.IsCancellationRequested)
                    {
                        ShutDown();
                        break;
                    }
                }
            }, token);
        }
    }
}