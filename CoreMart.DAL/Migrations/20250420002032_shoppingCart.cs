using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoreMart.DAL.Migrations
{
    /// <inheritdoc />
    public partial class shoppingCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderHeaders_AspNetUsers_CustomerId",
                table: "OrderHeaders");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "OrderHeaders",
                newName: "ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderHeaders_CustomerId",
                table: "OrderHeaders",
                newName: "IX_OrderHeaders_ApplicationUserId");

            migrationBuilder.AddColumn<decimal>(
                name: "PriceAtThatTime",
                table: "ShoppingCarts",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<string>(
                name: "PaymentStatus",
                table: "OrderHeaders",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "OrderStatus",
                table: "OrderHeaders",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Carrier",
                table: "OrderHeaders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ShippingDate",
                table: "OrderHeaders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "TrackingNumber",
                table: "OrderHeaders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderHeaders_AspNetUsers_ApplicationUserId",
                table: "OrderHeaders",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderHeaders_AspNetUsers_ApplicationUserId",
                table: "OrderHeaders");

            migrationBuilder.DropColumn(
                name: "PriceAtThatTime",
                table: "ShoppingCarts");

            migrationBuilder.DropColumn(
                name: "Carrier",
                table: "OrderHeaders");

            migrationBuilder.DropColumn(
                name: "ShippingDate",
                table: "OrderHeaders");

            migrationBuilder.DropColumn(
                name: "TrackingNumber",
                table: "OrderHeaders");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "OrderHeaders",
                newName: "CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderHeaders_ApplicationUserId",
                table: "OrderHeaders",
                newName: "IX_OrderHeaders_CustomerId");

            migrationBuilder.AlterColumn<string>(
                name: "PaymentStatus",
                table: "OrderHeaders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "OrderStatus",
                table: "OrderHeaders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderHeaders_AspNetUsers_CustomerId",
                table: "OrderHeaders",
                column: "CustomerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
