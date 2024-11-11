using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MenuOnlineUdemy.Migrations
{
    /// <inheritdoc />
    public partial class fixfieldnames1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "stock",
                table: "Variants",
                newName: "Stock");

            migrationBuilder.RenameColumn(
                name: "price",
                table: "Variants",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "sell_min_price",
                table: "Products",
                newName: "SellMinPrice");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Stock",
                table: "Variants",
                newName: "stock");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Variants",
                newName: "price");

            migrationBuilder.RenameColumn(
                name: "SellMinPrice",
                table: "Products",
                newName: "sell_min_price");
        }
    }
}
