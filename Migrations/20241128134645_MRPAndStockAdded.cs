using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetFeast_Backend2.Migrations
{
    /// <inheritdoc />
    public partial class MRPAndStockAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "User",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "User");

            migrationBuilder.AddColumn<decimal>(
                name: "MRP",
                table: "Products",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 100m);

            migrationBuilder.AddColumn<int>(
                name: "Stock",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 10);

            migrationBuilder.AlterColumn<DateTime>(
                name: "OrderDate",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MRP",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Stock",
                table: "Products");

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "User",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldDefaultValue: "User");

            migrationBuilder.AlterColumn<int>(
                name: "OrderDate",
                table: "Orders",
                type: "int",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }
    }
}
