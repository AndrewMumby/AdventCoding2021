using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCoding2021
{
    internal class Day12
    {

        public static string A(string input)
        {
            Dictionary<string, PathCave> caves = ParseInput(input);

            // Now just find all the paths. Ez
            return CountPathsBasic(caves["start"], new HashSet<PathCave>()).ToString();

        }

        public static string B(string input)
        {
            Dictionary<string, PathCave> caves = ParseInput(input);

            return CountPathsAdvanced(caves["start"],new HashSet<PathCave>(), false).ToString();

        }

        public static int CountPathsBasic(PathCave currentLocation, HashSet<PathCave> visited)
        {
            if (currentLocation.IsEnd())
            {
                return 1;
            }
            int count = 0;
            HashSet<PathCave> newVisited = visited.ToHashSet();
            newVisited.Add(currentLocation);
            foreach (PathCave option in currentLocation.GetAllLinks())
            {
                if (visited.Contains(option) && !option.IsBig())
                {
                    //can't go that way
                    continue;
                }
                count += CountPathsBasic(option, newVisited);

            }
            return count;
        }

        public static int CountPathsAdvanced(PathCave currentLocation, HashSet<PathCave> visited, bool smallCaved)
        {
            if (currentLocation.IsEnd())
            {
                return 1;
            }
            int count = 0;
            HashSet<PathCave> newVisited = visited.ToHashSet();
            newVisited.Add(currentLocation);
            foreach (PathCave option in currentLocation.GetAllLinks())
            {
                if (option.IsStart())
                {
                    // can't go that way
                    continue;
                }
                bool newSmallCaved = smallCaved;
                if (visited.Contains(option) && !option.IsBig())
                {
                    if (smallCaved)
                    {
                        // can't go that way
                        continue;
                    }
                    else
                    {
                        // use the small cave token
                        newSmallCaved = true;
                    }
                }
                count += CountPathsAdvanced(option, newVisited,newSmallCaved);

            }
            return count;
        }


        public static Dictionary<string, PathCave> ParseInput(string input)
        {
            Dictionary<string, PathCave> caves = new Dictionary<string, PathCave>();
            // first create the caves
            foreach (string line in input.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
            {
                foreach (string part in line.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (!caves.ContainsKey(part))
                    {
                        caves.Add(part, new PathCave(part));
                    }
                }
            }
            // now add the links
            foreach (string line in input.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
            {
                string[] parts = line.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                caves[parts[0]].AddLink(caves[parts[1]]);
                caves[parts[1]].AddLink(caves[parts[0]]);
            }

            return caves;
        }

        internal class PathCave
        {
            string name;
            bool bigCave;
            List<PathCave> linkedCaves;

            public PathCave(string name)
            {
                this.name = name;
                bigCave = Char.IsUpper(name[0]);
                linkedCaves = new List<PathCave>();
            }

            public void AddLink(PathCave cave)
            {
                linkedCaves.Add(cave);
            }
            public List<PathCave> GetAllLinks()
            {
                return linkedCaves;
            }

            public bool IsBig()
            {
                return bigCave;
            }
            public override string ToString()
            {
                return name;
            }

            internal bool IsStart()
            {
                return name == "start";
            }

            internal bool IsEnd()
            {
                return name == "end";
            }
        }
    }
}
