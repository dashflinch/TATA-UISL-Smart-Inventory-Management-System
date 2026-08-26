using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanyInventory.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTicketModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Products_ProductId",
                table: "Tickets");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "Tickets",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Tickets",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AddColumn<string>(
                name: "AffectedSystem",
                table: "Tickets",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Impact",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ResolutionNotes",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubCategory",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Products_ProductId",
                table: "Tickets",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Products_ProductId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "AffectedSystem",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "Impact",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "ResolutionNotes",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "SubCategory",
                table: "Tickets");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "Tickets",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Tickets",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Products_ProductId",
                table: "Tickets",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
