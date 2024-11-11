using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MenuOnlineUdemy.Migrations
{
    /// <inheritdoc />
    public partial class newcolumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MinToSelect",
                table: "ModifierGroups");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "ModifierGroups",
                newName: "Label");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "ModifierGroups",
                newName: "OptionsGroup");

            migrationBuilder.AddColumn<decimal>(
                name: "ExtraPrice",
                table: "ModifierGroups",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExtraPrice",
                table: "ModifierGroups");

            migrationBuilder.RenameColumn(
                name: "OptionsGroup",
                table: "ModifierGroups",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "Label",
                table: "ModifierGroups",
                newName: "Name");

            migrationBuilder.AddColumn<int>(
                name: "MinToSelect",
                table: "ModifierGroups",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
