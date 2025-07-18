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
        String  writeStr;
        StringBuilder stb , stbUnS;
        StringBuilder stbS;
        string[] lineWords;
        Thread thReadFile;
        Thread thDecode;
        Thread thWriteFile;
        private bool _run;
        private bool isReadCompleted = false;
        private readonly object lockStb = new Object();
        public event EventHandler<StringBuilder> DataReceived;
        public event EventHandler<String> ReadyTowrite;

        public Form1()
        {
            InitializeComponent();
            initThreads();
        }

        private void initThreads()
        {
            thReadFile = new Thread(ReadFileToStringBuilder);
            thReadFile.IsBackground = true;
            thDecode = new Thread(doCaesarCiper);
            _run = false;

            thWriteFile = new Thread(WriteStringToFile);
            thWriteFile.IsBackground = true;

            DataReceived += startProcess;
            ReadyTowrite += startTowrite;

        }

        private void WriteStringToFile()
        {
            var _sampleFilePath = "D:\\project\\testCaesar\\ceasarcpded33.txt";
            File.WriteAllText(_sampleFilePath, writeStr);
            txtMsg.Invoke(new Action(() =>
            {
                txtMsg.Text = "wited: " ;
            }));
        }

        private void startTowrite(object sender, string str)
        {
            writeStr = str;
            thWriteFile.Start();

        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            thReadFile.Start();
        }
        private void ReadFileToStringBuilder()
        {
          
            try
            {
                var _sampleFilePath = "D:\\project\\testCaesar\\test1.txt";
                var streamReader = new StreamReader(_sampleFilePath);

                // Part 1: create new FileInfo get Length.
                FileInfo info = new FileInfo(_sampleFilePath);
                long length = info.Length;
                long fileSizeinMbs = length / (1024 * 1024);

                var stb1 = new StringBuilder();
                string fileLine;
                while ((fileLine = streamReader.ReadLine()) != null)
                {
                    stb1.AppendLine(fileLine);
                }

                var sst = stb1.ToString();

                if (fileSizeinMbs > 1)
                {

                    #region .:Invoke all string to UI that it was wrong :.

                    /* int part_size = (int)(length / fileSizeinMbs); 
                     * var offSet = sst.Length / (int)part_size;
                    */
                    /* it was wrong coz ui hanged
                      for (int i = 0; i < part_size; i++)
                    {
                        var part = sst.Substring(i * offSet, offSet);
                        txtFile.Invoke(new Action(() =>
                        {
                            txtFile.Text += part;
                        }));
                    }
                     */
                    #endregion
                    // just show the last 1 Mb
                    var offSet = sst.Length / (int)fileSizeinMbs;
                    var part = sst.Substring((sst.Length - offSet), offSet);
                    txtFile.Invoke(new Action(() =>
                    {
                        txtFile.Text = part;
                    }));

                }
                else
                {
                    txtFile.Invoke(new Action(() =>
                    {
                        txtFile.Text = sst;
                    }));

                }

                DataReceived(this, stb1);
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
                    txtMsg.Text += "All data of File has read: \n";
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

                {
                    txtMsg.Invoke(new Action(() =>
                    {
                        txtMsg.Text += "Start encoding ... : \n";
                    }));
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
                  
                    var stSecured = stbS.ToString();
                    var part = stSecured.Substring((stSecured.Length - 500), 500);
                    txtsecured.Invoke(new Action(() =>
                    {
                        txtsecured.Text = part;
                        txtMsg.Text += "doCaesarCiper:\n ";
                    }));

                    stb.Clear();
                    stb = null;
                    _run = false;
                    
                    ReadyTowrite(this, stbS.ToString());
                    thDecode.Abort();
                }
                Thread.Sleep(2000);
            }

        }


    }
}
