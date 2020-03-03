using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PackageGenerator
{
    public partial class Form1 : Form
    {
        public List<string> resultArquivos = new List<string>();
        public List<string> resultName = new List<string>();
        public string numChamado;
        private string versao;
        public string bancoDeDados;
        public string login;
        public string senha;
        LeitorDeAquivosService arq = new LeitorDeAquivosService();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            button2.Enabled = false;
            CriandoLog.Visible = false;
        }

        private void txtNumChamado(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(textBox1.Text, "[^0-9]"))
            {
                MessageBox.Show("Esse campo deve conter apenas números!");
                textBox1.Text = textBox1.Text.Remove(textBox1.Text.Length - 1);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }



        private void btnSelArquivos(object sender, EventArgs e)
        {
            OpenFileDialog dialogo = new OpenFileDialog();
            // Define o título da janela
            dialogo.Title = "Procurar arquivos no computador";
            // Define o diretório inicial que a janela usará
            // Aqui provavelmente você vai querer colocar o
            // valor que está na sua caixa de texto
            dialogo.Multiselect = true;
            dialogo.InitialDirectory = @"Desktop/";
            dialogo.CheckFileExists = true;
            dialogo.CheckPathExists = true;
            dialogo.RestoreDirectory = true;
            dialogo.ShowReadOnly = true;
            // Define o filtro que você quiser para mostrar
            // apenas os arquivos do tipo que você conhece
            dialogo.Filter = "Arquivos SQL (*.sql)|*.sql";
            // Mostra a janela para o usuário, e guarda o retorno
            // que indica se ele chegou a selecionar um arquivo, o
            // cancelou a janela sem selecionar um arquivo...
            DialogResult resposta = dialogo.ShowDialog();
            // O usuário selecionou um arquivo e clicou em OK?
            if (resposta == DialogResult.OK)

            {
                // Obtém o caminho completo do arquivo
                string filePath;
                string fileName;
                int i = 0;
                List<OpenFileDialog> files = new List<OpenFileDialog>();

                foreach (var file in dialogo.FileNames)
                {
                    filePath = Path.GetFullPath(file);
                    fileName = Path.GetFileName(file);
                    resultName.Add(fileName);
                    resultArquivos.Add(filePath);
                    i = i + 1;
                    listView1.Items.Add(fileName);
                }
            }

            else
            {
                MessageBox.Show("Você não selecionou nenhum arquivo!");
            }
        }

        private void btnCriarPacote(object sender, EventArgs e)
        {

            try
            {


                if (radioButton1.Checked)
                {
                    arq.SelectDiretorio("LF");
                }
                if (radioButton2.Checked)
                {
                    arq.SelectDiretorio("NFE");
                }
                arq.CopyArquivosSatiPacotes(resultArquivos);
                numChamado = textBox1.Text;
                arq.ExecuteArqBat(numChamado);
                versao = textBox2.Text;

           
                if (radioButton4.Checked)
                {
                    arq.CreateVersaoPacoteLinux(versao);
                }
                else
                {
                    arq.CreateVersaoPacote(versao);
                }
                arq.AlterArqChamadoVersao();
                arq.AlterFirstLineArqChamadoVersao();
                //arq.MoveArquivosPastaChamado(resultArquivos);
                arq.MoveArquivosNFEouSATI();
                MessageBox.Show("Pacote Criado com Sucesso!");
                MessageBox.Show("Para Concluir o Processo, Gerar o Log!");
                button3.Enabled = false;
                button1.Enabled = false;
                button2.Enabled = true;
            

            }
            catch (Exception ex)
            {
                arq.DeletaArquivos(resultArquivos);
                listView1.Clear();
                MessageBox.Show(ex.Message);
            }


        }



        protected void btnGerarLog(object sender, EventArgs e)
        {


            bancoDeDados = comboBox1.Text;
            login = textBox3.Text;
            senha = textBox4.Text;
            if (bancoDeDados != "" && login != "" && senha != "")
            {
                try
                {
                    //using (MyFormBase myFormBase = new MyFormBase())
                    //{
                    //    myFormBase.Show();
                    //    arq.GerarArquivoLog(bancoDeDados, login, senha);
                    //    myFormBase.Close();
                    //}
                    CriandoLog.Visible = true;
                    arq.GerarArquivoLog(bancoDeDados, login, senha);
                    CriandoLog.Visible = false;
                    
                    arq.RetornarArquivosPacote();
              
                    if (radioButton4.Checked)
                    {
                        arq.AlterLinux();
                    }
                    MessageBox.Show("Log Gerado com Sucesso!");
                    Form1 NewForm = new Form1();
                    NewForm.Show();
                    this.Dispose(false);
                }
                catch (Exception ex)
                {

                    MessageBox.Show("Erro ao gerar o Log, " + ex.Message);
                }
            }

            else
            {
                MessageBox.Show("Todos os campos devem ser preenchidos!");
            }


        }

        public void lvArquivos(object sender, EventArgs e)
        {

        }

        private void txtVersao(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(textBox2.Text, "[^0-9]"))
            {
                MessageBox.Show("Esse campo deve conter apenas números!");
                textBox2.Text = textBox2.Text.Remove(textBox2.Text.Length - 1);
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            textBox4.PasswordChar = '\u25CF';
            if (Control.IsKeyLocked(Keys.CapsLock))
            {
                MessageBox.Show("The Caps Lock key is ON.");
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
           
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void criandoLog(object sender, EventArgs e)
        {

        }
    }
}
