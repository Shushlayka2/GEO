using System.Drawing;

namespace CoastMeasureEventHandler
{
    class Pixel
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Color Color = Color.White;

        public Pixel(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
