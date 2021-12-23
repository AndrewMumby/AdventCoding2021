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

        public static string B(string input)
        {
            AmphSorter sorter = new AmphSorter(input);
            sorter.AddFold();
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
            lowestScores = new Dictionary<AmphState, int>();
            AmphState startState = new AmphState(input);
            toDo.Add(0, new HashSet<AmphState> { startState });
        }

        public int FindCheapest()
        {
            while (true)
            {
                int lowestScore = toDo.Keys.Min();
                HashSet<AmphState> amphStates = toDo[lowestScore];
                Console.WriteLine(lowestScore + " " + amphStates.Count);
                toDo.Remove(lowestScore);
                foreach (AmphState amphState in amphStates)
                {
                    if (amphState.IsSolved())
                    {
                        return lowestScore;
                    }
                    // find all the states
                    Dictionary<int, HashSet<AmphState>> newStates = amphState.FindAllNewStates();
                    //Console.Clear();
                    //Console.WriteLine(amphState.Draw());

                    //Console.WriteLine("PRODUCES");

                    foreach (KeyValuePair<int, HashSet<AmphState>> scoreStatePair in newStates)
                    {
                        AddToToDo(lowestScore + scoreStatePair.Key, scoreStatePair.Value);
                    }
                }
            }
        }

        internal void AddFold()
        {
            AmphState startState = toDo[0].First();
            toDo.Remove(0);
            toDo.Add(0, new HashSet<AmphState>());
            AmphState newState = new AmphState(startState);
            newState.Unfold();
            toDo[0].Add(newState);
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

                //Console.WriteLine(state.Draw());
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

        private void AddToToDo(int score, HashSet<AmphState> states)
        {
            foreach (AmphState state in states)
            {
                AddToToDo(score, state);
            }
        }

    }

    internal class AmphState
    {
        Dictionary<IntVector2, char> amphs;
        static int maxY;
        static readonly HashSet<int> corridorXPoints = new HashSet<int>(){ 1, 2, 4, 6, 8, 10, 11};
        static readonly IntVector2[] corridorStoppingPoints = new IntVector2[] { new IntVector2(1, 1), new IntVector2(2, 1), new IntVector2(4, 1), new IntVector2(6, 1), new IntVector2(8, 1), new IntVector2(10, 1), new IntVector2(11, 1) };

        public AmphState(string input)
        {
            /*  0123456789012
                #############
                #...........#
                ###B#C#B#D###
                  #D#C#B#A#
                  #D#B#A#C#
                  #A#D#C#A#
                  #########
            */
            amphs = new Dictionary<IntVector2, char>();

            string[] lines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            for (int y = 0; y < lines.Length; y++)
            {
                for (int x = 0; x < lines[y].Length; x++)
                {
                    if (new char[] { 'A', 'B', 'C', 'D' }.Contains(lines[y][x]))
                    {
                        amphs.Add(new IntVector2(x, y), lines[y][x]);
                        maxY = Math.Max(maxY, y);
                    }
                }
            }
        }

        public AmphState(AmphState oldState)
        {
            amphs = new Dictionary<IntVector2, char>();
            foreach (KeyValuePair<IntVector2, char> pair in oldState.amphs)
            {
                amphs.Add(new IntVector2(pair.Key), pair.Value);
            }
        }

        private bool IsEmpty(IntVector2 location)
        {
            return !amphs.ContainsKey(location);
        }

        internal Dictionary<int, HashSet<AmphState>> FindAllNewStates()
        {
            Dictionary<int, HashSet<AmphState>> newStates = new Dictionary<int, HashSet<AmphState>>();
            // for each amph, make a new state for each location it can move to, and add it and the cost to the list
            foreach (KeyValuePair<IntVector2, char> pair in amphs)
            {
                IntVector2 location = pair.Key;
                char type = pair.Value;
                // Is it in the corridor?
                if (IsCorridor(location))
                {
                    // in a corridor
                    // Can only move into the right burrow
                    // Is the burrow ready for entry?
                    // Needs to be only appropriate amphs in there
                    if (!CleanBurrow(pair.Value))
                    {
                        // not clean - can't do anything with this amph
                        continue;
                    }

                    // Clean burrow.
                    // Can we move to that burrow?
                    IntVector2 newLocation = BurrowTopSlot(type);
                    if (!CleanPath(location, newLocation))
                    {
                        // Something's in the way - can't do anything with this amph
                        continue;
                    }
                    // Create a state where we move to the burrow
                    AmphState newState = new AmphState(this);
                    newState.ForceMove(location, newLocation);
                    int cost = location.Distance(newLocation) * MoveCost(type);
                    if (newStates.ContainsKey(cost))
                    {
                        newStates[cost].Add(newState);
                    }
                    else
                    {
                        newStates.Add(cost, new HashSet<AmphState> { newState });
                    }
                }
                else
                {
                    // in a burrow
                    // Can move to any available corridor slot
                    // Are we already home?
                    if (IsHome(location))
                    {
                        continue;
                    }
                    // Do we have a clean path out of the burrow?
                    if (!CleanPath(location, new IntVector2(location.X, 1)))
                    {
                        // Blocked
                        continue;
                    }
                    if (!IsEmpty(new IntVector2(location.X, 1)))
                    {
                        // Blocked
                        continue;
                    }

                    // Now we go left and right to each stopping point

                    // left
                    int xPos = location.X;
                    while (xPos >=1)
                    {
                        xPos--;
                        if (!IsEmpty(new IntVector2(xPos,1)))
                        { 
                            break;
                        }
                        if ( corridorXPoints.Contains(xPos))
                        {
                            IntVector2 destination = new IntVector2(xPos, 1);
                            AmphState newState = new AmphState(this);
                            newState.ForceMove(location, destination);
                            int cost = location.Distance(destination) * MoveCost(type);
                            if (newStates.ContainsKey(cost))
                            {
                                newStates[cost].Add(newState);
                            }
                            else
                            {
                                newStates.Add(cost, new HashSet<AmphState> { newState });
                            }
                        }
                    }

                    // right
                    xPos = location.X;
                    while (xPos <=11)
                    {
                        xPos++;
                        if (!IsEmpty(new IntVector2(xPos, 1)))
                        {
                            break;
                        }
                        if (corridorXPoints.Contains(xPos))
                        {
                            IntVector2 destination = new IntVector2(xPos, 1);
                            AmphState newState = new AmphState(this);
                            newState.ForceMove(location, destination);
                            int cost = location.Distance(destination) * MoveCost(type);
                            if (newStates.ContainsKey(cost))
                            {
                                newStates[cost].Add(newState);
                            }
                            else
                            {
                                newStates.Add(cost, new HashSet<AmphState> { newState });
                            }
                        }
                    }

                    /*
                    foreach (IntVector2 destination in corridorStoppingPoints)
                    {
                        if (IsEmpty(destination) && CleanPath(location, destination))
                        {
                            AmphState newState = new AmphState(this);
                            newState.ForceMove(location, destination);
                            int cost = location.Distance(destination) * MoveCost(type);
                            if (newStates.ContainsKey(cost))
                            {
                                newStates[cost].Add(newState);
                            }
                            else
                            {
                                newStates.Add(cost, new HashSet<AmphState> { newState });
                            }
                        }
                    }
                    */

                }
            }
            return newStates;
        }

        private bool IsHome(IntVector2 location)
        {
            // Home if we're in the right burrow, and there's noone below us that shouldn't be in this burrow
            if (BurrowLocation(amphs[location]) != location.X)
            {
                return false;
            }
            if (CleanBurrow(amphs[location]))
            {
                return true;
            }
            return false;
        }

        private bool CleanPath(IntVector2 location, IntVector2 newLocation)
        {
            // move from the lower point to the higher one, for simplicity
            if (location.Y > newLocation.Y)
            {
                IntVector2 wl = location;
                // Move up till at the right level
                while (wl.Y != newLocation.Y)
                {
                    wl = wl.North();
                    if (!IsEmpty(wl))
                    {
                        return false;
                    }
                }

                // Are we left or right?
                if (wl.X > newLocation.X)
                {
                    // Need to move left
                    while (wl.X != newLocation.X)
                    {
                        // opposite way around so we don't test the destination
                        if (!IsEmpty(wl))
                        {
                            return false;
                        }
                        wl = wl.West();
                    }
                }
                else
                {
                    // Need to move right
                    while (wl.X != newLocation.X)
                    {
                        // opposite way around so we don't test the destination
                        if (!IsEmpty(wl))
                        {
                            return false;
                        }
                        wl = wl.East();
                    }

                }
                // We made it! 
                return true;
            }
            else
            {
                return CleanPath(newLocation, location);
            }
        }

        private static int MoveCost(char type)
        {
            int exponent = type - 'A';
            int value = 1;
            while (exponent > 0)
            {
                value *= 10;
                exponent--;
            }
            return value;
        }

        private IntVector2 BurrowTopSlot(char type)
        {
            int x = BurrowLocation(type);
            for (int y = maxY; y >= 2; y--)
            {
                IntVector2 location = new IntVector2(x, y);
                if (IsEmpty(location))
                {
                    return location;
                }
            }
            Console.WriteLine(Draw());
            throw new Exception("Burrow is full");
        }

        private static bool IsCorridor(IntVector2 location)
        {
            return location.Y == 1;
        }

        private static int BurrowLocation(char c)
        {
            return 3 + ((c - 'A') * 2);
        }

        private bool CleanBurrow(char c)
        {
            int x = BurrowLocation(c);
            for (int y = 2; y <= 5; y++)
            {
                IntVector2 location = new IntVector2(x, y);
                if (amphs.ContainsKey(location) && amphs[location] != c)
                {
                    return false;
                }
            }
            return true;
        }

        private void ForceMove(IntVector2 start, IntVector2 end)
        {
            Dictionary<IntVector2, char> newAmphs = new Dictionary<IntVector2, char>();
            foreach (KeyValuePair<IntVector2, char> pair in amphs)
            {
                if (pair.Key.Equals(start))
                {
                    newAmphs.Add(end, pair.Value);
                }
                else
                {
                    newAmphs.Add(pair.Key, pair.Value);
                }
            }
            amphs = newAmphs;
        }

        internal bool IsSolved()
        {
            // All the burrows need to have their appropriate critters in
            int count = 0;
            foreach (char c in new char[] { 'A', 'B', 'C', 'D' })
            {
                int x = BurrowLocation(c);
                int y = 2;
                while (true)
                {
                    IntVector2 location = new IntVector2(x, y);
                    if (amphs.ContainsKey(location) && amphs[location] == c)
                    {
                        count++;
                    }
                    else
                    {
                        break;
                    }
                    y++;
                }
            }
            return count == amphs.Count;
        }

        internal string Draw()
        {
            StringBuilder sb = new StringBuilder();
            /*  0123456789012
                #############
                #...........#
                ###B#C#B#D###
                  #D#C#B#A#
                  #D#B#A#C#
                  #A#D#C#A#
                  #########
            */
            sb.AppendLine("#############");
            sb.Append('#');
            for (int x = 1; x <= 11; x++)
            {
                IntVector2 location = new IntVector2(x, 1);
                if (amphs.ContainsKey(location))
                {
                    sb.Append(amphs[location]);
                }
                else
                {
                    sb.Append('.');
                }
            }
            sb.AppendLine("#");
            for (int y = 2; y <= maxY; y++)
            {
                if (y == 2)
                {
                    sb.Append("###");
                }
                else
                {
                    sb.Append("  #");
                }
                sb.Append(AmphOrBlank(new IntVector2(3, y)));
                sb.Append('#');
                sb.Append(AmphOrBlank(new IntVector2(5, y)));
                sb.Append('#');
                sb.Append(AmphOrBlank(new IntVector2(7, y)));
                sb.Append('#');
                sb.Append(AmphOrBlank(new IntVector2(9, y)));
                if (y == 2)
                {
                    sb.AppendLine("###");
                }
                else
                {
                    sb.AppendLine("#");
                }
            }
            sb.AppendLine("  #########");

            return sb.ToString();


        }

        private char AmphOrBlank(IntVector2 location)
        {
            if (amphs.ContainsKey(location))
            {
                return amphs[location];
            }
            else
            {
                return '.';
            }

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
                foreach (KeyValuePair<IntVector2, char> pair in v.amphs)
                {
                    if (!amphs.ContainsKey(pair.Key))
                    {
                        return false;
                    }
                    if (amphs[pair.Key] != pair.Value)
                    {
                        return false;
                    }
                }
                    return true;
            }
        }

        public override int GetHashCode()
        {
            int hash = 0;
            foreach (IntVector2 v in amphs.Keys)
            {
                hash = hash ^ v.GetHashCode();
            }
            return hash;
        }

        internal void Unfold()
        {
            Dictionary<IntVector2, char> newAmphs = new Dictionary<IntVector2, char>();
            foreach (KeyValuePair<IntVector2, char> pair in amphs)
            {
                if (pair.Key.Y == 3)
                {
                    newAmphs.Add(new IntVector2(pair.Key.X, 5), pair.Value);
                }
                else
                {
                    newAmphs.Add(new IntVector2(pair.Key), pair.Value);
                }
            }
            maxY = 5;
            // Now add the new rows
            //  3 5 7 9
            //3#D#C#B#A#
            //4#D#B#A#C#

            newAmphs.Add(new IntVector2(3, 3), 'D');
            newAmphs.Add(new IntVector2(5, 3), 'C');
            newAmphs.Add(new IntVector2(7, 3), 'B');
            newAmphs.Add(new IntVector2(9, 3), 'A');
            newAmphs.Add(new IntVector2(3, 4), 'D');
            newAmphs.Add(new IntVector2(5, 4), 'B');
            newAmphs.Add(new IntVector2(7, 4), 'A');
            newAmphs.Add(new IntVector2(9, 4), 'C');
            amphs = newAmphs;
        }
    }
}
