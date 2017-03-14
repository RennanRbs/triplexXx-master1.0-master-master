using System;
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

            webcam = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo VideoCaptureDevice in webcam)
            {
                comboBox1.Items.Add(VideoCaptureDevice.Name);
            }
            comboBox1.SelectedIndex = 0;
            camera = new VideoCaptureDevice(webcam[comboBox1.SelectedIndex].MonikerString);
            camera.NewFrame += new NewFrameEventHandler(camera_NewFrame);
            camera.Start();
        }

        void camera_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap bit = (Bitmap)eventArgs.Frame.Clone();
            pictureBox1.Image = bit;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (camera.IsRunning)
            {
                camera.Stop();
                //saveFileDialog1.InitialDirectory = @"C:\Users\messyo\Desktop\github\triplexXx-master1.0-master\metodosMySql\Photos";
                /*
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    
                }*/
                //saveFileDialog1.FileName = this.CPF;
                if (CPF != "")
                {
                    pictureBox1.Image.Save(@"Photos\" + this.CPF + ".jpg");
                }

                
            }
            else
            {
                camera.Start();
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
    }
}
