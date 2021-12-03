using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCoding2021
{
    internal class Day3
    {
        public static string A(string input)
        {
            string[] lines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            // count the occurances in each position
            int[] counts = new int[lines[0].Length];
            foreach (string line in lines)
            {
                int i = 0;
                foreach (char c in line)
                {
                    if (c == '1')
                    {
                        counts[i]++;
                    }
                    i++;
                }
            }
            // build the answers
            int answer = 0;
            int antiAnswer = 0;
            foreach (int count in counts)
            {
                answer *= 2;
                antiAnswer *= 2;
                if (count > lines.Length / 2)
                {
                    answer++;
                }
                else
                {
                    antiAnswer++;
                }
            }
            //multiply to get the answer
            return (answer * antiAnswer).ToString();
        }
        public static string B(string input)
        {
            string[] lines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            int position = 0;
            List<string> remainingLines = lines.ToList();
            while (remainingLines.Count > 1)
            {
                // count the 1s in position
                int count = 0;
                foreach (string line in remainingLines)
                {
                    if (line[position] == '1')
                    {
                        count++;
                    }
                }
                char requiredDigit = '1';
                if (count*2 < remainingLines.Count)
                {
                    requiredDigit = '0';
                }
                List<string> newRemainingLines = new List<string>();
                foreach(string line in remainingLines)
                {
                    if (line[position]==requiredDigit)
                    {
                        newRemainingLines.Add(line);
                    }
                }
                remainingLines = newRemainingLines;
                position++;
            }
            // convert the remaining value to an int
            int firstAnswer = 0;
            foreach(char c in remainingLines[0])
            {
                firstAnswer *= 2;
                if (c == '1')
                {
                    firstAnswer++;
                }
            }

            // now do it again, but differently
            position = 0;
            remainingLines = lines.ToList();
            while (remainingLines.Count > 1)
            {
                // count the 1s in position
                int count = 0;
                foreach (string line in remainingLines)
                {
                    if (line[position] == '1')
                    {
                        count++;
                    }
                }
                char requiredDigit = '0';
                if (count*2 < remainingLines.Count)
                {
                    requiredDigit = '1';
                }
                List<string> newRemainingLines = new List<string>();
                foreach (string line in remainingLines)
                {
                    if (line[position] == requiredDigit)
                    {
                        newRemainingLines.Add(line);
                    }
                }
                remainingLines = newRemainingLines;
                position++;
            }
            // convert the remaining value to an int
            int secondAnswer = 0;
            foreach (char c in remainingLines[0])
            {
                secondAnswer *= 2;
                if (c == '1')
                {
                    secondAnswer++;
                }
            }

            return (firstAnswer * secondAnswer).ToString();

        }
    }
}
