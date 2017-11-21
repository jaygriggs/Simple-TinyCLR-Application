namespace TinyCLRApplication5
{
    public class Image
    {
        public int Height { get; }
        public int Width { get; }
        internal byte[] Data { get; }

        internal Image(int width, int height, byte[] data)
        {
            Height = height;
            Width = width;
            Data = data;
        }

        public Image()
        {

        }

    }
}
