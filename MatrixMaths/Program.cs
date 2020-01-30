using System;
using System.Collections.Generic;
using static System.Math;

namespace MatrixMaths
{
    class Program
    {
        static void Main(string[] args)
        {
            Matrix m = new Matrix(4, 4, new List<double> { 7, 6, 5, 15, 10, 9, 11, 16, 13, 4, 12, 0, 1, 3, 8, 2 });
            Console.WriteLine(Matrix.Determinant(m));
        }
    }

    /// <summary>
    /// Represents a matrix
    /// </summary>
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

        /// <summary>
        /// Finds the determinant of <paramref name="m"/> recursively.
        /// </summary>
        /// <param name="m">A square matrix</param>
        /// <returns>Determinant for matrix <c>m</c></returns>
        /// <exception cref="System.Exception">Thrown if the <paramref name="m"/> isn't square.</exception>
        public static double Determinant(Matrix m)
        {
            if (m.rows != m.columns)
            {
                throw new Exception("A matrix must be square to have a determinant.");
            }
            if (m.rows + m.columns == 4) // ==4 then hardcode Det(2x2 matrix)
            {
                return (m.elements[0, 0] * m.elements[1, 1]) - (m.elements[1, 0] * m.elements[0, 1]);
            }

            double det = 0;
            for (int i = 0; i < m.columns; i++)
            {
                det += Math.Pow(-1, i) * m.elements[0,i] * Matrix.Determinant(Matrix.SubMatrix(m, i));
            }

            return det;
        }

        /// <summary>
        /// Finds the sub-matrix/minor of <paramref name="m"/> at element with index <paramref name="i"/>.
        /// Assumes that the row is 0
        /// </summary>
        /// <param name="m">A square matrix</param>
        /// <param name="i">The column at which the minor is from. M[0, i] </param>
        /// <returns></returns>
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
