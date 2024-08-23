using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace orderit_api.Migrations
{
    /// <inheritdoc />
    public partial class SalespersonUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Sellers_SellerId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "Sellers");

            migrationBuilder.RenameColumn(
                name: "SellerId",
                table: "Orders",
                newName: "SalespersonId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_SellerId",
                table: "Orders",
                newName: "IX_Orders_SalespersonId");

            migrationBuilder.CreateTable(
                name: "Salespersons",
                columns: table => new
                {
                    SalespersonId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    SecondName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    SecondLastName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Salespersons", x => x.SalespersonId);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Salespersons_SalespersonId",
                table: "Orders",
                column: "SalespersonId",
                principalTable: "Salespersons",
                principalColumn: "SalespersonId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Salespersons_SalespersonId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "Salespersons");

            migrationBuilder.RenameColumn(
                name: "SalespersonId",
                table: "Orders",
                newName: "SellerId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_SalespersonId",
                table: "Orders",
                newName: "IX_Orders_SellerId");

            migrationBuilder.CreateTable(
                name: "Sellers",
                columns: table => new
                {
                    SellerId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    SecondLastName = table.Column<string>(type: "text", nullable: false),
                    SecondName = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sellers", x => x.SellerId);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Sellers_SellerId",
                table: "Orders",
                column: "SellerId",
                principalTable: "Sellers",
                principalColumn: "SellerId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
