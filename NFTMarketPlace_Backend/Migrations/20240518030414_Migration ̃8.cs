using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NFTMarketPlace_Backend.Migrations
{
    /// <inheritdoc />
    public partial class Migratioñ8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    CollectionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CollectionName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CollectionAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CollectionSymbol = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CollectionImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountAddress = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.CollectionId);
                    table.ForeignKey(
                        name: "FK_Category_Account_AccountAddress",
                        column: x => x.AccountAddress,
                        principalTable: "Account",
                        principalColumn: "AccountAddress",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NFT",
                columns: table => new
                {
                    NftId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TokenId = table.Column<int>(type: "int", nullable: false),
                    CollectionId = table.Column<int>(type: "int", nullable: false),
                    TokenAddress = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Price = table.Column<float>(type: "real", nullable: false),
                    Owner = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Creator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TokenURI = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsListed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NFT", x => x.NftId);
                    table.ForeignKey(
                        name: "FK_NFT_Category_CollectionId",
                        column: x => x.CollectionId,
                        principalTable: "Category",
                        principalColumn: "CollectionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Category_AccountAddress",
                table: "Category",
                column: "AccountAddress");

            migrationBuilder.CreateIndex(
                name: "IX_NFT_CollectionId",
                table: "NFT",
                column: "CollectionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NFT");

            migrationBuilder.DropTable(
                name: "Category");
        }
    }
}
