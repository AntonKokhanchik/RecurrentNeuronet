using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecurrentNeuronet
{//TODO: определить f и g
    class RecurrentNeuronet
    {
        // Параметры

        private int n; // количество "повторов" (максимальное время)
        private /*Vector*/double[/*n*/] x; // x[t] - входной вектор номер t,
        private Vector[/*n*/] h; // h[t] - состояние скрытого слоя для входа x[t] (h[0]=0 ), 
        private Vector y; // y[t] - выход сети для входа x[t], 
        private /*Matrix*/Vector V; // V - весовая матрица распределительного слоя,
        private Matrix U; // U - весовая(квадратная) матрица обратных связей скрытого слоя, 
        private Vector a; // bh - вектор сдвигов скрытого слоя,
        private Matrix W; // W - весовая матрица выходного слоя,
        private Vector b; // by - вектор сдвигов выходного слоя

        private Vector d; // d - вектор правильных ответов
        Vector[] s; // состояния скрытого слоя в момент t
        Vector q;
        Vector[] p; // множители Лагранжа

		// f - функция активации скрытого слоя
		private Vector f(Vector s)
		{
			// tanh
			Vector s2 = new Vector(s);
			for (int i = 0; i < s.Length; i++)
				s2[i] = Math.Tanh(s2[i]);
			return s2;
		}

		// f1 = f' - производная f
		private Vector f1(Vector s)
		{
			// 1/(cosh)^2
			Vector s2 = new Vector(s);
			for (int i = 0; i < s.Length; i++)
				s2[i] = 1 / Math.Pow(Math.Cosh(s2[i]), 2);
			return s2;
		}

		// g - функция активации выходного слоя
		private Vector g(Vector s)
		{
			// tanh
			Vector s2 = new Vector(s);
			for (int i = 0; i < s.Length; i++)
				s2[i] = Math.Tanh(s2[i]);
			return s2;
		}

		// g1 = g' - производная g
		private Vector g1(Vector s)
		{
			// 1/(cosh)^2
			Vector s2 = new Vector(s);
			for (int i = 0; i < s.Length; i++)
				s2[i] = 1 / Math.Pow(Math.Cosh(s2[i]), 2);
			return s2;
		}


        // Функции

        // Прямой проход:
        private void DirectPass()
        {
            // для каждого вектора последовательности { x(1),…,x(n) } : 
            for (int t = 0; t < n; t++)
            {
                //вычисляем состояния скрытого слоя { s(1),…,s(n) } и выходы скрытого слоя { h(1),…,h(n) }
                s[t + 1] = V * x[t + 1] + U * h[t] + a;
                h[t + 1] = f(s[t + 1]);
            }
            //вычисляем выход сети y
            y = g(W * h[n] + b);
        }

        // Обратный проход:
        private void BackwardPass()
        {
            // вычисляем ошибку выходного слоя δo
            q = y - d;
            // вычисляем ошибку скрытого слоя в конечном состоянии δh(n)
            p = new Vector[n+1];
            p[n] = Vector.TermByTermMultiplication(q, g1(W * h[n] + b)) * W;
            // вычисляем ошибки скрытого слоя в промежуточных состояниях δh(t) (t = 1,…,n)
            for (int t = n - 1; t > 0; t--)
                p[t] = Vector.TermByTermMultiplication(p[t + 1], f1(s[t + 1])) * U;
        }

        // 3. Вычисляем изменение весов:
        private void WeightsChange()
        {
            Matrix dW = -Vector.OuterMultiplication(Vector.TermByTermMultiplication(q, g1(W * h[n] + b)), h[n]);

            Vector dby = -Vector.TermByTermMultiplication(q, g1(W * h[n] + b));

            //Matrix dV = -Vector.OuterMultiplication(Vector.TermByTermMultiplication(p[1], f1(s[1])), x[1]);
            Vector dV = -Vector.TermByTermMultiplication(p[1], f1(s[1])) * x[1];
            for (int t = 1; t < n; t++)
                //dV += -Vector.OuterMultiplication(Vector.TermByTermMultiplication(p[t+1], f1(s[t+1])), x[t+1]);
                dV += -Vector.TermByTermMultiplication(p[t + 1], f1(s[t + 1])) * x[t + 1];

            Matrix dU = -Vector.OuterMultiplication(Vector.TermByTermMultiplication(p[1], f1(s[1])), h[0]);
            for (int t = 1; t < n; t++)
                dU += -Vector.OuterMultiplication(Vector.TermByTermMultiplication(p[t + 1], f1(s[t + 1])), h[t]);

            Vector dbh = -Vector.TermByTermMultiplication(p[1], f1(s[1]));
            for (int t = 1; t < n; t++)
                dbh += -Vector.TermByTermMultiplication(p[t + 1], f1(s[t + 1]));


            W += dW;
            b += dby;
            V += dV;
            U += dU;
            a += dbh;
        }


        // Обратная связь

        public RecurrentNeuronet(/*Vector*/double[/*строки*/][/*слова*/] enters, int innerLength, double epsilon)
        {
            // сюда ещё можно много исключений набросать, валидаторов

            n = maxInnerLength(enters);
            V = new Vector(innerLength); //new Matrix(innerLength, enters[i].Length);
            U = new Matrix(innerLength, innerLength);
            W = new Matrix(enters.Length, innerLength);
            a = new Vector(innerLength);
            b = new Vector(enters.Length);

            bool isLearnedInThisCicle;
            do
            {
                isLearnedInThisCicle = false;
                for (int i = 0; i < enters.Length; i++)
                {
                    x = new double[n + 1];
                    for (int j = 0; j < n; j++)
                        if (j < enters[i].Length)
                            x[j + 1] = enters[i][j];
                        else
                            x[j + 1] = 0;

                    d = new Vector(enters.Length);
                    d[i] = 1;

                    h = new Vector[n + 1];
                    for (int j = 0; j <= n; j++)
                        h[j] = new Vector(innerLength);
                    s = new Vector[n + 1];
                    for (int j = 1; j <= n; j++)
                        s[j] = new Vector(innerLength);

                    DirectPass();
                    while ((d - y) * (d - y) > epsilon)
                    {
                        BackwardPass();
                        WeightsChange();

                        isLearnedInThisCicle = true;

                        DirectPass();
                    }
                }
            } while (isLearnedInThisCicle);
        }

        public Vector Answer(double[] enter)
        {
			x = new double[n + 1];
			for (int j = 0; j < n; j++)
				if (j < enter.Length)
					x[j + 1] = enter[j];
				else
					x[j + 1] = 0;
			DirectPass();
            return y;
        }


        // вспомогательное
        private int maxInnerLength(double[][] a)
        {
            int max = a[0].Length;
            for (int i = 1; i < a.Length; i++)
                if (a[i].Length > max)
                    max = a[i].Length;
            return max;
        }
    }
}
