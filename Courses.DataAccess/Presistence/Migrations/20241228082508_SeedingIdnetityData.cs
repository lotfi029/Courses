using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Courses.DataAccess.Presistence.Migrations
{
    /// <inheritdoc />
    public partial class SeedingIdnetityData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "IsDefualt", "IsDeleted", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "019409cc-7157-71a4-99d5-e295c82679db", "019409cc-285c-796f-ba84-6d2d43a19e2e", false, false, "Admin", "ADMIN" },
                    { "019409cd-931b-7028-b668-bbc65d9213e0", "019409cd-7700-71c9-add3-699453281dc4", false, false, "Student", "STUDENT" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "DateOfBirth", "Email", "EmailConfirmed", "FirstName", "LastName", "Level", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "Rating", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "019409bf-3ae7-7cdf-995b-db4620f2ff5f", 0, "019409C1-DB8B-7B6F-A8A1-8E35FB4D0748", new DateOnly(2024, 12, 28), "admin@courses.edu", true, "Course", "Admin", "", false, null, "ADMIN@COURSES.EDU", "ADMIN", "AQAAAAIAAYagAAAAEB1mj/z+fDc1fii5XJzaIEYd/69RgtrQkCyJ+fbd3r00ddSKPyjGrvAJm2WNy/okcw==", null, false, null, "019409c1-af2c-7e25-bc46-da6e10412d65", false, "admin" });

            migrationBuilder.InsertData(
                table: "AspNetRoleClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "RoleId" },
                values: new object[,]
                {
                    { 1, "permissions", "category:add", "019409cc-7157-71a4-99d5-e295c82679db" },
                    { 2, "permissions", "category:read", "019409cc-7157-71a4-99d5-e295c82679db" },
                    { 3, "permissions", "category:update", "019409cc-7157-71a4-99d5-e295c82679db" },
                    { 4, "permissions", "category:delete", "019409cc-7157-71a4-99d5-e295c82679db" },
                    { 5, "permissions", "course:add", "019409cc-7157-71a4-99d5-e295c82679db" },
                    { 6, "permissions", "course:update", "019409cc-7157-71a4-99d5-e295c82679db" },
                    { 7, "permissions", "course:toggle", "019409cc-7157-71a4-99d5-e295c82679db" },
                    { 8, "permissions", "module:add", "019409cc-7157-71a4-99d5-e295c82679db" },
                    { 9, "permissions", "module:update", "019409cc-7157-71a4-99d5-e295c82679db" },
                    { 10, "permissions", "module:toggle", "019409cc-7157-71a4-99d5-e295c82679db" },
                    { 11, "permissions", "lesson:add", "019409cc-7157-71a4-99d5-e295c82679db" },
                    { 12, "permissions", "lesson:update", "019409cc-7157-71a4-99d5-e295c82679db" },
                    { 13, "permissions", "lesson:toggle", "019409cc-7157-71a4-99d5-e295c82679db" },
                    { 14, "permissions", "enrollment:add", "019409cc-7157-71a4-99d5-e295c82679db" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "019409cc-7157-71a4-99d5-e295c82679db", "019409bf-3ae7-7cdf-995b-db4620f2ff5f" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "019409cd-931b-7028-b668-bbc65d9213e0");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "019409cc-7157-71a4-99d5-e295c82679db", "019409bf-3ae7-7cdf-995b-db4620f2ff5f" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "019409cc-7157-71a4-99d5-e295c82679db");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "019409bf-3ae7-7cdf-995b-db4620f2ff5f");
        }
    }
}
