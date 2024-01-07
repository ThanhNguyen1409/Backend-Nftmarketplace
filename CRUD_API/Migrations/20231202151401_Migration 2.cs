using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRUD_API.Migrations
{
    /// <inheritdoc />
    public partial class Migration2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Customer_customerId",
                table: "Order");

            migrationBuilder.RenameColumn(
                name: "customerId",
                table: "Order",
                newName: "accountId");

            migrationBuilder.RenameIndex(
                name: "IX_Order_customerId",
                table: "Order",
                newName: "IX_Order_accountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Account_accountId",
                table: "Order",
                column: "accountId",
                principalTable: "Account",
                principalColumn: "accountId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Account_accountId",
                table: "Order");

            migrationBuilder.RenameColumn(
                name: "accountId",
                table: "Order",
                newName: "customerId");

            migrationBuilder.RenameIndex(
                name: "IX_Order_accountId",
                table: "Order",
                newName: "IX_Order_customerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Customer_customerId",
                table: "Order",
                column: "customerId",
                principalTable: "Customer",
                principalColumn: "customerId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
