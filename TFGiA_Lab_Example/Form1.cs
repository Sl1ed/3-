using System;
using System.Windows.Forms;

namespace TFGiA_Lab_Example
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            textBox1.AppendText("abc [[ 010 ]] 010 abc abc" + "\r\n");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox2.Clear();
            uSynt Synt = new uSynt();
            Synt.Lex.text = textBox1.Lines;
            try
            {
                Synt.Lex.NextToken();
                Synt.S();
                throw new Exception("Текст верный");
            }
            catch (Exception exc)
            {
                textBox2.Text += exc.Message;
            }

        }
    }
}
