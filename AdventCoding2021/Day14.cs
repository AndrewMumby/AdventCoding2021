using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCoding2021
{
    internal class Day14
    {
        public static string DoWork(string input, int iterations)
        {
            string[] lines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            Dictionary<string, long> pairCounts = new Dictionary<string, long>();

            for (int i = 0; i < lines[0].Length - 1; i++)
            {
                if (!pairCounts.ContainsKey(lines[0].Substring(i, 2)))
                {
                    pairCounts.Add(lines[0].Substring(i, 2), 0);
                }
                pairCounts[lines[0].Substring(i, 2)]++;
            }

            Dictionary<string, List<string>> rules = new Dictionary<string, List<string>>();
            for (int i = 1; i < lines.Length; i++)
            {
                // 0123456
                // CH -> B
                string start = "" + lines[i][0] + lines[i][1];
                string first = "" + lines[i][0] + lines[i][6];
                string second = "" + lines[i][6] + lines[i][1];
                rules.Add(start, new List<string> { first, second });
            }

            for (int i = 0; i < iterations; i++)
            {
                Dictionary<string, long> newPairCounts = new Dictionary<string, long>();

                foreach (string pair in pairCounts.Keys)
                {
                    long count = pairCounts[pair];
                    List<string> results = rules[pair];
                    foreach (string newPair in results)
                    {
                        if (!newPairCounts.ContainsKey(newPair))
                        {
                            newPairCounts.Add(newPair, 0);
                        }
                        newPairCounts[newPair] += count;
                    }
                }
                pairCounts = newPairCounts;
            }

            // hacky sum time

            Dictionary<char, long> finalCounts = new Dictionary<char, long>();
            foreach (KeyValuePair<string, long> pair in pairCounts)
            {
                if (!finalCounts.ContainsKey(pair.Key[0]))
                {
                    finalCounts.Add(pair.Key[0], 0);

                }
                finalCounts[pair.Key[0]] += pair.Value;
                if (!finalCounts.ContainsKey(pair.Key[1]))
                {
                    finalCounts.Add(pair.Key[1], 0);

                }

                finalCounts[pair.Key[1]] += pair.Value;
            }

            foreach (char c in finalCounts.Keys.ToList())
            {
                finalCounts[c] = finalCounts[c] / 2;
            }


            finalCounts[lines[0][0]]++;
            finalCounts[lines[0].Last()]++;

            return (finalCounts.Values.Max()- finalCounts.Values.Min()).ToString();  
        }

        public static string A(string input)
        {
            return DoWork(input, 10);
        }

        public static string B(string input)
        {
            return DoWork(input, 40);
        }

    }
}