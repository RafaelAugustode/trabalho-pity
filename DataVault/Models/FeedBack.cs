using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DataVault.Models
{
	[Table("feedback")]
	public class Feedback
	{
		[Key]
		[Column("id")]
		public int Id { get; set; }

		[Required]
		[Column("mensagem")]
		public string Mensagem { get; set; } = string.Empty;

		[Column("selecionar_nivel")]
		public string SelecionarNivel { get; set; } = string.Empty;

		[ForeignKey("Usuario")]
		[Column("id_usuario")]
		public int IdUsuario { get; set; }

		[Column("data_criacao")]
		public DateTime DataCriacao { get; set; } = DateTime.Now;

		public Usuario Usuario { get; set; }
	}
}
