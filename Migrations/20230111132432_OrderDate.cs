using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PROIECT.Migrations
{
    /// <inheritdoc />
    public partial class OrderDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "OrderDate",
                table: "Order",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderDate",
                table: "Order");
        }
    }
}
