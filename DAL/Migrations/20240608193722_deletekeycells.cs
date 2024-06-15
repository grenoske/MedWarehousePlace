using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class deletekeycells : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cells_Warehouses_WarehouseId",
                table: "Cells");

            migrationBuilder.DropIndex(
                name: "IX_Cells_WarehouseId",
                table: "Cells");

            migrationBuilder.DropColumn(
                name: "WarehouseId",
                table: "Cells");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WarehouseId",
                table: "Cells",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cells_WarehouseId",
                table: "Cells",
                column: "WarehouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cells_Warehouses_WarehouseId",
                table: "Cells",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "Id");
        }
    }
}
