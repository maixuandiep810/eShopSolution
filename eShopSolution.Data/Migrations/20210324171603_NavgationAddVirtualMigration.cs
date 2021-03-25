using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eShopSolution.Data.Migrations
{
    public partial class NavgationAddVirtualMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("a974f0d2-3de9-427e-bc33-23bd5c8d3c6e"),
                column: "ConcurrencyStamp",
                value: "48bbafae-8b15-4c0c-ad74-07cc8bdfe47b");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("9324b240-bfb5-4a9f-8d27-718bbd662613"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "5dd39ff5-3f12-4b94-94fc-bbf72a288f2b", "AQAAAAEAACcQAAAAELGcqB9XDV/lX+YGVxeNzzTh2hg70bPJVSy+zSgxxk73rXVI/jRumyiaH3vbRulLgQ==" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 3, 25, 0, 16, 2, 508, DateTimeKind.Local).AddTicks(1164));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
