﻿namespace MapApp
{
    public class Point
    {
        public int Id { get; set; }
        public double X { get; set; }
        public double Y { get; set; }

        public int ObjectItemId { get; set; }
        public ObjectItem ObjectItem { get; set; }
    }
}
