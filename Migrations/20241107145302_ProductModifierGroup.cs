using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MenuOnlineUdemy.Migrations
{
    /// <inheritdoc />
    public partial class ProductModifierGroup2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductModifierGroups",
                columns: table => new
                {
                    ModifierGroupId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductModifierGroups", x => new { x.ProductId, x.ModifierGroupId });
                    table.ForeignKey(
                        name: "FK_ProductModifierGroups_ModifierGroups_ModifierGroupId",
                        column: x => x.ModifierGroupId,
                        principalTable: "ModifierGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductModifierGroups_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductModifierGroups_ModifierGroupId",
                table: "ProductModifierGroups",
                column: "ModifierGroupId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductModifierGroups");
        }
    }
}
