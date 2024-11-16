using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebShopSportskeOpreme.Migrations
{
    /// <inheritdoc />
    public partial class AddedDiscountPercentageToPromotion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "DiscountPercentage",
                table: "Promotions",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiscountPercentage",
                table: "Promotions");
        }
    }
}
