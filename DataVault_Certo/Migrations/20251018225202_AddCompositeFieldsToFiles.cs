using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataVault.Migrations
{
    /// <inheritdoc />
    public partial class AddCompositeFieldsToFiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_feedback_usuario_id_usuario",
                table: "feedback");

            migrationBuilder.DropForeignKey(
                name: "FK_files_usuario_id_usuario",
                table: "files");

            migrationBuilder.DropForeignKey(
                name: "FK_pagamento_usuario_id_usuario",
                table: "pagamento");

            migrationBuilder.DropForeignKey(
                name: "FK_perfil_usuario_id_usuario",
                table: "perfil");

            migrationBuilder.DropPrimaryKey(
                name: "PK_usuario",
                table: "usuario");

            migrationBuilder.RenameTable(
                name: "usuario",
                newName: "Usuario");

            migrationBuilder.RenameColumn(
                name: "senha",
                table: "Usuario",
                newName: "Senha");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "Usuario",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Usuario",
                newName: "Id");

            migrationBuilder.AddColumn<bool>(
                name: "is_pasta",
                table: "files",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "pasta_pai_id",
                table: "files",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "data_criacao",
                table: "feedback",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Usuario",
                table: "Usuario",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_files_pasta_pai_id",
                table: "files",
                column: "pasta_pai_id");

            migrationBuilder.AddForeignKey(
                name: "FK_feedback_Usuario_id_usuario",
                table: "feedback",
                column: "id_usuario",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_files_Usuario_id_usuario",
                table: "files",
                column: "id_usuario",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_files_files_pasta_pai_id",
                table: "files",
                column: "pasta_pai_id",
                principalTable: "files",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_pagamento_Usuario_id_usuario",
                table: "pagamento",
                column: "id_usuario",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_perfil_Usuario_id_usuario",
                table: "perfil",
                column: "id_usuario",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_feedback_Usuario_id_usuario",
                table: "feedback");

            migrationBuilder.DropForeignKey(
                name: "FK_files_Usuario_id_usuario",
                table: "files");

            migrationBuilder.DropForeignKey(
                name: "FK_files_files_pasta_pai_id",
                table: "files");

            migrationBuilder.DropForeignKey(
                name: "FK_pagamento_Usuario_id_usuario",
                table: "pagamento");

            migrationBuilder.DropForeignKey(
                name: "FK_perfil_Usuario_id_usuario",
                table: "perfil");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Usuario",
                table: "Usuario");

            migrationBuilder.DropIndex(
                name: "IX_files_pasta_pai_id",
                table: "files");

            migrationBuilder.DropColumn(
                name: "is_pasta",
                table: "files");

            migrationBuilder.DropColumn(
                name: "pasta_pai_id",
                table: "files");

            migrationBuilder.DropColumn(
                name: "data_criacao",
                table: "feedback");

            migrationBuilder.RenameTable(
                name: "Usuario",
                newName: "usuario");

            migrationBuilder.RenameColumn(
                name: "Senha",
                table: "usuario",
                newName: "senha");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "usuario",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "usuario",
                newName: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_usuario",
                table: "usuario",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_feedback_usuario_id_usuario",
                table: "feedback",
                column: "id_usuario",
                principalTable: "usuario",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_files_usuario_id_usuario",
                table: "files",
                column: "id_usuario",
                principalTable: "usuario",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_pagamento_usuario_id_usuario",
                table: "pagamento",
                column: "id_usuario",
                principalTable: "usuario",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_perfil_usuario_id_usuario",
                table: "perfil",
                column: "id_usuario",
                principalTable: "usuario",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
