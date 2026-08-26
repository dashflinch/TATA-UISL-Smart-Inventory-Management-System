using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanyInventory.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProductCostFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Products",
                newName: "ServiceCost");

            migrationBuilder.AddColumn<decimal>(
                name: "MaterialCost",
                table: "Products",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaterialCost",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "ServiceCost",
                table: "Products",
                newName: "Price");
        }
    }
}
