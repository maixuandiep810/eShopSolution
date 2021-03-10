using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eShopSolution.Data.Migrations
{
    public partial class AddProductImageTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "OrderDate",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2021, 3, 10, 5, 56, 36, 358, DateTimeKind.Local).AddTicks(1268));

            migrationBuilder.CreateTable(
                name: "ProductImage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Caption = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    FileSize = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductImage_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_ProductImage_ProductId",
                table: "ProductImage",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductImage");

            migrationBuilder.AlterColumn<DateTime>(
                name: "OrderDate",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2021, 3, 10, 5, 56, 36, 358, DateTimeKind.Local).AddTicks(1268),
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("a974f0d2-3de9-427e-bc33-23bd5c8d3c6e"),
                column: "ConcurrencyStamp",
                value: "06174a40-907f-4933-975c-9370a6afdddf");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("9324b240-bfb5-4a9f-8d27-718bbd662613"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "9ac0af5b-26bc-49b1-b41c-283698717b8c", "AQAAAAEAACcQAAAAELY7/LDGyb/W2/VYmOtHM43e+80fim6AeycfcBqeYdPuzPTCZmP9A7T3LujjeMRJQg==" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 3, 10, 5, 56, 36, 404, DateTimeKind.Local).AddTicks(3566));
        }
    }
}
