using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;

namespace metodosMySql
{
    public partial class Form3 : Form
    {
        private Form1 nsei;
        private String CPF;
        public Form3(Form1 seila, String CPF)
        {
            InitializeComponent();
            this.nsei = seila;
            this.CPF = CPF;

        }
        private FilterInfoCollection webcam;
        private VideoCaptureDevice camera;

        private void Form3_Load(object sender, EventArgs e)
        {

            this.webcam = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo VideoCaptureDevice in webcam)
            {
                comboBox1.Items.Add(VideoCaptureDevice.Name);
            }
            //this.comboBox1.SelectedIndex = 0;
            this.camera = new VideoCaptureDevice(webcam[0].MonikerString);
            this.camera.NewFrame += new NewFrameEventHandler(camera_NewFrame);
            this.camera.Start();
        }

        void camera_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap bit = (Bitmap)eventArgs.Frame.Clone();
            pictureBox1.Image = bit;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.camera.IsRunning)
            {
                camera.Stop();
                //saveFileDialog1.InitialDirectory = @"C:\Users\messyo\Desktop\github\triplexXx-master1.0-master\metodosMySql\Photos";
                /*
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    
                }*/
                //saveFileDialog1.FileName = this.CPF;
                if (this.CPF != "")
                {
                    try
                    {
                        pictureBox1.Image.Save(@"Photos\" + this.CPF + ".jpg");
                    }
                    catch (Exception)
                    {
                        var folder = Directory.CreateDirectory("Photos\\");
                        pictureBox1.Image.Save(@"Photos\" + this.CPF + ".jpg");
                    }
                }


            }
            else
            {
                this.camera.Start();
            }
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
        
        public void Form3_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.camera.Stop();

        }

        private void saveFileDialog2_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex != 0)
            {
                this.camera.Stop();
                this.camera = new VideoCaptureDevice(webcam[comboBox1.SelectedIndex].MonikerString);
                this.camera.NewFrame += new NewFrameEventHandler(camera_NewFrame);
                this.camera.Start();
            }
            else
            {
                if (this.camera.IsRunning == true)
                {
                    this.camera.Stop();
                    this.camera = new VideoCaptureDevice(webcam[comboBox1.SelectedIndex].MonikerString);
                    this.camera.NewFrame += new NewFrameEventHandler(camera_NewFrame);
                    this.camera.Start();
                }

            }
        }
    }
}
