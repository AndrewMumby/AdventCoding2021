using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCoding2021
{
    class IntVector3 : IComparable
    {
        int x;
        int y;
        int z;
        internal static List<IntVector3> CardinalDirections = new List<IntVector3>
        {
            new IntVector3(0, 0, 1),
            new IntVector3(0, -1, 0),
            new IntVector3(-1, 0, 0),
            new IntVector3(1, 0, 0),
            new IntVector3(0, 1, 0),
            new IntVector3(0, 0, 1)
        };
   
        internal static List<IntVector3> CardinalDirectionsIncludingDiagonals = new List<IntVector3>
        {
            new IntVector3(-1,-1,-1),
            new IntVector3(-1,-1, 0),
            new IntVector3(-1, -1, 1),
            new IntVector3(-1, 0, -1),
            new IntVector3(-1, 0, 0),
            new IntVector3(-1, 0, 1),
            new IntVector3(-1, 1, -1),
            new IntVector3(-1, 1, 0),
            new IntVector3(-1, 1, 1),

            new IntVector3(0,-1,-1),
            new IntVector3(0,-1, 0),
            new IntVector3(0, -1, 1),
            new IntVector3(0, 0, -1),
            new IntVector3(0, 0, 1),
            new IntVector3(0, 1, -1),
            new IntVector3(0, 1, 0),
            new IntVector3(0, 1, 1),

            new IntVector3(1,-1,-1),
            new IntVector3(1,-1, 0),
            new IntVector3(1, -1, 1),
            new IntVector3(1, 0, -1),
            new IntVector3(1, 0, 0),
            new IntVector3(1, 0, 1),
            new IntVector3(1, 1, -1),
            new IntVector3(1, 1, 0),
            new IntVector3(1, 1, 1),
        };


        public IntVector3(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public IntVector3(string input)
        {
            string vector = input.Trim(new char[] { '(', ')' });
            x = int.Parse(vector.Split(',')[0]);
            y = int.Parse(vector.Split(',')[1]);
            z = int.Parse(vector.Split(',')[2]);
        }

        public IntVector3(IntVector3 location)
        {
            this.x = location.x;
            this.y = location.y;
            this.z = location.z;
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

        public override string ToString()
        {
            return "(" + x + "," + y + "," + z + ")";
        }

        internal int Distance(IntVector3 coord)
        {
            return Math.Abs(coord.x - x) + Math.Abs(coord.y - y) + Math.Abs(coord.z - z);
        }

        internal static int Distance(IntVector3 a, IntVector3 b)
        {
            return Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y) + Math.Abs(a.z - b.z);
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
                IntVector3 v = (IntVector3)obj;
                return (x == v.x) && (y == v.y) && (z == v.z);
            }
        }

        internal static IntVector3 Add(IntVector3 vector1, IntVector3 vector2)
        {
            return new IntVector3(vector1.x + vector2.x, vector1.y + vector2.y, vector1.z + vector2.z);
        }

        internal static IntVector3 Subtract(IntVector3 vector1, IntVector3 vector2)
        {
            return new IntVector3(vector1.x - vector2.x, vector1.y - vector2.y, vector1.z - vector2.z);
        }

        internal static IntVector3 Multiply(IntVector3 vector, int number)
        {
            return new IntVector3(vector.x * number, vector.y * number, vector.z * number);
        }


        public override int GetHashCode()
        {
            return x ^ y ^ z;
        }

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            IntVector3 otherVector = obj as IntVector3;
            if (otherVector != null)
            {
                int value = z.CompareTo(otherVector.z);
                if (value == 0)
                {
                    value = y.CompareTo(otherVector.y);
                    if (value == 0)
                    {
                        value = x.CompareTo(otherVector.x);
                    }
                }
                return value;
            }
            else
            {
                throw new ArgumentException("Object is not a IntVector3");
            }
        }

        internal static IntVector3 Origin()
        {
            return new IntVector3(0, 0, 0);
        }

        internal IntVector3 North()
        {
            return new IntVector3(x, y - 1, z);
        }

        internal IntVector3 East()
        {
            return new IntVector3(x + 1, y, z);
        }

        internal IntVector3 South()
        {
            return new IntVector3(x, y + 1, z);
        }

        internal IntVector3 West()
        {
            return new IntVector3(x - 1, y, z);
        }

        internal IntVector3 Up()
        {
            return new IntVector3(x, y, z + 1);
        }

        internal IntVector3 Down()
        {
            return new IntVector3(x, y, z - 1);
        }

        internal IntVector3 YawLeft()
        {
            return new IntVector3(-y, x, z);
        }
        internal IntVector3 YawRight()
        {
            return new IntVector3(y, -x, z);
        }
        internal IntVector3 RollLeft()
        {
            return new IntVector3(-z, y, x);
        }

        internal IntVector3 RollRight()
        {
            return new IntVector3(z, y, -x);
        }

        internal IntVector3 PitchUp()
        {
            return new IntVector3(x, -z, y);
        }

        internal IntVector3 PitchDown()
        {
            return new IntVector3(x, z, -y);
        }
    }
}
