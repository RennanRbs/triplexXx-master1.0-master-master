using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace metodosMySql
{
    public partial class Form4 : Form
    {
        private String cpf;
        public Form4(String CPF)
        {
            InitializeComponent();
            cpf = CPF;
        }
        
        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void Form4_Load(object sender, EventArgs e)
        {
            pictureBox1.ImageLocation = @"Photos\" + cpf + ".jpg";
        }
    }
}
