using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoreMart.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Orders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderHeaders_AspNetUsers_ApplicationUserId",
                table: "OrderHeaders");

            migrationBuilder.DropColumn(
                name: "Carrier",
                table: "OrderHeaders");

            migrationBuilder.DropColumn(
                name: "OrderStatus",
                table: "OrderHeaders");

            migrationBuilder.DropColumn(
                name: "PaymentIntentId",
                table: "OrderHeaders");

            migrationBuilder.DropColumn(
                name: "PaymentStatus",
                table: "OrderHeaders");

            migrationBuilder.DropColumn(
                name: "SessionId",
                table: "OrderHeaders");

            migrationBuilder.DropColumn(
                name: "ShippingDate",
                table: "OrderHeaders");

            migrationBuilder.DropColumn(
                name: "TrackingNumber",
                table: "OrderHeaders");

            migrationBuilder.RenameColumn(
                name: "TotalPrice",
                table: "OrderHeaders",
                newName: "TotalAmount");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "OrderHeaders",
                newName: "CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderHeaders_ApplicationUserId",
                table: "OrderHeaders",
                newName: "IX_OrderHeaders_CustomerId");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "OrderHeaders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderHeaders_AspNetUsers_CustomerId",
                table: "OrderHeaders",
                column: "CustomerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderHeaders_AspNetUsers_CustomerId",
                table: "OrderHeaders");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "OrderHeaders");

            migrationBuilder.RenameColumn(
                name: "TotalAmount",
                table: "OrderHeaders",
                newName: "TotalPrice");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "OrderHeaders",
                newName: "ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderHeaders_CustomerId",
                table: "OrderHeaders",
                newName: "IX_OrderHeaders_ApplicationUserId");

            migrationBuilder.AddColumn<string>(
                name: "Carrier",
                table: "OrderHeaders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrderStatus",
                table: "OrderHeaders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentIntentId",
                table: "OrderHeaders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentStatus",
                table: "OrderHeaders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SessionId",
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
    }
}
