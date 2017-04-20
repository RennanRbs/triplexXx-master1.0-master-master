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
            /*MySqlCommand buscacpf;
            if (cpforiginal == "")
            {
               buscacpf = new MySqlCommand("select cpf from pessoa where cpf =" + entradaCpf.Text + ";", conectar);
            }
            else
            {
               buscacpf = new MySqlCommand("select cpf from pessoa where cpf =" + cpforiginal + ";", conectar);
            }*/

            if (entradaID.Text != "")
            {
                conectar.Open();
                try
                {
                    if (entradaIDLit.Text == "")
                    {
                        conectar.Close();

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
                            string buscaidprof = "select professors.id from professors,pessoas where pessoas.id = professors.pessoa_id and pessoas.nome = '" + entradaOrientador.Text + "';";
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


                            MySqlCommand comandoPessoa = new MySqlCommand("INSERT INTO Pessoas(nome,email,cpf,celular,cod_digital) VALUES('" + entradaNome.Text + "','" + entradaEmail.Text + "','" + entradaCpf.Text + "','" + entradaCelular.Text + "', '" + entradaID.Text + "' )", conectar);
                            MySqlCommand comandoselect = new MySqlCommand(" select id from pessoas order by id DESC limit 1", conectar);
                            //MySqlCommand comandoselectprofessor = new MySqlCommand("SELECT id FROM bolsista order by id desc limit 1", conectar); -> comando inutil
                            MySqlCommand comandoselectremunerado = new MySqlCommand("SELECT id FROM bolsistas order by id desc limit 1", conectar);
                            MySqlCommand comandoinsertbol_proj = new MySqlCommand("select id from projetos where nome= '" + entradaProjeto.Text + "'", conectar);
                            comandoPessoa.ExecuteNonQuery();

                            reader = comandoselect.ExecuteReader();

                            if (reader.Read())
                            {

                                MySqlCommand comandoBolsista = new MySqlCommand("INSERT INTO Bolsistas(pessoa_id,endereco,bairro,rg,telefone,curso,matricula,instituicaodeensino,semestre,datadenascimento,cep,manha,tarde,noite,radioifce,radiooutra,radioremunerado,radiovoluntario,obs,ativar, orientador_id)" +
                                    " VALUES(" + reader.GetString("id") + " , '" + entradaEndereço.Text + "','" + entradaBairro.Text + "','" + entradaRg.Text + "','" + entradaTelefone.Text + "','" + entradaCurso.Text + "','" + entradaMatriula.Text + "','" + entradaINstituiçao.Text + "'" +
                                    " ,'" + entradaSemestre.Text + "','" + entradaDataDeNascimento.Text + "','" + entradaCep.Text + "'," + m + "," + t + "," + n + "," + ifce + "," + outra + ", " + remunerado + ", " + voluntario + " , '" + entradaOBS.Text + "'," + Ativar.Checked + "," + idprof + ");", conectar);



                                reader.Close();
                                comandoBolsista.ExecuteNonQuery();


                            }

                            if (radioremunerado.Checked)
                            {
                                reader = comandoselectremunerado.ExecuteReader();
                                reader.Read();
                                MySqlCommand comandoremunerado = new MySqlCommand("INSERT INTO remunerados(bolsista_id,agencia,conta,fonte_bolsa,banco)VALUES (" + reader.GetString("id") + ", '" + entradaAgencia.Text + "', '" + entradaConta.Text + "', '" + entradaFonteDaBolsa.Text + "', '" + entradaBanco.Text + "')", conectar);
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

                            MySqlCommand insertbol_proj = new MySqlCommand("insert into bolsista_projetos values(default," + id_bol + "," + id_proj + ");", conectar);
                            insertbol_proj.ExecuteNonQuery();


                            conectar.Close();
                            MessageBox.Show("Salvo com sucesso, que Topper!");
                            conectar.Open();
                            MySqlCommand pegarid = new MySqlCommand("select id from pessoas order by id desc limit 1", conectar);
                            reader = pegarid.ExecuteReader();
                            reader.Read();
                            entradaIDLit.Text = reader.GetString("id");
                            reader.Close();
                            conectar.Close();
                            if (Ativar.Checked)
                            {
                                string desativar = "update pessoas set cod_digital = null where id = " + entradaIDLit.Text + ";";
                                conectar.Open();
                                MySqlCommand comandodesativar = new MySqlCommand(desativar, conectar);
                                comandodesativar.ExecuteNonQuery();
                                conectar.Close();
                            }
                            buttonFoto.Enabled = true;
                            Form3 maisumform = new Form3(this, entradaIDLit.Text);
                            maisumform.ShowDialog();
                            pictureBoxFoto.ImageLocation = @"Photos\" + entradaIDLit.Text + ".jpg";

                        }
                        catch (Exception error)
                        {
                            MessageBox.Show("error.." + error.Message + "   Contate o suporte");
                            conectar.Close();
                        }
                        //pictureBoxFoto.Load("http://i2.kym-cdn.com/photos/images/facebook/000/862/065/0e9.jpg");
                    }
                    else
                    {
                        try
                        {
                            conectar.Close();
                            MySqlDataReader rdr;
                            string buscaidproj = "select id from projetos where nome = '" + entradaProjeto.Text + "';";
                            conectar.Open();
                            MySqlCommand comando = new MySqlCommand(buscaidproj, conectar);
                            rdr = comando.ExecuteReader();
                            string idproj = "";
                            if (rdr.Read())
                            {
                                idproj = rdr.GetString("id");
                            }
                            rdr.Close();
                            conectar.Close();
                            MySqlDataReader rdr2;
                            string buscaidprof = "select professors.id from professors,pessoas where pessoas.id = professors.pessoa_id and pessoas.nome = '" + entradaOrientador.Text + "';";
                            conectar.Open();
                            MySqlCommand comando2 = new MySqlCommand(buscaidprof, conectar);
                            rdr2 = comando2.ExecuteReader();
                            string idprof = "";
                            if (rdr2.Read())
                            {
                                idprof = rdr2.GetString("id");
                            }
                            rdr2.Close();
                            conectar.Close();

                            string buscaremunerado = "select remunerados.id from pessoas, bolsistas, remunerados where bolsistas.id = remunerados.bolsista_id and bolsistas.pessoa_id = pessoas.id and pessoas.cod_digital = " + entradaID.Text + ";";
                            conectar.Open();

                            MySqlCommand comando3 = new MySqlCommand(buscaremunerado, conectar);
                            rdr = comando3.ExecuteReader();
                            string idremunerado = "";
                            if (rdr.Read())
                            {
                                idremunerado = rdr.GetString("id");
                            }
                            rdr.Close();
                            conectar.Close();

                            if(Ativar.Checked)
                            {
                                string desativar = "update pessoas set cod_digital = null where id = " + entradaIDLit.Text + ";";
                                conectar.Open();
                                MySqlCommand comandodesativar = new MySqlCommand(desativar, conectar);
                                comandodesativar.ExecuteNonQuery();
                                conectar.Close();
                            }
                            

                            if (idremunerado != "")
                            {

                                string UpdatePessoa = "UPDATE pessoas,bolsistas,remunerados,bolsista_projetos SET cod_digital = '" + entradaID.Text + "', obs = '" + entradaOBS.Text + "',fonte_bolsa = '" + entradaFonteDaBolsa.Text + "',orientador_id = '" + idprof + "',banco = '" + entradaBanco.Text + "'," +
                                    "conta = '" + entradaConta.Text + "',agencia = '" + entradaAgencia.Text + "',rg = '" + entradaRg.Text + "',radioremunerado = " + radioremunerado.Checked + ",radiovoluntario = " + radiovoluntario.Checked + "," +
                                    "ativar = " + Ativar.Checked + ",radiooutra = " + radiooutra.Checked + ",radioifce = " + radioifce.Checked + ",manha = " + manha.Checked + ",tarde =" + tarde.Checked + ",noite = " + noite.Checked + "  ,email = '" + entradaEmail.Text + "', cep= '" + entradaCep.Text + "' ,cpf = '" + entradaCpf.Text + "',bairro = '" + entradaBairro.Text + "',datadenascimento = '" + entradaDataDeNascimento.Text + "', telefone = '" + entradaTelefone.Text + "',instituicaodeensino = '" + entradaINstituiçao.Text + "',matricula = '" + entradaMatriula.Text + "',semestre = '" + entradaSemestre.Text + "', celular = '" + entradaCelular.Text + "',curso = '" + entradaCurso.Text + "',  nome= '" + entradaNome.Text + "', endereco = '" + entradaEndereço.Text + "'" +
                                    ",projeto_id = '" + idproj +
                                    "' where bolsista_projetos.bolsista_id = bolsistas.id and bolsistas.pessoa_id =  pessoas.id and bolsistas.id = remunerados.bolsista_id and pessoas.id ='" + entradaIDLit.Text + "';";

                                conectar.Open();
                                MySqlCommand comando4 = new MySqlCommand(UpdatePessoa, conectar);

                                if (comando4.ExecuteNonQuery() == 4)
                                { 
                                    MessageBox.Show("dados atualizados");
                                }
                                else
                                {
                                    MessageBox.Show("nao atualizado");
                                }
                                conectar.Close();
                            }
                            else
                            {
                                string UpdatePessoa = "UPDATE pessoas,bolsistas,bolsista_projetos SET cod_digital = '" + entradaID.Text + "', obs = '" + entradaOBS.Text + "',orientador_id = '" + idprof + "'," +
                                    "rg = '" + entradaRg.Text + "',radioremunerado = " + radioremunerado.Checked + ",radiovoluntario = " + radiovoluntario.Checked + "," +
                                    "ativar = " + Ativar.Checked + ",radiooutra = " + radiooutra.Checked + ",radioifce = " + radioifce.Checked + ",manha = " + manha.Checked + ",tarde =" + tarde.Checked + ",noite = " + noite.Checked + "  ,email = '" + entradaEmail.Text + "', cep= '" + entradaCep.Text + "' ,cpf = '" + entradaCpf.Text + "',bairro = '" + entradaBairro.Text + "',datadenascimento = '" + entradaDataDeNascimento.Text + "', telefone = '" + entradaTelefone.Text + "',instituicaodeensino = '" + entradaINstituiçao.Text + "',matricula = '" + entradaMatriula.Text + "',semestre = '" + entradaSemestre.Text + "', celular = '" + entradaCelular.Text + "',curso = '" + entradaCurso.Text + "',  nome= '" + entradaNome.Text + "', endereco = '" + entradaEndereço.Text + "'" +
                                    ",projeto_id = '" + idproj +
                                    "' where bolsista_projetos.bolsista_id = bolsistas.id and bolsistas.pessoa_id =  pessoas.id and pessoas.id ='" + entradaIDLit.Text + "';";

                                conectar.Open();
                                MySqlCommand comando5 = new MySqlCommand(UpdatePessoa, conectar);
                                conectar.Close();

                                if (radioremunerado.Checked)
                                {
                                    string buscarBolsista = "select bolsistas.id from bolsistas, pessoas where bolsistas.pessoa_id = pessoas.id and pessoas.id = " + entradaIDLit.Text + ";";

                                    conectar.Open();

                                    MySqlCommand comando6 = new MySqlCommand(buscarBolsista, conectar);
                                    MySqlDataReader rdr3;
                                    rdr3 = comando6.ExecuteReader();
                                    rdr3.Read();
                                    string inserirRemunerado = "INSERT INTO remunerados(bolsista_id,agencia,conta,fonte_bolsa,banco)VALUES (" + rdr3.GetString("id") + ", '" + entradaAgencia.Text + "', '" + entradaConta.Text + "', '" + entradaFonteDaBolsa.Text + "', '" + entradaBanco.Text + "')";
                                    rdr3.Close();
                                    conectar.Close();

                                    conectar.Open();
                                    MySqlCommand comando7 = new MySqlCommand(inserirRemunerado, conectar);
                                    if (comando7.ExecuteNonQuery() == 1) { }
                                    conectar.Close();
                                }
                                conectar.Open();
                                if (comando5.ExecuteNonQuery() == 3) { MessageBox.Show("dados atualizados"); } else { MessageBox.Show("nao atualizado"); }
                                conectar.Close();
                            }

                        }
                        catch (Exception error)
                        {
                            MessageBox.Show("error.." + error.Message);
                            conectar.Close();
                        }


                    }
                }
                catch
                {
                    MessageBox.Show("Sem CPF, Sem busca!!");
                    conectar.Close();
                }
            }
            else
            {
                MessageBox.Show("Campo Código da Digital não pode estar em branco.");
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
            
            if (entradaID.Text != "")
            {
                try
                {

                    MySqlCommand comando;
                    MySqlDataReader reader;
                    conectar.Open();

                    string selecionarpessoa = "select (select nome from pessoas where cod_digital = '" + entradaID.Text + "') as nomebolsista," +
                                              "bolsistas.ativar,bolsistas.obs," +
                                              "bolsistas.semestre, bolsistas.endereco, bolsistas.bairro, bolsistas.rg, bolsistas.instituicaodeensino,bolsistas.cep ," +
                                              "bolsistas.matricula, bolsistas.curso, bolsistas.telefone, pessoas.id, pessoas.celular,bolsistas.datadenascimento" +
                                              " ,pessoas.email, pessoas.cpf, bolsistas.manha,bolsistas.tarde,bolsistas.noite,bolsistas.radioifce,bolsistas.radiooutra,"+
                                              "bolsistas.radioremunerado,bolsistas.radiovoluntario " +
                                              " from bolsistas,pessoas where bolsistas.pessoa_id =  pessoas.id and ativar = False and pessoas.cod_digital = '"+ entradaID.Text +"'; ";



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
                        entradaOBS.Text = reader.GetString("obs");
                        entradaIDLit.Text = reader.GetString("id");
                        Ativar.Checked = false;
                        


                        if (reader.GetBoolean("manha")) { manha.Checked = true; }
                        if (reader.GetBoolean("tarde") ) { tarde.Checked = true; }
                        if (reader.GetBoolean("noite")) { noite.Checked= true; }
                        if (reader.GetBoolean("radioifce")) { radioifce.Checked = true; }
                        if (reader.GetBoolean("radiooutra")) { radiooutra.Checked = true; }
                        if (reader.GetBoolean("radioremunerado")) { radioremunerado.Checked = true; }
                        if (reader.GetBoolean("radiovoluntario")) { radiovoluntario.Checked = true; }
                        if (reader.GetBoolean("ativar")) { Ativar.Checked = true; }
                        reader.Close();
                        conectar.Close();
                        
                        string nomedoorientador = "select nome from pessoas, professors, bolsistas where pessoas.id = professors.pessoa_id and professors.id = bolsistas.orientador_id and bolsistas.pessoa_id = (select id from pessoas where cod_digital = '"+ entradaID.Text+"');";
                        conectar.Open();
                        MySqlCommand comandonomedoorientador = new MySqlCommand(nomedoorientador, conectar);

                        reader = comandonomedoorientador.ExecuteReader();

                        if (reader.Read())
                        {
                            string prof = reader.GetString("nome");
                            for (int i = 0; i <= entradaOrientador.Items.Count; i++)
                            {
                                if (prof == entradaOrientador.Items[i].ToString())
                                {
                                    entradaOrientador.SelectedIndex = i;

                                    break;
                                }

                            }
                        }
                        else
                        {
                            entradaOrientador.SelectedIndex = 0;
                        }
                        conectar.Close();

                        string nomedoprojeto = "select projetos.nome from projetos, bolsistas, bolsista_projetos, pessoas where projetos.id = bolsista_projetos.projeto_id and bolsista_projetos.bolsista_id = bolsistas.id and bolsistas.pessoa_id = pessoas.id and pessoas.cod_digital = '" + entradaID.Text + "';";
                        conectar.Open();
                        MySqlCommand comandonomedoprojeto = new MySqlCommand(nomedoprojeto, conectar);

                        reader = comandonomedoprojeto.ExecuteReader();

                        if (reader.Read())
                        {
                            string proj = reader.GetString("nome");
                            for (int i = 0; i <= entradaProjeto.Items.Count; i++)
                            {
                                if (proj == entradaProjeto.Items[i].ToString())
                                {
                                    entradaProjeto.SelectedIndex = i;

                                    break;
                                }

                            }
                        }
                        else
                        {
                            entradaProjeto.SelectedIndex = 0;
                        }
                        conectar.Close();
                        string selectremunerado = "select remunerados.id, banco, agencia, conta, fonte_bolsa from pessoas, bolsistas, remunerados where bolsistas.id = remunerados.bolsista_id and bolsistas.pessoa_id = pessoas.id and pessoas.cod_digital = '" + entradaID.Text + "';";
                        conectar.Open();
                        MySqlCommand comando2 = new MySqlCommand(selectremunerado, conectar);

                        reader = comando2.ExecuteReader();

                        if(reader.Read())
                        {
                            entradaAgencia.Text = reader.GetString("agencia");
                            entradaConta.Text = reader.GetString("conta");
                            entradaFonteDaBolsa.Text = reader.GetString("fonte_bolsa");
                            entradaBanco.Text = reader.GetString("banco");

                            entradaAgencia.Enabled = true;
                            entradaConta.Enabled = true;
                            entradaBanco.Enabled = true;
                            entradaFonteDaBolsa.Enabled = true;
                        }
                        else
                        {
                            entradaAgencia.Text = string.Empty;
                            entradaConta.Text = string.Empty;
                            entradaFonteDaBolsa.Text = string.Empty;
                            entradaBanco.Text = string.Empty;
                        }


                        reader.Close();
                        conectar.Close();

                        pictureBoxFoto.ImageLocation = @"Photos\" + entradaIDLit.Text + ".jpg";
                        buttonFoto.Enabled = true;


                    }
                    else {
                        MessageBox.Show("sem dados com esse nome");
                        Ativar.Checked = false;
                        conectar.Close();
                    }
                    
                    

                }




                catch (Exception error)
                {
                    MessageBox.Show("error.." + error.Message);
                    conectar.Close();
                }
            }
            else { MessageBox.Show("Campo  em Branco"); }

           
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
                Ativar.Checked = false;

                entradaProjeto.SelectedIndex = 0;
                entradaOrientador.SelectedIndex = 0;
                pictureBoxFoto.ImageLocation = @"metodosMySql\Resources\" + "blank_user" + ".jpg";
                buttonFoto.Enabled = false;
            }
            else if (dialogResult == DialogResult.No) { }
            
        }


