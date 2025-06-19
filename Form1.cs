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
        StringBuilder stb, stbS;
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
            }
            txtsecured.Text = stbS.ToString();
            txtMsg.Text += "doCaesarCiper:\n ";
            Thread.Sleep(2000);
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

    }
}
