using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DataVault.Models.Composite
{
    public class FileNode
    {
        public string Name { get; set; } = string.Empty;
        public bool IsFile { get; set; }
        public List<FileNode> Children { get; set; } = new List<FileNode>();

        // Dados extras do arquivo, se for arquivo
        public long Size { get; set; }
        public string Type { get; set; } = string.Empty;

        public FileNode(string name, bool isFile)
        {
            Name = name;
            IsFile = isFile;
        }

        public void AddChild(FileNode node)
        {
            if (!IsFile)
                Children.Add(node);
        }
    }
}