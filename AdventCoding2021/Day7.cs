using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCoding2021
{
    internal class Day7
    {
        public static string A(string input)
        {
            RescueCrabField field = new RescueCrabField(input);
            return field.FindMinFuel().ToString();
        }

        public static string B(string input)
        {
            RescueCrabField field = new RescueCrabField(input);
            return field.FindMinExpFuel().ToString();

        }


    }

    internal class RescueCrabField
    {
        List<int> crabPositions;

        public RescueCrabField(string input)
        {
            crabPositions = new List<int>();
            string[] parts = input.Split(',');
            foreach (string part in parts)
            {
                crabPositions.Add(int.Parse(part));
            }
        }

        public int FindMinFuel()
        {
            int minPos = crabPositions.Min();
            int maxPos = crabPositions.Max();

            int minFuel = int.MaxValue;
            int bestPos = minPos;
            for (int i = minPos; i <= maxPos; i++)
            {
                int fuelNeeded = FuelAtPos(i);
                if (fuelNeeded < minFuel)
                {
                    minFuel = fuelNeeded;
                    bestPos = i;
                }
            }
            return minFuel;
        }

        internal object FindMinExpFuel()
        {
            int minPos = crabPositions.Min();
            int maxPos = crabPositions.Max();

            int minFuel = int.MaxValue;
            int bestPos = minPos;
            for (int i = minPos; i <= maxPos; i++)
            {
                int fuelNeeded = ExpFuelAtPos(i);
                if (fuelNeeded < minFuel)
                {
                    minFuel = fuelNeeded;
                    bestPos = i;
                }
            }
            return minFuel;
        }

        private int FuelAtPos(int pos)
        {
            int fuelNeeded = 0;
            foreach (int i in crabPositions)
            {
                fuelNeeded += Math.Abs(pos - i);
            }

            return fuelNeeded;
        }

        private int ExpFuelAtPos(int pos)
        {
            int fuelNeeded = 0;
            foreach (int i in crabPositions)
            {               
                int distance  = Math.Abs(pos - i);
                fuelNeeded += (distance * (distance + 1)) / 2;
            }
            return fuelNeeded;
        }
    }

}
