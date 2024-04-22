using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Poznamky.Migrations
{
    /// <inheritdoc />
    public partial class upravy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "email",
                table: "Uzivatele");

            migrationBuilder.AddColumn<bool>(
                name: "Agreed",
                table: "Uzivatele",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Agreed",
                table: "Uzivatele");

            migrationBuilder.AddColumn<string>(
                name: "email",
                table: "Uzivatele",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
