using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IT_Asset_Mang_BE.Migrations
{
    /// <inheritdoc />
    public partial class AddRequestedAssetIdToCheckoutRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RequestedAssetId",
                table: "CheckoutRequests",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequestedAssetId",
                table: "CheckoutRequests");
        }
    }
}
