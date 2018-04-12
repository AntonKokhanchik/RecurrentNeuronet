using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecurrentNeuronet
{
    class Vector
    {
        private double[] body;

        public Vector(int n)
        {
            body = new double[n];
        }

        public Vector(Vector v)
        {
            body = new double[v.Length];
			for (int i = 0; i < v.Length; i++)
				body[i] = v[i];
        }

        public Vector(double[] arr)
        {
			body = new double[arr.Length];
			for (int i = 0; i < arr.Length; i++)
				body[i] = arr[i];
        }

        public int Length => body.Length;

        public double this[int i]
        {
            get { return body[i]; }

            set { body[i] = value; }
        }

        override public string ToString()
        {
            StringBuilder s = new StringBuilder("[ ");
            for (int i = 0; i < body.Length; i++)
                if (i != body.Length - 1)
                    s.AppendFormat("{0}, ", body[i]);
                else
                    s.AppendFormat("{0} ]", body[i]);
            return s.ToString();
        }

        public static Vector operator +(Vector v1, Vector v2)
        {
            if (v1.Length != v2.Length)
                throw new ArgumentException("Векторы должны быть одной длины");

            Vector v3 = new Vector(v1);
            for (int i = 0; i < v3.Length; i++)
                v3[i] += v2[i];

            return v3;
        }

        public static Vector operator +(Vector v1, int c)
        {
            Vector v2 = new Vector(v1);
            for (int i = 0; i < v2.Length; i++)
                v2[i] += c;

            return v2;
        }

        public static Vector operator +(Vector v1, double c)
        {
            Vector v2 = new Vector(v1);
            for (int i = 0; i < v2.Length; i++)
                v2[i] += c;

            return v2;
        }

        public static Vector operator -(Vector v1)
        {
            Vector v2 = new Vector(v1);
            for (int i = 0; i < v1.Length; i++)
                v2[i] = -v1[i];

            return v2;
        }

        public static Vector operator -(Vector v1, Vector v2)
        {
            if (v1.Length != v2.Length)
                throw new ArgumentException("Векторы должны быть одной длины");

            Vector v3 = new Vector(v1);
            for (int i = 0; i < v3.Length; i++)
                v3[i] -= v2[i];

            return v3;
        }

        public static Vector operator -(Vector v1, int c)
        {
            Vector v2 = new Vector(v1);
            for (int i = 0; i < v2.Length; i++)
                v2[i] -= c;

            return v2;
        }

        public static Vector operator -(Vector v1, double c)
        {
            Vector v2 = new Vector(v1);
            for (int i = 0; i < v2.Length; i++)
                v2[i] -= c;

            return v2;
        }

        public static double operator *(Vector v1, Vector v2)
        {
            if (v1.Length != v2.Length)
                throw new ArgumentException("Векторы должны быть одной длины");

            double ans = 0;
            for (int i = 0; i < v1.Length; i++)
                ans += v1[i] * v2[i];

            return ans;
        }

        /// <summary>
        /// Внешнее (векторное) произведение, результат - матрица произведений:
        /// ans[i][j] = v1[i] * v2[j];
        /// </summary>
        public static Matrix OuterMultiplication(Vector v1, Vector v2)
        {
            Matrix ans = new Matrix(v1.Length, v2.Length);

            for (int i = 0; i < v1.Length; i++)
                for (int j = 0; j < v2.Length; j++)
                    ans[i][j] = v1[i] * v2[j];

            return ans;
        }

        /// <summary>
        /// Почленное произведение векторов
        /// </summary>
        public static Vector TermByTermMultiplication(Vector v1, Vector v2)
        {
            if (v1.Length != v2.Length)
                throw new ArgumentException("Векторы должны быть одной длины");

            Vector v3 = new Vector(v1);
            for (int i = 0; i < v3.Length; i++)
                v3[i] *= v2[i];

            return v3;
        }

        public static Vector operator *(Vector v1, int c)
        {
            Vector v2 = new Vector(v1);
            for (int i = 0; i < v2.Length; i++)
                v2[i] *= c;

            return v2;
        }

        public static Vector operator *(Vector v1, double c)
        {
            Vector v2 = new Vector(v1);
            for (int i = 0; i < v2.Length; i++)
                v2[i] *= c;

            return v2;
        }

        public static Vector operator *(int c, Vector v1)
        {
            Vector v2 = new Vector(v1);
            for (int i = 0; i < v2.Length; i++)
                v2[i] *= c;

            return v2;
        }

        public static Vector operator *(double c, Vector v1)
        {
            Vector v2 = new Vector(v1);
            for (int i = 0; i < v2.Length; i++)
                v2[i] *= c;

            return v2;
        }

        public static Vector operator /(Vector v1, int c)
        {
            Vector v2 = new Vector(v1);
            for (int i = 0; i < v2.Length; i++)
                v2[i] /= c;

            return v2;
        }

        public static Vector operator /(Vector v1, double c)
        {
            Vector v2 = new Vector(v1);
            for (int i = 0; i < v2.Length; i++)
                v2[i] /= c;

            return v2;
        }
    }
}
