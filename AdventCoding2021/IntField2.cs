using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCoding2021
{
    internal class IntField2
    {
        int [,] field;
        int outsideValue;

        public IntField2 (int xSize, int ySize)
        {
            field = new int[xSize, ySize];
            outsideValue = 0;
        }

        public IntField2(int xSize, int ySize, int startingValue)
        {
            field = new int [xSize, ySize];
            for (int x = 0; x < xSize; x++)
            {
                for (int y = 0; y < ySize; y++)
                {
                    field [x, y] = startingValue;
                }
            }
            outsideValue = startingValue;
        }

        public int GetValue(int x, int y)
        {
            if (Inside(x,y))
            {
                return field [x, y];
            }
            else
            {
                return outsideValue;
            }
        }

        public void SetValue(int x, int y, int value)
        {
            if (Inside(x, y))
            {
                field[x, y] = value;
            }
            else
            {
                throw new ArgumentOutOfRangeException("Location outside field");
            }
        }

        private bool Inside(int x, int y)
        {
            return x >= 0 && y >= 0 && x < field.GetLength(0) && y < field.GetLength(1);
        }

        public long Sum()
        {
            long sum = 0;
            foreach (int value in field)
            {
                sum += value;
            }
            return sum;
        }

        public int GetSize(int dimension)
        {
            return field.GetLength(dimension);
        }

        public void DrawField()
        {
            for (int y = 0; y < field.GetLength(1); y++)
            {
                for (int x = 0; x < field.GetLength(0); x++)
                {
                    Console.Write(field[x, y]);
                }
                Console.WriteLine();
            }
        }
    }
}
