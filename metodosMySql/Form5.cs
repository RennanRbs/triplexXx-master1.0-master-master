using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace metodosMySql
{
    public partial class Form5 : Form
    {
        public bool autenticarLogin = false;
        public Form5()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void btEntrar_Click(object sender, EventArgs e)
        {
            // Instancia uma nova conexão com o banco de dados
            //MySqlConnection conectar = new MySqlConnection("server=localhost;database=cadastro;uid=root;pwd=;"); // localhost
            MySqlConnection conectar = new MySqlConnection("Server=lamp01ppgcc.ddns.net; Database=cadastro_bolsistas_lit; Uid=controlador_lit; Pwd=123qwe!@#"); // Servidor
            // Coleta as informações dos campos de Login
            string userid = txtUsuario.Text;
            string password = txtSenha.Text;
            //
            MySqlCommand cmd = new MySqlCommand("select usuario,senha from adm where usuario='" + txtUsuario.Text + "'and senha='" + txtSenha.Text + "'", conectar);
            //
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            //
            DataTable dt = new DataTable();
            //
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                MessageBox.Show("Login sucesso");
                this.Close();
                autenticarLogin = true;

            }
            else
            {
                MessageBox.Show("Login invalido checar usuario e senha");
            }
            conectar.Close();
        }

        private void btCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtUsuario_TextChanged(object sender, EventArgs e)
        {

        }

        private void textSenha_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
