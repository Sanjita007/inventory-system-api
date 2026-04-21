using inventory_system_api;
using inventory_system_api.Middleware;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Data;
using System.Text;
using inventory_system_api.Application.Validator;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        // Normalize validation (FluentValidation / model binding) responses into a single shape
        options.InvalidModelStateResponseFactory = context =>
        {
            var errors = context.ModelState
                .Where(kvp => kvp.Value?.Errors?.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray()
                );

            var errorResponse = new inventory_system_api.Application.Models.ErrorResponse
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "Validation failed.", 
                Errors = errors,
                TraceId = context.HttpContext.TraceIdentifier
            };

            return new ObjectResult(errorResponse)
            {
                StatusCode = StatusCodes.Status400BadRequest
            };
        };
    });

builder.Services.AddEndpointsApiExplorer();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
                "http://localhost:3000",   // React CRA
                "http://localhost:5174",    // Vite dev server
                "http://localhost:5173",  // Vite dev server
                "https://inventory-app-san.netlify.app"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

// add repositories
builder.Services.AddRepositories();

#region API versioning
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    options.ReportApiVersions = true;
    options.RegisterMiddleware = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new QueryStringApiVersionReader("api-version"),
        new HeaderApiVersionReader("x-api-version"),
        new UrlSegmentApiVersionReader());
});

builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV"; // e.g. v1, v2
    options.SubstituteApiVersionInUrl = true;
});
#endregion

#region Swagger
builder.Services.AddSwaggerGen(options =>
{
    // Security definition only
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter JWT token (Bearer {token})",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            Array.Empty<string>()
        }
    });
});

// Register configuration hook for Swagger per version
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
#endregion

#region JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
            RoleClaimType = "Roles"
        };
    });
#endregion

builder.Services.AddProblemDetails();

builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddTransient<IDbConnection>(sp =>
{
    var connectionString = sp.GetRequiredService<IConfiguration>().GetConnectionString("DefaultConnectionString");
    return new SqlConnection(connectionString);
});

// FluentValidation registrations MUST be done before builder.Build()
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<ProductValidator>();

// Remove or replace any AddExceptionHandler registrations that conflict with the custom middleware.
// builder.Services.AddExceptionHandler<GlobalExceptionHandler>(); // (removed)

var app = builder.Build();

// Global exception middleware added EARLY in the pipeline so all errors are captured
app.UseMiddleware<ExceptionMiddleware>();

app.UseRouting();

// CORS must come BEFORE Authentication/Authorization
app.UseCors("AllowFrontend");

// version provider
var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

// Swagger
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint(
                $"/swagger/{description.GroupName}/swagger.json",
                $"inventory-system API {description.ApiVersion}"
            );
        }
});

app.Use((context, next) =>
{
    context.Request.EnableBuffering();
    return next();
});

// Remove the built-in exception handler middleware call if you prefer the custom middleware
// app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();


// 🔹 This class wires up SwaggerDocs for each version
public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;

    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
    {
        _provider = provider;
    }

    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, new OpenApiInfo
            {
                Title = $"inventory-system API {description.ApiVersion}",
                Version = description.ApiVersion.ToString()
            });
        }
    }
}
public partial class Program { }