using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRX.Migrations
{
    /// <inheritdoc />
    public partial class test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_userStates_Users_UserId",
                table: "userStates");

            migrationBuilder.DropForeignKey(
                name: "FK_userTypes_Users_UserId",
                table: "userTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_userTypes",
                table: "userTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_userStates",
                table: "userStates");

            migrationBuilder.RenameTable(
                name: "userTypes",
                newName: "UserTypes");

            migrationBuilder.RenameTable(
                name: "userStates",
                newName: "UserStates");

            migrationBuilder.RenameIndex(
                name: "IX_userTypes_UserId",
                table: "UserTypes",
                newName: "IX_UserTypes_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_userStates_UserId",
                table: "UserStates",
                newName: "IX_UserStates_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserTypes",
                table: "UserTypes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserStates",
                table: "UserStates",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserStates_Users_UserId",
                table: "UserStates",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserTypes_Users_UserId",
                table: "UserTypes",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserStates_Users_UserId",
                table: "UserStates");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTypes_Users_UserId",
                table: "UserTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserTypes",
                table: "UserTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserStates",
                table: "UserStates");

            migrationBuilder.RenameTable(
                name: "UserTypes",
                newName: "userTypes");

            migrationBuilder.RenameTable(
                name: "UserStates",
                newName: "userStates");

            migrationBuilder.RenameIndex(
                name: "IX_UserTypes_UserId",
                table: "userTypes",
                newName: "IX_userTypes_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserStates_UserId",
                table: "userStates",
                newName: "IX_userStates_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_userTypes",
                table: "userTypes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_userStates",
                table: "userStates",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_userStates_Users_UserId",
                table: "userStates",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_userTypes_Users_UserId",
                table: "userTypes",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
