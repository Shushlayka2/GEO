using System.Collections.Generic;

namespace MapApp
{
    public class ObjectItem
    {
        public int Id { get; set; }
        public List<Point> Points { get; set; }
        public string Name { get; set; }

        public bool IsPointInside(Point p)
        {
            bool result = true;
            bool isNegative = ((Points[0].X - p.X) * (Points[1].Y - Points[0].Y) - (Points[1].X - Points[0].X) * (Points[0].Y - p.Y)) < 0;
            for (int i = 1; i < Points.Count - 1; i++)
            {
                result = isNegative == ((Points[i].X - p.X) * (Points[i + 1].Y - Points[i].Y) - (Points[i + 1].X - Points[i].X) * (Points[i].Y - p.Y) < 0);
                if (!result)
                    return result;
            }
            return result = isNegative == ((Points[Points.Count - 1].X - p.X) * (Points[0].Y - Points[Points.Count - 1].Y) - (Points[0].X - Points[Points.Count - 1].X) * (Points[Points.Count - 1].Y - p.Y) < 0);
        }
    }
}
