using System;
using System.Collections.Generic;
using System.Linq;



namespace AdventCoding2021
{
    internal class IntCuboid
    {
        IntVector3 min;
        IntVector3 max;

        public IntCuboid(IntVector3 minCorner, IntVector3 maxCorner)
        {
            this.min = minCorner;
            this.max = maxCorner;
        }

        public IntCuboid(IntCuboid a)
        {
            this.min = new IntVector3(a.min);
            this.max = new IntVector3(a.max);
        }

        internal long Volume()
        {
            return (max.X - min.X + 1) * (max.Y - min.Y + 1) * (max.Z - min.Z + 1);
        }

        internal bool Contains(IntVector3 point)
        {
            return point.X >= min.X && point.X <= max.X && point.Y >= min.Y && point.Y <= max.Y && point.Z >= min.Z && point.Z <= max.Z;

        }

        internal static List<IntCuboid> GenerateIntersectionCuboids(IntCuboid a, IntCuboid b)
        {
            throw new NotImplementedException();
            if (!Intersects(a, b))
            {
                return new List<IntCuboid> { new IntCuboid(a), new IntCuboid(b) };
            }
            else
            {
                List<IntCuboid> list = new List<IntCuboid>();
                List<IntVector3> corners = new List<IntVector3>();
                corners.AddRange(a.Corners());
                corners.AddRange(b.Corners());
                List<int> xValues = new List<int>();
                List<int> yValues = new List<int>();
                List<int> zValues = new List<int>();
                foreach (IntVector3 corner in corners)
                {
                    xValues.Add(corner.X);
                    yValues.Add(corner.Y);
                    zValues.Add(corner.Z);
                }
                xValues.Sort();
                yValues.Sort();
                zValues.Sort();

                xValues = xValues.Distinct().ToList();
                yValues = yValues.Distinct().ToList();
                zValues = zValues.Distinct().ToList();

                for (int xIndex = 0; xIndex < xValues.Count - 1; xIndex++)
                {
                    for (int yIndex = 0; yIndex < yValues.Count - 1; yIndex++)
                    {
                        for (int zIndex = 0; zIndex < zValues.Count - 1; zIndex++)
                        {
                            list.Add(new IntCuboid(new IntVector3(xValues[xIndex], yValues[yIndex], zValues[zIndex]), new IntVector3(xValues[xIndex + 1], yValues[yIndex + 1], zValues[zIndex + 1])));
                        }
                    }
                }

                // now remove rectangles that aren't in either of the two rectangles
                // How? No fucking clue
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    if (!a.ContainsEntirely(list[i]) && !b.ContainsEntirely(list[i]))
                    {

                    }

                }
            }


        }

        internal static bool Intersects(IntCuboid a, IntCuboid b)
        {
            if (a == null)
            {
                throw new ArgumentNullException(nameof(a));
            }
            if (b == null)
            {
                throw new ArgumentNullException(nameof(b));
            }

            // They intersect if a corner of the first of them is inside the volume of the other
            foreach (IntVector3 corner in a.Corners())
            {
                if (b.Contains(corner))
                {
                    return true;
                }

            }
            return false;
        }

        internal bool ContainsEntirely(IntCuboid a)
        {
            // a is inside if both its corners are inside this
            return this.Contains(a.min) && this.Contains(a.max);
        }

        internal List<IntVector3> Corners()
        {
            List<IntVector3> corners = new List<IntVector3>();
            corners.Add(new IntVector3(min.X, min.Y, min.Z));
            corners.Add(new IntVector3(min.X, min.Y, max.Z));
            corners.Add(new IntVector3(min.X, max.Y, min.Z));
            corners.Add(new IntVector3(min.X, max.Y, max.Z));
            corners.Add(new IntVector3(max.X, min.Y, min.Z));
            corners.Add(new IntVector3(max.X, min.Y, max.Z));
            corners.Add(new IntVector3(max.X, max.Y, min.Z));
            corners.Add(new IntVector3(max.X, max.Y, max.Z));
            return corners;
        }

        internal int MinX()
        {
            return min.X;
        }

        internal int MaxX()
        {
            return max.X;
        }

        internal int MinY()
        {
            return min.Y;
        }

        internal int MaxY()
        {
            return max.Y;
        }

        internal int MinZ()
        {
            return min.Z;
        }

        internal int MaxZ()
        {
            return max.Z;
        }
    }
}