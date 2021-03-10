using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eShopSolution.Data.Migrations
{
    public partial class ProductImageFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "FileSize",
                table: "ProductImage",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("a974f0d2-3de9-427e-bc33-23bd5c8d3c6e"),
                column: "ConcurrencyStamp",
                value: "8c42cabc-3bbd-4160-898d-62f134ea386f");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("9324b240-bfb5-4a9f-8d27-718bbd662613"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "4fc2c3da-690c-4e2a-9334-3633648d1b75", "AQAAAAEAACcQAAAAEH/S/oYXqGv4uDBGLc4mBfSYbDM+gG3JJduDeWKmaVdO5wZRqjcg0BoAU3rB2bV4AQ==" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 3, 10, 21, 15, 0, 697, DateTimeKind.Local).AddTicks(1031));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "FileSize",
                table: "ProductImage",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("a974f0d2-3de9-427e-bc33-23bd5c8d3c6e"),
                column: "ConcurrencyStamp",
                value: "406d59ce-733d-4408-a167-e0e14e7d98d3");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("9324b240-bfb5-4a9f-8d27-718bbd662613"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "a3046c90-5ee1-4d0b-a0db-87957f9813a5", "AQAAAAEAACcQAAAAEJ/usEGU+3v6wGbLMDUephCLVdE+oj8OGBdGMdAvwkty8JnX2ZQ7YasOP/9OD7e52w==" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 3, 10, 15, 41, 48, 859, DateTimeKind.Local).AddTicks(8311));
        }
    }
}
