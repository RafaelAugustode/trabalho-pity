using System.IO;

namespace DataVault.Models.Strategy
{
    public class LeitorTxtStrategy : ILeitorArquivoStrategy
    {
        public string LerConteudo(string caminhoArquivo)
        {
            return File.ReadAllText(caminhoArquivo);
        }
    }
}