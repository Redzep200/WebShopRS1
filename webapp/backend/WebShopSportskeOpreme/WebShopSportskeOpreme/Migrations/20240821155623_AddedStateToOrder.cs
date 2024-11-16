using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebShopSportskeOpreme.Migrations
{
    /// <inheritdoc />
    public partial class AddedStateToOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
               name: "State",
               table: "Orders");
        }
    }
}
