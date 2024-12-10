using System.Runtime.CompilerServices;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Shared.Core.Interfaces.Services;
using Shared.Infrastructure.Middlewares;
using Swashbuckle.AspNetCore.SwaggerUI;

[assembly: InternalsVisibleTo("Bootstrapper")]

namespace Shared.Infrastructure.Extensions;

internal static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseSharedInfrastructure(this IApplicationBuilder app)
    {
        app.UseHttpsRedirection();
        app.UseMiddleware<GlobalExceptionHandler>();
        app.UseRouting();

        app.UseCors("CorsPolicy");
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseHangfireDashboard("/jobs", new DashboardOptions
        {
            DashboardTitle = "Current Jobs"
        });
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        app.UseSwaggerDocumentation();
        app.Initialize();

        return app;
    }

    internal static IApplicationBuilder Initialize(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();

        var initializers = serviceScope.ServiceProvider.GetServices<IDatabaseSeeder>();

        foreach (var initializer in initializers) initializer.Initialize();

        return app;
    }

    private static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.DefaultModelsExpandDepth(-1);
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            options.SwaggerEndpoint("/swagger/v2/swagger.json", "v2");
            options.RoutePrefix = "swagger";
            options.DisplayRequestDuration();
            options.DocExpansion(DocExpansion.None);
        });
        return app;
    }
}