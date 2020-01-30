using System;
using System.Collections.Generic;
using static System.Math;

namespace MatrixMaths
{
    class Program
    {
        static void Main(string[] args)
        {
            Matrix m = new Matrix(3, 3, new List<double> { 7, 9, 34, 2, 1, 7, 1, 0, 0 });
            Console.WriteLine(Matrix.Determinant(m));
        }
    }

    class Matrix
    {
        public double[,] elements;
        public int rows;
        public int columns;
        public bool square;

        public Matrix(int rows, int columns, List<double> elements)
        {
            if (rows*columns != elements.Count)
            {
                throw new System.Exception("The number of elements must be correct for the given matrix size.");
            }

            this.rows = rows;
            this.columns = columns;
            if (rows == columns)
            {
                this.square = true;
            }
            else
            {
                this.square = false;
            }

            this.elements = new double[rows, columns];

            for (int row = 0; row < this.rows; row++)
            {
                for (int column = 0; column < this.columns; column++)
                {
                    //Console.WriteLine("{0}, {1}, {2}", row, column, elements[row * this.columns + column]);
                    this.elements[row, column] = elements[row * this.columns + column];
                }
            }
            //Console.WriteLine();


            /*for (int i = 0; i < elements.Count; i++)
            {
                this.elements[i] = elements[i];
            }*/
        }

        public static double Determinant(Matrix m)
        {
            if (m.rows != m.columns)
            {
                throw new Exception("A matrix must be square to have a determinant.");
            }
            if (m.rows + m.columns == 2) // ==4 then hardcode Det(2x2 matrix)
            {
                return m.elements[0,0];
            }

            double det = 0;
            for (int i = 0; i < m.columns; i++)
            {
                det += Math.Pow(-1, i) * Matrix.Determinant(Matrix.SubMatrix(m, i));
            }

            return det;
        }

        public static Matrix SubMatrix(Matrix m, int i)
        {
            int c = i;
            int r = 0;
            List<double> elements = new List<double>();

            for (int row = 0; row < m.rows; row++)
            {
                if (row == r)
                {
                    continue;
                }
                for (int column = 0; column < m.columns; column++)
                {
                    if (column == c)
                    {
                        continue;
                    }
                    elements.Add(m.elements[row, column]);
                }
            }
            return new Matrix(m.rows - 1, m.columns - 1, elements);
        }
    }
}
