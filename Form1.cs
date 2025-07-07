using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;
namespace CaesarCiper
{
    public partial class Form1 : Form
    {
        String line;
        StringBuilder stb, stbS, stbUnS;
        string[] lineWords;
        Thread thReadFile;
        Thread thDecode;
        private bool _run;
        private bool isReadCompleted = false;
        private readonly object lockStb = new Object();
        public event EventHandler<StringBuilder> DataReceived;

        public Form1()
        {
            InitializeComponent();
            initThreads();
        }

        private void initThreads()
        {
            thReadFile = new Thread(ReadFileToStringBuilder);
            thDecode = new Thread(doCaesarCiper);
            _run = false;

            DataReceived += startProcess;

        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            thReadFile.Start();
          
           

        }
        private void ReadFileToStringBuilder()
        {
            try
            {
                StreamReader sr = new StreamReader("D:\\project\\testCaesar\\ceasar.txt");
                //what is best scope for lock ?? just line 55 or all of them till line 63?
                lock (lockStb)
                {
                    stb = new StringBuilder(sr.ReadToEnd());

                    sr.Close();

                    txtFile.Invoke(new Action(() =>
                    {
                        txtFile.Text = stb.ToString();
                    }));

                }
                DataReceived(this, stb);
            }
            catch (Exception e)
            {

                txtMsg.Invoke(new Action(() =>
                 {
                     txtMsg.Text = "Exception: " + e.Message;
                 }));
            }
            finally
            {
                txtMsg.Invoke(new Action(() =>
                {
                    txtMsg.Text += "finally: \n";
                }));
            }

        }
        private void startProcess(object sender, StringBuilder stbGet)
        {
            stb = stbGet;
            thDecode.Start();
            _run = true;
            isReadCompleted = true;
        }
        private void doCaesarCiper()
        {

            while (_run)
            {
                if (!isReadCompleted)
                {
                   Thread.Sleep(20);
                    continue;
                }
                lock (lockStb)
                {
                    stbS = new StringBuilder();
                    int nShift = 3;
                    int i = 0;
                    Char ch;
                    progressBar1.Invoke(new Action(() =>
                    {
                        progressBar1.Minimum = 0;
                        progressBar1.Maximum = stb.Length;
                    }));

                    while (i < stb.Length)
                    {
                        ch = stb[i];
                        if (Char.IsLetter(stb[i]))
                        {
                            if (Char.IsUpper(stb[i]))
                            {
                                ch = (Char)(((int)stb[i] + nShift - 65) % 26 + 65);
                            }
                            else if (Char.IsLetter(stb[i]) && Char.IsLower(stb[i]))
                            {
                                ch = (Char)(((int)stb[i] + nShift - 97) % 26 + 97);
                            }

                        }
                        i++;

                        stbS.Append(ch);
                        progressBar1.Invoke(new Action(() =>
                        {
                            progressBar1.Value++;
                        }));

                    }
                    txtsecured.Invoke(new Action(() =>
                    {
                        txtsecured.Text = stbS.ToString();
                        txtMsg.Text += "doCaesarCiper:\n ";
                    }));
                    
                    stb.Clear();
                     stb = null;
                     _run = false;
                    thDecode.Abort();
                }
                Thread.Sleep(2000);
            }

        }

        private void btnSolve_Click(object sender, EventArgs e)
        {
            //  doSolved();
            //  txtSolved.Text = stbUnS.ToString(); ;
        }

    }
}
