using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRUD_API.Migrations
{
    /// <inheritdoc />
    public partial class Migration7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "rateStar",
                table: "Rating",
                newName: "ratingStar");

            migrationBuilder.RenameColumn(
                name: "rateComment",
                table: "Rating",
                newName: "ratingText");

            migrationBuilder.RenameColumn(
                name: "rateId",
                table: "Rating",
                newName: "ratingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ratingText",
                table: "Rating",
                newName: "rateComment");

            migrationBuilder.RenameColumn(
                name: "ratingStar",
                table: "Rating",
                newName: "rateStar");

            migrationBuilder.RenameColumn(
                name: "ratingId",
                table: "Rating",
                newName: "rateId");
        }
    }
}
