using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCoding2021
{
    internal class Day23
    {
        public static string A(string input)
        {
            AmphSorter sorter = new AmphSorter(input);
            return sorter.FindCheapest().ToString();
        }
    }

    internal class AmphSorter
    {
        SortedDictionary<int, HashSet<AmphState>> toDo;
        Dictionary<AmphState, int> lowestScores;

        public AmphSorter(string input)
        {
            toDo = new SortedDictionary<int, HashSet<AmphState>>();
            lowestScores = new Dictionary<AmphState,int>();
            AmphState startState = new AmphState(input);
            toDo.Add(0, new HashSet<AmphState> { startState });
        }

        public int FindCheapest()
        {
            while (true)
            {
                int lowestScore = toDo.Keys.Min();
                int prevScore = 0;
                HashSet<AmphState> amphStates = toDo[lowestScore];
                //if (lowestScore % 100 == 0)
                {
                    //Console.WriteLine();
                   // Console.WriteLine("Working on lowestScore = " + lowestScore +" Size = " + amphStates.Count + " Done = " + done.Count);
                    //Console.WriteLine();
                }

                toDo.Remove(lowestScore);
                foreach (AmphState amphState in amphStates)
                {
                    amphState.DebugWatch(lowestScore);
                    if (amphState.IsSolved())
                    {
                        return lowestScore;
                    }
                    // find all the states
                    for (int i = 0; i < AmphState.slotCount; i++)
                    {
                        if (!amphState.IsEmpty(i))
                        {
                            // It's a amph
                            // Where can it go?
                            // either its in the corridor, or its in a burrow
                            if (AmphState.InCorridor(i))
                            {
                                // in the corridor
                                // we can only move into our assigned burrow, into the lowest slot
                                // 1) burrow needs to be empty
                                if (!amphState.BurrowReady(i))
                                {
                                    continue;
                                }
                                // 2) need to have a clear path to the burrow
                                if (!amphState.CanPathToBurrow(i))
                                {
                                    continue;
                                }

                                // If we're here, we can go to the burrow

                                int cost = amphState.MoveToBurrow(i, out AmphState newState);
                                AddToToDo(lowestScore + cost, newState);
                            }
                            else
                            {
                                // in a burrow
                                // We can move to any empty corridor slot we can reach
                                // 1) burrow slot above us needs to be empty
                                //  a) We're on the top slot
                                //  b) We're in the bottom slot and the top slot is empty
                                if (amphState.IsBottomSlot(i) && !amphState.IsEmptyAbove(i))
                                {
                                    continue;
                                }

                                // 2) We're not already home and not blocking an intruder
                                if (amphState.IsHome(i) && amphState.IsTopSlot(i) && amphState.IsHome(i+1))
                                {
                                    continue;
                                }

                                // iterate out each way, working out the cost, until we hit walls. Add all the possibilities to the todo list
                                // first, left
                                int pos = i;
                                while (true)
                                {
                                    pos = AmphState.LeftCorridor(pos);
                                    if (pos == -1 || !amphState.IsEmpty(pos))
                                    {
                                        break;
                                    }
                                    // Add the move to this location to the todo list
                                    int cost = amphState.MoveToCorridor(i, pos, out AmphState newState);
                                    AddToToDo(lowestScore + cost, newState);
                                }
                                // then right
                                pos = i;
                                while (true)
                                {
                                    pos = AmphState.RightCorridor(pos);
                                    if (pos == -1 || !amphState.IsEmpty(pos))
                                    {
                                        break;
                                    }
                                    // Add the move to this location to the todo list
                                    int cost = amphState.MoveToCorridor(i, pos, out AmphState newState);
                                    AddToToDo(lowestScore + cost, newState);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void AddToToDo(int score, AmphState state)
        {
            if (!lowestScores.ContainsKey(state) || lowestScores[state] > score)
            {
                if (!lowestScores.ContainsKey(state))
                {
                    lowestScores.Add(state, score);
                }
                else
                {
                    lowestScores[state] = score;
                }

                // Console.WriteLine("Adding " + state.ToString() + " at cost " + score);
                if (!toDo.ContainsKey(score))
                {
                    toDo.Add(score, new HashSet<AmphState> { state });
                }
                else
                {
                    toDo[score].Add(state);
                }
            }
        }

    }

    internal class AmphState
    {
        // field has 2 side corridors, 3 spots above burrows and then another 2 side corridors. There are 4 burrows, each with 2 slots top then bottom
        // ssbbtbbtbbtbbss
        // ..
        string layout;

        private static readonly int[] corridorSlots = new int[] { 0, 1, 4, 7, 10, 13, 14 };
        private static readonly int[] topSlots = new int[] { 2, 5, 8, 11 };
        private static readonly int[] bottomSlots = new int[] { 3, 6, 9, 12 };
        internal static readonly int slotCount = 15;
        private static readonly int[] costs = new int[] { 1, 10, 100, 1000 };


        public AmphState(string input)
        {
            string[] lines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length == 1)
            {
                layout = input;
            }
            else
            {
                /*
                    0123456789012
                  0 #############
                  1 #...........#
                  2 ###B#C#B#D###
                  3   #A#D#C#A#
                  4   #########
                */
                // 012345678901234
                // ssbbtbbtbbtbbss

                StringBuilder sb = new StringBuilder();
                sb.Append(lines[1][1]);
                sb.Append(lines[1][2]);
                sb.Append(lines[2][3]);
                sb.Append(lines[3][3]);
                sb.Append(lines[1][4]);
                sb.Append(lines[2][5]);
                sb.Append(lines[3][5]);
                sb.Append(lines[1][6]);
                sb.Append(lines[2][7]);
                sb.Append(lines[3][7]);
                sb.Append(lines[1][8]);
                sb.Append(lines[2][9]);
                sb.Append(lines[3][9]);
                sb.Append(lines[1][10]);
                sb.Append(lines[1][11]);

                layout = sb.ToString();
            }
        }

        public static bool InCorridor(int i)
        {
            return corridorSlots.Contains(i);
        }

        private int AmphNo(char amph)
        {
            return amph - 'A';
        }

        private int BurrowTopLocation(char amph)
        {
            return topSlots[AmphNo(amph)];
        }

        private int BurrowBottomLocation(char amph)
        {
            return bottomSlots[AmphNo(amph)];
        }

        public bool BurrowReady(int pos)
        {
            int burrowTopNo = BurrowTopLocation(layout[pos]);
            int burrowBottomNo = BurrowBottomLocation(layout[pos]);

            if (layout[burrowTopNo] != '.')
            {
                return false;
            }
            if (layout[burrowBottomNo] == layout[pos] || layout[burrowBottomNo] == '.')
            {
                return true;
            }
            return false;
        }

        public override bool Equals(object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                AmphState v = (AmphState)obj;
                return v.layout == layout;
            }
        }

        public override int GetHashCode()
        {
            return layout.GetHashCode();
        }

        public string Draw()
        {
            StringBuilder sb = new StringBuilder();
            /*
                0123456789012
                #############
                #...........#
                ###B#C#B#D###
                  #A#D#C#A#
                  #########
            */
            // 012345678901234
            // ssbbtbbtbbtbbss
            sb.AppendLine("#############");

            sb.Append("#");
            sb.Append(layout.Substring(0, 2));
            sb.Append('.');
            sb.Append(layout[4]);
            sb.Append('.');
            sb.Append(layout[7]);
            sb.Append('.');
            sb.Append(layout[10]);
            sb.Append('.');
            sb.Append(layout.Substring(13, 2));
            sb.AppendLine("#");

            sb.Append("###");
            sb.Append(layout[2]);
            sb.Append('#');
            sb.Append(layout[5]);
            sb.Append('#');
            sb.Append(layout[8]);
            sb.Append('#');
            sb.Append(layout[11]);
            sb.AppendLine("###");

            sb.Append("  #");
            sb.Append(layout[3]);
            sb.Append('#');
            sb.Append(layout[6]);
            sb.Append('#');
            sb.Append(layout[9]);
            sb.Append('#');
            sb.Append(layout[12]);
            sb.AppendLine("#");

            sb.AppendLine("  #########");

            return sb.ToString();
        }

        public override string ToString()
        {
            return layout;
        }

        internal bool IsBottomSlot(int i)
        {
            return bottomSlots.Contains(i);
        }

        internal bool IsTopSlot(int i)
        {
            return topSlots.Contains(i);
        }

        internal bool IsEmptyAbove(int i)
        {
            if (!bottomSlots.Contains(i))
            {
                throw new Exception("Can't be empty above if not in a bottom slot");
            }
            return IsEmpty(i - 1);
        }

        internal bool IsHome(int i)
        {
            char c = layout[i];
            return i == BurrowBottomLocation(c) || i == BurrowTopLocation(c);
        }

        internal bool IsEmpty(int i)
        {
            return layout[i] == '.';
        }

        internal static int LeftCorridor(int i)
        {
            i--;
            while (i >= 0)
            {
                if (corridorSlots.Contains(i))
                {
                    return i;
                }
                i--;
            }
            return -1;
        }
        internal static int RightCorridor(int i)
        {
            i++;
            while (i < 15)
            {
                if (corridorSlots.Contains(i))
                {
                    return i;
                }
                i++;
            }
            return -1;
        }

        internal bool CanPathToBurrow(int i)
        {
            int burrowLoc = BurrowTopLocation(layout[i]);
            if (burrowLoc < i)
            {
                // need to move left
                // find our spot in the corridor array
                int corridorSpot = Array.IndexOf(corridorSlots, i);
                while (true)
                {
                    corridorSpot--;
                    if (corridorSlots[corridorSpot] < burrowLoc)
                    {
                        return true;
                    }
                    if (!IsEmpty(corridorSlots[corridorSpot]))
                    {
                        return false;
                    }
                }
            }
            else
            {
                // need to move right
                // find our spot in the corridor array
                int corridorSpot = Array.IndexOf(corridorSlots, i);
                while (true)
                {
                    corridorSpot++;
                    if (corridorSlots[corridorSpot] > burrowLoc)
                    {
                        return true;
                    }
                    if (!IsEmpty(corridorSlots[corridorSpot]))
                    {
                        return false;
                    }
                }

            }
        }

        internal int MoveToBurrow(int i, out AmphState newState)
        {
            int moves;
            char[] newStateArray = layout.ToCharArray();
            newStateArray[i] = '.';
            if (IsEmpty(BurrowBottomLocation(layout[i])))
            {
                newStateArray[BurrowBottomLocation(layout[i])] = layout[i];
            }
            else
            {
                newStateArray[BurrowTopLocation(layout[i])] = layout[i];
            }
            newState = new AmphState(new string(newStateArray));

            // need to work out the move count
            if (IsEmpty(BurrowBottomLocation(layout[i])))
            {
                moves = Distance(i, BurrowBottomLocation(layout[i]));
            }
            else
            {
                moves = Distance(i, BurrowTopLocation(layout[i]));
            }

            return moves * costs[AmphNo(layout[i])];
        }

        internal int MoveToCorridor(int i, int pos, out AmphState newState)
        {
            int moves;
            char[] newStateArray = layout.ToCharArray();
            newStateArray[i] = '.';
            newStateArray[pos] = layout[i];
            newState = new AmphState(new string(newStateArray));

            moves = Distance(i, pos);
            return moves * costs[AmphNo(layout[i])];
        }

        internal static int Distance(int start, int end)
        {

            if (!AmphState.InCorridor(start))
            {
                int distance = 0;
                // We're going from a burrow to a corridor
                if (bottomSlots.Contains(start))
                {
                    distance++;
                }
                distance++;
                // Now we're on the corridor level
                // Is the end to the left or the right?
                if (end < start)
                {
                    // going left
                    int i = start;
                    while (!InCorridor(i))
                    {
                        i--;
                    }
                    distance++;
                    while (i != end)
                    {
                        if (i == 1 || i == 14)
                        {
                            // far ends are only 1 square movement
                            distance--;
                        }
                        distance += 2;
                        i = LeftCorridor(i);
                    }
                }
                else
                {
                    // going right
                    int i = start;
                    while (!InCorridor(i))
                    {
                        i++;
                    }
                    distance++;
                    while (i != end)
                    {
                        if (i == 0 || i == 13)
                        {
                            // far ends are only 1 square movement
                            distance--;
                        }
                        distance += 2;
                        i = RightCorridor(i);
                    }

                }
                return distance;
            }
            else
            {
                // Distances are symmetrical so we can just find the other direction
                return Distance(end, start);
            }
        }



        internal bool IsSolved()
        {
            return layout.Equals("..AA.BB.CC.DD..");
        }

        internal void DebugWatch(int score)
        {
            /*
            if (new string[] {"..BA.CD.BC.DA..",
"..BABCD..C.DA..",
"..BAB.D.CC.DA..",
"..BAB..DCC.DA..",
"..BA..BDCC.DA..",
"...AB.BDCC.DA..",
"...A.BBDCC.DA..",
"...A.BBDCCD.A..",
"...A.BBDCCD..A.",
"...A.BBDCC..DA.",
"...A.BB.CC.DDA.",
"..AA.BB.CC.DD.." }.Contains(layout))
            {
                Console.WriteLine(layout + " " + score);
            }

            if (layout == "...AB.BDCC.DA..")
            {
                Console.WriteLine("This one");
            }
            */

        }

        internal int AmphCount()
        {
            int count = 0;
            for (int i = 0; i < slotCount; i++)
            {
                if (layout[i] != '.')
                {
                    count++;
                }
            }
            return count;
        }
    }
}
