using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PackageGenerator
{
    public partial class Home : Form
    {
        public List<string> resultArquivos = new List<string>();
        public List<string> resultName = new List<string>();
        public Home()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
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
                LeitorDeAquivosService arq = new LeitorDeAquivosService();
                arq.LeitorDeAquivosServicec(resultName);
                arq.CopyArquivosSatiPacotes(resultArquivos);
            }
            catch (Exception ex)
            {

                MessageBox.Show("Não é possivel ler o arquivo");
                MessageBox.Show(ex.Message);
            }
            

        }


        private void btnGerarLog(object sender, EventArgs e)
        {
          
        }

        public void lvArquivos(object sender, EventArgs e)
        {

        }

        
    }
}
