using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecurrentNeuronet
{//TODO: определить f и g, описать условия выхода
    class RecurrentNeuronet
    {
        // Параметры

        private int n; // количество "повторов" (максимальное время)
        private /*Vector*/double[/*n*/] x; // x[t] - входной вектор номер t,
        private Vector[/*n*/] h; // h[t] - состояние скрытого слоя для входа x[t] (h[0]=0 ), 
        private Vector y; // y[t] - выход сети для входа x[t], 
        private /*Matrix*/Vector V; // V - весовая матрица распределительного слоя,
        private Matrix U; // U - весовая(квадратная) матрица обратных связей скрытого слоя, 
        private Vector bh; // bh - вектор сдвигов скрытого слоя,
        private Matrix W; // W - весовая матрица выходного слоя,
        private Vector by; // by - вектор сдвигов выходного слоя

        private Vector d; // d - вектор правильных ответов
        Vector[] s; // состояния скрытого слоя в момент t
        Vector q; 
        Vector[] p; // множители Лагранжа

        // f - функция активации скрытого слоя
        private Vector f(Vector s)
        {
            throw new NotImplementedException();
        }

        // f1 = f' - производная f
        private Vector f1(Vector s)
        {
            throw new NotImplementedException();
        }

        // g - функция активации выходного слоя
        private Vector g(Vector s)
        {
            throw new NotImplementedException();
        }

        // g1 = g' - производная g
        private Vector g1(Vector s)
        {
            throw new NotImplementedException();
        }


        // Функции

        // Прямой проход:
        private void DirectPass()
        {
            // для каждого вектора последовательности { x(1),…,x(n) } : 
            for (int t = 0; t < n; t++)
            {
                //вычисляем состояния скрытого слоя { s(1),…,s(n) } и выходы скрытого слоя { h(1),…,h(n) }
                s[t+1] = V * x[t+1] + U * h[t] + bh;
                h[t+1] = f(s[t+1]);
            }
            //вычисляем выход сети y
            y = g(W * h[n] + by);
        }

        // Обратный проход:
        private void BackwardPass()
        {
            // вычисляем ошибку выходного слоя δo
            q = y - d;
            // вычисляем ошибку скрытого слоя в конечном состоянии δh(n)
            p = new Vector[n];
            p[n] = Vector.TermByTermMultiplication(q, g1(W * h[n] + by)) * W;
            // вычисляем ошибки скрытого слоя в промежуточных состояниях δh(t) (t = 1,…,n)
            for (int t = n - 1; t > 0; t--)
                p[t] = Vector.TermByTermMultiplication(p[t + 1], f1(s[t+1])) * U;
        }

        // 3. Вычисляем изменение весов:
        private void WeightsChange()
        {
            Matrix dW = -Vector.OuterMultiplication(Vector.TermByTermMultiplication(q, g1(W * h[n] + by)), h[n]);

            Vector dby = -Vector.TermByTermMultiplication(q, g1(W * h[n] + by));

            //Matrix dV = -Vector.OuterMultiplication(Vector.TermByTermMultiplication(p[1], f1(s[1])), x[1]);
            Vector dV = -Vector.TermByTermMultiplication(p[1], f1(s[1])) * x[1];
            for (int t = 1; t < n; t++)
                //dV += -Vector.OuterMultiplication(Vector.TermByTermMultiplication(p[t+1], f1(s[t+1])), x[t+1]);
                dV += -Vector.TermByTermMultiplication(p[t+1], f1(s[t+1])) * x[t+1];

            Matrix dU = -Vector.OuterMultiplication(Vector.TermByTermMultiplication(p[1], f1(s[1])), h[0]);
            for (int t = 1; t < n; t++)
                dU += -Vector.OuterMultiplication(Vector.TermByTermMultiplication(p[t + 1], f1(s[t + 1])), h[t]);

            Vector dbh = -Vector.TermByTermMultiplication(p[1], f1(s[1]));
            for (int t = 1; t < n; t++)
                dbh += -Vector.TermByTermMultiplication(p[t + 1], f1(s[t + 1]));


            W += dW;
            by += dby;
            V += dV;
            U += dU;
            bh += dbh;
        }


        // Обратная связь

        public RecurrentNeuronet(/*Vector*/double [][] enters, Vector[] answers, int innerLength)
        {
            if (enters.Length != answers.Length)
                throw new ArgumentException("В обучающей выборке неравное число входов и выходов");

            while (true)
            {
                for (int i = 0; i < enters.Length; i++)
                {
                    n = enters[i].Length;
                    x = new double[n + 1];
                    for (int j = 1; j <= n; j++)
                        x[j + 1] = enters[i][j];
                    d = answers[i];

                    h = new Vector[n + 1];
                    for (int j = 0; j <= n; j++)
                        h[i] = new Vector(innerLength);
                    V = new Vector(innerLength); //new Matrix(innerLength, enters[i].Length);
                    U = new Matrix(innerLength, innerLength);
                    W = new Matrix(answers[i].Length, innerLength);
                    bh = new Vector(innerLength);
                    by = new Vector(answers[i].Length);
                    s = new Vector[n + 1];
                    for (int j = 1; j <= n; j++)
                        h[j] = new Vector(innerLength);

                    DirectPass();
                    while (true)
                    {
                        BackwardPass();
                        WeightsChange();
                        DirectPass();
                    }
                }
            }
        }

        public Vector Answer (double[] enter)
        {
            x = enter;
            DirectPass();
            return y;
        }
    }
}
