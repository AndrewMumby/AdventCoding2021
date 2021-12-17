using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCoding2021
{
    internal class Day17
    {
        public static string A (string input)
        {
            BallisticComputer comp = new BallisticComputer(input);
            return comp.MaxHeight(comp.HighestShot()).ToString();
        }

        public static string B(string input)
        {
            BallisticComputer comp = new BallisticComputer(input);
            return comp.AllGoodShots().Count().ToString();
        }

    }

    internal class BallisticComputer
    {
        int targetAreaLeft;
        int targetAreaRight;
        int targetAreaTop;
        int targetAreaBottom;

        public BallisticComputer (string input)
        {   // 01234567890123456789012345678901234567
            // target area: x = 20..30, y = -10..-5

            string subString = input.Substring(13);
            string[] parts = subString.Split(new char[] {'=', ' ','.',','}, StringSplitOptions.RemoveEmptyEntries);
            targetAreaLeft = int.Parse(parts[1]);
            targetAreaRight = int.Parse(parts[2]);
            targetAreaTop = int.Parse(parts[5]);
            targetAreaBottom = int.Parse(parts[4]);
        }

        internal IntVector2 HighestShot()
        {
            // ignore the x position
            int best = 0;
            for (int y = 0; y < 1000; y++)
            {
                if (HitsVertically(new IntVector2(0, y)))
                {
                    best = y;
                }
            }
                return new IntVector2(0, best);
        }

        private bool HitsVertically(IntVector2 shotVector)
        {
            IntVector2 position = new IntVector2(0, 0);
            IntVector2 velocity = new IntVector2(shotVector);
            while (position.X < targetAreaRight && position.Y > targetAreaBottom)
            {
                position = position.Add(velocity);
                if (velocity.X > 0)
                {
                    velocity.X--;
                }
                else if (velocity.X < 0)
                {
                    velocity.X++;
                }
                velocity.Y--;
                
                // we only care about verticality
                if (position.Y >= targetAreaBottom && position.Y <= targetAreaTop)
                {
                    //Console.WriteLine(shotVector.ToString() + " hits!");
                    return true;
                }
            }
            return false;

        }

        private bool Hits(IntVector2 shotVector)
        {
            IntVector2 position = new IntVector2(0, 0);
            IntVector2 velocity = new IntVector2(shotVector);
            while (position.X < targetAreaRight && position.Y > targetAreaBottom && position.Y < 9181)
            {
                position = position.Add(velocity);
                if (velocity.X > 0)
                {
                    velocity.X--;
                }
                else if (velocity.X < 0)
                {
                    velocity.X++;
                }
                velocity.Y--;

                if (position.X >= targetAreaLeft && position.X <= targetAreaRight && position.Y >= targetAreaBottom && position.Y <= targetAreaTop)
                {
                    //Console.WriteLine(shotVector.ToString() + " hits!");
                    return true;
                }
//                Console.WriteLine(shotVector.ToString() + " misses!");

            }
            return false;

        }

        internal int MaxHeight(IntVector2 shotVector)
        {
            IntVector2 position = new IntVector2(0, 0);
            IntVector2 velocity = new IntVector2(shotVector);
            while (velocity.Y != 0)
            {
                position = position.Add(velocity);
                if (velocity.X > 0)
                {
                    velocity.X--;
                }
                else if (velocity.X < 0)
                {
                    velocity.X++;
                }
                velocity.Y--;
            }

            return position.Y;
        }

        internal List<IntVector2> AllGoodShots()
        {
            List<IntVector2> goodShots = new List<IntVector2>();
            for (int y = -1000; y <= 1000; y++)
            {
                for (int x = -1000; x <= 1000; x++)
                {
                    IntVector2 shotVector = new IntVector2(x, y);
                    if (Hits(shotVector))
                    {
                        goodShots.Add(shotVector);
                    }
                }
            }
            return goodShots;
        }
    }
}
