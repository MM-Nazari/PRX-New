using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Net.Http.Json;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using PRX.Utils;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
//.AddJsonOptions(options =>
//{
//    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
//});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

builder.Services.AddDbContext<PRX.Data.PRXDbContext>(options =>
            options.UseSqlServer("Server=Pirhayati\\MSSQLSERVER01;Database=PRX_BACKUP;Integrated Security=True;TrustServerCertificate=True;"));

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

    o.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(' ').Last();
            context.Token = accessToken;
            return Task.CompletedTask;
        },
        OnAuthenticationFailed = context =>
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            var result = JsonConvert.SerializeObject(new { message = "Authentication failed." });
            return context.Response.WriteAsync(result);
        },
        OnForbidden = context =>
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            var result = JsonConvert.SerializeObject(new { message = ResponseMessages.Forbidden });
            return context.Response.WriteAsync(result);
        }
    };

    //o.Events = new JwtBearerEvents
    //{
    //    OnMessageReceived = context =>
    //    {
    //        // Get token from the 'Authorization' header, removing the 'Bearer ' prefix
    //        var accessToken = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(' ').Last();

    //        // Set the token retrieved from the header
    //        context.Token = accessToken;

    //        return Task.CompletedTask;
    //    }
    //};
});


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("UserRolePolicy", policy =>
        policy.RequireRole("User"));

    options.AddPolicy("AdminRolePolicy", policy =>
        policy.RequireRole("Admin"));
});


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
        var allowedGroups = new[] { "Users", "UserDocuments", "Admins", "UserAssets", "UserAssetTypes", "UserDebts", "UserFinancialChanges", "UserAnswers", "UserAnswerOptions", "UserQuestions", "UserTestScores", "HaghighiUserProfiles", "HaghighiUserRelationships", "HaghighiUserFinancialProfiles", "HaghighiUserEmploymentHistories", "HaghighiUserEducationStatuses", "HoghooghiUsersAssets", "HoghooghiUserBoardOfDirectors", "HoghooghiUserCompaniesWithMajorInvestors", "HoghooghiUsers", "HoghooghiUserInvestmentDepartmentStaff", "UserDeposits", "UserFuturePlans", "UserInvestments", "UserInvestmentExperiences", "UserMoreInformations", "UserStates", "UserTypes", "UserWithdrawals"}; 
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

app.UseRouting();

// Enable CORS globally
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
