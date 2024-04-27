using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRX.Migrations
{
    /// <inheritdoc />
    public partial class temp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserAssetTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAssetTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserQuestions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserQuestions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ReferenceCode = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserAnswerOptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAnswerOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAnswerOptions_UserQuestions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "UserQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HaghighiUserEducationStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LastDegree = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FieldOfStudy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GraduationYear = table.Column<int>(type: "int", nullable: false),
                    IssuingAuthority = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsComplete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HaghighiUserEducationStatuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HaghighiUserEducationStatuses_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HaghighiUserEmploymentHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    EmployerLocation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MainActivity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WorkAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WorkPhone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsComplete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HaghighiUserEmploymentHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HaghighiUserEmploymentHistories_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HaghighiUserFinancialProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    MainContinuousIncome = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OtherIncomes = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SupportFromOthers = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ContinuousExpenses = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OccasionalExpenses = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ContributionToOthers = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsComplete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HaghighiUserFinancialProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HaghighiUserFinancialProfiles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HaghighiUserProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FathersName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NationalNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BirthPlace = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BirthCertificateNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    MaritalStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostalCode = table.Column<int>(type: "int", maxLength: 10, nullable: false),
                    HomePhone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fax = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BestTimeToCall = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ResidentialAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsComplete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HaghighiUserProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HaghighiUserProfiles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HaghighiUserRelationships",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RelationshipStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BirthYear = table.Column<int>(type: "int", nullable: false),
                    EducationLevel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmploymentStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AverageMonthlyIncome = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AverageMonthlyExpense = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ApproximateAssets = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ApproximateLiabilities = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsComplete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HaghighiUserRelationships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HaghighiUserRelationships_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HoghooghiUserBoardOfDirectors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EducationalLevel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FieldOfStudy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExecutiveExperience = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FamiliarityWithCapitalMarket = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PersonalInvestmentExperienceInStockExchange = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsComplete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoghooghiUserBoardOfDirectors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HoghooghiUserBoardOfDirectors_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HoghooghiUserCompaniesWithMajorInvestors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanySubject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PercentageOfTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsComplete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoghooghiUserCompaniesWithMajorInvestors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HoghooghiUserCompaniesWithMajorInvestors_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HoghooghiUserInvestmentDepartmentStaff",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EducationalLevel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FieldOfStudy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExecutiveExperience = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FamiliarityWithCapitalMarket = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PersonalInvestmentExperienceInStockExchange = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsComplete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoghooghiUserInvestmentDepartmentStaff", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HoghooghiUserInvestmentDepartmentStaff_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HoghooghiUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegistrationNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RegistrationLocation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NationalId = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    MainActivityBasedOnCharter = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MainActivityBasedOnPastThreeYearsPerformance = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    LandlinePhone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fax = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    BestTimeToCall = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RepresentativeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RepresentativeNationalId = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    RepresentativeMobilePhone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsComplete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoghooghiUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HoghooghiUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HoghooghiUsersAssets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    FiscalYear = table.Column<int>(type: "int", nullable: false),
                    RegisteredCapital = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ApproximateAssetValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalLiabilities = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalInvestments = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OperationalIncome = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OtherIncome = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OperationalExpenses = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OtherExpenses = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OperationalProfitOrLoss = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NetProfitOrLoss = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AccumulatedProfitOrLoss = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsComplete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoghooghiUsersAssets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HoghooghiUsersAssets_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserAssets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    AssetTypeId = table.Column<int>(type: "int", nullable: false),
                    AssetValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AssetPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsComplete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAssets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAssets_UserAssetTypes_AssetTypeId",
                        column: x => x.AssetTypeId,
                        principalTable: "UserAssetTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserAssets_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserDebts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    DebtTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DebtAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DebtDueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DebtRepaymentPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsComplete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDebts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserDebts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserDeposits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    DepositAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DepositDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DepositSource = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsComplete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDeposits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserDeposits_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserFinancialChanges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsComplete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFinancialChanges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserFinancialChanges_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserFuturePlans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsComplete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFuturePlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserFuturePlans_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserInvestmentExperiences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    InvestmentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InvestmentAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    InvestmentDurationMonths = table.Column<int>(type: "int", nullable: false),
                    ProfitLossAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProfitLossDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConversionReason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsComplete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInvestmentExperiences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserInvestmentExperiences_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserInvestments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsComplete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInvestments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserInvestments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserMoreInformations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Info = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsComplete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMoreInformations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserMoreInformations_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "userStates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userStates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_userStates_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTestScores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTestScores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserTestScores_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "userTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_userTypes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserWithdrawals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    WithdrawalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WithdrawalDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WithdrawalReason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsComplete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserWithdrawals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserWithdrawals_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserAnswers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    AnswerOptionId = table.Column<int>(type: "int", nullable: false),
                    AnswerText = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAnswers_UserAnswerOptions_AnswerOptionId",
                        column: x => x.AnswerOptionId,
                        principalTable: "UserAnswerOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserAnswers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HaghighiUserEducationStatuses_UserId",
                table: "HaghighiUserEducationStatuses",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HaghighiUserEmploymentHistories_UserId",
                table: "HaghighiUserEmploymentHistories",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_HaghighiUserFinancialProfiles_UserId",
                table: "HaghighiUserFinancialProfiles",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HaghighiUserProfiles_NationalNumber_Email_BirthCertificateNumber",
                table: "HaghighiUserProfiles",
                columns: new[] { "NationalNumber", "Email", "BirthCertificateNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HaghighiUserProfiles_UserId",
                table: "HaghighiUserProfiles",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HaghighiUserRelationships_UserId",
                table: "HaghighiUserRelationships",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_HoghooghiUserBoardOfDirectors_UserId",
                table: "HoghooghiUserBoardOfDirectors",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_HoghooghiUserCompaniesWithMajorInvestors_UserId",
                table: "HoghooghiUserCompaniesWithMajorInvestors",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_HoghooghiUserInvestmentDepartmentStaff_UserId",
                table: "HoghooghiUserInvestmentDepartmentStaff",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_HoghooghiUsers_UserId",
                table: "HoghooghiUsers",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HoghooghiUsersAssets_UserId",
                table: "HoghooghiUsersAssets",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAnswerOptions_QuestionId",
                table: "UserAnswerOptions",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAnswers_AnswerOptionId",
                table: "UserAnswers",
                column: "AnswerOptionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserAnswers_UserId",
                table: "UserAnswers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAssets_AssetTypeId",
                table: "UserAssets",
                column: "AssetTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAssets_UserId",
                table: "UserAssets",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDebts_UserId",
                table: "UserDebts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDeposits_UserId",
                table: "UserDeposits",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFinancialChanges_UserId",
                table: "UserFinancialChanges",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserFuturePlans_UserId",
                table: "UserFuturePlans",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserInvestmentExperiences_UserId",
                table: "UserInvestmentExperiences",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserInvestments_UserId",
                table: "UserInvestments",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserMoreInformations_UserId",
                table: "UserMoreInformations",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_PhoneNumber_ReferenceCode",
                table: "Users",
                columns: new[] { "PhoneNumber", "ReferenceCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_userStates_UserId",
                table: "userStates",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserTestScores_UserId",
                table: "UserTestScores",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_userTypes_UserId",
                table: "userTypes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserWithdrawals_UserId",
                table: "UserWithdrawals",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HaghighiUserEducationStatuses");

            migrationBuilder.DropTable(
                name: "HaghighiUserEmploymentHistories");

            migrationBuilder.DropTable(
                name: "HaghighiUserFinancialProfiles");

            migrationBuilder.DropTable(
                name: "HaghighiUserProfiles");

            migrationBuilder.DropTable(
                name: "HaghighiUserRelationships");

            migrationBuilder.DropTable(
                name: "HoghooghiUserBoardOfDirectors");

            migrationBuilder.DropTable(
                name: "HoghooghiUserCompaniesWithMajorInvestors");

            migrationBuilder.DropTable(
                name: "HoghooghiUserInvestmentDepartmentStaff");

            migrationBuilder.DropTable(
                name: "HoghooghiUsers");

            migrationBuilder.DropTable(
                name: "HoghooghiUsersAssets");

            migrationBuilder.DropTable(
                name: "UserAnswers");

            migrationBuilder.DropTable(
                name: "UserAssets");

            migrationBuilder.DropTable(
                name: "UserDebts");

            migrationBuilder.DropTable(
                name: "UserDeposits");

            migrationBuilder.DropTable(
                name: "UserFinancialChanges");

            migrationBuilder.DropTable(
                name: "UserFuturePlans");

            migrationBuilder.DropTable(
                name: "UserInvestmentExperiences");

            migrationBuilder.DropTable(
                name: "UserInvestments");

            migrationBuilder.DropTable(
                name: "UserMoreInformations");

            migrationBuilder.DropTable(
                name: "userStates");

            migrationBuilder.DropTable(
                name: "UserTestScores");

            migrationBuilder.DropTable(
                name: "userTypes");

            migrationBuilder.DropTable(
                name: "UserWithdrawals");

            migrationBuilder.DropTable(
                name: "UserAnswerOptions");

            migrationBuilder.DropTable(
                name: "UserAssetTypes");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "UserQuestions");
        }
    }
}
