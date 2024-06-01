using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class inventoryBinRel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InventoryItemId",
                table: "Bins",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bins_InventoryItemId",
                table: "Bins",
                column: "InventoryItemId",
                unique: true,
                filter: "[InventoryItemId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Bins_InventoryItems_InventoryItemId",
                table: "Bins",
                column: "InventoryItemId",
                principalTable: "InventoryItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bins_InventoryItems_InventoryItemId",
                table: "Bins");

            migrationBuilder.DropIndex(
                name: "IX_Bins_InventoryItemId",
                table: "Bins");

            migrationBuilder.DropColumn(
                name: "InventoryItemId",
                table: "Bins");
        }
    }
}
