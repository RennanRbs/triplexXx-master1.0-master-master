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
                MySqlCommand tabela = new MySqlCommand("Select pessoas.id, pessoas.cod_digital, pessoas.nome From pessoas, bolsistas where bolsistas.ativar = 0 and pessoas.id = bolsistas.pessoa_id;", conectar);
                adapter.SelectCommand = tabela;
                adapter.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];

                DataSet ds2 = new DataSet();
                MySqlCommand tabela2 = new MySqlCommand("Select pessoas.id, pessoas.cod_digital, pessoas.nome From pessoas, bolsistas where bolsistas.ativar = 1 and pessoas.id = bolsistas.pessoa_id;", conectar);
                adapter.SelectCommand = tabela2;
                adapter.Fill(ds2);
                dataGridView2.DataSource = ds2.Tables[0];
                label1.Text = "Todos";
            }

            else
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                DataSet ds = new DataSet();
                MySqlCommand tabela = new MySqlCommand("Select id, cod_digital, nome From pessoas WHERE nome LIKE  '%"+ label1.Text +"%'  ", conectar);
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
            Form5 login = new Form5();
            login.ShowDialog();
            if (login.autenticarLogin)
            {
                
                Form6 outro = new Form6();
                /*string cod = outro.cod;
                string id = this.dataGridView2.CurrentRow.Cells[0].Value.ToString();
                string comando = "UPDATE pessoas, bolsistas SET pessoas.cod_digital = " + cod + ", bolsistas.ativar = 0 WHERE pessoas.id = " + id + " and bolsistas.pessoa_id = " + id + ";";
                */
                conectar.Open();
                bool deucerto = false;
                while (deucerto == false)
                {
                    //MySqlCommand update = new MySqlCommand(comando, conectar);
                    try
                    {
                        outro.ShowDialog();
                        string cod = outro.cod;
                        string id = this.dataGridView2.CurrentRow.Cells[0].Value.ToString();
                        string comando = "UPDATE pessoas, bolsistas SET pessoas.cod_digital = " + cod + ", bolsistas.ativar = 0 WHERE pessoas.id = " + id + " and bolsistas.pessoa_id = " + id + ";";
                        MySqlCommand update = new MySqlCommand(comando, conectar);
                        update.ExecuteNonQuery();
                        deucerto = true;
                    }
                    catch
                    {
                        MessageBox.Show("Já existe um cadastro com esse codigo!");
                    }
                }
                if (deucerto)
                {
                    MySqlDataAdapter adapter = new MySqlDataAdapter();
                    MySqlDataAdapter adapter2 = new MySqlDataAdapter();
                    DataSet ds = new DataSet();
                    MySqlCommand tabela = new MySqlCommand("Select pessoas.id, pessoas.cod_digital, pessoas.nome From pessoas, bolsistas where bolsistas.ativar = 0 and pessoas.id = bolsistas.pessoa_id;", conectar);
                    adapter.SelectCommand = tabela;
                    adapter.Fill(ds);
                    dataGridView1.DataSource = ds.Tables[0];

                    DataSet ds2 = new DataSet();
                    MySqlCommand tabela2 = new MySqlCommand("Select pessoas.id, pessoas.cod_digital, pessoas.nome From pessoas, bolsistas where bolsistas.ativar = 1 and pessoas.id = bolsistas.pessoa_id;", conectar);
                    adapter2.SelectCommand = tabela2;
                    adapter2.Fill(ds2);
                    dataGridView2.DataSource = ds2.Tables[0];
                }
                conectar.Close();
            }
        }
    }
}

