using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCoding2021
{
    internal class Day8
    {
        public static string A(string input)
        {
            int count = 0;
            string[] lines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            foreach (string line in lines)
            {
                SSSolver solver = new SSSolver(line);
                count += solver.CountEasyOnes();
            }

            return count.ToString();
        }

        public static string B(string input)
        {
            int score = 0;
            string[] lines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            foreach (string line in lines)
            {
                SSSolver solver = new SSSolver(line);
                solver.Deduce();
                score += solver.GetOutput();
            }
            return score.ToString();
        }
    }

    internal class SSSolver
    {
        Dictionary<string, int> key;
        Dictionary<int, string> antiKey;
        List<string> patterns;
        List<string> outputs;

        public SSSolver(string line)
        {
            patterns = new List<string>();
            outputs = new List<string>();
            key = new Dictionary<string, int>();
            antiKey = new Dictionary<int, string>();

            string[] parts = line.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            string[] subparts = parts[0].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string subpart in subparts)
            {
                patterns.Add(subpart);
            }
            subparts = parts[1].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string subpart in subparts)
            {
                outputs.Add(subpart);
            }
        }

        internal int CountEasyOnes()
        {
            int count = 0;
            foreach (string output in outputs)
            {
                if (output.Length == 2 || output.Length == 4 || output.Length == 3 || output.Length == 7)
                {
                    count++;
                }
            }
            return count;
        }

        internal int GetOutput()
        {
            int output = 0;
            for (int i = 0; i < outputs.Count; i++)
            {
                output *= 10;
                output += Solve(outputs[i]);
            }
            return output; 
        }

        internal void Deduce()
        {
            // first, sort all the entries
            for (int i = 0; i < patterns.Count; i++)
            {
                patterns[i] = String.Concat(patterns[i].OrderBy(c => c));
            }

            for (int i = 0; i < outputs.Count; i++)
            {
                outputs[i] = String.Concat(outputs[i].OrderBy(c => c));

            }

            // 1,4,7 and 8 are easy, so lets do those first

            List<string> remainingPatterns = new List<string>(patterns);
            foreach (string pattern in patterns)
            {
                if (pattern.Length == 2)
                {
                    AddKey(pattern, 1);
                }
                else if (pattern.Length == 4)
                {
                    AddKey(pattern, 4);
                }
                else if (pattern.Length == 3)
                {
                    AddKey(pattern, 7);
                }
                else if (pattern.Length == 7)
                {
                    AddKey(pattern, 8);
                               }
            }
            foreach (string key in key.Keys)
            {
                remainingPatterns.Remove(key);
            }

            // 3 has 5 segments and shares 2 segments with 1
            AddKey(PatternSearch(remainingPatterns, 5, 1, 2), 3);
            remainingPatterns.Remove(KeyLook(3));

            // 6 has 6 segments and shares 1 segment with 1
            AddKey(PatternSearch(remainingPatterns, 6, 1, 1), 6);
            remainingPatterns.Remove(KeyLook(6));

            // 0 has 6 segments and shares 3 segments with 4
            AddKey(PatternSearch(remainingPatterns, 6, 4, 3), 0);
            remainingPatterns.Remove(KeyLook(0));

            // 9 has 6 segments and shares 4 segments with 4
            AddKey(PatternSearch(remainingPatterns, 6, 4, 4), 9);
            remainingPatterns.Remove(KeyLook(9));

            // 2 has 5 segments and shares 2 segments with 4
            AddKey(PatternSearch(remainingPatterns, 5, 4, 2), 2);
            remainingPatterns.Remove(KeyLook(2));

            // 5 has 5 segments and shares 3 segments with 4
            AddKey(PatternSearch(remainingPatterns, 5, 4, 3), 5);
        }
   
        private int Solve(string character)
        {
            return key[character];
        }

        private int Compare(string A, string B)
        {
            int count = 0;
            foreach (char c in A)
            {
                if (B.Contains(c))
                {
                    count++;
                }
            }
            return count;
        }

        private void AddKey(string pattern, int value)
        {
            key.Add(pattern, value);
            antiKey.Add(value, pattern);
        }

            private string KeyLook(int value)
            {
                return antiKey[value];
            }
            
            private int KeyLook(string pattern)
            {
            return key[pattern];
            }

        private string PatternSearch (List<string> remainingPatterns, int segments, int compare, int score)
        {
            foreach (string pattern in remainingPatterns)
            {
                if (pattern.Length == segments && Compare(pattern, KeyLook(compare)) == score)
                {
                    return pattern;
                }
            }
            throw new Exception("No pattern found");

        }

    }
}
