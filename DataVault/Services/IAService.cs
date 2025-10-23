// Services/IAService.cs
using System.Text;
using System.Text.Json;

namespace DataVault.Services
{
    public class IAService
    {
        private readonly HttpClient _httpClient;

        public IAService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<string> EnviarPrompt(string prompt)
        {

            var payload = new
            {
                model = "phi3",
                prompt = prompt,
                stream = false,
                options = new { temperature = 0.3 }
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("http://localhost:11434/api/generate", content);
            var responseText = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(responseText);
          
            return doc.RootElement.GetProperty("response").GetString()?.Trim() ?? "Resumo não disponível.";
        }

        public async Task<string> ExtrairTema(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return "Outros";

            // Limita o texto para evitar lentidão
            var trecho = texto.Length > 1500 ? texto.Substring(0, 1500) : texto;

            var prompt = $@"
             Você é um assistente de organização de documentos.
               Analise o seguinte texto e retorne APENAS o nome de uma pasta que represente o assunto central.
                O nome da pasta deve ser descritivo, em português, com no máximo 6 palavras, sem pontuação, sem aspas.
                 Se o texto for muito curto ou genérico, retorne 'Documentos Gerais'.
               Texto: {trecho}
                             ";

            var payload = new
            {
                model = "phi3",
                prompt = prompt,
                stream = false,
                options = new { temperature = 0.3 }
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync("http://localhost:11434/api/generate", content);
                var responseText = await response.Content.ReadAsStringAsync();

                using var doc = JsonDocument.Parse(responseText);
                var rawResponse = doc.RootElement.GetProperty("response").GetString() ?? "";

                // Limpa a resposta
                var tema = rawResponse
                    .Split('\n')[0]
                    .Replace("\"", "")
                    .Replace(".", "")
                    .Trim();

                return string.IsNullOrWhiteSpace(tema) ? "Documentos Gerais" : tema;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro na IA: {ex.Message}");
                return "Documentos Gerais";
            }
        }
    }
}