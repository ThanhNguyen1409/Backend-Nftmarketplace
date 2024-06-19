using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NFTMarketPlace_Backend.Migrations
{
    /// <inheritdoc />
    public partial class Migratioñ9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Category_Account_AccountAddress",
                table: "Category");

            migrationBuilder.DropForeignKey(
                name: "FK_NFT_Category_CollectionId",
                table: "NFT");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Category",
                table: "Category");

            migrationBuilder.RenameTable(
                name: "Category",
                newName: "Collection");

            migrationBuilder.RenameIndex(
                name: "IX_Category_AccountAddress",
                table: "Collection",
                newName: "IX_Collection_AccountAddress");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Collection",
                table: "Collection",
                column: "CollectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Collection_Account_AccountAddress",
                table: "Collection",
                column: "AccountAddress",
                principalTable: "Account",
                principalColumn: "AccountAddress",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NFT_Collection_CollectionId",
                table: "NFT",
                column: "CollectionId",
                principalTable: "Collection",
                principalColumn: "CollectionId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Collection_Account_AccountAddress",
                table: "Collection");

            migrationBuilder.DropForeignKey(
                name: "FK_NFT_Collection_CollectionId",
                table: "NFT");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Collection",
                table: "Collection");

            migrationBuilder.RenameTable(
                name: "Collection",
                newName: "Category");

            migrationBuilder.RenameIndex(
                name: "IX_Collection_AccountAddress",
                table: "Category",
                newName: "IX_Category_AccountAddress");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Category",
                table: "Category",
                column: "CollectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Category_Account_AccountAddress",
                table: "Category",
                column: "AccountAddress",
                principalTable: "Account",
                principalColumn: "AccountAddress",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NFT_Category_CollectionId",
                table: "NFT",
                column: "CollectionId",
                principalTable: "Category",
                principalColumn: "CollectionId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
