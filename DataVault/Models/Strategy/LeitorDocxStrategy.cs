// DataVault/Models/Strategy/LeitorDocxStrategy.cs
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace DataVault.Models.Strategy
{
    public class LeitorDocxStrategy : ILeitorArquivoStrategy
    {
        public string LerConteudo(string caminhoArquivo)
        {
            try
            {
                using (var doc = WordprocessingDocument.Open(caminhoArquivo, false))
                {
                    var body = doc.MainDocumentPart.Document.Body;
                    return body.InnerText.Replace("\r", " ").Replace("\n", " ").Trim();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao ler {caminhoArquivo}: {ex.Message}");
                return ""; // ou lance uma exceção personalizada
            }
            
        }
    }
}