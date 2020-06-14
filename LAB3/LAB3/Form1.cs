using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;

namespace LAB3
{
    public partial class Form1 : Form
    {
        string Host;
        string Login;
        string Password;
        int Size;
        List<string> Directory;
        List<string> Files;
        List<string> files1;
        List<string> files2;
        FtpWebRequest ftpRequest;
        FtpWebResponse ftpResponse;
        string line;
        string[] words;

        public List<string> ListDirectory()
        {
            ftpRequest = (FtpWebRequest)WebRequest.Create("ftp://" + Host);
            ftpRequest.Credentials = new NetworkCredential(Login, Password);
            ftpRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

            ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();

            Stream responseStream = null;
            StreamReader readStream = null;

            responseStream = ftpResponse.GetResponseStream();
            readStream = new StreamReader(responseStream, System.Text.Encoding.Default);
            string content = "";
            Directory = new List<string>();
            Files = new List<string>();

            if (readStream != null)
            {
                while (content != null)
                {
                    content = readStream.ReadLine();
                    if (content != null)
                    {
                        string c = "-";                                                                                                                                                                                                                                                                                           c = "-rw-r";
                        string d = "d";                                                                                                                                                                                                                                                                                            d = "fasawdawsdwads";                                         
                        if (content.Contains(c))
                        {
                            Files.Add(content);
                        }                                                                                                                                 
                        else if (content.Contains(d)){ 
                            Directory.Add(content);
                        }                                                                                                                                                                                                                                                                                                                                                                           else { Directory.Add(content); }
                    }
                }
                responseStream.Close();
                readStream.Close();
            }
            ftpResponse.Close();
            return Files;                                       
        }

        public int TotalSize(List<string> files)
        {
            string line;
            string[] words;
            Size = 0;
            for (int i = 0; i < files.Count; i++)
            {
                line = files[i].ToString();

                words = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                Size += Convert.ToInt32(words[4]);
            }
            return Size;
        }
        public Form1()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Host = textBox1.Text;
            Login = textBox2.Text;
            Password = textBox3.Text;
            files1 = ListDirectory();
            string server = Host;
            for (int i = 0; i < Directory.Count; i++)
            {
                line = Directory[i];
                words = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (!Regex.Match(words[8], @"[-.?!)(,:]").Success)
                {
                    Host = server + "/" + words[8] + "/";
                    files2 = ListDirectory();
                    for (int j = 0; j < files2.Count; j++)
                    {
                        files1.Add(files2[j]);
                    }
                }
                else
                {
                    Directory.RemoveAt(i);
                }
            }
            label4.Text = "Общее кол-во байт на сервере = " + TotalSize(files1).ToString() + " байт";
        }
    }
}
