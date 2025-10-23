// DataVault/Models/Strategy/LeitorXmlStrategy.cs
using System.Xml;

namespace DataVault.Models.Strategy
{
    public class LeitorXmlStrategy : ILeitorArquivoStrategy
    {
        public string LerConteudo(string caminhoArquivo)
        {
            try
            {
                var doc = new XmlDocument();
                doc.Load(caminhoArquivo);
                return doc.InnerText.Replace("\r", " ").Replace("\n", " ").Trim();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao ler {caminhoArquivo}: {ex.Message}");
                return ""; // ou lance uma exceção personalizada
            }
        }
    }
}