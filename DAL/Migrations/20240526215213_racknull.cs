using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class racknull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Racks_Aisles_AisleId",
                table: "Racks");

            migrationBuilder.AlterColumn<int>(
                name: "AisleId",
                table: "Racks",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Racks_Aisles_AisleId",
                table: "Racks",
                column: "AisleId",
                principalTable: "Aisles",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Racks_Aisles_AisleId",
                table: "Racks");

            migrationBuilder.AlterColumn<int>(
                name: "AisleId",
                table: "Racks",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Racks_Aisles_AisleId",
                table: "Racks",
                column: "AisleId",
                principalTable: "Aisles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
