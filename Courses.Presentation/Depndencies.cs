using Courses.Business.Authentication;
using Courses.Business.Entities;
using Courses.DataAccess.Presistence;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Courses.DataAccess.Services;
using Courses.Business;
using Microsoft.AspNetCore.Identity;
using Courses.Business.Authentication.Filters;

namespace Courses.Presentation;

public static class Depndencies
{
    public static IServiceCollection AddDependency(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddOpenApi();

        services.InjectServices();

        services.AddDBConfig(configuration);

        services.AddAuthConfig(configuration);

        services.AddMappingConfig();

        services.AddValidationConfig();

        return services;
    }
    public static IServiceCollection InjectServices(this IServiceCollection services)
    {
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<ICourseService, CourseService>();
        services.AddScoped<IEnrollmentService, EnrollmentService>();
        services.AddScoped<ILessonService, LessonService>();
        services.AddScoped<IModuleService, ModuleService>();
        services.AddScoped<IFileService, FileSerivce>();
        services.AddScoped<IExamService, ExamService>();

        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IUserService, UserService>();




        return services;
    }
    public static IServiceCollection AddDBConfig(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection") 
            ?? throw new Exception("Invalid Connection String");
        
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });

        return services;
    }
    public static IServiceCollection AddAuthConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentity<ApplicationUser, ApplicationRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
     
        
        services.AddTransient<IAuthorizationHandler, PermissionAuthorizationHandler>();
        services.AddTransient<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();


        services.AddScoped<IAuthService, AuthService>();

        services.AddOptions<JwtOptions>()
            .BindConfiguration(JwtOptions.SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.Configure<IdentityOptions>(options =>
        {
            options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789._!@#$";
            options.User.RequireUniqueEmail = true;
            options.Password.RequiredLength = 8;
            options.SignIn.RequireConfirmedEmail = true;
        });

        var jwtSettings = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(op =>
        {
            op.SaveToken = true;
            op.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ValidIssuer = jwtSettings!.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings!.Key))
            };
        });

        services.AddSingleton<IJwtProvider, JwtProvider>();

        return services;
    }
    public static IServiceCollection AddMappingConfig(this IServiceCollection services)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(typeof(AssemblyReference).Assembly);
        services.AddSingleton<IMapper>(new Mapper(config));

        return services;
    }
    public static IServiceCollection AddValidationConfig(this IServiceCollection services)
    {
        services
            .AddFluentValidationAutoValidation()
            .AddValidatorsFromAssemblyContaining<AssemblyReference>();

        return services;
    }
}
