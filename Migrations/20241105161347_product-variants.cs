using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MenuOnlineUdemy.Migrations
{
    /// <inheritdoc />
    public partial class productvariants : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "Variants",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Variants_ProductId",
                table: "Variants",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Variants_Products_ProductId",
                table: "Variants",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Variants_Products_ProductId",
                table: "Variants");

            migrationBuilder.DropIndex(
                name: "IX_Variants_ProductId",
                table: "Variants");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Variants");
        }
    }
}
