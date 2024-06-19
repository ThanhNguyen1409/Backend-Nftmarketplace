using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NFTMarketPlace_Backend.Migrations
{
    /// <inheritdoc />
    public partial class MigrationAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Avartar",
                table: "Account",
                newName: "Avatar");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Avatar",
                table: "Account",
                newName: "Avartar");
        }
    }
}
