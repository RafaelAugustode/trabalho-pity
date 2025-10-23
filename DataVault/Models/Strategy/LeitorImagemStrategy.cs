// DataVault/Models/Strategy/LeitorImagemStrategy.cs
using Tesseract;

namespace DataVault.Models.Strategy
{
    public class LeitorImagemStrategy : ILeitorArquivoStrategy
    {
        public string LerConteudo(string caminhoArquivo)
        {
          
            try
            {
                // Caminho da pasta tessdata (deve estar em wwwroot/tessdata/)
                var pastaTessdata = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "tessdata");

                using (var engine = new TesseractEngine(pastaTessdata, "por", EngineMode.Default))
                using (var img = Pix.LoadFromFile(caminhoArquivo))
                {
                    using (var page = engine.Process(img))
                    {
                        return page.GetText().Trim();
                    }
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