using DataVault.Models.Strategy;

namespace DataVault.Models
{
    public class LeitorArquivoContext
    {
        private readonly ILeitorArquivoStrategy _strategy;

        public LeitorArquivoContext(ILeitorArquivoStrategy strategy)
        {
            _strategy = strategy;
        }

        public string Ler(string caminhoArquivo)
        {
            return _strategy.LerConteudo(caminhoArquivo);
        }
    }
}