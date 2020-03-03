using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PackageGenerator
{
    public class LeitorDeAquivosService
    {
        public string FileName { get; private set; }
        public string CaminhoDiretorio { get; set; }
        public string NumChamado { get; private set; }
        public string Versao { get; private set; }
        public string NovoNomeAquivo { get; set; }
        public string NovoNomePasta { get; set; }
        public string Diretorio { get; set; }
        public string DiretorioFirstLine { get; set; }
        public string DiretorioLog { get; set; }



        public string SelectDiretorio(string diretorio)
        {
            string caminhoSatiPacotes = "C:\\SATI_PACOTES\\";
            string caminhoNfePacotes = "C:\\NFE_PACOTES\\";
            string caminhoSati = @"C:\SATI\";
            string caminhoNfe = @"C:\NFE\";
            if (diretorio == "LF")
            {
                Diretorio = caminhoSatiPacotes;
                DiretorioFirstLine = "sati";
                DiretorioLog = caminhoSati;
            }
            if (diretorio == "NFE")
            {
                Diretorio = caminhoNfePacotes;
                DiretorioFirstLine = "nfe";
                DiretorioLog = caminhoNfe;
            }

            return Diretorio;
        }

        public void CopyArquivosSatiPacotes(List<string> resultPath)
        {


            foreach (var strArquivo in resultPath)
            {
                FileName = strArquivo.Split('\\').Last();
                //fileNameSplitted.= FileName; 
                File.Copy(strArquivo, @Diretorio + FileName);
            }
        }

        public void DeletaArquivos(List<string> resultPath)
        {
            foreach (var strArquivo in resultPath)
            {
                FileName = strArquivo.Split('\\').Last();
                File.Delete(@Diretorio + FileName);
            }
        }


        public void ExecuteArqBat(string numChamado)
        {
            if (FileName.Contains(numChamado))
            {
                NumChamado = numChamado;
                ProcessStartInfo processStartInfo = new ProcessStartInfo("cmd.exe");
                processStartInfo.RedirectStandardInput = true;
                processStartInfo.RedirectStandardOutput = true;
                processStartInfo.UseShellExecute = false;
                Process process = Process.Start(processStartInfo);
                process.StandardInput.WriteLine(@"cd " + Diretorio);
                process.StandardInput.WriteLine(@"chamado.bat " + numChamado);
                process.StandardInput.WriteLine(@"exit");
                process.WaitForExit();
                process.Dispose();
                process.Close();
            }
            else
            {
                throw new Exception("O nome do arquivo deve conter o número do chamado!");
                
            }

        }

        public void CreateVersaoPacote(string versao)
        {
            CaminhoDiretorio = String.Concat(@Diretorio, "Chamado_", NumChamado);
            Versao = String.Concat("_V", versao);
            NovoNomePasta = String.Concat(@Diretorio, "Chamado_", NumChamado, Versao);
            try
            {
                Directory.Move(@CaminhoDiretorio, @NovoNomePasta);
            }
            catch (Exception)
            {

                throw new Exception("Não foi possível alterar o nome do diretório!");
            }
        }
        public void CreateVersaoPacoteLinux(string versao)
        {
            CaminhoDiretorio = String.Concat(@Diretorio, "Chamado_", NumChamado);
            Versao = String.Concat("_V", versao);
            NovoNomePasta = String.Concat(@Diretorio, "Chamado_", NumChamado, Versao, "_Lx");
            try
            {
                Directory.Move(@CaminhoDiretorio, @NovoNomePasta);
            }
            catch (Exception)
            {

                throw new Exception("Não foi possível alterar o nome do diretório!");
            }
        }

        //Alterar a Vesao do pacote no nome do arquivo e também na sua primeira linha
        public void AlterArqChamadoVersao()
        {
            //string caminhoArquivo = String.Concat(CaminhoDiretorio, Versao, "\\Chamado_", NumChamado, ".sql");
            string caminhoArquivo = String.Concat(NovoNomePasta, "\\Chamado_", NumChamado, ".sql");
            NovoNomeAquivo = String.Concat(NovoNomePasta, "\\Chamado_", NumChamado, Versao, ".sql");
            try
            {
                File.Move(@caminhoArquivo, @NovoNomeAquivo);

            }
            catch (Exception)
            {

                throw new Exception("Não foi possível alterar o nome do arquivo!");
            }
        }


        private string CaminhoGerarLog
        {
            get
            {
                var diretorioLog = String.Concat(DiretorioLog, "Chamado_", NumChamado, Versao, ".sql");

                return diretorioLog;
            }


        }

        public void AlterFirstLineArqChamadoVersao()
        {

            try
            {
                var allLines = File.ReadAllLines(NovoNomeAquivo);
                allLines[0] = "spool c:\\" + DiretorioFirstLine + "\\log_Chamado_" + NumChamado + Versao + ".txt";
                File.WriteAllLines(NovoNomeAquivo, allLines);
            }
            catch (Exception e)
            {
                MessageBox.Show("Não é possivel ler o arquivo");
                MessageBox.Show(e.Message);
            }
        }


        public void AlterLinux()
        {
            StreamReader sr = new StreamReader(NovoNomeAquivo);
            StringBuilder sb = new StringBuilder();

            while (!sr.EndOfStream)
            {
                string s = sr.ReadLine();
                if (s.IndexOf( @"\sati\") > -1)
                {
                    s = s.Replace(@"\sati\", @"/tmp/");
                }
                sb.AppendLine(s);
            }
            sr.Close();

            StreamWriter sw = new StreamWriter(NovoNomeAquivo);
            sw.Write(sb);

            sw.Close();

        }

        public void MoveArquivosPastaChamado(List<string> resultPath)
        {

            foreach (var strArquivo in resultPath)
            {
                FileName = strArquivo.Split('\\').Last();
                //fileNameSplitted.= FileName; 
                File.Move(Diretorio + FileName, NovoNomePasta + "\\" + FileName);
            }
        }

        public void MoveArquivosNFEouSATI()
        {
            DirectoryInfo dir = new DirectoryInfo(NovoNomePasta);
            string destino = DiretorioLog;

            foreach (FileInfo f in dir.GetFiles("*.sql"))
            {
                File.Move(f.FullName, @destino + f.Name);
            }
        }



        public void GerarArquivoLog(string bancoDeDados, string login, string senha)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo("cmd.exe");
            processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            processStartInfo.RedirectStandardInput = true;
            processStartInfo.RedirectStandardOutput = false;
            processStartInfo.UseShellExecute = false;
            processStartInfo.CreateNoWindow = true;
            using (Process process = Process.Start(processStartInfo))
            {
                process.StandardInput.WriteLine("sqlplus /nolog");
                process.StandardInput.WriteLine("set instance " + bancoDeDados);
                process.StandardInput.WriteLine("conn");
                process.StandardInput.WriteLine(login);
                process.StandardInput.WriteLine(senha);
                process.StandardInput.WriteLine("@" + CaminhoGerarLog);
                process.StandardInput.WriteLine("quit");
                process.StandardInput.WriteLine("exit");
                process.WaitForExit(20000);


            };

        }

        public void RetornarArquivosPacote()
        {
            DirectoryInfo dir = new DirectoryInfo(DiretorioLog);
            string destino = NovoNomePasta + "\\";

            foreach (FileInfo f in dir.GetFiles())
            {
                File.Move(f.FullName, @destino + f.Name);
            }

        }
    
        
    }
}

