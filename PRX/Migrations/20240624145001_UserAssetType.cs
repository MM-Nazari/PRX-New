using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PRX.Migrations
{
    /// <inheritdoc />
    public partial class UserAssetType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "UserAssetTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "ساختمان و ملک" },
                    { 2, "خودرو" },
                    { 3, "طلا و ارز" },
                    { 4, "سهام" },
                    { 5, " اوراق مشارکت دولتی و شرکتی" },
                    { 6, "وجه نقد/ مطالبات از سایز افراد/ حساب پس انداز و سپرده بانکی" },
                    { 7, "سایر دارایی ها" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserAssetTypes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "UserAssetTypes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "UserAssetTypes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "UserAssetTypes",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "UserAssetTypes",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "UserAssetTypes",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "UserAssetTypes",
                keyColumn: "Id",
                keyValue: 7);
        }
    }
}
