using System;
using System.Device.Gpio;
using System.Device.I2c;
using System.Threading;
using System.Threading.Tasks;
using Iot.Device.CharacterLcd;
using Iot.Device.Pcx857x;

namespace IoTBasicSamples
{
    public class LcdRunner
    {
        public Task RunAsync(CancellationToken token)
        {
            return Task.Run(() =>
            {
                var i2CDevice = I2cDevice.Create(new I2cConnectionSettings(busId: 1, deviceAddress: 0x27));
                var driver = new Pcf8574(i2CDevice);
                using var lcd = new Lcd1602(
                    registerSelectPin: 0, 
                    enablePin: 2, 
                    dataPins: new int[] { 4, 5, 6, 7 }, 
                    backlightPin: 3, readWritePin: 1, controller: new GpioController(PinNumberingScheme.Logical, driver));

                while (true)
                {
                    lcd.SetCursorPosition(0,0);
                    lcd.Write("Hello!");
                    lcd.SetCursorPosition(0,1);
                    lcd.Write(DateTime.Now.ToString("h:mm:ss tt zz"));
                    Thread.Sleep(1000);
                    
                    lcd.SetCursorPosition(0,0);
                    lcd.Write("World!");
                    lcd.SetCursorPosition(0,1);
                    lcd.Write(DateTime.Now.ToString("h:mm:ss tt zz"));
                    Thread.Sleep(1000);
                    
                    if (token.IsCancellationRequested)
                    {
                        lcd.BacklightOn = false;
                        break;
                    }
                }
            }, token);
        }
    }
}