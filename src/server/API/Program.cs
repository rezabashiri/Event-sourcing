using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Modules.Accounting.Api.Extensions;
using Modules.EmployeeManagement.Extensions;
using Shared.Core.Extensions;
using Shared.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddDistributedMemoryCache()
    .AddSerialization(builder.Configuration)
    .AddSharedInfrastructure(builder.Configuration)
    .AddSharedApplication(builder.Configuration)
    .AddEmployeeManagementModule(builder.Configuration)
    .AddAccountingModule(builder.Configuration);

var app = builder.Build();

app.UseSharedInfrastructure();

app.Run();