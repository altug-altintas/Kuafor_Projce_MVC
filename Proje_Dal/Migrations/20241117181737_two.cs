using Microsoft.EntityFrameworkCore.Migrations;

namespace Proje_Dal.Migrations
{
    public partial class two : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2b1de3fd-ef45-4168-a7d4-626d38a1ff5d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "78e38b74-99a2-4a38-bd36-7bb2c6935b9b");

            migrationBuilder.AddColumn<decimal>(
                name: "isLemUcreti",
                table: "KuaforTakvim",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1bc3c9a8-9766-4e75-b4f4-24572fe525a2", "4059454a-8918-4a73-8c94-3c6d4cbacd0b", "Member", "MEMBER" },
                    { "a527794e-1f74-4fec-a766-67777a1ee06d", "054fd8bd-a818-44a9-a696-b75c9448d9f7", "Admın", "ADMIN" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1bc3c9a8-9766-4e75-b4f4-24572fe525a2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a527794e-1f74-4fec-a766-67777a1ee06d");

            migrationBuilder.DropColumn(
                name: "isLemUcreti",
                table: "KuaforTakvim");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2b1de3fd-ef45-4168-a7d4-626d38a1ff5d", "a4f97db1-2da4-4f09-8895-a3c2bf0f729b", "Member", "MEMBER" },
                    { "78e38b74-99a2-4a38-bd36-7bb2c6935b9b", "5e331fc9-7ef1-4864-ae2e-be8f32b138bb", "Admın", "ADMIN" }
                });
        }
    }
}
