using System.Threading;
using GHIElectronics.TinyCLR.Devices.Gpio;
using GHIElectronics.TinyCLR.Pins;

namespace TinyCLRApplication5
{
    internal class Program
    {
        private static ScreenUtils _screenUtils;
        private static GpioPin _ledblue;
        private static GpioPin _ledred;
        private static GpioPin _ledgreen;
        private static GpioPin _button1;
        private static GpioPin _button2;
        private static GpioPin _button3;
        private static GpioPin _button4;
        private static GpioController _gpio;
        private static Accelerometer _accelerometer;
        private static double _accelX;
        private static double _accelY;
        private static double _accelZ;
        private static int _directionPressed;

        private static void Main()
        {
            Setup();

            while (true)
            {
                Loop();
            }
        }

        private static void Loop()
        {
            ReadAcceleromter();
            ReadButtons();
            ProcessButtonPresses(_directionPressed);
            _screenUtils.ShowOnTheScreen();
            Thread.Sleep(10);
        }

        private static void ProcessButtonPresses(int directionPressed)
        {
            switch (directionPressed)
            {
                case 1:
                    _ledblue.Write(GpioPinValue.High);
                    _ledred.Write(GpioPinValue.Low);
                    _ledgreen.Write(GpioPinValue.Low);
                    _screenUtils.ClearAndWriteText(1, 0, "Axis Z\n\r" + _accelZ);
                    break;
                case 2:
                    _ledred.Write(GpioPinValue.Low);
                    _ledblue.Write(GpioPinValue.Low);
                    _ledgreen.Write(GpioPinValue.Low);
                    _screenUtils.ClearTheScreen();
                    var image = new Image(66, 64, Monkey.Data);
                    _screenUtils.DrawImage(32, 0, image);
                    break;
                case 3:
                    _ledgreen.Write(GpioPinValue.High);
                    _ledred.Write(GpioPinValue.Low);
                    _ledblue.Write(GpioPinValue.Low);
                    _screenUtils.ClearAndWriteText(1, 0, "Axis Y\n\r"+_accelY);
                    break;
                case 4:
                    _ledred.Write(GpioPinValue.High);
                    _ledblue.Write(GpioPinValue.Low);
                    _ledgreen.Write(GpioPinValue.Low);
                    _screenUtils.ClearAndWriteText(1, 0, "Axis X\n\r" + _accelX);
                    break;
                default:
                    _ledred.Write(GpioPinValue.Low);
                    _ledblue.Write(GpioPinValue.Low);
                    _ledgreen.Write(GpioPinValue.Low);
                    _screenUtils.ClearTheScreen();
                    break;
            }
        }

        private static void ReadButtons()
        {
            if (_button1.Read() == GpioPinValue.Low)
                _directionPressed = 1;
            if (_button4.Read() == GpioPinValue.Low)
                _directionPressed = 4;
            if (_button3.Read() == GpioPinValue.Low)
                _directionPressed = 3;
            if (_button2.Read() == GpioPinValue.Low)
                _directionPressed = 2;
        }

        private static void ReadAcceleromter()
        {
            _accelX = _accelerometer.ReadX() * 1000;
            _accelY = _accelerometer.ReadY() * 1000;
            _accelZ = _accelerometer.ReadZ() * 1000;
        }

        private static void Setup()
        {
            _gpio = GpioController.GetDefault();
            _screenUtils = new ScreenUtils();
            SetupLeds();
            SetButtons();
            _accelerometer = new Accelerometer();
        }

        private static void SetButtons()
        {
            _button1 = _gpio.OpenPin(STM32F4.GpioPin.PA15);
            _button1.SetDriveMode(GpioPinDriveMode.InputPullUp);
            _button2 = _gpio.OpenPin(STM32F4.GpioPin.PB10);
            _button2.SetDriveMode(GpioPinDriveMode.InputPullUp);
            _button3 = _gpio.OpenPin(STM32F4.GpioPin.PA5);
            _button3.SetDriveMode(GpioPinDriveMode.InputPullUp);
            _button4 = _gpio.OpenPin(STM32F4.GpioPin.PC13);
            _button4.SetDriveMode(GpioPinDriveMode.InputPullUp);
        }

        private static void SetupLeds()
        {
            _ledblue = _gpio.OpenPin(STM32F4.GpioPin.PC6);
            _ledblue.SetDriveMode(GpioPinDriveMode.Output);
            _ledgreen = _gpio.OpenPin(STM32F4.GpioPin.PC8);
            _ledgreen.SetDriveMode(GpioPinDriveMode.Output);
            _ledred = _gpio.OpenPin(STM32F4.GpioPin.PC9);
            _ledred.SetDriveMode(GpioPinDriveMode.Output);
        }
    }
}