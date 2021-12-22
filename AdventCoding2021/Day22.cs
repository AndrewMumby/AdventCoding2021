using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCoding2021
{
    internal class Day22
    {
        public static string A(string input)
        {
            string[] lines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            List<RebootStep> steps = new List<RebootStep>();
            foreach (string line in lines)
            {
                steps.Add(new RebootStep(line));
            }

            long count = 0;
            for (int x = -50; x <= 50; x++)
            {
                for (int y = -50; y <= 50; y++)
                {
                    for (int z = -50; z <= 50; z++)
                    {
                        IntVector3 point = new IntVector3(x, y, z);
                        int set = 0;
                        foreach (RebootStep step in steps)
                        {
                            int newSet = step.IsSet(point);
                            if (newSet >= 0)
                            {
                                set = newSet;
                            }
                        }
                        if (set == 1)
                        {
                            count++;
                        }
                    }
                }
            }


            return count.ToString();
        }

        public static string B(string input)
        {
            Reactor reactor = new Reactor(input);
            return reactor.CountCubes().ToString();
        }


        internal class Reactor
        {
            List<RebootStep> steps;
            public Reactor(string input)
            {
                steps = new List<RebootStep>();
                foreach (string line in input.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
                {
                    steps.Add(new RebootStep(line));
                }
            }

            internal long CountCubes()
            {
                // Work out the zones on the x axis
                List<int> xBounds = new List<int>();
                foreach (RebootStep step in steps)
                {
                    xBounds.AddRange(TripleBound(step.MinX()));
                    xBounds.AddRange(TripleBound(step.MaxX()));
                }

                xBounds = xBounds.Distinct().ToList();
                xBounds.Sort();

                List<Task<long>> taskList = new List<Task<long>>();
                for (int i = 0; i < xBounds.Count - 1; i++)
                {
                    taskList.Add(StartSliceTask(xBounds[i], xBounds[i + 1] - xBounds[i]));
                }

                foreach (Task<long> task in taskList)
                {
                    task.Start();
                }

                Task.WaitAll(taskList.ToArray());

                long sum = 0;
                foreach (Task<long> task in taskList)
                {
                    sum += task.Result;
                }

                return sum;
            }

            private Task<long> StartSliceTask(int xCoord, int width)
            {

                return new Task<long>(() => { return SliceTotal(xCoord, width); });
            }

            private long SliceTotal(int xCoord, int width)
            {
                // Find all the y bounds at this x level
                List<int> yBounds = new List<int>();
                foreach (RebootStep step in steps)
                {
                    if (step.ExistsAtX(xCoord))
                    {
                        yBounds.AddRange(TripleBound(step.MinY()));
                        yBounds.AddRange(TripleBound(step.MaxY()));
                    }
                }
                yBounds = yBounds.Distinct().ToList();
                yBounds.Sort();

                long sum = 0;
                for (int i = 0; i < yBounds.Count - 1; i++)
                {
                    sum += LineTotal(xCoord, yBounds[i]) * (yBounds[i + 1] - yBounds[i]);
                }
                return sum * width;
            }


            private long LineTotal(int xCoord, int yCoord)
            {
                //Find all the z bounds at this x,y coord
                List<int> zBounds = new List<int>();
                List<RebootStep> testingSteps = new List<RebootStep>();
                foreach (RebootStep step in steps)
                {
                    if (step.ExistsAtXY(xCoord, yCoord))
                    {
                        zBounds.AddRange(TripleBound(step.MinZ()));
                        zBounds.AddRange(TripleBound(step.MaxZ()));
                        testingSteps.Add(step);
                    }

                }
                zBounds = zBounds.Distinct().ToList();
                zBounds.Sort();

                long sum = 0;
                for (int i = 0; i < zBounds.Count - 1; i++)
                {
                    int distance = zBounds[i + 1] - zBounds[i];
                    int set = 0;
                    for (int j = 0; j < testingSteps.Count; j++)
                    {
                        int newSet = testingSteps[j].Contains(xCoord, yCoord, zBounds[i]);
                        if (newSet >= 0)
                        {
                            set = newSet;
                        }
                    }
                    sum = sum + (set * distance);
                }

                return sum;
            }

            private List<int> TripleBound(int x)
            {
                return new List<int> { x - 1, x, x + 1 };
            }
        }

    }

    internal class RebootStep
    {
        int set;
        IntCuboid volume;

        public RebootStep(string line)
        {
            // on x=10..12,y=10..12,z=10..12
            string[] parts = line.Split(new char[] { ' ', '=', '.', ',' }, StringSplitOptions.RemoveEmptyEntries);
            //  0 1  2   3 4  5   6 7  8   9
            // on x 10  12 y 10  12 z 10  12
            set = parts[0] == "on" ? 1 : 0;
            IntVector3 minCorner = new IntVector3(Math.Min(int.Parse(parts[2]), int.Parse(parts[3])), Math.Min(int.Parse(parts[5]), int.Parse(parts[6])), Math.Min(int.Parse(parts[8]), int.Parse(parts[9])));
            IntVector3 maxCorner = new IntVector3(Math.Max(int.Parse(parts[2]), int.Parse(parts[3])), Math.Max(int.Parse(parts[5]), int.Parse(parts[6])), Math.Max(int.Parse(parts[8]), int.Parse(parts[9])));
            volume = new IntCuboid(minCorner, maxCorner);
        }

        public long Volume()
        {
            if (set == 1)
            {
                return volume.Volume();
            }
            else
                return 0;

        }

        internal int IsSet(IntVector3 point)
        {
            if (volume.Contains(point))
            {
                return set;
            }
            return -1;
        }

        internal int Contains(int x, int y, int z)
        {
            if (volume.Contains(new IntVector3(x, y, z)))
            {
                return set;
            }
            else
            {
                return -1;
            }
        }

        internal int MinX()
        {
            return volume.MinX();
        }

        internal int MaxX()
        {
            return volume.MaxX();
        }

        internal int MinY()
        {
            return volume.MinY();
        }

        internal int MaxY()
        {
            return volume.MaxY();
        }

        internal int MinZ()
        {
            return volume.MinZ();
        }

        internal int MaxZ()
        {
            return volume.MaxZ();
        }

        internal bool ExistsAtX(int xCoord)
        {
            return (volume.MinX() <= xCoord && xCoord <= volume.MaxX());
        }

        internal bool ExistsAtXY(int xCoord, int yCoord)
        {
            return ExistsAtX(xCoord) && volume.MinY() <= yCoord && yCoord <= volume.MaxY();
        }
    }
}
