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

        public Form1()
        {
            InitializeComponent();
        }
        private void btnStart_Click(object sender, EventArgs e)
        {
            ReadFileToStringBuilder();
            doCaesarCiper();
        }
        private void ReadFileToStringBuilder()
        {
            try
            {
                //Pass the file path and file name to the StreamReader constructor
                StreamReader sr = new StreamReader("D:\\project\\testCaesar\\ceasar.txt");
                //Read the first line of text
                stb = new StringBuilder(sr.ReadToEnd());
                sr.Close();

                txtFile.Text = stb.ToString(); 
            }
            catch (Exception e)
            {
                txtMsg.Text = "Exception: " + e.Message;
            }
            finally
            {
                txtMsg.Text += "finally: \n";
            }

        }
        private void doCaesarCiper()
        {
            stbS = new StringBuilder();
            int nShift = 3;
            int i = 0;
            Char ch;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = stb.Length;
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
                progressBar1.Value++;
            }
            txtsecured.Text = stbS.ToString();
            txtMsg.Text += "doCaesarCiper:\n ";
            Thread.Sleep(2000);
        }

        private void btnSolve_Click(object sender, EventArgs e)
        {
            doSolved();
            txtSolved.Text = stbUnS.ToString(); ;
        }

        private void ReadFile()
        {
            try
            {
                //Pass the file path and file name to the StreamReader constructor
                StreamReader sr = new StreamReader("D:\\project\\testCaesar\\ceasar.txt");
                //Read the first line of text
                line = sr.ReadLine();
                //Continue to read until you reach end of file
                while (line != null)
                {
                    //write the line to console window
                    txtFile.Text += line;
                    //Read the next line
                    line = sr.ReadLine();
                    // lineWords = line.Split(' ');
                }
                //close the file
                sr.Close();
                Console.ReadLine();
                txtMsg.Text += "Read Finished \n";
            }
            catch (Exception e)
            {
                txtMsg.Text = "Exception: " + e.Message;
            }
            finally
            {
                txtMsg.Text += "finally: \n";
            }
            Thread.Sleep(2000);
        }
       // to Do 
        private void doSolved()
        {
            stbUnS = new StringBuilder();
            int nShift = 3;
            int i = 0;
            Char ch;
            while (i < stbS.Length)
            {
                ch = stbS[i];
                if (Char.IsLetter(stbS[i]))
                {
                    if (Char.IsUpper(stbS[i]))
                    {
                        ch = (Char)(((int)stbS[i] - nShift) + 26 );
                    }
                    else if (Char.IsLower(stbS[i]))
                    {
                        ch = (Char)(((int)stbS[i] - nShift) + 26);
                    }

                }
                i++;

                stbUnS.Append(ch);
            }
            txtsecured.Text = stbS.ToString();
            txtMsg.Text += "doCaesarCiper:\n ";
            Thread.Sleep(2000);
        }
    }
}
