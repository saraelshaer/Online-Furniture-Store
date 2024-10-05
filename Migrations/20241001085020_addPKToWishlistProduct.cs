using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FurnitureStore.Migrations
{
    /// <inheritdoc />
    public partial class addPKToWishlistProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_WishListProducts",
                table: "WishListProducts");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "WishListProducts",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WishListProducts",
                table: "WishListProducts",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_WishListProducts_ProductId",
                table: "WishListProducts",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_WishListProducts",
                table: "WishListProducts");

            migrationBuilder.DropIndex(
                name: "IX_WishListProducts_ProductId",
                table: "WishListProducts");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "WishListProducts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WishListProducts",
                table: "WishListProducts",
                columns: new[] { "ProductId", "WishListId" });
        }
    }
}
