using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataVault.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarTags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_files_files_pasta_pai_id",
                table: "files");

            migrationBuilder.AddColumn<string>(
                name: "tags",
                table: "files",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_files_files_pasta_pai_id",
                table: "files",
                column: "pasta_pai_id",
                principalTable: "files",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_files_files_pasta_pai_id",
                table: "files");

            migrationBuilder.DropColumn(
                name: "tags",
                table: "files");

            migrationBuilder.AddForeignKey(
                name: "FK_files_files_pasta_pai_id",
                table: "files",
                column: "pasta_pai_id",
                principalTable: "files",
                principalColumn: "id");
        }
    }
}
