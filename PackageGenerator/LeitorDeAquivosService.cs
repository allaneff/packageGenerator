using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PackageGenerator
{
    public class LeitorDeAquivosService
    {
        public string FileName { get; private set; }

        public void LeitorDeAquivosServicec(List<string> resultName)
        {
            foreach (var strName in resultName)
            {
                FileName = strName;
            }
        }

        public void CopyArquivosSatiPacotes(List<string> resultPath)
        {
         
            foreach (var strArquivo in resultPath)
            {
                File.Copy(strArquivo, @"C:\teste\" + FileName);
            }
        }
    }
}