//************************************************* abrir aba de busca ********************************************************//
        private void button5_Click(object sender, EventArgs e)
        {
            passando = entradaNome.Text;
            
            Form2 outroform = new Form2(this, passando);
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
            string professores = "select pessoas.nome from pessoas, professors where pessoas.id = professors.pessoa_id;";
            MySqlCommand buscaprofessores = new MySqlCommand(professores, conectar);
            reader2 = buscaprofessores.ExecuteReader();
            
            while (reader2.Read())
            {
                entradaOrientador.Items.Add(reader2.GetString("nome"));
            }
            entradaOrientador.SelectedIndex = 0;
            conectar.Close();
        }

        private void pictureBoxFoto_Click(object sender, EventArgs e)
        {
            if (entradaCpf.Text != "")
            {
                Form4 maisumoutroform = new Form4(entradaIDLit.Text);
                maisumoutroform.ShowDialog();
            }
        }

//************************************************** Colocar imagem *********************************************

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void buttonFoto_Click(object sender, EventArgs e)
        {
            Form3 maisumform = new Form3(this,entradaIDLit.Text);
            maisumform.ShowDialog();
            pictureBoxFoto.ImageLocation = @"Photos\" + entradaIDLit.Text + ".jpg";
            
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

        private void entradaIDLit_TextChanged(object sender, EventArgs e)
        {
        }

        private void radioremunerado_CheckedChanged(object sender, EventArgs e)
        {
            if (radioremunerado.Checked)
            {
                entradaBanco.Enabled = true;
                entradaAgencia.Enabled = true;
                entradaConta.Enabled = true;
                entradaFonteDaBolsa.Enabled = true;
            }
            else
            {
                entradaBanco.Enabled = false;
                entradaAgencia.Enabled = false;
                entradaConta.Enabled = false;
                entradaFonteDaBolsa.Enabled = false;
            }
            
        }

        private void radiovoluntario_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void entradaID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                button2.PerformClick();
                e.Handled = true;
            }
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        public void alterarform(string digital)
        {
            entradaID.Text = digital;
            button2.PerformClick();
        }
    }
}
