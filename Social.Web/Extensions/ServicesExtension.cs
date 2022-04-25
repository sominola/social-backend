using System.Reflection;
using System.Text;
using System.Text.Json;
using Social.Domain.Dto.Users;
using Social.Domain.Mapping;
using Social.Domain.Services;
using Social.Domain.Services.Interfaces;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Social.Data.Context;
using Social.Data.Repositories;
using Social.Data.Repositories.Interfaces;
using Social.Web.Filters;

namespace Social.Web.Extensions;

public static class ServicesExtension
{
    private static IServiceCollection Services { get; set; }
    private static IConfiguration Configuration { get; set; }

    private static void Init(this WebApplicationBuilder builder)
    {
        Services = builder.Services;
        Configuration = builder.Configuration;
    }
    
    public static void AddServices(this WebApplicationBuilder builder)
    {
        builder.Init();
        Services.AddControllers(options =>
        {
            options.Filters.Add(new ExceptionFilter());
            options.Filters.Add(typeof(ValidationFilter));
        }).AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        });

        Services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });
        Services.AddDbContext();
        Services.AddMapper();
        Services.AddJwtAuthentication();
        Services.AddScoped<IUnitOfWork, UnitOfWork>();
        Services.AddScoped<IUserService, UserService>();
        Services.AddScoped<ITokenService, TokenService>();
        Services.AddScoped<IAuthService, AuthService>();
        Services.AddValidation();
        Services.AddSwaggerGen(cfg =>
        {
            cfg.SwaggerDoc("v1", new OpenApiInfo { Title = "Social.Web", Version = "v1" });

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            cfg.IncludeXmlComments(xmlPath);
            cfg.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey
            });
            cfg.AddSecurityRequirement(new OpenApiSecurityRequirement
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
        Services.AddRouting(options =>
        {
            options.LowercaseUrls = true;
            options.LowercaseQueryStrings = true;
        });
        Services.AddCors();
    }

    private static void AddDbContext(this IServiceCollection services)
    {
        var connectionString = Configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(connectionString));
    }

    private static void AddJwtAuthentication(this IServiceCollection services)
    {
        services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"])),
                };
            });
    }

    private static void AddMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(UserMapper));
    }

    private static void AddValidation(this IServiceCollection services)
    {
        services.AddFluentValidation(cfg =>
        {
            cfg.RegisterValidatorsFromAssemblyContaining<LoginDto>();
            cfg.LocalizationEnabled = false;
        });
    }
}