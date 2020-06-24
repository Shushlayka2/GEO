using System.Collections.Generic;
using System.Drawing;

namespace CoastMeasureEventHandler
{
    class VerticalLine
    {
        List<Pixel> Line = new List<Pixel>();

        internal void FillLine(int x, int y)
        {
            Line.Add(new Pixel(x, y));
        }

        internal List<Pixel> GetPaintingPixels(double level)
        {
            List<Pixel> result = new List<Pixel>();
            foreach (Pixel p in Line)
            {
                if (p.Y < level && p.Color != Color.White)
                    result.Add(p);
                if (p.Y >= level && p.Color == Color.White)
                    result.Add(p);
                else if (p.Y >= level && p.Color != Color.White)
                    break;
            }
            return result;
        }
    }
}
