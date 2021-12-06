using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCoding2021
{
    internal class Day6
    {
        public static string A(string input)
        {
            FishSchool school = new FishSchool(input);
            school.Run(80);
            return school.FishCount().ToString();
        }
        public static string B(string input)
        {
            FishSchool school = new FishSchool(input);
            school.Run(256);
            return school.FishCount().ToString();
        }

    }

    internal class FishSchool
    {
        long[] ageGroups;

        public FishSchool(string input)
        {
            ageGroups = new long[9];
            string[] parts = input.Split(new char[] { ',' });
            foreach (long age in parts.Select(x => Convert.ToInt64(x)))
            {
                ageGroups[age]++;
            }
        }

        internal object FishCount()
        {
            long count = 0;
            foreach (long ageCount in ageGroups)
            {
                count += ageCount;
            }
            return count;
        }

        internal void Run(int iterations)
        {
            for (int i = 0; i < iterations; i++)
            {
                Run();
            }
        }

        private void Run()
        {
            long[] newAgeGroups = new long[9];

            newAgeGroups[8] = ageGroups[0];
            newAgeGroups[6] = ageGroups[0];
            for (int i = 0; i < 8; i++)
            {
                newAgeGroups[i] += ageGroups[i + 1];
            }
            ageGroups = newAgeGroups;
        }
    }
}

