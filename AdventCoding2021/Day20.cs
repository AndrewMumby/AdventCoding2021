using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCoding2021
{
    internal class Day20
    {
        public static string A (string input)
        {
            // first line is the enhancement string
            string[] lines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            string enhancement = lines[0];

            List<string> imageList = new List<string>();
            for (int i = 1; i < lines.Length; i++)
            {
                imageList.Add(lines[i]);    
            }

            OceanImage image = new OceanImage(enhancement, imageList);
            image.Enhance();
            image.Enhance();
            return image.CountPixels().ToString();
        }

        public static string B(string input)
        {
            // first line is the enhancement string
            string[] lines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            string enhancement = lines[0];

            List<string> imageList = new List<string>();
            for (int i = 1; i < lines.Length; i++)
            {
                imageList.Add(lines[i]);
            }

            OceanImage image = new OceanImage(enhancement, imageList);
            for (int i = 0; i <50; i++)
            {
                image.Enhance();
            }
          
            return image.CountPixels().ToString();
        }



    }

    internal class OceanImage
    {
        string enhancement;
        IntField2 field;

        public OceanImage(string enhancement, List<string> image)
        {
            this.enhancement = enhancement;

            field = new IntField2(image[0].Length, image.Count);

            for (int y = 0; y < image.Count; y++)
            {
                for (int x = 0; x < image[0].Length; x++)
                {
                    field.SetValue(x, y, (image[y][x] == '#' ? 1 : 0));
                }
            }
        }

        internal int CountPixels()
        {
            return (int) field.Sum();
        }

        internal void Enhance()
        {
            // Create a new, larger image
            IntField2 newField = new IntField2(field.GetSize(0) + 2, field.GetSize(1) + 2, GetNewPixel(CalcTotal(-10,-10)));
            for (int x = 0; x < newField.GetSize(0);x++)
            {
                for (int y = 0; y < newField.GetSize(1);y++)
                {
                    newField.SetValue(x, y, GetNewPixel(CalcTotal(x - 1, y - 1)));
                }
            }
            field = newField;
        }

        private int CalcTotal(int x, int y)
        {
            int value = 0;
            for (int yMod = -1; yMod <= 1; yMod++)
            {
                for (int xMod = -1; xMod <= 1; xMod++)
                {
                    value *= 2;
                    value += field.GetValue(x + xMod, y + yMod);
                }
            }
            return value;
        }

        private int GetNewPixel(int value)
        {
            if (enhancement[value] == '#')
            {
                return 1;
            }
            return 0;
        }
    }
}
