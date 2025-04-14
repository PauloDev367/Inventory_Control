using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryControl.Migrations
{
    /// <inheritdoc />
    public partial class AddMinimumStockColumnToProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MinimumStock",
                table: "products",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MinimumStock",
                table: "products");
        }
    }
}
