using System.IO;

namespace DataVault.Models.Composite
{
    public class FilesTreeBuilder
    {
        private readonly string _userRootPath;

        public FilesTreeBuilder(string userRootPath)
        {
            _userRootPath = userRootPath;
        }

        public FileNode BuildTree()
        {
            if (!Directory.Exists(_userRootPath))
                Directory.CreateDirectory(_userRootPath);

            var rootNode = new FileNode("Meus Arquivos", false);
            BuildRecursive(_userRootPath, rootNode);
            return rootNode;
        }

        private void BuildRecursive(string path, FileNode parent)
        {
            // Adiciona pastas
            foreach (var dir in Directory.GetDirectories(path))
            {
                var dirNode = new FileNode(Path.GetFileName(dir), false);
                parent.AddChild(dirNode);
                BuildRecursive(dir, dirNode);
            }

            // Adiciona arquivos
            foreach (var file in Directory.GetFiles(path))
            {
                var fileInfo = new FileInfo(file);
                var fileNode = new FileNode(fileInfo.Name, true)
                {
                    Size = fileInfo.Length,
                    Type = fileInfo.Extension
                };
                parent.AddChild(fileNode);
            }
        }
    }
}