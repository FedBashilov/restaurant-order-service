#nullable disable

namespace Web.Facade.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddMenuItemIdsMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MenuItemIds",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MenuItemIds",
                table: "Orders");
        }
    }
}
