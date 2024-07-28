using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OfficeSync.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AdminUsersSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Profiles",
                columns: new[] { "Id", "DateOfBirth", "FirstName", "IsActive", "LastName", "LastUpdatedAt", "LastUpdatedBy", "MiddleName", "Suffix" },
                values: new object[,]
                {
                    { 2, null, "Dibya", true, "Joshi", new DateTimeOffset(new DateTime(2024, 7, 28, 8, 47, 13, 337, DateTimeKind.Unspecified).AddTicks(8475), new TimeSpan(0, 0, 0, 0, 0)), "SA", null, null },
                    { 3, null, "Sandesh", true, "Thapa", new DateTimeOffset(new DateTime(2024, 7, 28, 8, 47, 13, 337, DateTimeKind.Unspecified).AddTicks(8620), new TimeSpan(0, 0, 0, 0, 0)), "SA", null, null },
                    { 4, null, "Abin", true, "Shrestha", new DateTimeOffset(new DateTime(2024, 7, 28, 8, 47, 13, 337, DateTimeKind.Unspecified).AddTicks(8651), new TimeSpan(0, 0, 0, 0, 0)), "SA", null, null },
                    { 5, null, "Nabin", true, "Khadka", new DateTimeOffset(new DateTime(2024, 7, 28, 8, 47, 13, 337, DateTimeKind.Unspecified).AddTicks(8681), new TimeSpan(0, 0, 0, 0, 0)), "SA", null, null }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "DefaultMfaProvider", "Email", "EmailConfirmed", "IsActive", "LastUpdatedAt", "LastUpdatedBy", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { 2, 0, "b91d74d8-5516-4cfb-b7e5-02d3885cb2bd", 0, "developerdibya77@gmail.com", true, true, new DateTimeOffset(new DateTime(2024, 7, 28, 8, 47, 13, 337, DateTimeKind.Unspecified).AddTicks(8584), new TimeSpan(0, 0, 0, 0, 0)), "SA", true, null, "DEVELOPERDIBYA77@GMAIL.COM", "DEVELOPERDIBYA77@GMAIL.COM", null, null, false, "851536ae-aded-4c9d-b342-738d0fb066eb", false, "developerdibya77@gmail.com" },
                    { 3, 0, "b91d74d8-5516-4cfb-b7e5-02d3885cb2bd", 0, "sandeshthapa5907@gmail.com", true, true, new DateTimeOffset(new DateTime(2024, 7, 28, 8, 47, 13, 337, DateTimeKind.Unspecified).AddTicks(8631), new TimeSpan(0, 0, 0, 0, 0)), "SA", true, null, "SANDESHTHAPA5907@GMAIL.COM", "SANDESHTHAPA5907@GMAIL.COM", null, null, false, "851536ae-aded-4c9d-b342-738d0fb066eb", false, "sandeshthapa5907@gmail.com" },
                    { 4, 0, "b91d74d8-5516-4cfb-b7e5-02d3885cb2bd", 0, "shresthaabin88@gmail.com", true, true, new DateTimeOffset(new DateTime(2024, 7, 28, 8, 47, 13, 337, DateTimeKind.Unspecified).AddTicks(8661), new TimeSpan(0, 0, 0, 0, 0)), "SA", true, null, "SHRESTHAABIN88@GMAIL.COM", "SHRESTHAABIN88@GMAIL.COM", null, null, false, "851536ae-aded-4c9d-b342-738d0fb066eb", false, "shresthaabin88@gmail.com" },
                    { 5, 0, "b91d74d8-5516-4cfb-b7e5-02d3885cb2bd", 0, "nabinkhadka330@gmail.com", true, true, new DateTimeOffset(new DateTime(2024, 7, 28, 8, 47, 13, 337, DateTimeKind.Unspecified).AddTicks(8695), new TimeSpan(0, 0, 0, 0, 0)), "SA", true, null, "NABINKHADKA330@GMAIL.COM", "NABINKHADKA330@GMAIL.COM", null, null, false, "851536ae-aded-4c9d-b342-738d0fb066eb", false, "nabinkhadka330@gmail.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { 1, 2 },
                    { 1, 3 },
                    { 1, 4 },
                    { 1, 5 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 1, 2 });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 1, 3 });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 1, 4 });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 1, 5 });

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Profiles",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Profiles",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Profiles",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Profiles",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
