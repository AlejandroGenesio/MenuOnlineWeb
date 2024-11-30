using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MenuOnlineUdemy.Migrations
{
    /// <inheritdoc />
    public partial class Update02 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ExtraPrice",
                table: "ProductModifierGroups",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GroupStyle",
                table: "ProductModifierGroups",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GroupStyleClosed",
                table: "ProductModifierGroups",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Label",
                table: "ProductModifierGroups",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OptionsGroup",
                table: "ProductModifierGroups",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExtraPrice",
                table: "ProductModifierGroups");

            migrationBuilder.DropColumn(
                name: "GroupStyle",
                table: "ProductModifierGroups");

            migrationBuilder.DropColumn(
                name: "GroupStyleClosed",
                table: "ProductModifierGroups");

            migrationBuilder.DropColumn(
                name: "Label",
                table: "ProductModifierGroups");

            migrationBuilder.DropColumn(
                name: "OptionsGroup",
                table: "ProductModifierGroups");
        }
    }
}
