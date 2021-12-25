using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCoding2021
{
    internal class Day25
    {
        public static string A(string input)
        {
            CucField field = new CucField(input);
            return field.StepsToEnd().ToString();
        }
    }
    internal class CucField
    {
        HashSet<IntVector2> easts;
        HashSet<IntVector2> souths;
        int maxX;
        int maxY;

        public CucField(string input)
        {
            easts = new HashSet<IntVector2>();
            souths = new HashSet<IntVector2>();
            string[] lines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            for (int y = 0; y < lines.Length; y++)
            {
                for (int x = 0; x < lines[y].Length; x++)
                {
                    switch (lines[y][x])
                    {
                        case '>':
                            maxX = Math.Max(x, maxX);
                            maxY = Math.Max(y, maxY);
                            easts.Add(new IntVector2(x, y));
                            break;
                        case 'v':
                            maxX = Math.Max(x, maxX);
                            maxY = Math.Max(y, maxY);

                            souths.Add(new IntVector2(x, y));
                            break;
                        case '.':
                            break;
                        default:
                            throw new Exception("Unknown cucumber " + lines[y][x]);
                    }
                }
            }
            maxX++;
            maxY++;
        }

        public int StepsToEnd()
        {
            int steps = 0;
            bool moved = false;
            do
            {
                //Draw();
                //Console.WriteLine();
                steps++;
                moved = false;
                HashSet<IntVector2> newEasts = new HashSet<IntVector2>();
                foreach (IntVector2 east in easts)
                {
                    IntVector2 newLoc = Normalise(east.East());
                    if (IsEmpty(newLoc))
                    {
                        newEasts.Add(newLoc);
                        moved = true;
                    }
                    else
                    {
                        newEasts.Add(east);
                    }
                }
                easts = newEasts;

                HashSet<IntVector2> newSouths = new HashSet<IntVector2>();
                foreach (IntVector2 south in souths)
                {
                    IntVector2 newLoc = Normalise(south.South());
                    if (IsEmpty(newLoc))
                    {
                        newSouths.Add(newLoc);
                        moved = true;
                    }
                    else
                    {
                        newSouths.Add(south);
                    }
                }
                souths = newSouths;

            } while (moved);
            return steps;
        }

        public void Draw()
        {
            for (int y = 0; y < maxY; y++)
            {
                for (int x = 0; x < maxX; x++)
                {
                    IntVector2 location = new IntVector2(x, y);
                    if (easts.Contains(location))
                    {
                        Console.Write('>');                            
                    }
                    else if (souths.Contains(location))
                    {
                        Console.Write('v');
                    }
                    else
                    {
                        Console.Write('.');
                    }
                }
                Console.WriteLine();
            }
        }

        private bool IsEmpty(IntVector2 location)
        {
            return !easts.Contains(location) && !souths.Contains(location);

        }

        private IntVector2 Normalise(IntVector2 location)
        {
            if (location.X < 0 || location.Y < 0 || location.X >= maxX || location.Y >= maxY)
            {
                int x = location.X;
                int y = location.Y;
                x = x + maxX;
                y = y + maxY;
                x = x % maxX;
                y = y % maxY;
                return new IntVector2(x, y);
            }
            else
            {
                return location;
            }
        }
    }
}
