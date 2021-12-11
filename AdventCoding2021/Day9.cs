using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCoding2021
{
    internal class Day9
    {
        public static string A(string input)
        {
            CaveMap map = new CaveMap(input);
            List<IntVector2> lowPoints = map.FindLowPoints();
            int sum = 0;
            foreach (IntVector2 lowPoint in lowPoints)
            {
                foreach (IntVector2 direction in IntVector2.CardinalDirections)
                {
                    IntVector2 newLocation = lowPoint.Add(direction);
                }
                sum += map.GetHeight(lowPoint) + 1;
            }
            return sum.ToString();
        }

        public static string B(string input)
        {
            CaveMap map = new CaveMap(input);
            return map.SumTopThreeBasins();


        }
    }

    internal class CaveMap
    {
        Dictionary<IntVector2, int> heights;

        public CaveMap(string input)
        {
            heights = new Dictionary<IntVector2, int>();
            string[] lines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            int y = 0;
            foreach (string line in lines)
            {
                for (int x = 0; x < line.Length; x++)
                {
                    heights.Add(new IntVector2(x, y), (line[x] - '0'));
                }
                y++;
            }
        }

        internal List<IntVector2> FindLowPoints()
        {
            List<IntVector2> lowPoints = new List<IntVector2>();
            foreach (IntVector2 location in heights.Keys)
            {
                bool lowest = true;
                foreach (IntVector2 direction in IntVector2.CardinalDirections)
                {
                    IntVector2 newLocation = location.Add(direction);
                    if (heights.ContainsKey(newLocation) && heights[newLocation] <= heights[location])
                    {
                        lowest = false;

                    }
                }
                if (lowest)
                {
                    lowPoints.Add(location);
                }

            }
            return lowPoints;
        }

        internal int GetHeight(IntVector2 lowPoint)
        {
            if (heights.ContainsKey(lowPoint))
            {
                return heights[lowPoint];
            }
            else
            {
                return 10;
            }
        }

        internal string SumTopThreeBasins()
        {
            // assign all the non-9 points to a low point
            Dictionary<IntVector2, IntVector2> basinAssignment = new Dictionary<IntVector2, IntVector2>();
            foreach (IntVector2 location in heights.Keys)
            {
                if (GetHeight(location) < 9)
                {
                    bool bottom = false;
                    IntVector2 newLocation = new IntVector2(location);
                    do
                    {
                        // Have we already done the work here?
                        if (basinAssignment.ContainsKey(newLocation))
                        {
                            newLocation = basinAssignment[newLocation];
                            bottom = true;
                        }
                        else
                        {
                            // find the low point around us, if it exists
                            IntVector2 bestLow = null;
                            int bestDepth = 10;
                            foreach (IntVector2 direction in IntVector2.CardinalDirections)
                            {
                                IntVector2 nextLocation = newLocation.Add(direction);
                                if (GetHeight(nextLocation) < bestDepth)
                                {
                                    bestLow = nextLocation;
                                    bestDepth = GetHeight(nextLocation);
                                }
                            }
                            if (bestDepth < GetHeight(newLocation))
                            {
                                newLocation = bestLow;
                            }
                            else
                            {
                                bottom = true;
                            }
                        }


                    } while (!bottom);

                    basinAssignment.Add(location, newLocation);
                }
            }

            // Now we have a load of location - lowpoint pairs. Sum them up
            Dictionary<IntVector2, int> scores = new Dictionary<IntVector2, int>();

            foreach (IntVector2 value in basinAssignment.Values)
            {
                if (!scores.ContainsKey(value))
                {
                    scores.Add(value, 0);
                }
                scores[value]++;
            }

            // Now we need to find the top 3 basins
            List<int> sortedScores = scores.Values.OrderByDescending(x=>x).ToList();
            int totalScore = 1;
            for (int i = 0; i< 3; i++)
            {
                totalScore *= sortedScores[i];
            }
            return totalScore.ToString();
        }
    }
}
