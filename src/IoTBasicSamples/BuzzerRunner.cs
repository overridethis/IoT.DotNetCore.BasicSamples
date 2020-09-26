using System.Threading;
using System.Threading.Tasks;
using Iot.Device.Buzzer;

namespace IoTBasicSamples
{
    public class BuzzerRunner
    {
        private readonly int _pin;

        public BuzzerRunner(int pin)
        {
            _pin = pin;
        }

        public Task RunAsync(CancellationToken token)
        {
            return Task.Run(() =>
            {
                var buzzer = new Buzzer(_pin);
                
                while (true)
                {
                    buzzer.PlayTone(440, 1000);
                    Thread.Sleep(1000);
                    
                    if (token.IsCancellationRequested)
                    {
                        break;
                    }    
                }
            }, token);
        }
    }
}