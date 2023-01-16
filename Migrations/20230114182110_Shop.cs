using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PROIECT.Migrations
{
    /// <inheritdoc />
    public partial class Shop : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Plant",
                type: "decimal(6,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.CreateTable(
                name: "Shop",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShopName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Adress = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shop", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AvailablePlant",
                columns: table => new
                {
                    ShopID = table.Column<int>(type: "int", nullable: false),
                    PlantID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AvailablePlant", x => new { x.PlantID, x.ShopID });
                    table.ForeignKey(
                        name: "FK_AvailablePlant_Plant_PlantID",
                        column: x => x.PlantID,
                        principalTable: "Plant",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AvailablePlant_Shop_ShopID",
                        column: x => x.ShopID,
                        principalTable: "Shop",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AvailablePlant_ShopID",
                table: "AvailablePlant",
                column: "ShopID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AvailablePlant");

            migrationBuilder.DropTable(
                name: "Shop");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Plant",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(6,2)");
        }
    }
}
