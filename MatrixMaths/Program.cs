using System;
using System.Collections.Generic;
using static System.Math;
using System.Timers;
using System.Threading;

namespace MatrixMaths
{
    class Program
    {
        static void Main(string[] args)
        {
            //Matrix m1 = new Matrix(4, 4, new List<double> { 7, 6, 5, 15, 10, 9, 11, 16, 13, 4, 12, 0, 1, 3, 8, 2 });
            //Console.WriteLine(Matrix.Determinant(m));

            Matrix m1 = new Matrix(3, 3, new List<double> { 7, 6, 5, 15, 10, 9, 11, 16, 13 });
            //Matrix m1 = new Matrix(2, 2, new List<double> { 1, 2, 3, 4 });
            Matrix m2 = new Matrix(2, 2, new List<double> { 1, 2, 1, 3 });
            Matrix m3 = new Matrix(1, 1, new List<double> { 5 });

            double time = 0;
            int iterations = 1000000;
            for (int i = 0; i < iterations; i++)
            {
                DateTime start = DateTime.Now;
                //------------------------------------------------
                Matrix result = m1 * Matrix.Inverse(m1);
                //------------------------------------------------
                DateTime finish = DateTime.Now;
                time += (finish - start).TotalMilliseconds;
            }


            Console.WriteLine("Average: " + Math.Round((time/iterations), 6).ToString() + "ms");
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
                    this.elements[row, column] = elements[row * this.columns + column];
                }
            }
        }

        public Double this[int i, int j]
        {
            get { return this.elements[i, j]; }
            set { this.elements[i, j] = value; }
        }

        public override string ToString()
        {
            string toReturn = string.Empty;
            for (int row = 0; row < this.rows; row++)
            {
                toReturn += '[';
                for (int column = 0; column < this.columns; column++)
                {
                    toReturn += Math.Round(this[row, column], 3) + ", ";
                }
                toReturn = toReturn.Substring(0, toReturn.Length - 2);
                toReturn += "], \n";
            }
            return toReturn.Substring(0, toReturn.Length - 3); ;
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
            else if (m.rows + m.columns == 2)
            {
                return m[0, 0];
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
        /// <returns>Matrix</returns>
        public static Matrix SubMatrix(Matrix m, int i, int r = 0)
        {
            int c = i;
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

        public static Matrix Inverse(Matrix m)
        {
            double det = Determinant(m);
            if (det == 0)
            {
                throw new DivideByZeroException("A singular matrix does not have an inverse.");
            }
            if (!m.square)
            {
                throw new Exception("A matrix must be square to have an inverse.");
            }

            double scalar = 1/Matrix.Determinant(m);
            
            if (m.columns + m.rows == 4)
            {
                return scalar * new Matrix(m.rows, m.columns, new List<double> { m[1, 1], -m[0, 1], -m[1, 0], m[0, 0] });
            }
            else if (m.columns + m.rows == 2)
            {
                return new Matrix(1, 1, new List<double> { 1 / m[0, 0] });
            }

            //Minors
            List<double> elements = new List<double>();
            for (int row = 0; row < m.rows; row++)
            {
                for (int column = 0; column < m.columns; column++)
                {
                    elements.Add(Determinant(SubMatrix(m, column, row)));
                }
            }
            
            //Cofactors
            for (int i = 0; i < m.elements.Length; i++)
            {
                elements[i] *= Math.Pow(-1, i);
            }
            Matrix m2 = new Matrix(m.rows, m.columns, elements);

            //Transpose
            List<double> final = new List<double>();
            for (int row = 0; row < m.rows; row++)
            {
                for (int column = 0; column < m.columns; column++)
                {
                    final.Add(m2[column, row]);
                }
            }
            return scalar * new Matrix(m.rows, m.columns, final);
        }

        public static Matrix operator +(Matrix m1, Matrix m2)
        {
            if (!(m1.columns == m2.columns && m1.rows == m2.rows))
            {
                throw new ArithmeticException("Matrices must be the same size before adding them.");
            }

            List<double> elements = new List<double>();
            for (int row = 0; row < m1.rows; row++)
            {
                for (int column = 0; column < m1.columns; column++)
                {
                    elements.Add(m1[row, column] + m2[row, column]);
                }
            }
            return new Matrix(m1.rows, m1.columns, elements);
        }
        public static Matrix operator -(Matrix m1, Matrix m2)
        {
            if (!(m1.columns == m2.columns && m1.rows == m2.rows))
            {
                throw new ArithmeticException("Matrices must be the same size before subtracting them.");
            }

            List<double> elements = new List<double>();
            for (int row = 0; row < m1.rows; row++)
            {
                for (int column = 0; column < m1.columns; column++)
                {
                    elements.Add(m1[row, column] - m2[row, column]);
                }
            }
            return new Matrix(m1.rows, m1.columns, elements);
        }
        public static Matrix operator *(Matrix m1, double scalar)
        {
            List<double> elements = new List<double>();
            for (int row = 0; row < m1.rows; row++)
            {
                for (int column = 0; column < m1.columns; column++)
                {
                    elements.Add(m1[row, column]*scalar);
                }
            }
            return new Matrix(m1.rows, m1.columns, elements);
        }
        public static Matrix operator *(double scalar, Matrix m1)
        {
            List<double> elements = new List<double>();
            for (int row = 0; row < m1.rows; row++)
            {
                for (int column = 0; column < m1.columns; column++)
                {
                    elements.Add(m1[row, column] * scalar);
                }
            }
            return new Matrix(m1.rows, m1.columns, elements);
        }
        public static Matrix operator *(Matrix m1, Matrix m2)
        {
            if (m1.columns != m2.rows)
            {
                throw new ArithmeticException("The two matrices cannot be multiplied.");
            }
            int newRows = m1.rows;
            int newCols = m2.columns;
            List<double> elements = new List<double>();
            for (int row = 0; row < newRows; row++)
            {
                for (int column = 0; column < newCols; column++)
                {
                    double dotProduct = 0;
                    for (int k = 0; k < m1.columns; k++)
                    {
                        dotProduct += m1[row, k] * m2[k, column];
                    }
                    elements.Add(dotProduct);
                }
            }
            return new Matrix(newRows, newCols, elements);
        }
    }
}
