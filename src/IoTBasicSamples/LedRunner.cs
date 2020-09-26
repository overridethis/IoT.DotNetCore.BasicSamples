using System;
using System.Device.Gpio;
using System.Threading;
using System.Threading.Tasks;

namespace IoTBasicSamples
{
    public class LedRunner
    {
        private readonly int pin;

        public LedRunner(int pin)
        {
            this.pin = pin;
        }

        public Task RunAsync(CancellationToken token)
        {
            return Task.Run(() =>
            {
                using var controller = new GpioController();
                
                controller.OpenPin(pin);
                controller.SetPinMode(pin, PinMode.Output);

                while (true)
                {
                    controller.Write(pin, PinValue.High);    
                    Thread.Sleep(250);
                    controller.Write(pin, PinValue.Low);    
                    Thread.Sleep(250);

                    if (token.IsCancellationRequested)
                    {
                        controller.ClosePin(pin);
                        break;
                    }
                }
                
            }, token);
        }
    }
}
