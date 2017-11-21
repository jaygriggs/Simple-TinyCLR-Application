using GHIElectronics.TinyCLR.Devices.I2c;

namespace TinyCLRApplication5
{
    public class Accelerometer
    {
        private readonly byte[] _buffer1 = new byte[1];
        private readonly byte[] _buffer2 = new byte[2];
        private readonly I2cDevice _device;

        public Accelerometer()
        {
            _device = I2cDevice.FromId("GHIElectronics.TinyCLR.NativeApis.STM32F4.I2cProvider\\0", new I2cConnectionSettings(28)
            {
                BusSpeed = I2cBusSpeed.FastMode,
                SharingMode = I2cSharingMode.Shared
            });
            WriteRegister(42, 1);
        }

        private void WriteRegister(byte register, byte data)
        {
            _buffer2[0] = register;
            _buffer2[1] = data;
            _device.Write(_buffer2);
        }

        private void ReadRegisters(byte register, byte[] data)
        {
            _buffer1[0] = register;
            _device.WriteRead(_buffer1, data);
        }

        private double ReadAxis(byte register)
        {
            ReadRegisters(register, _buffer2);
            double num = _buffer2[0] << 2 | _buffer2[1] >> 6;
            if (num > 511.0)
                num -= 1024.0;
            return num / 256.0;
        }

        public double ReadY()
        {
            return -1.0 * ReadAxis(1);
        }

        public double ReadX()
        {
            return ReadAxis(3);
        }

        public double ReadZ()
        {
            return -1.0 * ReadAxis(5);
        }
    }
}
