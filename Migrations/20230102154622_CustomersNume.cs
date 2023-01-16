using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PROIECT.Migrations
{
    /// <inheritdoc />
    public partial class CustomersNume : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Customer",
                newName: "Nume");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Nume",
                table: "Customer",
                newName: "Name");
        }
    }
}
