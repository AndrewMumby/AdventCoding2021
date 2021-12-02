using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCoding2021
{
    internal class Day1
    {
        private static List<int> ParseInput(string input)
        {
            string[] inputList = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            List<int> numberList = new List<int>();
            foreach (string inputItem in inputList)
            {
                numberList.Add(Int32.Parse(inputItem));
            }
            return numberList;
        }
        public static string A(string input)
        {
            List<int> numbers = ParseInput(input);
            int count = 0;
            for (int i = 0; i < numbers.Count - 1; i++)
            {
                if (numbers[i] < numbers[i + 1])
                {
                    count++;
                }
            }
            return count.ToString();
        }

        public static string B(string input)
        {
            List<int> numbers = ParseInput(input);
            int count = 0;
            for (int i = 0; i < numbers.Count - 3; i++)
            {
                if (numbers[i] < numbers[i + 3])
                {
                    count++;
                }
            }
            return count.ToString();
        }
    }
}
