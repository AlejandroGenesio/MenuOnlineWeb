using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MenuOnlineUdemy.Migrations
{
    /// <inheritdoc />
    public partial class update04 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "GroupStyleClosed",
                table: "ModifierGroups",
                type: "bit",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "GroupStyleClosed",
                table: "ModifierGroups",
                type: "int",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");
        }
    }
}
