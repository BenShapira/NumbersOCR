using NumbersOCR.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace NumbersOCR
{
    public class Helper
    {
        public List<Point> Find(Bitmap source, List<ColorPixel> sourcePixels, Bitmap number)
        {
            List<Point> Found = new List<Point>();
            List<Color> NumberFirstLinePixels = GetFirstLinePixels(number);

            //get all refrences that has a first line match (to avoid looping through all of the refrences if the first line already failed) - 
            List<List<ColorPixel>> firstLineMatches = FirstLineMatches(sourcePixels, NumberFirstLinePixels);

            //compare entire matches - 
            List<ColorPixel> croppedPixels = new List<ColorPixel>();
            List<ColorPixel> numberPixels = GetPixels(number);

            foreach (var firstLine in firstLineMatches)
            {
                Bitmap bmp = CropImage(source, firstLine[0].Pixel.X, firstLine[0].Pixel.Y, number.Width, number.Height);
                croppedPixels = GetPixels(bmp);
                if(FindCompleteMatch(croppedPixels, numberPixels))
                {
                    Found.Add(firstLine[0].Pixel);
                }
            }
            return Found;

        }

        public bool FindCompleteMatch(List<ColorPixel> croppedPixels, List<ColorPixel> numberPixels)
        {
            for (var k = 0; k < croppedPixels.Count; k++)
            {
                if (!AreColorsSimilar(croppedPixels[k].Color, numberPixels[k].Color, 50))
                {
                    return false;
                }
            }
            return true;
            
        }

        public List<ColorPixel> GetPixels(Bitmap image)
        {
            Bitmap b = new Bitmap(image);

            List<ColorPixel> pixelsList = new List<ColorPixel>();

            for (int y = 0; y < b.Height; y++)
            {
                for (int x = 0; x < b.Width; x++)
                {
                    pixelsList.Add(new ColorPixel(b.GetPixel(x, y), new Point(x, y)));
                }

            }
            return pixelsList;
        }

        private List<Color> GetFirstLinePixels(Bitmap image)
        {
            Bitmap b = new Bitmap(image);
            List<Color> pixelsList = new List<Color>();
            for (int x = 0; x < b.Width; x++)
            {
                pixelsList.Add(b.GetPixel(x, 0));

            }
            return pixelsList;
        }

        public static Bitmap CropImage(Image source, int x, int y, int width, int height)
        {
            Rectangle crop = new Rectangle(x, y, width, height);

            var bmp = new Bitmap(crop.Width, crop.Height);
            using (var gr = Graphics.FromImage(bmp))
            {
                gr.DrawImage(source, new Rectangle(0, 0, bmp.Width, bmp.Height), crop, GraphicsUnit.Pixel);
            }
            return bmp;
        }

        private List<List<ColorPixel>> FirstLineMatches(List<ColorPixel> sourcePixels, List<Color> NumberFirstLinePixels)
        {
            List<List<ColorPixel>> FirstLineMatches = new List<List<ColorPixel>>();
            List<ColorPixel> sourceComparisonList = new List<ColorPixel>();
            var numberAddCount = 0;
            Boolean firstLineMatch = true;

            for (var i = NumberFirstLinePixels.Count - 1; i < sourcePixels.Count; i++)
            {
                //reset 
                firstLineMatch = true;
                sourceComparisonList = new List<ColorPixel>();

                //make comparison array from source by number pixels count
                numberAddCount = NumberFirstLinePixels.Count - 1;
                while (numberAddCount >= 0)
                {
                    sourceComparisonList.Add(sourcePixels[i - numberAddCount]);
                    numberAddCount--;
                }

                //compare the lists
                for (var j = 0; j < NumberFirstLinePixels.Count; j++)
                {
                    if (!AreColorsSimilar(NumberFirstLinePixels[j], sourceComparisonList[j].Color, 50))
                    {
                        firstLineMatch = false;
                        break;
                    }
                }
                if(firstLineMatch)
                {
                    FirstLineMatches.Add(sourceComparisonList);
                }
            }
            return FirstLineMatches;
        }

        public Bitmap ConvertBlackAndWhite(Bitmap source)
        {
            using (Graphics gr = Graphics.FromImage(source)) // SourceImage is a Bitmap object
            {
                var gray_matrix = new float[][] {
                new float[] { 0.299f, 0.299f, 0.299f, 0, 0 },
                new float[] { 0.587f, 0.587f, 0.587f, 0, 0 },
                new float[] { 0.114f, 0.114f, 0.114f, 0, 0 },
                new float[] { 0,      0,      0,      1, 0 },
                new float[] { 0,      0,      0,      0, 1 }
            };

                var ia = new ImageAttributes();
                ia.SetColorMatrix(new ColorMatrix(gray_matrix));
                ia.SetThreshold((float)0.8); // Change this threshold as needed
                var rc = new Rectangle(0, 0, source.Width, source.Height);
                gr.DrawImage(source, rc, 0, 0, source.Width, source.Height, GraphicsUnit.Pixel, ia);
            }

            return source;
        }

        public static bool AreColorsSimilar(Color c1, Color c2, int tolerance)
        {
            return Math.Abs(c1.R - c2.R) < tolerance &&
                   Math.Abs(c1.G - c2.G) < tolerance &&
                   Math.Abs(c1.B - c2.B) < tolerance;
        }

    }
}
