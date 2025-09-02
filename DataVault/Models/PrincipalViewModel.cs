using System.Collections.Generic;

namespace DataVault.Models
{
    public class PrincipalViewModel
    {
        public string NomeCliente { get; set; } = string.Empty;
        public List<Files> Arquivos { get; set; } = new List<Files>();
    }
}