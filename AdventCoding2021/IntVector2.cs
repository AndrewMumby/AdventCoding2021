using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCoding2021
{
    class IntVector2 : IComparable
    {
        int x;
        int y;
        internal static List<IntVector2> CardinalDirections = new List<IntVector2> {
            new IntVector2(0, -1),
            new IntVector2(-1, 0),
            new IntVector2(1, 0),
            new IntVector2(0, 1)
        };
        internal static List<IntVector2> CardinalDirectionsIncludingDiagonals = new List<IntVector2>
        {
            new IntVector2(-1,-1),
            new IntVector2(-1, 0),
            new IntVector2(-1, 1),
            new IntVector2(0, -1),
            new IntVector2(0, 1),
            new IntVector2(1, -1),
            new IntVector2(1, 0),
            new IntVector2(1,1)
        };

        public IntVector2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public IntVector2(string input)
        {
            string vector = input.Trim(new char[] { '(', ')' });
            x = Convert.ToInt32(vector.Split(',')[0]);
            y = Convert.ToInt32(vector.Split(',')[1]);
        }

        public IntVector2(IntVector2 location)
        {
            this.x = location.x;
            this.y = location.y;
        }

        public IntVector2()
        {
            this.x = 0;
            this.y = 0;
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

        public override string ToString()
        {
            return "(" + x + "," + y + ")";
        }

        internal int Distance(IntVector2 coord)
        {
            return Math.Abs(coord.x - x) + Math.Abs(coord.y - y);
        }

        internal static int Distance(IntVector2 a, IntVector2 b)
        {
            return Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y);
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
                IntVector2 v = (IntVector2)obj;
                return (x == v.x) && (y == v.y);
            }
        }

        internal static IntVector2 Add(IntVector2 vector1, IntVector2 vector2)
        {
            return new IntVector2(vector1.x + vector2.x, vector1.y + vector2.y);
        }

        internal IntVector2 Add(IntVector2 vector)
        {
            return IntVector2.Add(this, vector);
        }

        internal static IntVector2 Multiply(IntVector2 vector, int quantity)
        {
            return new IntVector2(vector.x * quantity, vector.y * quantity);
        }

        internal IntVector2 Multiply(int quantity)
        {
            return IntVector2.Multiply(this, quantity);
        }

        public override int GetHashCode()
        {
            return x ^ y;
        }

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            IntVector2 otherVector = obj as IntVector2;
            if (otherVector != null)
            {
                int value = y.CompareTo(otherVector.y);
                if (value == 0)
                {
                    return x.CompareTo(otherVector.x);
                }
                else
                {
                    return value;
                }

            }
            else
                throw new ArgumentException("Object is not a IntVector2");
        }

        internal IntVector2 North()
        {
            return new IntVector2(this.x, this.y - 1);
        }

        internal IntVector2 North(int distance)
        {
            return new IntVector2(this.x, this.y - distance);
        }

        internal IntVector2 East()
        {
            return new IntVector2(this.x + 1, this.y);
        }

        internal IntVector2 East(int distance)
        {
            return new IntVector2(this.x + distance, this.y);
        }

        internal IntVector2 South()
        {
            return new IntVector2(this.x, this.y + 1);
        }

        internal IntVector2 South(int distance)
        {
            return new IntVector2(this.x, this.y + distance);
        }

        internal IntVector2 West()
        {
            return new IntVector2(this.x - 1, this.y);
        }

        internal IntVector2 West(int distance)
        {
            return new IntVector2(this.x - distance, this.y);
        }

        internal IntVector2 Left()
        {
            return new IntVector2(this.y, -this.x);
        }
        internal IntVector2 Right()
        {
            return new IntVector2(-this.y, this.x);
        }

        internal IntVector2 DirectionTo(IntVector2 target)
        {
            IntVector2 direction = new IntVector2(target.X - x, target.Y - y);
            int a = direction.X;
            int b = direction.Y;

            a = Math.Abs(a);
            b = Math.Abs(b);

            while (a != 0 && b != 0)
            {
                if (a > b)
                    a %= b;
                else
                    b %= a;
            }

            int gcd = (a == 0 ? b : a);

            direction.X = direction.X / gcd;
            direction.Y = direction.Y / gcd;
            return direction;   
        }

        internal double BearingTo(IntVector2 target)
        {
            if (this.Equals(target))
            {
                throw new Exception("Cannot find bearing to self");
            }
            IntVector2 direction = DirectionTo(target);
            double bearing = Math.Atan2(direction.X, -direction.Y);
            if (bearing < 0)
            {
                bearing += Math.PI * 2;
            }
            return bearing;
        }
    }
}
