using System.Collections.Generic;

namespace DataVault.Models.Composite
{
    public class FolderComposite : IFileComponent
    {
        public string Nome { get; private set; }
        public bool ArquivoFile => false;

        private List<IFileComponent> _children = new List<IFileComponent>();

        public FolderComposite(string nome)
        {
            Nome = nome;
        }

        public void Add(IFileComponent component) => _children.Add(component);

        public List<IFileComponent> GetChildren() => _children;
      

        public long Tamanho => 0; // pastas não têm tamanho direto
    }
}
