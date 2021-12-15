using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCoding2021
{
    internal class Day15
    {
        public static string A(string input)
        {
            DangerMap map = new DangerMap(input);
            return map.FindShortestRoute().ToString();
        }

        public static string B(string input)
        {
            DangerMap map = new DangerMap(input);
            map.Expand();
            return map.FindShortestRoute().ToString();
        }

    }

    internal class DangerMap
    {
        short[,] map;
        public DangerMap(string input)
        {
            string[] lines = input.Split(new String[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            map = new short[lines[0].Length, lines.Length];
            int y = 0;
            foreach (string line in lines)
            {
            int x = 0;
                foreach (char c in line)
                {
                    map[x, y] = (short)(c - '0');
                    x++;
                }
                y++;
            }
        }

        public int FindShortestRoute()
        {
            int[,] score = new int[map.GetLength(0), map.GetLength(1)];
            for (int y = 0; y < map.GetLength(1); y++)
            {
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    score[x, y] = int.MaxValue/2;
                }
            }
            Queue<IntVector2> workQueue = new Queue<IntVector2>();
            score[0, 0] = 0;
            workQueue.Enqueue(new IntVector2(0, 0));

            while (workQueue.Count > 0)
            {
                IntVector2 location = workQueue.Dequeue();
                if (location.X >= 0 && location.Y >= 0 && location.X < map.GetLength(0) && location.Y < map.GetLength(1))
                {
                    // check each adjacent location to see if this location's updated score makes it easier to get to
                    foreach (IntVector2 direction in IntVector2.CardinalDirections)
                    {
                        IntVector2 testLocation = location.Add(direction);
                        if (testLocation.X >= 0 && testLocation.Y >= 0 && testLocation.X < map.GetLength(0) && testLocation.Y < map.GetLength(1))
                        {
                            // test location actually exists
                            int testScore = score[location.X, location.Y] + map[testLocation.X, testLocation.Y];
                            if (testScore < score[testLocation.X, testLocation.Y])
                            {
                                score[testLocation.X, testLocation.Y] = testScore;
                                workQueue.Enqueue(testLocation);
                            }
                        }
                    }
                }
            }

            return score[map.GetLength(0) - 1, map.GetLength(1) - 1];
        }

        internal void Expand()
        {
            short[,] newMap = new short[map.GetLength(0)*5, map.GetLength(1)*5];
            for (int y = 0; y < newMap.GetLength(1); y++)
            {
                for (int x = 0; x < newMap.GetLength(0);x++)
                {
                    int oldMapX = x % map.GetLength(0);
                    int oldMapY  = y % map.GetLength(1);
                    int xAdder = x / map.GetLength(0);
                    int yAdder = y / map.GetLength(1);
                    int newValue = map[oldMapX, oldMapY] + xAdder + yAdder;
                    if (newValue > 9)
                    {
                        newValue -= 9;
                    }
                    newMap[x, y] = (short) newValue;
                }
            }
            map = newMap;
        }
    }
}
