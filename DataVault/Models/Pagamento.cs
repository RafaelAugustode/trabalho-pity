using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DataVault.Models
{
	[Table("pagamento")]
	public class Pagamento
	{
		[Key]
		[Column("id")]
		public int Id { get; set; }

		[Column("pix")]
		public string Pix { get; set; } = string.Empty;

		[Column("extrato")]
		public string Extrato { get; set; } = string.Empty;

		[Column("catao_debito")]
		public long CartaoDebito { get; set; }

		[Column("cartao_credito")]
		public long CartaoCredito { get; set; }

		[ForeignKey("Usuario")]
		[Column("id_usuario")]
		public int IdUsuario { get; set; }

		public Usuario Usuario { get; set; }
	}
}
