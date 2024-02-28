using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HeyStock.Migrations
{
    /// <inheritdoc />
    public partial class AddedprodutoscolumnandremovedstockscolumnfromtheVendatable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stocks_Vendas_vendaId",
                table: "Stocks");

            migrationBuilder.DropIndex(
                name: "IX_Stocks_vendaId",
                table: "Stocks");

            migrationBuilder.AddColumn<int>(
                name: "vendaId",
                table: "Produtos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Produtos_vendaId",
                table: "Produtos",
                column: "vendaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Produtos_Vendas_vendaId",
                table: "Produtos",
                column: "vendaId",
                principalTable: "Vendas",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Produtos_Vendas_vendaId",
                table: "Produtos");

            migrationBuilder.DropIndex(
                name: "IX_Produtos_vendaId",
                table: "Produtos");

            migrationBuilder.DropColumn(
                name: "vendaId",
                table: "Produtos");

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_vendaId",
                table: "Stocks",
                column: "vendaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Stocks_Vendas_vendaId",
                table: "Stocks",
                column: "vendaId",
                principalTable: "Vendas",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
