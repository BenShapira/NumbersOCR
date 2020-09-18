using NumbersOCR.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace NumbersOCR
{
    public class ReadNumbers
    {
        public int ReadNumber(Bitmap source, FileInfo[] numbers)
        {
            Helper helper = new Helper();
            source = helper.ConvertBlackAndWhite(source);
            List<ColorPixel> sourcePixels = helper.GetPixels(source);
            List<NumberFound> numbersFound = new List<NumberFound>();

            //Loop through all the number refrences
            foreach (var number in numbers)
            {
                var numberValue = 0;
                Int32.TryParse((Path.GetFileNameWithoutExtension(number.Name))[0].ToString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out numberValue);
                Bitmap numberBitmap = new Bitmap(number.FullName);
                List<Point> numberPoints = new List<Point>();
                numberPoints = helper.Find(source, sourcePixels, numberBitmap);

                //if found add to numbersFound list
                foreach (var num in numberPoints)
                {
                    numbersFound.Add(new NumberFound(numberValue, num));
                }
            }

            //convert dictionary to number
            int result = 0;

            numbersFound.Sort((p, q) => p.Pixel.X.CompareTo(q.Pixel.X));
            foreach (var digit in numbersFound)
            {
                result = result * 10 + digit.Value;
            }

            return result;
        }
    }
}
