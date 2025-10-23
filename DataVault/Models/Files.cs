using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataVault.Models
{
    [Table("files")]
    public class Files
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("arquivo_file")]
        public bool ArquivoFile { get; set; }

        [Column("nome_arquivo")]
        public string NomeArquivo { get; set; } = string.Empty;

        [Column("favorito")]
        public bool Favorito { get; set; }

        [Column("compartilhado")]
        public bool Compartilhado { get; set; }

        [Column("excluido")]
        public bool Excluido { get; set; }

        [Column("tipo_arquivo")]
        public string TipoArquivo { get; set; } = string.Empty;

        [Column("tamanho_arquivo")]
        public long TamanhoArquivo { get; set; }

        [Column("nome_pasta")]
        public string NomePasta { get; set; } = string.Empty;

        [ForeignKey("Usuario")]
        [Column("id_usuario")]
        public int IdUsuario { get; set; }

        public Usuario Usuario { get; set; }

        // ➕ Novos campos para o Composite
        [Column("is_pasta")]
        public bool IsPasta { get; set; } = false;

        [Column("pasta_pai_id")]
        public int? PastaPaiId { get; set; }

        [Column("tags")]
        public string? Tags { get; set; } = "";

        [Column("conteudo_sensivel")]
        public bool ConteudoSensivel { get; set; } = false;

        [ForeignKey("PastaPaiId")]
        public virtual Files? PastaPai { get; set; }
       

        public virtual ICollection<Files> Filhos { get; set; } = new List<Files>();
    }
}