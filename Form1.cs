using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Media;
using System.Runtime.InteropServices;


namespace MyAudioPlayer
{
    public partial class Form1 : Form
    {
        [DllImport("winmm.dll")]
        private static extern long mciSendString(string strCommand,StringBuilder strReturn, int iReturnLength,IntPtr hwndCallback);

        bool mousepressing = false;
        int MX;
        int MY;
        string File=null;
        bool isplay = false;
        bool isnew = true;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            mousepressing = true;
            MX = e.X;
            MY = e.Y;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MX = 0;
            MY = 0;

        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            mousepressing = false;
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mousepressing)
            {
                Left -= (MX - e.X);
                Top -= (MY - e.Y);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!isplay)
            {
                string sCommand = null;
                if (isnew)
                {
                    try
                    {
                        string sCommand2 = "close MediaFile notify";
                        mciSendString(sCommand2, null, 0, this.Handle);
                    }
                    catch { }
                    if (File.Substring(File.Length - 3, 3) == "wav")
                    {
                        sCommand = "open \"" + File + "\" type mpegvideo alias MediaFile";
                        mciSendString(sCommand, null, 0, IntPtr.Zero);
                        if (button6.Top > 500) 
                        {
                            button6.Top -= 500;
                            Height -= 500;
                        }
                    }
                    else if (File.Substring(File.Length - 3, 3) == "avi") 
                    {
                        sCommand = "open \"" + File + "\" type mpegvideo alias MediaFile parent " + panel1.Handle.ToInt32().ToString() + " style child";
                        mciSendString(sCommand, null, 0, IntPtr.Zero);

                        if (button6.Top < 500)
                        {
                            button6.Top += 500;
                            Height += 500;
                        }
                    }
                }
                sCommand = "play MediaFile notify";
                mciSendString(sCommand, null, 0, this.Handle);
                isplay = true;
                button3.Text = "Pause";
            }
            else
            {
                string sCommand = "pause MediaFile notify";
                mciSendString(sCommand, null, 0, this.Handle);
                isplay = false;
                button3.Text = "Resume";
                isnew = false;
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string file = null;
            openFileDialog1.Filter = "WAV files (*.wav)|*.wav|AVI files (*.avi)|*.avi|All files|*.*";
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                file = openFileDialog1.FileName;
            }
            try
            {
                File = file;
                label1.Text = File;
                label1.Text += "   ";
                stoping();
            }
            catch { }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            stoping();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (label1.Text.Length > 2)
            {
                string l = label1.Text;
                label1.Text = label1.Text.Substring(1);
                label1.Text += l.Substring(0, 1);
            }
        }
        private void stoping() 
        {
            string sCommand = "seek MediaFile notify";
            mciSendString(sCommand, null, 0, this.Handle);
            sCommand = "stop MediaFile notify";
            mciSendString(sCommand, null, 0, this.Handle);
            isplay = false;
            isnew = true;
            button3.Text = "Play";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string sCommand = "set cdaudio door open";
            mciSendString(sCommand, null, 127, this.Handle);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (button6.Top < 500)
            {
                button6.Top += 500;
                Height += 500;
            }
            else
            {
                button6.Top -= 500;
                Height -= 500;
            }
        }
    }
}
