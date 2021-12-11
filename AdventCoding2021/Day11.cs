using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCoding2021
{
    internal class Day11
    {
        public static string A(string input)
        {
            SquidField field = new SquidField(input);
            int flashCount = 0;
            for (int i = 0; i < 100; i++)
            {
                flashCount += field.Iterate();
            }
            return flashCount.ToString();
        }

        public static string B(string input)
        {
            SquidField field = new SquidField(input);
            long step = 1;
            while (field.Iterate() != 100)
            {
                step++;
            }
            return step.ToString();

        }
    }

    internal class SquidField
    {
        int[,] field;
        public SquidField(string input)
        {
            field = new int[10, 10];
            string[] lines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            int y = 0;
            foreach (string line in lines)
            {

                for (int x = 0; x < line.Length; x++)
                {
                    field[x, y] = line[x]-'0';
                }
                y++;
            }
        }

        public int Iterate()
        {
            // First, the energy level of each octopus increases by 1.
            Queue<IntVector2> flashQueue = new Queue<IntVector2>();
            int flashCount = 0;
            for (int y = 0; y < field.GetLength(1); y++)
            {
                for (int x = 0; x < field.GetLength(0); x++)
                {
                    field[x, y]++;
                    if (field[x, y] > 9)
                    {
                        flashQueue.Enqueue(new IntVector2(x, y));
                    }
                }
            }
            // Then, any octopus with an energy level greater than 9 flashes.
            //      This increases the energy level of all adjacent octopuses by 1,
            //      including octopuses that are diagonally adjacent.
            //      If this causes an octopus to have an energy level greater than 9,
            //      it also flashes.This process continues as long as new octopuses
            //      keep having their energy level increased beyond 9.
            //      (An octopus can only flash at most once per step.)

            while (flashQueue.Count > 0)
            {
                flashCount++;
                IntVector2 flashLocation = flashQueue.Dequeue();
                // Boost all the adjacent squid
                foreach (IntVector2 direction in IntVector2.CardinalDirectionsIncludingDiagonals)
                {
                    IntVector2 newLocation = flashLocation.Add(direction);
                    if (newLocation.X >= 0 && newLocation.X <= 9 && newLocation.Y >= 0 && newLocation.Y <= 9)
                    {
                        field[newLocation.X, newLocation.Y]++;
                        if (field[newLocation.X, newLocation.Y] == 10)
                        {
                            flashQueue.Enqueue(newLocation);
                        }
                    }
                }


            }

            // Finally, any octopus that flashed during this step has its energy level set to 0, as it used all of its energy to flash.
            for (int y = 0; y < field.GetLength(1); y++)
            {
                for (int x = 0; x < field.GetLength(0); x++)
                {
                    if (field[x, y] > 9)
                    {
                        field[x, y] = 0;
                    }
                }
            }

            return flashCount;
        }

        public void DrawField()
        {
            for (int y = 0; y < field.GetLength(1); y++)
            {
                for (int x = 0; x < field.GetLength(0); x++)
                {
                    Console.Write(field[x, y]);
                }
                Console.WriteLine();
            }
        }
    }
}