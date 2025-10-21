using DataVault.Models.Composite;

public class FileLeaf : IFileComponent
{
    public string Nome { get; private set; }
    public string TipoArquivo { get; private set; }
    public long Tamanho { get; private set; }
    public bool ArquivoFile => true;

    public FileLeaf(string nome, string tipoArquivo, long tamanho)
    {
        Nome = nome;
        TipoArquivo = tipoArquivo;
        Tamanho = tamanho;
    }

    public List<IFileComponent> GetChildren() => null;
}