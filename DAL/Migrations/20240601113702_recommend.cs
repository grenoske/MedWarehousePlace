using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class recommend : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CellSize",
                table: "Warehouses",
                newName: "MaximumWeightOnUpperShelves");

            migrationBuilder.AddColumn<int>(
                name: "TurnoverRate",
                table: "Items",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "Weight",
                table: "Items",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "CellId",
                table: "Bins",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bins_CellId",
                table: "Bins",
                column: "CellId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bins_Cells_CellId",
                table: "Bins",
                column: "CellId",
                principalTable: "Cells",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bins_Cells_CellId",
                table: "Bins");

            migrationBuilder.DropIndex(
                name: "IX_Bins_CellId",
                table: "Bins");

            migrationBuilder.DropColumn(
                name: "TurnoverRate",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "CellId",
                table: "Bins");

            migrationBuilder.RenameColumn(
                name: "MaximumWeightOnUpperShelves",
                table: "Warehouses",
                newName: "CellSize");
        }
    }
}
