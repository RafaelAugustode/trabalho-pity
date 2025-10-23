using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;

namespace DataVault.Models.Strategy
{
    public class LeitorPdf : ILeitorArquivoStrategy
    {
        public string LerConteudo(string caminhoArquivo)
        {
            try
            {
                using (var reader = new PdfReader(caminhoArquivo))
                using (var pdfDoc = new PdfDocument(reader))
                {
                    int numPaginas = pdfDoc.GetNumberOfPages();
                    var texto = "";

                    for (int i = 1; i <= numPaginas; i++)
                    {
                        var page = pdfDoc.GetPage(i);
                        var strategy = new SimpleTextExtractionStrategy();
                        var content = PdfTextExtractor.GetTextFromPage(page, strategy);
                        texto += content + "\n";
                    }

                    return texto.Trim(); // Remove espaços extras
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