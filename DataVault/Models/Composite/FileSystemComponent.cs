namespace DataVault.Models.Composite
{
    public abstract class FileSystemComponent
    {
        public string Name { get; set; } = string.Empty;
        public abstract bool IsFolder { get; }
    }

  
}