using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IT_Asset_Mang_BE.Migrations
{
    /// <inheritdoc />
    public partial class RemovedCheckoutRequests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckoutRequests_Users_RequestedByUserId",
                table: "CheckoutRequests");

            migrationBuilder.AddForeignKey(
                name: "FK_CheckoutRequests_Users_RequestedByUserId",
                table: "CheckoutRequests",
                column: "RequestedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckoutRequests_Users_RequestedByUserId",
                table: "CheckoutRequests");

            migrationBuilder.AddForeignKey(
                name: "FK_CheckoutRequests_Users_RequestedByUserId",
                table: "CheckoutRequests",
                column: "RequestedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
