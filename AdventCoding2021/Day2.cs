using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCoding2021
{
    internal class Day2
    {
        public static string A(string input)
        {
            string[] lines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            int horizontal = 0;
            int vertical = 0;
            foreach (string line in lines)
            {
                string[] parts = line.Split(new char[] { ' ' });
                switch (parts[0])
                {
                    case "forward":
                        horizontal += Convert.ToInt32(parts[1]);
                        break;
                    case "down":
                        vertical += Convert.ToInt32(parts[1]);
                        break;
                    case "up":
                        vertical -= Convert.ToInt32(parts[1]);
                        break;
                    default: throw new ArgumentException("Invalid direction " + parts[0]);
                }
            }
            return (horizontal * vertical).ToString();
        }
        public static string B(string input)
        {
            string[] lines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            int horizontal = 0;
            int vertical = 0;
            int aim = 0;
            foreach (string line in lines)
            {
                string[] parts = line.Split(new char[] { ' ' });
                switch (parts[0])
                {
                    case "forward":
                        horizontal+= Convert.ToInt32(parts[1]);
                        vertical += Convert.ToInt32(parts[1]) * aim;
                        break;
                    case "down":
                        aim += Convert.ToInt32(parts[1]);
                        break;
                    case "up":
                        aim -= Convert.ToInt32(parts[1]);   
                        break;
                    default: throw new ArgumentException("Invalid direction " + parts[0]);
                }
            }
            return (horizontal * vertical).ToString();
        }
    }
}
