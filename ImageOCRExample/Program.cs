using NumbersOCR;
using System;
using System.Drawing;
using System.IO;
using System.Reflection;

namespace ImageOCRExample
{
    class Program
    {
        static void Main(string[] args)
        {
            //Number refrence files
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Images");
            FileInfo[] numbers = new DirectoryInfo(path+"\\Numbers").GetFiles();

            //example number image to read
            Bitmap exampleImage = new Bitmap(path+"\\Samples\\sample1.PNG");

            ReadNumbers ocr = new ReadNumbers();
            var result = ocr.ReadNumber(exampleImage, numbers);

            Console.WriteLine(result);
        }
    }
}
