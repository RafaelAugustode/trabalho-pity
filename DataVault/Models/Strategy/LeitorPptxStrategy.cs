// DataVault/Models/Strategy/LeitorPptxStrategy.cs
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Presentation;

namespace DataVault.Models.Strategy
{
    public class LeitorPptxStrategy : ILeitorArquivoStrategy
    {
        public string LerConteudo(string caminhoArquivo)
        {
            
            try
            {
                var texto = "";
                using (var presentation = PresentationDocument.Open(caminhoArquivo, false))
                {
                    var slides = presentation.PresentationPart.SlideParts;
                    foreach (var slidePart in slides)
                    {
                        var slide = slidePart.Slide;
                        var text = slide.InnerText;
                        if (!string.IsNullOrWhiteSpace(text))
                            texto += text + " ";
                    }
                }
                return texto.Replace("\r", " ").Replace("\n", " ").Trim();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao ler {caminhoArquivo}: {ex.Message}");
                return ""; // ou lance uma exceção personalizada
            }
        }
    }
}