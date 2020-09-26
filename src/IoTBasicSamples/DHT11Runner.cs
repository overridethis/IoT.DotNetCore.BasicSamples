using System;
using System.Device.Gpio;
using System.Device.Gpio.Drivers;
using System.Threading;
using System.Threading.Tasks;
using Iot.Device.DHTxx;

namespace IoTBasicSamples
{
    public class DHT11Runner
    {
        private readonly int _pin;

        public DHT11Runner(int pin)
        {
            _pin = pin;
        }

        public Task RunAsync(CancellationToken token)
        {
            return Task.Run(() =>
            {
                var driver = new RaspberryPi3Driver();
                using var controller = new GpioController(PinNumberingScheme.Logical, driver);
                using var dht11 = new Dht22(_pin, gpioController: controller);
                
                while (true)
                {
                    Thread.Sleep(5000);
                    if (dht11.IsLastReadSuccessful)
                    {
                        Console.WriteLine($"Temperature: {dht11.Temperature}, Humidity: {dht11.Humidity}");   
                    }
                    
                    if (token.IsCancellationRequested)
                    {
                        break;
                    }    
                }
            }, token);
        }
    }
}