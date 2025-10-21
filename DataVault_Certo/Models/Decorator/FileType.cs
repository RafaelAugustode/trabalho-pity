using System.Collections.Generic;
using System.Linq;
using DataVault.Models.Composite;

namespace DataVault.Models.Decorator
{
    // Interface base do decorator
    public interface IFileFilter
    {
        List<IFileComponent> GetFiles();
    }

    // Implementação concreta: retorna todos os arquivos de um FolderComposite
    public class FileFilterBase : IFileFilter
    {
        private readonly FolderComposite _root;

        public FileFilterBase(FolderComposite root)
        {
            _root = root;
        }

        public List<IFileComponent> GetFiles()
        {
            return Flatten(_root);
        }

        private List<IFileComponent> Flatten(IFileComponent node)
        {
            var result = new List<IFileComponent>();

            if (node.ArquivoFile)
            {
                result.Add(node);
            }
            else
            {
                foreach (var child in node.GetChildren())
                {
                    result.AddRange(Flatten(child));
                }
            }

            return result;
        }
    }

    // Decorator abstrato
    public abstract class FileFilterDecorator : IFileFilter
    {
        protected readonly IFileFilter _inner;

        public FileFilterDecorator(IFileFilter inner)
        {
            _inner = inner;
        }

        public abstract List<IFileComponent> GetFiles();
    }

    // Decorator concreto: filtra por tipo de arquivo
    public class FileTypeFilter : FileFilterDecorator
    {
        private readonly string _tipoArquivo;

        public FileTypeFilter(IFileFilter inner, string tipoArquivo) : base(inner)
        {
            _tipoArquivo = tipoArquivo;
        }

        public override List<IFileComponent> GetFiles()
        {
            var allFiles = _inner.GetFiles();
            return allFiles
                .Where(f => f is FileLeaf leaf && leaf.TipoArquivo.ToLower() == _tipoArquivo.ToLower())
                .ToList<IFileComponent>();
        }
    }
}