using Microsoft.EntityFrameworkCore.Migrations;

namespace BackEnd.Migrations
{
    public partial class TipoPessoa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fornecedores_Empresas_EmpresaFK",
                table: "Fornecedor");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Fornecedores",
                table: "Fornecedor");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Empresas",
                table: "Empresa");

            migrationBuilder.RenameIndex(
                name: "IX_Fornecedores_EmpresaFK",
                table: "Fornecedor",
                newName: "IX_Fornecedor_EmpresaFK");

            migrationBuilder.AlterColumn<long>(
                name: "EmpresaFK",
                table: "Fornecedor",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TipoPessoa",
                table: "Fornecedor",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Fornecedor",
                table: "Fornecedor",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Empresa",
                table: "Empresa",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Fornecedor_Empresa_EmpresaFK",
                table: "Fornecedor",
                column: "EmpresaFK",
                principalTable: "Empresa",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fornecedor_Empresa_EmpresaFK",
                table: "Fornecedor");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Fornecedor",
                table: "Fornecedor");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Empresa",
                table: "Empresa");

            migrationBuilder.DropColumn(
                name: "TipoPessoa",
                table: "Fornecedor");

            migrationBuilder.RenameIndex(
                name: "IX_Fornecedor_EmpresaFK",
                table: "Fornecedor",
                newName: "IX_Fornecedores_EmpresaFK");

            migrationBuilder.AlterColumn<long>(
                name: "EmpresaFK",
                table: "Fornecedor",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Fornecedores",
                table: "Fornecedor",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Empresas",
                table: "Empresa",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Fornecedores_Empresas_EmpresaFK",
                table: "Fornecedor",
                column: "EmpresaFK",
                principalTable: "Empresa",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
