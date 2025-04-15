using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryControl.Migrations
{
    /// <inheritdoc />
    public partial class AddProductPriceTableConfigs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductPrice_products_ProductId",
                table: "ProductPrice");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductPrice",
                table: "ProductPrice");

            migrationBuilder.RenameTable(
                name: "ProductPrice",
                newName: "product_price");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "product_price",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_ProductPrice_ProductId",
                table: "product_price",
                newName: "IX_product_price_ProductId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "product_price",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddPrimaryKey(
                name: "PK_product_price",
                table: "product_price",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_product_price_products_ProductId",
                table: "product_price",
                column: "ProductId",
                principalTable: "products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_product_price_products_ProductId",
                table: "product_price");

            migrationBuilder.DropPrimaryKey(
                name: "PK_product_price",
                table: "product_price");

            migrationBuilder.RenameTable(
                name: "product_price",
                newName: "ProductPrice");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "ProductPrice",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "IX_product_price_ProductId",
                table: "ProductPrice",
                newName: "IX_ProductPrice_ProductId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "ProductPrice",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductPrice",
                table: "ProductPrice",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductPrice_products_ProductId",
                table: "ProductPrice",
                column: "ProductId",
                principalTable: "products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
