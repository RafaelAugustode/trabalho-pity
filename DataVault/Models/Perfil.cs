using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DataVault.Models
{
	[Table("perfil")]
	public class Perfil
	{
		[Key]
		[Column("id")]
		public int Id { get; set; }

		[Column("modo_site")]
		public string ModoSite { get; set; } = string.Empty;

		[Column("lingua")]
		public string Lingua { get; set; } = string.Empty;

		[Column("historico_mensagens")]
		public string HistoricoMensagens { get; set; } = string.Empty;

		[ForeignKey("Usuario")]
		[Column("id_usuario")]
		public int IdUsuario { get; set; }

		public Usuario Usuario { get; set; }
	}
}
