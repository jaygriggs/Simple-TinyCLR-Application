using System;
using GHIElectronics.TinyCLR.Devices.I2c;

namespace TinyCLRApplication5
{
    public class ScreenUtils
    {
        private readonly I2cDevice _i2CDevice = I2cDevice.FromId(
            "GHIElectronics.TinyCLR.NativeApis.STM32F4.I2cProvider\\0", new I2cConnectionSettings(60)
            {
                BusSpeed = I2cBusSpeed.FastMode,
                SharingMode = I2cSharingMode.Shared
            });

        private readonly byte[] _vram;

        public ScreenUtils()
        {
            _vram = new byte[1025];
            Ssd1306_command(174);
            Ssd1306_command(213);
            Ssd1306_command(128);
            Ssd1306_command(168);
            Ssd1306_command(63);
            Ssd1306_command(211);
            Ssd1306_command(0);
            Ssd1306_command(64);
            Ssd1306_command(141);
            Ssd1306_command(20);
            Ssd1306_command(32);
            Ssd1306_command(0);
            Ssd1306_command(161);
            Ssd1306_command(200);
            Ssd1306_command(218);
            Ssd1306_command(18);
            Ssd1306_command(129);
            Ssd1306_command(207);
            Ssd1306_command(217);
            Ssd1306_command(241);
            Ssd1306_command(216);
            Ssd1306_command(64);
            Ssd1306_command(164);
            Ssd1306_command(166);
            Ssd1306_command(46);
            Ssd1306_command(175);
            Ssd1306_command(33);
            Ssd1306_command(0);
            Ssd1306_command(sbyte.MaxValue);
            Ssd1306_command(34);
            Ssd1306_command(0);
            Ssd1306_command(7);

            ClearTheScreen();
            ShowOnTheScreen();
        }

        public int Width { get; } = 32;
        public int Height { get; } = 32;

        private void Ssd1306_command(int cmd)
        {
            _i2CDevice.Write(new byte[]
            {
                0,
                (byte) cmd
            });
        }

        public void ShowOnTheScreen()
        {
            _i2CDevice.Write(_vram);
        }

        private void Pixel(int x, int y, bool set)
        {
            if (x < 0 || x > sbyte.MaxValue || y < 0 || y > 63)
                return;

            var index3 = x + y / 8 * 128 + 1;
            if (set)
            {
                _vram[index3] |= (byte) (1 << (y % 8));
                return;
            }
            _vram[index3] &= (byte) ~(1 << (y % 8));
        }

        public void ClearTheScreen()
        {
            if (_vram == null)
                return;
            Array.Clear(_vram, 0, _vram.Length);
            _vram[0] = 64;
        }

        public void ClearPartOfTheScreen(int x, int y, int width, int height)
        {
            if (x == 0 && y == 0 && width == 128 && height == 64)
                ClearTheScreen();
            for (var x1 = x; x1 < width + x; ++x1)
            for (var y1 = y; y1 < height + y; ++y1)
                Pixel(x1, y1, false);
        }

        public void DrawImage(int x, int y, Image image)
        {
            if (image == null)
                throw new ArgumentNullException(nameof(Image));
            for (var index1 = 0; index1 < image.Width; ++index1)
            for (var index2 = 0; index2 < image.Height; ++index2)
                Pixel(x + index1, y + index2, image.Data[image.Width * index2 + index1] == 1);
        }

        private void DrawText(int x, int y, char letter, int hScale, int vScale)
        {
            Font font = new Font();
            var num = 5 * (letter - 32);
            for (var index1 = 0; index1 < 5; ++index1)
            for (var index2 = 0; index2 < hScale; ++index2)

            for (var index3 = 0; index3 < 8; ++index3)
            {
                var set = (font.Font1[num + index1] & (uint) (1 << index3)) > 0U;
                for (var index4 = 0; index4 < vScale; ++index4)
                    Pixel(x + index1 * hScale + index2, y + index3 * vScale + index4, set);
            }
            ClearPartOfTheScreen(x + 5 * hScale, y, hScale, 8 * vScale);
        }

        public void ClearAndWriteText(int x, int y, string text)
        {
            ClearTheScreen();
            var num = x;

            if (text == null)
                throw new ArgumentNullException("data");
            for (var index = 0; index < text.Length; ++index)
                if (text[index] >= 32) //space bar
                {
                    DrawText(x, y, text[index], 2, 2);
                    x += 12;
                }
                else
                {
                    if (text[index] == 10) //new lf
                        y += 18;
                    if (text[index] == 13) //new cr
                        x = num;
                }
            ShowOnTheScreen();
        }
    }
}