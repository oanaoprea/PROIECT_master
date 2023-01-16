using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PROIECT.Migrations
{
    /// <inheritdoc />
    public partial class OrderNumberAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Plant_PlantID",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_PlantID",
                table: "Order");

            migrationBuilder.AddColumn<int>(
                name: "Number",
                table: "Order",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "OrderPlant",
                columns: table => new
                {
                    OrdersOrderID = table.Column<int>(type: "int", nullable: false),
                    PlantsID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderPlant", x => new { x.OrdersOrderID, x.PlantsID });
                    table.ForeignKey(
                        name: "FK_OrderPlant_Order_OrdersOrderID",
                        column: x => x.OrdersOrderID,
                        principalTable: "Order",
                        principalColumn: "OrderID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderPlant_Plant_PlantsID",
                        column: x => x.PlantsID,
                        principalTable: "Plant",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderPlant_PlantsID",
                table: "OrderPlant",
                column: "PlantsID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderPlant");

            migrationBuilder.DropColumn(
                name: "Number",
                table: "Order");

            migrationBuilder.CreateIndex(
                name: "IX_Order_PlantID",
                table: "Order",
                column: "PlantID");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Plant_PlantID",
                table: "Order",
                column: "PlantID",
                principalTable: "Plant",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
