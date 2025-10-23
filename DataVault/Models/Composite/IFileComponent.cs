namespace DataVault.Models.Composite
{
    public interface IFileComponent
    {
        string Nome { get; }
        long Tamanho { get; }
        bool ArquivoFile { get; } 
        List<IFileComponent> GetChildren(); 
    }
}