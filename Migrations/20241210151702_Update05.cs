using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MenuOnlineUdemy.Migrations
{
    /// <inheritdoc />
    public partial class Update05 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ModifierGroupModifierOptions",
                columns: table => new
                {
                    ModifierOptionId = table.Column<int>(type: "int", nullable: false),
                    ModifierGroupId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModifierGroupModifierOptions", x => new { x.ModifierGroupId, x.ModifierOptionId });
                    table.ForeignKey(
                        name: "FK_ModifierGroupModifierOptions_ModifierGroups_ModifierGroupId",
                        column: x => x.ModifierGroupId,
                        principalTable: "ModifierGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ModifierGroupModifierOptions_ModifierOptions_ModifierOptionId",
                        column: x => x.ModifierOptionId,
                        principalTable: "ModifierOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ModifierGroupModifierOptions_ModifierOptionId",
                table: "ModifierGroupModifierOptions",
                column: "ModifierOptionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ModifierGroupModifierOptions");
        }
    }
}
