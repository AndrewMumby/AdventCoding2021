using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCoding2021
{
    class IntVector4 : IComparable
    {
        int x;
        int y;
        int z;
        int t;
        internal static List<IntVector4> CardinalDirections = new List<IntVector4>
        {
            new IntVector4(0,0,0,-1),
            new IntVector4(0, 0, -1, 0),
            new IntVector4(0, -1, 0,0),
            new IntVector4(-1, 0, 0,0),
            new IntVector4(1, 0, 0,0),
            new IntVector4(0, 1, 0,0),
            new IntVector4(0, 0, 1,0),
            new IntVector4(0,0,0,1)
        };

        internal static List<IntVector4> CardinalDirectionsIncludingDiagonals = new List<IntVector4>
        {
            new IntVector4(-1, -1,-1,-1),
            new IntVector4(-1, -1,-1, 0),
            new IntVector4(-1, -1, -1, 1),
            new IntVector4(-1, -1, 0, -1),
            new IntVector4(-1, -1, 0, 0),
            new IntVector4(-1, -1, 0, 1),
            new IntVector4(-1, -1, 1, -1),
            new IntVector4(-1, -1, 1, 0),
            new IntVector4(-1, -1, 1, 1),

            new IntVector4(-1, 0,-1,-1),
            new IntVector4(-1, 0,-1, 0),
            new IntVector4(-1, 0, -1, 1),
            new IntVector4(-1, 0, 0, -1),
            new IntVector4(-1, 0, 0, 0),
            new IntVector4(-1, 0, 0, 1),
            new IntVector4(-1, 0, 1, -1),
            new IntVector4(-1, 0, 1, 0),
            new IntVector4(-1, 0, 1, 1),

            new IntVector4(-1, 1,-1,-1),
            new IntVector4(-1, 1,-1, 0),
            new IntVector4(-1, 1, -1, 1),
            new IntVector4(-1, 1, 0, -1),
            new IntVector4(-1, 1, 0, 0),
            new IntVector4(-1, 1, 0, 1),
            new IntVector4(-1, 1, 1, -1),
            new IntVector4(-1, 1, 1, 0),
            new IntVector4(-1, 1, 1, 1),

            new IntVector4(0, -1,-1,-1),
            new IntVector4(0, -1,-1, 0),
            new IntVector4(0, -1, -1, 1),
            new IntVector4(0, -1, 0, -1),
            new IntVector4(0, -1, 0, 0),
            new IntVector4(0, -1, 0, 1),
            new IntVector4(0, -1, 1, -1),
            new IntVector4(0, -1, 1, 0),
            new IntVector4(0, -1, 1, 1),

            new IntVector4(0, 0,-1,-1),
            new IntVector4(0, 0,-1, 0),
            new IntVector4(0, 0, -1, 1),
            new IntVector4(0, 0, 0, -1),
            new IntVector4(0, 0, 0, 1),
            new IntVector4(0, 0, 1, -1),
            new IntVector4(0, 0, 1, 0),
            new IntVector4(0, 0, 1, 1),

            new IntVector4(0, 1,-1,-1),
            new IntVector4(0, 1,-1, 0),
            new IntVector4(0, 1, -1, 1),
            new IntVector4(0, 1, 0, -1),
            new IntVector4(0, 1, 0, 0),
            new IntVector4(0, 1, 0, 1),
            new IntVector4(0, 1, 1, -1),
            new IntVector4(0, 1, 1, 0),
            new IntVector4(0, 1, 1, 1),


            new IntVector4(1, -1,-1,-1),
            new IntVector4(1, -1,-1, 0),
            new IntVector4(1, -1, -1, 1),
            new IntVector4(1, -1, 0, -1),
            new IntVector4(1, -1, 0, 0),
            new IntVector4(1, -1, 0, 1),
            new IntVector4(1, -1, 1, -1),
            new IntVector4(1, -1, 1, 0),
            new IntVector4(1, -1, 1, 1),

            new IntVector4(1, 0,-1,-1),
            new IntVector4(1, 0,-1, 0),
            new IntVector4(1, 0, -1, 1),
            new IntVector4(1, 0, 0, -1),
            new IntVector4(1, 0, 0, 0),
            new IntVector4(1, 0, 0, 1),
            new IntVector4(1, 0, 1, -1),
            new IntVector4(1, 0, 1, 0),
            new IntVector4(1, 0, 1, 1),

            new IntVector4(1, 1,-1,-1),
            new IntVector4(1, 1,-1, 0),
            new IntVector4(1, 1, -1, 1),
            new IntVector4(1, 1, 0, -1),
            new IntVector4(1, 1, 0, 0),
            new IntVector4(1, 1, 0, 1),
            new IntVector4(1, 1, 1, -1),
            new IntVector4(1, 1, 1, 0),
            new IntVector4(1, 1, 1, 1),

        };


        public IntVector4(int x, int y, int z, int t)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.T = t;
        }

        public IntVector4(string input)
        {
            string vector = input.Trim(new char[] { '(', ')' });
            x = int.Parse(vector.Split(',')[0]);
            y = int.Parse(vector.Split(',')[1]);
            z = int.Parse(vector.Split(',')[2]);
            T = int.Parse(vector.Split(',')[3]);
        }

        public IntVector4(IntVector4 location)
        {
            this.x = location.x;
            this.y = location.y;
            this.z = location.z;
            this.T = location.T;
        }

        public int X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
            }
        }

        public int Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
            }
        }

        public int Z
        {
            get
            {
                return z;
            }
            set
            {
                z = value;
            }
        }

        public int T
        {
            get
            {
                return t;
            }

            set
            {
                t = value;
            }
        }

        public override string ToString()
        {
            return "(" + x + "," + y + "," + z + "," + t + ")";
        }

        internal int Distance(IntVector4 coord)
        {
            return Math.Abs(coord.x - x) + Math.Abs(coord.y - y) + Math.Abs(coord.z - z) + Math.Abs(coord.t - t);
        }

        internal static int Distance(IntVector4 a, IntVector4 b)
        {
            return Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y) + Math.Abs(a.z - b.z) + Math.Abs(a.t - b.t);
        }

        public override bool Equals(Object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                IntVector4 v = (IntVector4)obj;
                return (x == v.x) && (y == v.y) && (z == v.z) && (t == v.t);
            }
        }

        internal static IntVector4 Add(IntVector4 vector1, IntVector4 vector2)
        {
            return new IntVector4(vector1.x + vector2.x, vector1.y + vector2.y, vector1.z + vector2.z, vector1.t + vector2.t);
        }

        internal static IntVector4 Subtract(IntVector4 vector1, IntVector4 vector2)
        {
            return new IntVector4(vector1.x - vector2.x, vector1.y - vector2.y, vector1.z - vector2.z, vector1.t - vector2.t);
        }

        internal static IntVector4 Multiply(IntVector4 vector, int number)
        {
            return new IntVector4(vector.x * number, vector.y * number, vector.z * number, vector.t * number);
        }


        public override int GetHashCode()
        {
            return x ^ y ^ z ^ t;
        }

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            IntVector4 otherVector = obj as IntVector4;
            if (otherVector != null)
            {
                int value = t.CompareTo(otherVector.t);
                if (value == 0)
                {
                    value = z.CompareTo(otherVector.z);
                    if (value == 0)
                    {
                        value = y.CompareTo(otherVector.y);
                        if (value == 0)
                        {
                            value = x.CompareTo(otherVector.x);
                        }

                    }
                }
                return value;
            }
            else
            {
                throw new ArgumentException("Object is not a IntVector4");
            }
        }

        internal static IntVector4 Origin()
        {
            return new IntVector4(0, 0, 0, 0);
        }

        internal IntVector4 North()
        {
            return new IntVector4(x, y - 1, z, t);
        }

        internal IntVector4 East()
        {
            return new IntVector4(x + 1, y, z, t);
        }

        internal IntVector4 South()
        {
            return new IntVector4(x, y + 1, z, t);
        }

        internal IntVector4 West()
        {
            return new IntVector4(x - 1, y, z, t);
        }

        internal IntVector4 Up()
        {
            return new IntVector4(x, y, z + 1, t);
        }

        internal IntVector4 Down()
        {
            return new IntVector4(x, y, z - 1, t);
        }

        internal IntVector4 Backwards()
        {
            return new IntVector4(x, y, z, t - 1);
        }

        internal IntVector4 Forwards()
        {
            return new IntVector4(x, y, z, t + 1);
        }
    }
}
