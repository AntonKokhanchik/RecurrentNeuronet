using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RecurrentNeuronet
{
    public partial class Form1 : Form
    {
        Encoder encoder;
        RecurrentNeuronet neuronet;

        public Form1()
        {
            InitializeComponent();
        }

        private void buttonSelectFile_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            string[][] text = getWords();
            encoder = new Encoder(text);
            neuronet = new RecurrentNeuronet(encoder.EncodeText(text), l, epsilon);
        }

        private string[][] getWords()
        {
            List<string[]> text = new List<string[]>();
            StreamReader file = new StreamReader(openFileDialog1.OpenFile());
 
            string s = file.ReadLine();
            while (s != null)
            {
                text.Add(s.Split(' '));
                s = file.ReadLine();
            }

            return text.ToArray();
        }
    }
}
