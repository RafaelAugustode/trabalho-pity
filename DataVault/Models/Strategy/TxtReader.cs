using System.IO;

namespace DataVault.Models.Strategy
{
    public class LeitorTxtStrategy : ILeitorArquivoStrategy
    {
        public string LerConteudo(string caminhoArquivo)
        {
     
            try
            {
                return File.ReadAllText(caminhoArquivo);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao ler {caminhoArquivo}: {ex.Message}");
                return ""; // ou lance uma exceção personalizada
            }
        }
    }
}