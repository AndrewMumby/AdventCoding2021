using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCoding2021
{
    internal class Day10
    {
        public static string A(string input)
        {
            string[] lines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            int score = 0;
            foreach (string line in lines)
            {
                score += ChunkParser.GetCorruptedScore(line);
            }
            return score.ToString();
        }

        public static string B(string input)
        {
            string[] lines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            List<long> scoreList = new List<long>();
            foreach (string line in lines)
            {
                long score = ChunkParser.GetIncompleteScore(line);
                if (score > 0)
                {
                    scoreList.Add(score);
                }
            }
            scoreList.Sort();
            return scoreList[scoreList.Count / 2].ToString();
        }
    }

    internal class ChunkParser
    {
        private static List<char> openers = new char[] { '(', '[', '{', '<' }.ToList();
        private static List<char> closers = new char[] { ')', ']', '}', '>' }.ToList();
        internal static int GetCorruptedScore(string line)
        {
            Stack<char> bracketStack = new Stack<char>();
            foreach (char c in line)
            {
                if (openers.Contains(c))
                {
                    bracketStack.Push(closers[openers.IndexOf(c)]);
                }
                else
                {
                    if (c != bracketStack.Pop())
                    {
                        /*
                        ): 3 points.
                        ]: 57 points.
                        }: 1197 points.
                        >: 25137 points.
                        */
                        switch (c)
                        {
                            case ')':
                                return 3;
                            case ']':
                                return 57;
                            case '}':
                                return 1197;
                            case '>':
                                return 25137;
                            default:
                                throw new Exception("Unknown character " + c);
                        }
                    }
                }
            }
            return 0;
        }

        internal static long GetIncompleteScore(string line)
        {
            Stack<char> bracketStack = new Stack<char>();
            foreach (char c in line)
            {

                if (openers.Contains(c))
                {
                    bracketStack.Push(closers[openers.IndexOf(c)]);
                }
                else
                {
                    if (c != bracketStack.Pop())
                    {
                        return 0;
                    }
                }
            }
            // bracketStack now contains the missing brackets. Produce the score
            long score = 0;
            while (bracketStack.Count > 0)
            {
                score *= 5;

                /*
                ): 1 point.
                ]: 2 points.
                }: 3 points.
                >: 4 points.
                */
                char c = bracketStack.Pop();
                switch (c)
                {
                    case ')':
                        score += 1;
                        break;
                    case ']':
                        score += 2;
                        break;
                    case '}':
                        score += 3;
                        break;
                    case '>':
                        score += 4;
                        break;
                    default:
                        throw new Exception("Unknown character " + c);
                }
            }
            return score;
        }
    }
}
