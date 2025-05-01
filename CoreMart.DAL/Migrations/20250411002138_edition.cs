using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoreMart.DAL.Migrations
{
    /// <inheritdoc />
    public partial class edition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "SubCategories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImageURL",
                table: "SubCategories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImageURL",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImageURL",
                table: "Brands",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "SubCategories");

            migrationBuilder.DropColumn(
                name: "ImageURL",
                table: "SubCategories");

            migrationBuilder.DropColumn(
                name: "ImageURL",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "ImageURL",
                table: "Brands");
        }
    }
}
