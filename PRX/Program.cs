using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<PRX.Data.PRXDbContext>(options =>
            options.UseSqlServer("Server=Pirhayati\\MSSQLSERVER01;Database=PRX;Integrated Security=True;TrustServerCertificate=True;"));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"])),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true
    };
});


builder.Services.AddAuthorization();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "PRX API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. 
                       Enter 'Bearer' [space] and then your token in the text input below.
                       Example: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    // Make sure Swagger UI requires a JWT token
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new List<string>()
        }
    });


    c.DocInclusionPredicate((docName, apiDesc) =>
    {

        if (!apiDesc.TryGetMethodInfo(out var methodInfo)) return false;

        // Exclude controllers without group names
        if (string.IsNullOrWhiteSpace(apiDesc.GroupName)) return false;

        // Users Groups
        // Include controllers with specific group names
        var allowedGroups = new[] { "Users" /*, "UserAssets", "UserAssetTypes", "UserDebts", "UserDeposits", "UserFinancialChanges", "UserFuturePlans", "UserInvestments", "UserInvestmentExperiences", "UserMoreInformations", "UserStates", "UserTypes", "UserWithdrawals", "HaghighiUserEducationStatuses", "HaghighiUserEmploymentHistories", "HaghighiUserFinancialProfiles", "HaghighiUserProfiles", "HaghighiUserRelationships", "HoghooghiUsers", "HoghooghiUsersAssets", "HoghooghiUserBoardOfDirectors", "HoghooghiUserCompaniesWithMajorInvestors", "HoghooghiUserInvestmentDepartmentStaff", "UserAnswers", "UserAnswerOptions", "UserQuestions", "UserTestScores" */ }; 
        return allowedGroups.Contains(apiDesc.GroupName);


    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
