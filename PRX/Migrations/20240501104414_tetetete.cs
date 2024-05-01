using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRX.Migrations
{
    /// <inheritdoc />
    public partial class tetetete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "UserWithdrawals",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "UserTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "UserTestScores",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "UserStates",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "UserQuestions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "UserMoreInformations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "UserInvestments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "UserInvestmentExperiences",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "UserFuturePlans",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "UserFinancialChanges",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "UserDeposits",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "UserDebts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "UserAssets",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "UserAnswers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "UserAnswerOptions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "HoghooghiUsersAssets",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "HoghooghiUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "HoghooghiUserInvestmentDepartmentStaff",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "HoghooghiUserCompaniesWithMajorInvestors",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "HoghooghiUserBoardOfDirectors",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "HaghighiUserRelationships",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "HaghighiUserProfiles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "HaghighiUserFinancialProfiles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "HaghighiUserEmploymentHistories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "HaghighiUserEducationStatuses",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "UserWithdrawals");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "UserTypes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "UserTestScores");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "UserStates");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "UserQuestions");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "UserMoreInformations");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "UserInvestments");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "UserInvestmentExperiences");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "UserFuturePlans");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "UserFinancialChanges");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "UserDeposits");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "UserDebts");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "UserAssets");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "UserAnswers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "UserAnswerOptions");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "HoghooghiUsersAssets");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "HoghooghiUsers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "HoghooghiUserInvestmentDepartmentStaff");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "HoghooghiUserCompaniesWithMajorInvestors");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "HoghooghiUserBoardOfDirectors");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "HaghighiUserRelationships");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "HaghighiUserProfiles");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "HaghighiUserFinancialProfiles");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "HaghighiUserEmploymentHistories");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "HaghighiUserEducationStatuses");
        }
    }
}
