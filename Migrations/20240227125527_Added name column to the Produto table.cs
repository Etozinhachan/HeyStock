using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HeyStock.Migrations
{
    /// <inheritdoc />
    public partial class AddednamecolumntotheProdutotable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "nome",
                table: "Produtos",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "nome",
                table: "Produtos");
        }
    }
}
