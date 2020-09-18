using System.Drawing;

namespace NumbersOCR.Models
{
    public class NumberFound
    {
        public int Value { get; set; }
        public Point Pixel { get; set; }

        public NumberFound(int i_value, Point i_pixel)
        {
            Value = i_value;
            Pixel = i_pixel;
        }
    }
}
