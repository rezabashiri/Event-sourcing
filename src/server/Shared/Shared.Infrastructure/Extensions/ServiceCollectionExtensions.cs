using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using FluentValidation;
using FluentValidation.AspNetCore;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Shared.Core.EventLogging;
using Shared.Core.Exceptions;
using Shared.Core.Extensions;
using Shared.Core.IntegrationServices.Application;
using Shared.Core.Interfaces;
using Shared.Core.Interfaces.Services;
using Shared.Core.Settings;
using Shared.Infrastructure.Controllers;
using Shared.Infrastructure.EventLogging;
using Shared.Infrastructure.Interceptors;
using Shared.Infrastructure.Middlewares;
using Shared.Infrastructure.Persistence;
using Shared.Infrastructure.Services;
using Shared.Infrastructure.Swagger.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;

[assembly: InternalsVisibleTo("Bootstrapper")]
[assembly: InternalsVisibleTo("Shared.Test")]

namespace Shared.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    internal static IServiceCollection AddSharedInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddPersistenceSettings(config);
        services
            .AddDatabaseContext<ApplicationDbContext>(config)
            .AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());

        services.AddScoped<IEventLogger, EventLogger>();
        services.AddApiVersioning(o =>
        {
            o.AssumeDefaultVersionWhenUnspecified = true;
            o.DefaultApiVersion = new ApiVersion(1, 0);
        });
        services.AddControllers()
            .ConfigureApplicationPartManager(manager =>
            {
                manager.FeatureProviders.Add(new InternalControllerFeatureProvider());
            })
            .AddMvcOptions(options =>
            {
                options.ModelBindingMessageProvider.SetAttemptedValueIsInvalidAccessor((value, propertyName) =>
                    throw new CustomException($"{propertyName}: value '{value}' is invalid.", statusCode: HttpStatusCode.BadRequest));
            });
        services.AddTransient<IValidatorInterceptor, ValidatorInterceptor>();
        services.AddApplicationLayer(config);
        services.AddLocalization(options =>
        {
            options.ResourcesPath = "Resources";
        });
        services.AddLogging(options => options.AddConsole());
        services.AddRouting(options => options.LowercaseUrls = true);
        services.AddHangfireServer();
        services.AddSingleton<GlobalExceptionHandler>();
        services.AddSwaggerDocumentation();
        services.AddCorsPolicy(config);
        services.AddApplicationSettings(config);
        return services;
    }

    private static IServiceCollection AddApplicationLayer(this IServiceCollection services, IConfiguration config)
    {
        services.AddScoped<IJobService, HangfireService>();
        services.AddTransient<IEventLogService, EventLogService>();
        services.AddTransient<IEntityReferenceService, EntityReferenceService>();
        return services;
    }

    private static IServiceCollection AddPersistenceSettings(this IServiceCollection services, IConfiguration config) => services
        .Configure<PersistenceSettings>(config.GetSection(nameof(PersistenceSettings)));

    private static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
    {
        return services.AddSwaggerGen(options =>
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (!assembly.IsDynamic)
                {
                    string xmlFile = $"{assembly.GetName().Name}.xml";
                    string xmlPath = Path.Combine(baseDirectory, xmlFile);
                    if (File.Exists(xmlPath))
                    {
                        options.IncludeXmlComments(xmlPath);
                    }
                }
            }

            options.AddSwaggerDocs();

            options.OperationFilter<RemoveVersionFromParameterFilter>();
            options.DocumentFilter<ReplaceVersionWithExactValueInPathFilter>();
            options.OperationFilter<SwaggerExcludeFilter>();
            options.DocInclusionPredicate((version, desc) =>
            {
                if (!desc.TryGetMethodInfo(out var methodInfo))
                {
                    return false;
                }

                var versions = methodInfo
                    .DeclaringType?
                    .GetCustomAttributes(true)
                    .OfType<ApiVersionAttribute>()
                    .SelectMany(attr => attr.Versions);

                var maps = methodInfo
                    .GetCustomAttributes(true)
                    .OfType<MapToApiVersionAttribute>()
                    .SelectMany(attr => attr.Versions)
                    .ToList();

                return versions?.Any(v => $"v{v}" == version) == true
                       && (!maps.Any() || maps.Any(v => $"v{v}" == version));
            });

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                Description = "Input your Bearer token in this format - Bearer {your token here} to access this API"
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
                        },
                        Scheme = "Bearer",
                        Name = "Bearer",
                        In = ParameterLocation.Header
                    },
                    new List<string>()
                }
            });
            options.MapType<TimeSpan>(() => new OpenApiSchema
            {
                Type = "string",
                Nullable = true,
                Pattern = @"^([0-9]{1}|(?:0[0-9]|1[0-9]|2[0-3])+):([0-5]?[0-9])(?::([0-5]?[0-9])(?:.(\d{1,9}))?)?$",
                Example = new OpenApiString("02:00:00")
            });
        });
    }

    private static void AddSwaggerDocs(this SwaggerGenOptions options)
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "API v1",
            License = new OpenApiLicense
            {
                Name = "MIT License",
                Url = new Uri("https://opensource.org/licenses/MIT")
            }
        });
    }

    private static IServiceCollection AddApplicationSettings(this IServiceCollection services, IConfiguration configuration) => services
        .Configure<ApplicationSettings>(configuration.GetSection(nameof(ApplicationSettings)));

    private static IServiceCollection AddCorsPolicy(this IServiceCollection services, IConfiguration configuration)
    {
        var corsSettings = services.GetOptions<CorsSettings>(nameof(CorsSettings), configuration);
        return services.AddCors(opt =>
        {
            opt.AddPolicy("CorsPolicy", policy =>
            {
                policy.AllowAnyHeader().AllowAnyMethod().WithOrigins(corsSettings.Url);
            });
        });
    }
}