using System;
using System.Device.Gpio;
using System.Threading;
using System.Threading.Tasks;

namespace IoTBasicSamples
{
    public class ButtonRunner
    {
        private readonly int _pin;

        public ButtonRunner(int pin)
        {
            _pin = pin;
        }

        public Task RunAsync(CancellationToken token)
        {
            return Task.Run(() =>
            {
                using var controller = new GpioController();
                controller.OpenPin(_pin);
                controller.SetPinMode(_pin, PinMode.Input);

                while (true)
                {
                    if (controller.Read(_pin) == PinValue.High)
                    {
                        Console.WriteLine($"[RUN:Button] Button on pin {_pin} was pressed.");
                    }

                    Thread.Sleep(100);
                    if (token.IsCancellationRequested)
                    {
                        return;
                    }                    
                }
                
            }, token);
        }
    }
}