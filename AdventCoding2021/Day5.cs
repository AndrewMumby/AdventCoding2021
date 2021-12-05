using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCoding2021
{
    internal class Day5
    {
        public static string A(string input)
        {
            string[] lines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            // Build the map        
            Dictionary<IntVector2, int> floor = new Dictionary<IntVector2, int>();
            foreach (string line in lines)
            {
                // 0,9-> 5,9
                string[] parts = line.Split(new char[] { ',', '-', '>', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                IntVector2 first = new IntVector2(Convert.ToInt32(parts[0]), Convert.ToInt32(parts[1]));
                IntVector2 second = new IntVector2(Convert.ToInt32(parts[2]), Convert.ToInt32(parts[3]));
                if (first.X == second.X)
                {
                    IntVector2 currentLocation = new IntVector2(first);
                    int step = -1;
                    if (second.Y > currentLocation.Y)
                    {
                        step = 1;
                    }
                    do
                    {
                        if (!floor.ContainsKey(currentLocation))
                        {
                            floor.Add(currentLocation, 0);
                        }
                        floor[currentLocation]++;
                        IntVector2 nextLocation = new IntVector2(currentLocation);
                        nextLocation.Y += step;
                        currentLocation = nextLocation;
                    } while (!currentLocation.Equals(second));
                    // Add the last point because cludge
                    if (!floor.ContainsKey(second))
                    {
                        floor.Add(second, 0);
                    }
                    floor[second]++;

                }
                else if (first.Y == second.Y)
                {
                    IntVector2 currentLocation = new IntVector2(first);
                    int step = -1;
                    if (second.X > currentLocation.X)
                    {
                        step = 1;
                    }
                    do
                    {
                        if (!floor.ContainsKey(currentLocation))
                        {
                            floor.Add(currentLocation, 0);
                        }
                        floor[currentLocation]++;
                        IntVector2 nextLocation = new IntVector2(currentLocation);
                        nextLocation.X += step;
                        currentLocation = nextLocation;
                    } while (!currentLocation.Equals(second));
                    // Add the last point because cludge
                    if (!floor.ContainsKey(second))
                    {
                        floor.Add(second, 0);
                    }
                    floor[second]++;

                }


                /*
                // Draw the map
                int maxX = 0;
                int maxY = 0;
                foreach (KeyValuePair<IntVector2, int> pair in floor)
                {
                    maxX = Math.Max(maxX, pair.Key.X);
                    maxY = Math.Max(maxY, pair.Key.Y);
                }
                for (int y = 0; y <= maxY; y++)
                {
                    for (int x = 0; x <= maxX; x++)
                    {
                        IntVector2 location = new IntVector2(x, y);
                        if (floor.ContainsKey(location))
                        {
                            Console.Write(floor[location]);
                        }
                        else
                        {
                            Console.Write(".");
                        }
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
                */
            }

            // Count the overlaps
            int count = 0;
            foreach (KeyValuePair<IntVector2, int> pair in floor)
            {
                if (pair.Value > 1)
                {
                    count++;
                }
            }
            return count.ToString();
        }

        public static string B(string input)
        {
            string[] lines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            // Build the map        
            Dictionary<IntVector2, int> floor = new Dictionary<IntVector2, int>();
            foreach (string line in lines)
            {
                // 0,9-> 5,9
                string[] parts = line.Split(new char[] { ',', '-', '>', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                IntVector2 first = new IntVector2(Convert.ToInt32(parts[0]), Convert.ToInt32(parts[1]));
                IntVector2 second = new IntVector2(Convert.ToInt32(parts[2]), Convert.ToInt32(parts[3]));
                IntVector2 direction = first.DirectionTo(second);
                IntVector2 currentLocation = new IntVector2(first);
                while (!currentLocation.Equals(second))
                {
                    if (!floor.ContainsKey(currentLocation))
                    {
                        floor.Add(currentLocation, 0);
                    }
                    floor[currentLocation]++;
                    currentLocation = currentLocation.Add(direction);
                }
                // Add the last point because cludge
                if (!floor.ContainsKey(second))
                {
                    floor.Add(second, 0);
                }
                floor[second]++;




                /*
                // Draw the map
                int maxX = 0;
                int maxY = 0;
                foreach (KeyValuePair<IntVector2, int> pair in floor)
                {
                    maxX = Math.Max(maxX, pair.Key.X);
                    maxY = Math.Max(maxY, pair.Key.Y);
                }
                for (int y = 0; y <= maxY; y++)
                {
                    for (int x = 0; x <= maxX; x++)
                    {
                        IntVector2 location = new IntVector2(x, y);
                        if (floor.ContainsKey(location))
                        {
                            Console.Write(floor[location]);
                        }
                        else
                        {
                            Console.Write(".");
                        }
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
                */
            }

            // Count the overlaps
            int count = 0;
            foreach (int depth in floor.Values)
            {
                if (depth > 1)
                {
                    count++;
                }
            }
            return count.ToString();

        }
    }
}
