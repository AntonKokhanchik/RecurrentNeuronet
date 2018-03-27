﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecurrentNeuronet
{
    class Encoder
    {
        private Dictionary<string, double> dictionary;

        public Encoder(string[][] text)
        {
            dictionary = new Dictionary<string, double>();
            double wordsCount = 0;

            for (int i = 0; i < text.Length; i++)
                for (int j = 0; j < text[i].Length; j++)
                {
                    if (dictionary.ContainsKey(text[i][j]))
                        dictionary[text[i][j]]++;
                    else
                        dictionary.Add(text[i][j], 1);
                    wordsCount++;
                }

            dictionary.OrderBy(d => d.Value);

            List<string> d2 = new List<string>();
            for (int i = 0; i < dictionary.Count; i++)
            {
                dictionary[dictionary.ElementAt(i).Key] /= wordsCount;

                // Отменяем вероятность получения двух слов с одинаковой частотой
                if (dictionary[dictionary.ElementAt(i).Key] == dictionary[dictionary.ElementAt(i - 1).Key])
                    if (d2.Count == 0)
                    {
                        d2.Add(dictionary.ElementAt(i).Key);
                        d2.Add(dictionary.ElementAt(i - 1).Key);
                    }
                    else
                        d2.Add(dictionary.ElementAt(i).Key);

                else if (d2.Count > 0)
                {
                    for (int j = 0; j < d2.Count; j++)
                        dictionary[d2.ElementAt(j)] += (j / d2.Count) / wordsCount;
                    d2.Clear();
                }
            }
        }

        public double[][] EncodeText(string[][] text)
        {
            double[][] answer = new double[text.Length][];

            for (int i = 0; i < text.Length; i++)
            {
                answer[i] = new double[text[i].Length];
                for(int j = 0; j< text[i].Length; j++)
                    answer[i][j] = dictionary[text[i][j]];
            }
            return answer;
        }

        public double[] EncodeString(string s)
        {
            string[] words = s.Split(' ');
            double[] answer = new double[words.Length];

            for (int i = 0; i < s.Length; i++)
                answer[i] = dictionary[words[i]];

            return answer;
        }
    }
}
