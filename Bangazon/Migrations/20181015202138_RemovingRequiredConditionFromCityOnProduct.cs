using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Bangazon.Migrations
{
    public partial class RemovingRequiredConditionFromCityOnProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { "2ae83db7-ee9e-4e9f-9a5d-15e15f7b2075", "f3c84b69-74c5-4a1c-9857-179f5ecdcaa2" });

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "Product",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "StreetAddress", "TwoFactorEnabled", "UserName" },
                values: new object[] { "c7ad5097-f85a-4db2-8c7b-aa2341fab612", 0, "0ca1ee64-83b9-4819-9db5-9b3ef46e4473", "admin@admin.com", true, "admin", "admin", false, null, "ADMIN@ADMIN.COM", "ADMIN@ADMIN.COM", "AQAAAAEAACcQAAAAEP23NJVIT6ay+9nn4+z1Fs2moT7oshXGQwRX3TYeeIkkfZPkZHJFmzk6QKDzR+1j7g==", null, false, "eea7cd19-62c4-45ce-b2ec-b5b8f0f4c62c", "123 Infinity Way", false, "admin@admin.com" });

            migrationBuilder.UpdateData(
                table: "PaymentType",
                keyColumn: "PaymentTypeId",
                keyValue: 1,
                columns: new[] { "DateCreated", "UserId" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "c7ad5097-f85a-4db2-8c7b-aa2341fab612" });

            migrationBuilder.UpdateData(
                table: "PaymentType",
                keyColumn: "PaymentTypeId",
                keyValue: 2,
                columns: new[] { "DateCreated", "UserId" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "c7ad5097-f85a-4db2-8c7b-aa2341fab612" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { "c7ad5097-f85a-4db2-8c7b-aa2341fab612", "0ca1ee64-83b9-4819-9db5-9b3ef46e4473" });

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "Product",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "StreetAddress", "TwoFactorEnabled", "UserName" },
                values: new object[] { "2ae83db7-ee9e-4e9f-9a5d-15e15f7b2075", 0, "f3c84b69-74c5-4a1c-9857-179f5ecdcaa2", "admin@admin.com", true, "admin", "admin", false, null, "ADMIN@ADMIN.COM", "ADMIN@ADMIN.COM", "AQAAAAEAACcQAAAAEOtP2cc19ZQOfWPmktNtpFZH7ZD+iErq5OFHRIF3A4sUDW6dj6Nz39MLtExfs7GmHQ==", null, false, "fc54531d-4530-490a-bcf9-0b44a3ff86fe", "123 Infinity Way", false, "admin@admin.com" });

            migrationBuilder.UpdateData(
                table: "PaymentType",
                keyColumn: "PaymentTypeId",
                keyValue: 1,
                columns: new[] { "DateCreated", "UserId" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2ae83db7-ee9e-4e9f-9a5d-15e15f7b2075" });

            migrationBuilder.UpdateData(
                table: "PaymentType",
                keyColumn: "PaymentTypeId",
                keyValue: 2,
                columns: new[] { "DateCreated", "UserId" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2ae83db7-ee9e-4e9f-9a5d-15e15f7b2075" });
        }
    }
}
