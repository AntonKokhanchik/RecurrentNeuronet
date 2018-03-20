using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecurrentNeuronet
{
    class Matrix
    {
        private double[][] body;

        public int Rows => body.Length;

        public int Columns => body[0].Length;

        public Matrix(int rows, int columns)
        {
            body = new double[rows][];
            for (int i = 0; i < rows; i++)
                body[i] = new double[columns];
        }

        public Matrix(double[][] arr)
        {
            int n = arr[0].Length;
            for (int i = 1; i < arr.Length; i++)
                if (arr[i].Length != n)
                    throw new ArgumentException("Массив должен быть прямоугольным (не ступенчатым)");

            body = arr;
        }

        public Matrix(double[,] arr)
        {
            int m = arr.GetLength(0);
            int n = arr.GetLength(1);

            body = new double[m][];
            for (int i = 0; i < m; i++)
            {
                body[i] = new double[n];
                for (int j = 0; j < n; j++)
                    body[i][j] = arr[i, j];
            }
        }

        public Matrix(Vector[] v)
        {
            int m = v.Length;
            int n = v[0].Length;

            body = new double[m][];
            for (int i = 0; i < m; i++)
            {
                if (v[i].Length != n)
                    throw new ArgumentException("Векторы должны быть одной длины");

                body[i] = new double[n];
                for (int j = 0; j < n; j++)
                    body[i][j] = v[i][j];
            }
        }

        public Matrix(Matrix M)
        {
            body = M.body;
        }

        public double[] this[int i]
        {
            get { return body[i]; }

            set { body[i] = value; }
        }

        public Matrix GetTranspose()
        {
            Matrix t = new Matrix(Columns, Rows);

            for (int i = 0; i < Rows; i++)
                for (int j = 0; j < Columns; j++)
                    t.body[j][i] = body[i][j];

            return t;
        }

        public static Matrix operator +(Matrix m1, Matrix m2)
        {
            if (m1.Rows != m2.Rows || m1.Columns != m2.Columns)
                throw new ArgumentException("Плохое сложение матриц");

            Matrix m3 = new Matrix(m1);
            for (int i = 0; i < m3.Rows; i++)
                for (int j = 0; j < m3.Columns; j++)
                    m3[i][j] += m2[i][j];

            return m3;
        }

        public static Matrix operator +(Matrix m1, int c)
        {
            Matrix m2 = new Matrix(m1);
            for (int i = 0; i < m2.Rows; i++)
                for (int j = 0; j < m2.Columns; j++)
                    m2[i][j] += c;

            return m2;
        }

        public static Matrix operator +(Matrix m1, double c)
        {
            Matrix m2 = new Matrix(m1);
            for (int i = 0; i < m2.Rows; i++)
                for (int j = 0; j < m2.Columns; j++)
                    m2[i][j] += c;

            return m2;
        }

        public static Matrix operator -(Matrix m1, Matrix m2)
        {
            if (m1.Rows != m2.Rows && m1.Columns != m2.Columns)
                throw new ArgumentException("Плохое сложение матриц");

            Matrix m3 = new Matrix(m1);
            for (int i = 0; i < m3.Rows; i++)
                for (int j = 0; j < m3.Columns; j++)
                    m3[i][j] -= m2[i][j];

            return m3;
        }

        public static Matrix operator -(Matrix m1)
        {
            Matrix m2 = new Matrix(m1);
            for (int i = 0; i < m1.Rows; i++)
                for (int j = 0; j < m1.Columns; j++)
                    m2[i][j] = -m1[i][j];

            return m2;
        }

        public static Matrix operator -(Matrix m1, int c)
        {
            Matrix m2 = new Matrix(m1);
            for (int i = 0; i < m2.Rows; i++)
                for (int j = 0; j < m2.Columns; j++)
                    m2[i][j] -= c;

            return m2;
        }

        public static Matrix operator -(Matrix m1, double c)
        {
            Matrix m2 = new Matrix(m1);
            for (int i = 0; i < m2.Rows; i++)
                for (int j = 0; j < m2.Columns; j++)
                    m2[i][j] -= c;

            return m2;
        }

        public static Matrix operator *(Matrix m1, Matrix m2)
        {
            if (m1.Columns != m2.Rows)
                throw new ArgumentException("Плохое умножение матриц");

            Matrix m3 = new Matrix(m1.Rows, m2.Columns);
            for (int i = 0; i < m1.Rows; i++)
                for (int j = 0; j < m2.Columns; j++)
                    for(int k = 0; k< m1.Columns; k++)
                        m3[i][j] += m1[i][k]*m2[k][j];

            return m3;
        }

        public static Vector operator *(Vector v, Matrix m)
        {
            if (v.Length != m.Rows)
                throw new ArgumentException("Плохое умножение вектора на матрицу");

            Vector v2 = new Vector(m.Columns);
            for (int j = 0; j < m.Columns; j++)
                for (int k = 0; k < v.Length; k++)
                    v2[j] += v[k] * m[k][j];

            return v2;
        }

        public static Vector operator *(Matrix m, Vector v)
        {
            if (m.Columns != v.Length)
                throw new ArgumentException("Плохое умножение матриц");

            Vector v2 = new Vector(m.Rows);
            for (int i = 0; i < m.Rows; i++)
                for (int k = 0; k < m.Columns; k++)
                    v2[i] += m[i][k] * v[k];

            return v2;
        }

        public static Matrix operator *(Matrix m1, int c)
        {
            Matrix m2 = new Matrix(m1);
            for (int i = 0; i < m2.Rows; i++)
                for (int j = 0; j < m2.Columns; j++)
                    m2[i][j] *= c;

            return m2;
        }

        public static Matrix operator *(Matrix m1, double c)
        {
            Matrix m2 = new Matrix(m1);
            for (int i = 0; i < m2.Rows; i++)
                for (int j = 0; j < m2.Columns; j++)
                    m2[i][j] *= c;

            return m2;
        }

        public static Matrix operator /(Matrix m1, int c)
        {
            Matrix m2 = new Matrix(m1);
            for (int i = 0; i < m2.Rows; i++)
                for (int j = 0; j < m2.Columns; j++)
                    m2[i][j] /= c;

            return m2;
        }

        public static Matrix operator /(Matrix m1, double c)
        {
            Matrix m2 = new Matrix(m1);
            for (int i = 0; i < m2.Rows; i++)
                for (int j = 0; j < m2.Columns; j++)
                    m2[i][j] /= c;

            return m2;
        }
    }
}
