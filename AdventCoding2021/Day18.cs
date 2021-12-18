using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCoding2021
{
    internal class Day18
    {
        public static string A( string input)
        {
            // first, the tests
            // Tests();
            string workingString = "";
            foreach (string line in input.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (workingString.Length == 0)
                {
                    workingString = line;
                }
                else
                {
                    //Console.WriteLine("Added to");
                    workingString = SnailMaths.Add(workingString, line);
                    //Console.WriteLine(workingString);
                }
                workingString = SnailMaths.FullReduce(workingString);
            }
            return SnailMaths.Magnitude(workingString).ToString();
        }

        public static string B(string input)
        {
            string[] lines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            int bestScore = 0;
            for (int a = 0; a < lines.Length; a++)
            {
                for (int b = 0; b < lines.Length; b++)
                {
                    if (a!=b)
                    {
                        int score = SnailMaths.Magnitude(SnailMaths.FullReduce(SnailMaths.Add(lines[a], lines[b])));
                        //Console.WriteLine(lines[a] + " + " + lines[b] + ": " + score);
                        bestScore = Math.Max(bestScore, score);
                    }
                }
            }

            return bestScore.ToString();
        }

        public static bool Tests()
        {
            Console.WriteLine("[1,2] + [[3,4],5]");
            Console.WriteLine("Expected: [[1,2],[[3,4],5]]");
            Console.WriteLine("Actual:   " + SnailMaths.Add("[1,2]", "[[3,4],5]"));
            Console.WriteLine();

            ExplodeTest("[[[[[9,8],1],2],3],4]", "[[[[0,9],2],3],4]");
            ExplodeTest("[7,[6,[5,[4,[3,2]]]]]", "[7,[6,[5,[7,0]]]]");
            ExplodeTest("[[6,[5,[4,[3,2]]]],1]", "[[6,[5,[7,0]]],3]");
            ExplodeTest("[[3,[2,[1,[7,3]]]],[6,[5,[4,[3,2]]]]]", "[[3,[2,[8,0]]],[9,[5,[4,[3,2]]]]]");
            ExplodeTest("[[3,[2,[8,0]]],[9,[5,[4,[3,2]]]]]", "[[3,[2,[8,0]]],[9,[5,[7,0]]]]");

            SplitTest("10", "[5,5]");
            SplitTest("11", "[5,6]");
            SplitTest("12", "[6,6]");
            return true;
        }

        private static void SplitTest(string v1, string v2)
        {
            Console.WriteLine(v1);
            Console.WriteLine("Expected:" + v2);
            Console.WriteLine("Actual:  " + SnailMaths.Split(v1));
        }

        private static void ExplodeTest(string v1, string v2)
        {
            Console.WriteLine(v1);
            Console.WriteLine("Expected:" + v2);
            Console.WriteLine("Actual:  " + SnailMaths.Explode(v1));
            Console.WriteLine();
        }
    }

    internal class SnailMaths
    {
        internal static string FullReduce(string input)
        {
            string workingString = input;
            while (true)
            {
                string newWorkingString = Reduce(workingString);
                if (newWorkingString.Equals(workingString))
                {
                    break;
                }
                workingString = newWorkingString; 
            }

            return workingString;
        }

        internal static string Reduce (string input)
        {
            string workingString = Explode(input);
            if (workingString.Equals(input))
            {
                workingString = Split(input);
                //Console.WriteLine("Split to");
            }
            else
            {
                //Console.WriteLine("Explode to");
            }
            //Console.WriteLine(workingString);
            return workingString;
        }

        internal static string Explode(string input)
        {
            //If any pair is nested inside four pairs, the leftmost such pair explodes.
            //To explode a pair, the pair's left value is added to the first regular number to the left of the exploding pair (if any),
            //and the pair's right value is added to the first regular number to the right of the exploding pair(if any).
            //Exploding pairs will always consist of two regular numbers. Then, the entire exploding pair is replaced with the regular number 0.

            // First, look to see if there's anything to explode
            int level = 0;
            int startBracketLocation = 0;
            for (startBracketLocation=0; startBracketLocation < input.Length; startBracketLocation++)
            {
                if (input[startBracketLocation] == '[')
                {
                    if (level ==4 )
                    {
                        // Explode
                        break;
                    }
                    else
                    {
                        level++;
                    }
                }
                else if (input[startBracketLocation] == ']')
                {
                    level--;
                }
            }
            if (startBracketLocation != input.Length)
            {
                // We found something to explode
                // location is the open bracket of the exploder
                // look for a digit to the left of it
                int leftNumberEnd = startBracketLocation;
                int leftNumberStart = startBracketLocation;
                bool leftNumberFound = false;
                int outerLeftNumber = 0;
                while (leftNumberEnd >=0)
                {
                    if (input[leftNumberEnd] >= '0' && input[leftNumberEnd] <= '9')
                    {
                        leftNumberFound = true;
                        leftNumberStart = leftNumberEnd-1;
                        // We've got a number. Find the start of that number
                        while (leftNumberStart >= 0 && input[leftNumberStart] >= '0' && input[leftNumberStart] <= '9')
                            leftNumberStart--;
                        leftNumberStart++;
                        outerLeftNumber = int.Parse(input.Substring(leftNumberStart, leftNumberEnd - leftNumberStart+1));
                        break;

                    }
                    leftNumberEnd--;

                }

                int rightNumberStart = startBracketLocation;
                bool rightNumberFound = false;
                int rightNumberEnd = startBracketLocation;
                int outerRightNumber = 0;
                while (rightNumberStart < input.Length)
                {
                    rightNumberStart++;
                    // look for a ']' first
                    if (input[rightNumberStart] ==']')
                    {
                        break;
                    }
                }
                while (rightNumberStart < input.Length)
                {
                    if (input[rightNumberStart] >= '0' && input[rightNumberStart] <= '9')
                    {
                        rightNumberFound = true;
                        rightNumberEnd = rightNumberStart+1;
                        // We've got a number. Find the end of that number
                        while (rightNumberEnd < input.Length && input[rightNumberEnd] >= '0' && input[rightNumberEnd] <= '9')
                        {
                            rightNumberEnd++;
                        }
                        rightNumberEnd--;
                        outerRightNumber = int.Parse(input.Substring(rightNumberStart, rightNumberEnd - rightNumberStart+1));
                        break;
                    }
                    rightNumberStart++;
                }

                // Now we know what we're exploding into, we need to know what's exploding
                int location = startBracketLocation + 1;
                while (location < input.Length && input[location] >= '0' && input[location] <='9')
                {
                    location++;
                }
                int firstNumber = int.Parse(input.Substring(startBracketLocation + 1, location - startBracketLocation-1));
                while (location < input.Length && (input[location] < '0' || input[location] > '9'))
                {
                    location++;
                }
                int secondNumberStart = location;
                while (location < input.Length && input[location] >= '0' && input[location] <= '9')
                {
                    location++;
                }
                int secondNumber = int.Parse(input.Substring(secondNumberStart, location - secondNumberStart));
                int endBracketLocation = location;

                // I think we've finally got all the information we need.

                StringBuilder newStringBuilder = new StringBuilder();
                if (leftNumberFound)
                {
                    string outerLeftString = input.Substring(0, leftNumberStart);
                    string newFirstNumber = (outerLeftNumber + firstNumber).ToString();
                    string innerLeftString = input.Substring(leftNumberEnd + 1, startBracketLocation -leftNumberEnd -1);
                    newStringBuilder.Append(outerLeftString);
                    newStringBuilder.Append(newFirstNumber);
                    newStringBuilder.Append(innerLeftString);
                }
                else
                {
                    string leftString = input.Substring(0, startBracketLocation);
                    newStringBuilder.Append(leftString);
                }
                newStringBuilder.Append("0");
                
                if (rightNumberFound)
                {

                    string innerRightString = input.Substring(endBracketLocation + 1, rightNumberStart - endBracketLocation-1);
                    string newSecondNumber = (outerRightNumber + secondNumber).ToString();
                    string outerRightString = input.Substring(rightNumberEnd + 1);
                    newStringBuilder.Append(innerRightString);
                    newStringBuilder.Append(newSecondNumber);
                    newStringBuilder.Append(outerRightString);
                }
                else
                {
                    string rightString = input.Substring(endBracketLocation + 1);
                    newStringBuilder.Append(rightString);
                }

                return newStringBuilder.ToString();


            }
            return input;
        }
        internal static string Split(string input)
        {
            // If any regular number is 10 or greater, the leftmost such regular number splits.
            // To split a regular number, replace it with a pair;
            // the left element of the pair should be the regular number divided by two and rounded down,
            // while the right element of the pair should be the regular number divided by two and rounded up

            // numbers greater than 10 have at least two digits next to each other
            int startLocation = 0;
            while (startLocation < input.Length - 1)
            {
                if (input[startLocation] >= '0' && input[startLocation] <= '9' && input[startLocation + 1] >= '0' && input[startLocation+1] <= '9')
                {
                    // Got one.
                    break;
                }
                startLocation++;
            }

            if (startLocation != input.Length-1)
            {
                // We found something to split
                // Find the end of the number
                int endLocation = startLocation+1;
                while (endLocation < input.Length)
                {
                    if (input[endLocation] < '0' || input[endLocation] >'9')
                    {
                        break;
                    }
                    endLocation++;
                }
                endLocation--;

                int number = int.Parse(input.Substring(startLocation, endLocation - startLocation+1));
                StringBuilder newStringBuilder = new StringBuilder();
                string pre = input.Substring(0, startLocation);
                newStringBuilder.Append(pre);
                newStringBuilder.Append("[");
                newStringBuilder.Append(number / 2);
                newStringBuilder.Append(",");
                newStringBuilder.Append(number / 2 + number % 2);
                newStringBuilder.Append(']');
                string post = input.Substring(endLocation+1);
                newStringBuilder.Append(post);
                return newStringBuilder.ToString();
            }

            return input;


        }

        internal static string Add(string v1, string v2)
        {
            return "["+v1+","+v2+"]";
        }

        internal static int Magnitude(string input)
        {
            // is this a bracket or a number
            if (input[0] == '[')

            {
                // got a tree
                // find the comma in the middle
                int level = 0;
                int position = 0;
                while (!(level == 1 && input[position] ==','))
                {
                    if (input[position] == '[')
                    {
                        level++;
                    }
                    else if (input[position] == ']')
                    {
                        level--;
                    }
                    position++;
                }
                string firstString = input.Substring(1, position - 1);
                string secondString = input.Substring(position + 1, input.Length-position-2);
                return Magnitude(firstString)*3 + Magnitude(secondString)*2;
            }
            else
            {
                // it's a number
                return int.Parse(input);
            }
            throw new NotImplementedException();
        }
    }
}
