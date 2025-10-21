using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataVault.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarConteudoSensivel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "tags",
                table: "files",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "conteudo_sensivel",
                table: "files",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "conteudo_sensivel",
                table: "files");

            migrationBuilder.UpdateData(
                table: "files",
                keyColumn: "tags",
                keyValue: null,
                column: "tags",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "tags",
                table: "files",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
