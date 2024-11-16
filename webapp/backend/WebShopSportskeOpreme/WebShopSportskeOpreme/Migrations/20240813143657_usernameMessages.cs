using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebShopSportskeOpreme.Migrations
{
    /// <inheritdoc />
    public partial class usernameMessages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "CustomerQuestions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Username",
                table: "CustomerQuestions");
        }
    }
}
