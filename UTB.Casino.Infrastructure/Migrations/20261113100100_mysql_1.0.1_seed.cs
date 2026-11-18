using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UTB.Casino.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class mysql_101_seed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { 1, "9cf14c2c-19e7-40d6-b744-8917505c3106", "Admin", "ADMIN" },
                    { 2, "be0efcde-9d0a-461d-8eb6-444b043d6660", "Manager", "MANAGER" },
                    { 3, "29dafca7-cd20-4cd9-a3dd-4779d7bac3ee", "Customer", "CUSTOMER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Balance", "ConcurrencyStamp", "CreatedAt", "DateOfBirth", "DisplayName", "Email", "EmailConfirmed", "IsBlocked", "LockoutEnabled", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { 1, 0, 0m, "5d0c3c0e-6a49-4c6b-9d52-0b7f4a1e8c11", new DateTime(2026, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Administrator", "admin@casino.local", true, false, true, "ADMIN@CASINO.LOCAL", "ADMIN", "AQAAAAEAAYagAAAAEOptWUKPaPstlzSUxZEj3V+pyyxkQ6FNHQVbpQVyK9dZsixNW4D0mmFMLAurbMh2ug==", false, "K3GQXW5NCV4UJ2R7M6ZPTD8YHBL9EA1F", false, "admin" },
                    { 2, 0, 0m, "8a2f6b71-3e0d-4d5a-b1c9-7e4d2f9a6b22", new DateTime(2026, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Manager", "manager@casino.local", true, false, true, "MANAGER@CASINO.LOCAL", "MANAGER", "AQAAAAEAAYagAAAAEAmOaHI+ChEOY/NUvyfelrUG2qWYcvSTbcaQF4i2GMlcPKO9nuVJ7GaCucDusCvsAg==", false, "T7NPA2LJ5XQ9WE4RB8MCHY3UD6KZGV1S", false, "manager" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 2 }
                });

            migrationBuilder.InsertData(
                table: "TransactionTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Deposit" },
                    { 2, "Stake" },
                    { 3, "Win" },
                    { 4, "Adjustment" }
                });

            migrationBuilder.InsertData(
                table: "Games",
                columns: new[] { "Id", "Description", "IsActive", "MaxBet", "MinBet", "Name", "Slug" },
                values: new object[,]
                {
                    { 1, "Three reels, one line. Triple sevens pay 100 times your bet.", true, 100m, 1m, "Lucky Reels", "Slots" },
                    { 2, "Numbers 0 to 36. Back a number, a colour, odd or even, or a half.", true, 500m, 1m, "European Roulette", "Roulette" },
                    { 3, "Beat the dealer to 21. Blackjack pays 3 to 2, dealer stands on 17.", true, 250m, 5m, "Blackjack", "Blackjack" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "TransactionTypes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TransactionTypes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "TransactionTypes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "TransactionTypes",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
