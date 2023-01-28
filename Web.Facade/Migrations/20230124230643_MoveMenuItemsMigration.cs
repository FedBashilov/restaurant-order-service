#nullable disable

namespace Web.Facade.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class MoveMenuItemsMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MenuItemIds",
                table: "Orders");

            migrationBuilder.CreateTable(
                name: "OrderMenuItems",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    MenuItemId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderMenuItems", x => new { x.OrderId, x.MenuItemId });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderMenuItems");

            migrationBuilder.AddColumn<string>(
                name: "MenuItemIds",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
