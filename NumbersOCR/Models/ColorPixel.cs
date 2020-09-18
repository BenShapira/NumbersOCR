using System.Drawing;

namespace NumbersOCR.Models
{
    public class ColorPixel
    {
        public Color Color { get; set; }
        public Point Pixel { get; set; }

        public ColorPixel() { }
        public ColorPixel(Color i_color, Point i_point)
        {
            Color = i_color;
            Pixel = i_point;
        }
    }
}
