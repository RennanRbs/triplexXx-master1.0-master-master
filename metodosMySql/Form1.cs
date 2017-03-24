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
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
        }
        public string passando { get; set; }
        
        MySqlConnection conectar = new MySqlConnection("Server=lamp01ppgcc.ddns.net; Database=cadastro_bolsistas_lit; Uid=controlador_lit; Pwd=123qwe!@#");

   //***************************************************** salvar Bolsista *****************************************************************************//
        private void button1_Click(object sender, EventArgs e)
        {
            
            bool m = false, t = false, n = false, ifce = false, outra = false, remunerado = false, voluntario = false;
            if (manha.Checked) { m = true; }
            if (tarde.Checked) { t = true; }
            if (noite.Checked) { n = true; }
            if (radioifce.Checked) { ifce = true; }
            if (radiooutra.Checked) { outra = true; }
            if (radiovoluntario.Checked) { voluntario = true; }
            if (radioremunerado.Checked) { remunerado = true; }
            try
            {
                
                MySqlDataReader rdr2;
                string buscaidprof = "select professor.id from professor,pessoa where pessoa.id = professor.pessoa_id and pessoa.nome = '" + entradaOrientador.Text + "';";
                conectar.Open();
                MySqlCommand comando2 = new MySqlCommand(buscaidprof, conectar);
                rdr2 = comando2.ExecuteReader();
                string idprof = "";
                if (rdr2.Read())
                {
                    idprof = rdr2.GetString("id");
                }
                conectar.Close();
                MySqlDataReader reader;
                MySqlDataReader reader2;
                conectar.Open();


                MySqlCommand comandoPessoa = new MySqlCommand("INSERT INTO Pessoa(nome,email,cpf,celular,cod_digital) VALUES('" + entradaNome.Text + "','" + entradaEmail.Text + "','" + entradaCpf.Text + "','" + entradaCelular.Text + "', '" + entradaID.Text + "' )", conectar);
                MySqlCommand comandoselect = new MySqlCommand(" select id from pessoa order by id DESC limit 1", conectar);
                //MySqlCommand comandoselectprofessor = new MySqlCommand("SELECT id FROM bolsista order by id desc limit 1", conectar); -> comando inutil
                MySqlCommand comandoselectremunerado = new MySqlCommand("SELECT id FROM bolsista order by id desc limit 1", conectar);
                MySqlCommand comandoinsertbol_proj = new MySqlCommand("select id from projetos where nome= '" + entradaProjeto.Text + "'", conectar);
                comandoPessoa.ExecuteNonQuery();

                reader = comandoselect.ExecuteReader();

                if (reader.Read())
                {
                    
                    MySqlCommand comandoBolsista = new MySqlCommand("INSERT INTO Bolsista(pessoa_id,endereco,bairro,rg,telefone,curso,matricula,instituicaodeensino,semestre,datadenascimento,cep,manha,tarde,noite,radioifce,radiooutra,radioremunerado,radiovoluntario,obs,ativar, orientador_id)" +
                        " VALUES(" + reader.GetString("id") + " , '" + entradaEndereço.Text + "','" + entradaBairro.Text + "','" + entradaRg.Text + "','" + entradaTelefone.Text + "','" + entradaCurso.Text + "','" + entradaMatriula.Text + "','" + entradaINstituiçao.Text + "'" +
                        " ,'" + entradaSemestre.Text + "','" + entradaDataDeNascimento.Text + "','" + entradaCep.Text + "'," + m + "," + t + "," + n + "," + ifce + "," + outra + ", " + remunerado + ", " + voluntario + " , '" + entradaOBS.Text + "'," + Ativar.Checked + ","+ idprof +");", conectar);

                    

                    reader.Close();
                    comandoBolsista.ExecuteNonQuery();
                    

                }

                reader = comandoselectremunerado.ExecuteReader();

                if (reader.Read())
                {
                    
                    MySqlCommand comandoremunerado = new MySqlCommand("INSERT INTO remunerado(bolsista_id,agencia,conta,fonte_bolsa,banco)VALUES (" + reader.GetString("id") + ", '" + entradaAgencia.Text + "', '" + entradaConta.Text + "', '" + entradaFonteDaBolsa.Text + "', '" + entradaBanco.Text + "')", conectar);
                    reader.Close();
                    comandoremunerado.ExecuteNonQuery();
                    
                }
                reader = comandoselectremunerado.ExecuteReader();
                reader.Read();
                string id_bol = reader.GetString("id");
                reader.Close();
               
                reader2 = comandoinsertbol_proj.ExecuteReader();
                reader2.Read();
                string id_proj = reader2.GetString("id");
                reader2.Close();
                
                MySqlCommand insertbol_proj = new MySqlCommand("insert into bolsista_projeto values(default," + id_bol + "," + id_proj + ");", conectar);
                insertbol_proj.ExecuteNonQuery();
                    
                    
                conectar.Close();
                MessageBox.Show("Salvo com sucesso, que Topper!");

                buttonFoto.Enabled = true;
                Form3 maisumform = new Form3(this, entradaCpf.Text);
                maisumform.ShowDialog();
                pictureBoxFoto.ImageLocation = @"Photos\" + entradaCpf.Text + ".jpg";
                

                

                
            }




            catch (Exception error)
            {
                MessageBox.Show("error.." + error.Message + "   Contate o suporte");
                conectar.Close();
            }
            /*
            Form3 maisumform = new Form3(this, entradaCpf.Text);
            maisumform.ShowDialog();
            pictureBoxFoto.ImageLocation = @"Photos\" + entradaCpf.Text + ".jpg";*/
            //pictureBoxFoto.Load("http://i2.kym-cdn.com/photos/images/facebook/000/862/065/0e9.jpg");
        }
        // ******************************************************* botao para buscar aluno ************************************************//
        private void button2_Click(object sender, EventArgs e)
        {
            
            if (entradaIDLit.Text != "")
            {
                try
                {

                    MySqlCommand comando;
                    MySqlDataReader reader;
                    conectar.Open();

                    string selecionarpessoa = "select (select nome from pessoa where id = "+ entradaIDLit.Text + ") as nomebolsista," +
                                              "(select nome from pessoa, professor, bolsista where bolsista.pessoa_id = " + entradaIDLit.Text + " and bolsista.orientador_id = professor.id and professor.pessoa_id = pessoa.id) as nomeorientador,"+
                                              "(select nome from projetos, bolsista, bolsista_projeto where projetos.id = bolsista_projeto.projeto_id and bolsista_projeto.bolsista_id = bolsista.id and bolsista.pessoa_id = " + entradaIDLit.Text + ") as nomeprojeto," +
                                              "bolsista.ativar,bolsista.obs,remunerado.banco ,remunerado.agencia,remunerado.conta,remunerado.fonte_bolsa," +
                                              "bolsista.semestre, bolsista.endereco, bolsista.bairro, bolsista.rg, bolsista.instituicaodeensino,bolsista.cep ," +
                                              "bolsista.matricula, bolsista.curso, bolsista.telefone, pessoa.cod_digital, pessoa.celular,bolsista.datadenascimento" +
                                              " ,pessoa.email, pessoa.cpf, bolsista.manha,bolsista.tarde,bolsista.noite,bolsista.radioifce,bolsista.radiooutra,"+
                                              "bolsista.radioremunerado,bolsista.radiovoluntario " +
                                              " from bolsista,pessoa,remunerado where bolsista.pessoa_id =  pessoa.id and bolsista.id = remunerado.bolsista_id and ativar = False and  pessoa.id = " + entradaIDLit.Text + "; ";



                    comando = new MySqlCommand(selecionarpessoa, conectar);

                    reader = comando.ExecuteReader();


                    if (reader.Read())
                    {
                        entradaMatriula.Text = reader.GetString("matricula");
                        entradaCelular.Text = reader.GetString("celular");
                        entradaCpf.Text = reader.GetString("cpf");
                        entradaEmail.Text = reader.GetString("email");
                        entradaCep.Text = reader.GetString("cep");
                        entradaEndereço.Text = reader.GetString("endereco");
                        entradaNome.Text = reader.GetString("nomebolsista");
                        entradaSemestre.Text = reader.GetString("semestre");
                        entradaBairro.Text = reader.GetString("bairro");
                        entradaRg.Text = reader.GetString("rg");
                        entradaINstituiçao.Text = reader.GetString("instituicaodeensino");
                        entradaTelefone.Text = reader.GetString("telefone");
                        entradaDataDeNascimento.Text = reader.GetString("datadenascimento");
                        entradaCurso.Text = reader.GetString("curso");
                        entradaAgencia.Text = reader.GetString("agencia");
                        entradaConta.Text = reader.GetString("conta");
                        entradaFonteDaBolsa.Text = reader.GetString("fonte_bolsa");
                        entradaBanco.Text = reader.GetString("banco");
                        entradaOBS.Text = reader.GetString("obs");
                        entradaID.Text = reader.GetString("cod_digital");

                        string prof = reader.GetString("nomeorientador");
                        for(int i = 0; i<=entradaOrientador.Items.Count; i++)
                        {
                            if (prof == entradaOrientador.Items[i].ToString())
                            {
                                entradaOrientador.SelectedIndex = i;
                                
                                break;
                            }
                            
                        }

                        string proj = reader.GetString("nomeprojeto");
                        for (int i = 0; i <= entradaProjeto.Items.Count; i++)
                        {
                            if (proj == entradaProjeto.Items[i].ToString())
                            {
                                entradaProjeto.SelectedIndex = i;

                                break;
                            }
                            
                        }


                        if (reader.GetBoolean("manha")) { manha.Checked = true; }
                        if (reader.GetBoolean("tarde") ) { tarde.Checked = true; }
                        if (reader.GetBoolean("noite")) { noite.Checked= true; }
                        if (reader.GetBoolean("radioifce")) { radioifce.Checked = true; }
                        if (reader.GetBoolean("radiooutra")) { radiooutra.Checked = true; }
                        if (reader.GetBoolean("radioremunerado")) { radioremunerado.Checked = true; }
                        if (reader.GetBoolean("radiovoluntario")) { radiovoluntario.Checked = true; }
                        if (reader.GetBoolean("ativar")) { Ativar.Checked = true; }
                        reader.Close();

                       
                        
                            
                        
                        

                        pictureBoxFoto.ImageLocation = @"Photos\" + entradaCpf.Text + ".jpg";
                        buttonFoto.Enabled = true;


                    }
                    else {
                        MessageBox.Show("sem dados com esse nome");
                        Ativar.Checked = false;
                    }




                    conectar.Close();
                    
                    

                }




                catch (Exception error)
                {
                    MessageBox.Show("error.." + error.Message);
                    conectar.Close();
                }
            }
            else { MessageBox.Show("Campo  em Branco"); }

           
        }
    //****************************botao para dar update no aluno ********************************************//
        private void button3_Click_1(object sender, EventArgs e)

        {         
                try
            {
                MySqlDataReader rdr;
                string buscaidproj = "select id from projetos where nome = '"+entradaProjeto.Text+"';";
                conectar.Open();
                MySqlCommand comando = new MySqlCommand(buscaidproj, conectar);
                rdr = comando.ExecuteReader();
                string idproj = "";
                if (rdr.Read())
                {
                    idproj = rdr.GetString("id");
                }
                conectar.Close();
                MySqlDataReader rdr2;
                string buscaidprof = "select professor.id from professor,pessoa where pessoa.id = professor.pessoa_id and pessoa.nome = '" + entradaOrientador.Text + "';";
                conectar.Open();
                MySqlCommand comando2 = new MySqlCommand(buscaidprof, conectar);
                rdr2 = comando2.ExecuteReader();
                string idprof = "";
                if (rdr2.Read())
                {
                    idprof = rdr2.GetString("id");
                }
                conectar.Close();
                string UpdatePessoa = "UPDATE pessoa,bolsista,remunerado,bolsista_projeto SET cod_digital = '" + entradaID.Text + "',obs = '"+entradaOBS.Text+"',fonte_bolsa = '"+entradaFonteDaBolsa.Text+"',orientador_id = '"+ idprof+"',banco = '"+entradaBanco.Text+"',"+
                    "conta = '"+entradaConta.Text+"',agencia = '"+entradaAgencia.Text+"',rg = '"+entradaRg.Text+"',radioremunerado = "+radioremunerado.Checked+",radiovoluntario = "+radiovoluntario.Checked+","+
                    "ativar = "+Ativar.Checked+",radiooutra = "+radiooutra.Checked+",radioifce = "+ radioifce.Checked+",manha = "+manha.Checked+",tarde ="+tarde.Checked+",noite = "+noite.Checked+"  ,email = '"+entradaEmail.Text+"', cep= '"+entradaCep.Text+"' ,cpf = '"+entradaCpf.Text+"',bairro = '"+entradaBairro.Text+"',datadenascimento = '"+entradaDataDeNascimento.Text+"', telefone = '"+entradaTelefone.Text+"',instituicaodeensino = '"+entradaINstituiçao.Text+"',matricula = '"+entradaMatriula.Text+"',semestre = '"+entradaSemestre.Text+"', celular = '"+entradaCelular.Text+ "',curso = '"+entradaCurso.Text+"',  nome= '" + entradaNome.Text + "', endereco = '"+ entradaEndereço.Text+ "'" +
                    ",projeto_id = '"+ idproj +
                    "' where bolsista_projeto.bolsista_id = bolsista.id and bolsista.pessoa_id =  pessoa.id and bolsista.id = remunerado.bolsista_id and pessoa.id ='" +entradaIDLit.Text +"';";
                
                conectar.Open();
                 MySqlCommand comando3 = new MySqlCommand(UpdatePessoa,conectar);
                
                 if ( comando3.ExecuteNonQuery()== 4) { MessageBox.Show("dados atualizados"); } else { MessageBox.Show("nao atualizado"); }
                 conectar.Close();
            }
            catch (Exception error)
            {
                MessageBox.Show("error.." + error.Message);
                conectar.Close();
            }
        }










 //*************************************************************limpar a tabela **********************************************//
        private void button4_Click(object sender, EventArgs e)
        {

            DialogResult dialogResult = MessageBox.Show("Desejar mesmo apagar o formulario?", "                            Lit 2017", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {

                foreach (Control ctrl in Controls)
                {

                    if (ctrl is TextBox)
                    {
                        ((TextBox)(ctrl)).Text = String.Empty;
                    }

                    if (ctrl is MaskedTextBox)
                    {
                        ((MaskedTextBox)(ctrl)).Text = String.Empty;
                    }
                }
                entradaOBS.Text = "";
                radioifce.Checked = true;
                radiooutra.Checked = false;
                radiovoluntario.Checked = true;
                radioremunerado.Checked = false;
                manha.Checked = false;
                tarde.Checked = false;
                noite.Checked = false;
                radiooutra.Checked = false;
                radioifce.Checked = false;
                Ativar.Checked = false;

                entradaProjeto.SelectedIndex = 0;
                entradaOrientador.SelectedIndex = -1;
            }
            else if (dialogResult == DialogResult.No) { }
            pictureBoxFoto.ImageLocation = @"metodosMySql\Resources\" + "blank_user" + ".jpg";
            buttonFoto.Enabled = false;
        }


//************************************************* abrir aba de busca ********************************************************//
        private void button5_Click(object sender, EventArgs e)
        {
            passando = entradaNome.Text;
            
            Form2 outroform = new Form2(passando);
            outroform.Show();
            

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            conectar.Open();
            MySqlDataReader reader;
            MySqlDataReader reader2;
            string projetos = "select nome from projetos;";
            MySqlCommand buscarprojetos = new MySqlCommand(projetos, conectar);
            reader = buscarprojetos.ExecuteReader();
            while (reader.Read())
            {
                entradaProjeto.Items.Add(reader.GetString("nome")); 
            }
            conectar.Close();
            entradaProjeto.SelectedIndex = 0;
            conectar.Open();
            string professores = "select pessoa.nome from pessoa, professor where pessoa.id = professor.pessoa_id;";
            MySqlCommand buscaprofessores = new MySqlCommand(professores, conectar);
            reader2 = buscaprofessores.ExecuteReader();
            
            while (reader2.Read())
            {
                entradaOrientador.Items.Add(reader2.GetString("nome"));
            }
            conectar.Close();
        }

        private void pictureBoxFoto_Click(object sender, EventArgs e)
        {
            if (entradaCpf.Text != "")
            {
                Form4 maisumoutroform = new Form4(entradaCpf.Text);
                maisumoutroform.ShowDialog();
            }
        }

//************************************************** Colocar imagem *********************************************

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void buttonFoto_Click(object sender, EventArgs e)
        {
            Form3 maisumform = new Form3(this,entradaCpf.Text);
            maisumform.ShowDialog();
            pictureBoxFoto.ImageLocation = @"Photos\" + entradaCpf.Text + ".jpg";
            
        }

        private void entradaCpf_TextChanged(object sender, EventArgs e)
        {

        }

        private void Ativar_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void entradaNome_TextChanged(object sender, EventArgs e)
        {

        }

        private void entradaProjeto_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void radioifce_CheckedChanged(object sender, EventArgs e)
        {
            if(radioifce.Checked == true)
            {
                entradaINstituiçao.Enabled = false;
            }
            else
            {
                entradaINstituiçao.Enabled = true;
            }
        }

        private void entradaIDLit_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                button2.PerformClick();
                e.Handled = true;
            }
        }

        private void entradaNome_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                button5.PerformClick();
                e.Handled = true;
            }
        }
    }
}
