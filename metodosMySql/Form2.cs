using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace metodosMySql
{
    public partial class Form2 : Form
    {




        MySqlConnection conectar = new MySqlConnection("Server=lamp01ppgcc.ddns.net; Database=cadastro_bolsistas_lit; Uid=controlador_lit; Pwd=123qwe!@#");

        public Form2(string cor)
        {
            InitializeComponent();
            label1.Text = cor;

        }

        private void Form2_Load(object sender, EventArgs e)
        {


            if (label1.Text == "")
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                DataSet ds = new DataSet();
                MySqlCommand tabela = new MySqlCommand("Select pessoa.id, pessoa.nome From pessoa, bolsista where bolsista.ativar = 0 and pessoa.id = bolsista.pessoa_id;", conectar);
                adapter.SelectCommand = tabela;
                adapter.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];

                DataSet ds2 = new DataSet();
                MySqlCommand tabela2 = new MySqlCommand("Select pessoa.id, pessoa.nome From pessoa, bolsista where bolsista.ativar = 1 and pessoa.id = bolsista.pessoa_id;", conectar);
                adapter.SelectCommand = tabela2;
                adapter.Fill(ds2);
                dataGridView2.DataSource = ds2.Tables[0];
                label1.Text = "Todos";
            }

            else
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                DataSet ds = new DataSet();
                MySqlCommand tabela = new MySqlCommand("Select id,nome From pessoa WHERE nome LIKE  '%"+ label1.Text +"%'  ", conectar);
                adapter.SelectCommand = tabela;
                adapter.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
            }

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            string id = this.dataGridView2.CurrentRow.Cells[0].Value.ToString();            
            string comando = "UPDATE bolsista SET ativar = 0 WHERE pessoa_id =" + id + ";";
            conectar.Open();
            MySqlCommand update = new MySqlCommand(comando, conectar);
            update.ExecuteNonQuery();

            if (update.ExecuteNonQuery() == 1)
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                DataSet ds = new DataSet();
                MySqlCommand tabela = new MySqlCommand("Select pessoa.id, pessoa.nome From pessoa, bolsista where bolsista.ativar = 0 and pessoa.id = bolsista.pessoa_id;", conectar);
                adapter.SelectCommand = tabela;
                adapter.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];

                DataSet ds2 = new DataSet();
                MySqlCommand tabela2 = new MySqlCommand("Select pessoa.id, pessoa.nome From pessoa, bolsista where bolsista.ativar = 1 and pessoa.id = bolsista.pessoa_id;", conectar);
                adapter.SelectCommand = tabela2;
                adapter.Fill(ds2);
                dataGridView2.DataSource = ds2.Tables[0];
            }
            conectar.Close();
            

        }
    }

        
       

    }

