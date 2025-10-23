using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataVault.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome � obrigat�rio")]
        public string nome_cliente { get; set; }

        [Required(ErrorMessage = "O e-mail � obrigat�rio")]
        [EmailAddress(ErrorMessage = "E-mail inv�lido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A senha � obrigat�ria")]
        public string Senha { get; set; }

        // N�o s�o obrigat�rios
      
    }
}