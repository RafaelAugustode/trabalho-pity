namespace DataVault.Models.Strategy
{
    public interface ILeitorArquivoStrategy
    {
        string LerConteudo(string caminhoArquivo);
    }
}