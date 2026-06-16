using System;

namespace ShapePainter
{
    public class DbShape 
    {
        public long Id { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public ShapeType Type { get; set; } = ShapeType.Rectangle;
    }
}