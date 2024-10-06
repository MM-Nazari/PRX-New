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
using Microsoft.Extensions.Caching.Memory;
using DotNet.RateLimiter;
using PRX;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.SqlClient;
using OfficeOpenXml;
using System.Net;
using Microsoft.EntityFrameworkCore;
using static Microsoft.IO.RecyclableMemoryStreamManager;


var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    WebRootPath = "wwwroot"
});


builder.Services.AddRateLimitService(builder.Configuration);
ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

builder.Services.AddHttpClient();

builder.Services.AddControllers()
     .AddNewtonsoftJson(options =>
     {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;  // Ignore self-referencing loops
     })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.WriteIndented = true; // Optional: for more readable JSON output
        
    });


builder.Services.AddMemoryCache();

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

    // local connection
    options.UseSqlServer("Server=Pirhayati\\MSSQLSERVER01;Database=PRX_V2;Integrated Security=True;TrustServerCertificate=True;"));
    //options.UseSqlServer("Server=host.docker.internal;Database=PRX_V2;Integrated Security=True;TrustServerCertificate=True;"));
    //options.UseSqlServer("Server=172.21.18.73\\MSSQLSERVER01;Database=PRX_V2;User Id=MMNazari;Password=123;TrustServerCertificate=True;"));
    //options.UseSqlServer("Server=host.docker.internal,1433;Database=PRX_V2;User Id=MMNazari;Password=123;TrustServerCertificate=True;"));

    // windows server connection
    //options.UseSqlServer("Server=192.168.103.253, 1433\\WIN-04F3FJE618C;Database=PRX_V2;User Id=prx-server;Password=123;TrustServerCertificate=True;"));

    // nib dev database connection
    //options.UseSqlServer("Server=db.nibdev.com;Database=PRX_V2;User Id=prx-server;Password=123;TrustServerCertificate=True;"));
//options.UseSqlServer("Server=192.168.103.253, 1433\\WIN-04F3FJE618C;Database=PRX_V2;Integrated Security=True;TrustServerCertificate=True;"));

//builder.Services.AddDbContext<PRX.Data.PRXDbContext>(options =>
//{
//    options.UseSqlServer("Server=192.168.103.253, 1433\\WIN-04F3FJE618C;Database=PRX_V2;User Id=prx-server;Password=123;TrustServerCertificate=True;",
//        sqlServerOptions => sqlServerOptions.EnableRetryOnFailure(
//            maxRetryCount: 5,           // Maximum number of retry attempts
//            maxRetryDelay: TimeSpan.FromSeconds(10),  // Maximum delay between retries
//            errorNumbersToAdd: null));  // Optional: additional SQL error numbers to treat as transient
//});




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
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero // Reduce the clock skew
    };

    o.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var endpoint = context.HttpContext.GetEndpoint();
            var authorizeAttribute = endpoint?.Metadata?.GetMetadata<AuthorizeAttribute>();

            // Only check for tokens if the endpoint requires authorization
            if (authorizeAttribute != null)
            {
                var accessToken = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(' ').Last();
                context.Token = accessToken;

                if (accessToken != null)
                {
                    var memoryCache = context.HttpContext.RequestServices.GetRequiredService<IMemoryCache>();
                    if (memoryCache.TryGetValue(accessToken, out _))
                    {
                        context.Fail("This token is blacklisted.");
                    }
                }
            }
            return Task.CompletedTask;
        },
        OnAuthenticationFailed = context =>
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            var result = JsonConvert.SerializeObject(new { message = "Authentication failed.", detail = context.Exception?.Message });
            return context.Response.WriteAsync(result);
        },
        OnForbidden = context =>
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            var result = JsonConvert.SerializeObject(new { message = "Forbidden." });
            return context.Response.WriteAsync(result);
        }
    };
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
        var allowedGroups = new[] { "OTP", "NationalCodeCheck", "Reports", "Requests", "Messages", "Tickets", "Users", "Admins", "HaghighiUserProfiles", "HaghighiUserRelationships", "HaghighiUserFinancialProfiles", "UserAnswers", "UserAnswerOptions", "UserQuestions", "UserTestScores", "HaghighiUserBankInfo" , "UserDocuments", "UserAssets", "UserAssetTypes", "UserDebts", "UserFinancialChanges", "HaghighiUserEmploymentHistories", "HaghighiUserEducationStatuses", "HoghooghiUsersAssets", "HoghooghiUserBoardOfDirectors", "HoghooghiUserCompaniesWithMajorInvestors", "HoghooghiUsers", "HoghooghiUserInvestmentDepartmentStaff", "UserDeposits", "UserFuturePlans", "UserInvestments", "UserInvestmentExperiences", "UserMoreInformations", "UserStates", "UserTypes", "UserWithdrawals"}; 
        return allowedGroups.Contains(apiDesc.GroupName);


    });
});

var app = builder.Build();

app.UseExceptionHandlingMiddleware();

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
app.MapGet("/", () => "Server is up and running!");

app.Run();
