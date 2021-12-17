using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCoding2021
{
    internal class Day13
    {
        public static string A(string input)
        {
            string[] lines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            List<string> dots = new List<string>();
            List<string> folds = new List<string>();
            int i = 0;
            while (lines[i].Length != 0)
            {
                dots.Add(lines[i]);
                i++;
            }
            i++;
            while (i < lines.Length)
            {
                folds.Add(lines[i]);
                i++;
            }

            GiantPaper paper = new GiantPaper(dots);
            paper.Fold(folds[0]);

         //   paper.DrawImage();

            return paper.DotCount();
        }
        public static string B(string input)
        {
            string[] lines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            List<string> dots = new List<string>();
            List<string> folds = new List<string>();
            int i = 0;
            while (lines[i].Length != 0)
            {
                dots.Add(lines[i]);
                i++;
            }
            i++;
            while (i < lines.Length)
            {
                folds.Add(lines[i]);
                i++;
            }

            GiantPaper paper = new GiantPaper(dots);
            foreach (string line in folds)
            {
                paper.Fold(line);
            }

            paper.DrawImage();

            return paper.DotCount();
        }
    }

    internal class GiantPaper
    {
        List<IntVector2> dots;
        public GiantPaper(List<string> inputDots)
        {
            dots = new List<IntVector2> ();
            foreach (string line in inputDots)
            {
                IntVector2 dotLocation = new IntVector2(line);
                dots.Add(dotLocation);
            }
        }

        internal string DotCount()
        {
            return dots.Distinct().Count().ToString();
        }

        internal void DrawImage()
        {
            int minX = 0;
            int minY = 0;
            int maxX = 0;
            int maxY = 0;

            foreach (IntVector2 dot in dots)
            {
                minX = Math.Min(minX, dot.X);
                maxX = Math.Max(maxX, dot.X);
                minY = Math.Min(minY, dot.Y);
                maxY = Math.Max(maxY, dot.Y);
            }
            StringBuilder output = new StringBuilder();
            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    if (dots.Contains(new IntVector2(x, y)))
                    {
                        output.Append("#");
                    }
                    else
                    {
                        output.Append('.');
                    }
                }
                output.AppendLine();
            }
            Console.WriteLine(output.ToString());
        }

        internal void Fold(string foldString)
        {
            // fold along y=7

            bool vertical = foldString[11] == 'x';
            int location = int.Parse(foldString.Substring(13));
            List<IntVector2> newDots = new List<IntVector2>();
            foreach (IntVector2 dot in dots)
            {
                IntVector2 newDot;
                if (vertical)
                {
                    if (dot.X > location)
                    {
                        newDot = new IntVector2(location - (dot.X - location), dot.Y);
                    }
                    else
                    {
                        newDot = new IntVector2(dot.X, dot.Y);
                    }
                }
                else
                {
                    if (dot.Y > location)
                    {
                        newDot = new IntVector2(dot.X, location - (dot.Y-location));
                    }
                    else
                    {
                        newDot = new IntVector2(dot.X, dot.Y);
                    }

                }
                newDots.Add(newDot);
            }
            dots = newDots;
        }
    }
}
