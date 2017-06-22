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
        private String ID_LIT;
        public Form3(Form1 seila, String ID_LIT)
        {
            InitializeComponent();
            this.nsei = seila;
            this.ID_LIT = ID_LIT;

        }
        private FilterInfoCollection webcam;
        private VideoCaptureDevice camera;

        private void Form3_Load(object sender, EventArgs e)
        {

            webcam = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo VideoCaptureDevice in webcam)
            {
                comboBox1.Items.Add(VideoCaptureDevice.Name);
            }            
            this.camera = new VideoCaptureDevice(webcam[0].MonikerString);
            this.camera.NewFrame += new NewFrameEventHandler(camera_NewFrame);
            camera.Start();
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

                if (this.ID_LIT != "")
                {
                    try
                    {
                        pictureBox1.Image.Save(@"Photos\" + this.ID_LIT + ".jpg");
                    }
                    catch (Exception)
                    {
                        var folder = Directory.CreateDirectory("Photos\\");
                        pictureBox1.Image.Save(@"Photos\" + this.ID_LIT + ".jpg");
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
            camera.Stop();
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
