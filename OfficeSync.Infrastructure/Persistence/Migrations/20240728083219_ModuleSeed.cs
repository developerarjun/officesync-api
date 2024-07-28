using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OfficeSync.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ModuleSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Module",
                columns: new[] { "Id", "IsActive", "LastUpdatedAt", "LastUpdatedBy", "Name" },
                values: new object[] { 1, true, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "SA", "User Management" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Module",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
