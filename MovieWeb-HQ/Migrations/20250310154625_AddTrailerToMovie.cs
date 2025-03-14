using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieWeb_HQ.Migrations
{
    /// <inheritdoc />
    public partial class AddTrailerToMovie : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TrailerURL",
                table: "Movies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrailerURL",
                table: "Movies");
        }
    }
}
