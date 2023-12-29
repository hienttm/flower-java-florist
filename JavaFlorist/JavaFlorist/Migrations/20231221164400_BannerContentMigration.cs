using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JavaFlorist.Migrations
{
    /// <inheritdoc />
    public partial class BannerContentMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "BannerContents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_BannerContents_ProductId",
                table: "BannerContents",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_BannerContents_Products_ProductId",
                table: "BannerContents",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BannerContents_Products_ProductId",
                table: "BannerContents");

            migrationBuilder.DropIndex(
                name: "IX_BannerContents_ProductId",
                table: "BannerContents");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "BannerContents");
        }
    }
}
