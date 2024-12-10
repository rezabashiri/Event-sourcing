FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["API/Bootstrapper.csproj", "API/"]
COPY ["Modules/EmployeeManagement/Modules.EmployeeManagement/Modules.EmployeeManagement.csproj", "Modules/EmployeeManagement/Modules.EmployeeManagement/"]
COPY ["Modules/EmployeeManagement/Modules.EmployeeManagement.Core/Modules.EmployeeManagement.Core.csproj", "Modules/EmployeeManagement/Modules.EmployeeManagement.Core/"]
COPY ["Shared/Shared.Core/Shared.Core.csproj", "Shared/Shared.Core/"]
COPY ["Shared/Shared.DTOs/Shared.DTOs.csproj", "Shared/Shared.DTOs/"]
COPY ["Modules/EmployeeManagement/Modules.EmployeeManagement.Infrastructure/Modules.EmployeeManagement.Infrastructure.csproj", "Modules/EmployeeManagement/Modules.EmployeeManagement.Infrastructure/"]
COPY ["Shared/Shared.Infrastructure/Shared.Infrastructure.csproj", "Shared/Shared.Infrastructure/"]
COPY ["Modules/Accounting/Modules.Accounting.Domain/Modules.Accounting.Domain/Modules.Accounting.Domain.csproj", "Modules/Accounting/Modules.Accounting.Domain/Modules.Accounting.Domain/"]
COPY ["Modules/Accounting/Modules.Accounting.Application/Modules.Accounting.Application/Modules.Accounting.Application.csproj", "Modules/Accounting/Modules.Accounting.Application/Modules.Accounting.Application/"]
COPY ["Modules/Accounting/Modules.Accounting.Infrastructure/Modules.Accounting.Infrastructure/Modules.Accounting.Infrastructure.csproj", "Modules/Accounting/Modules.Accounting.Infrastructure/Modules.Accounting.Infrastructure/"]
COPY ["Modules/Accounting/Modules.Accounting.Api/Modules.Accounting.Api/Modules.Accounting.Api.csproj", "Modules/Accounting/Modules.Accounting.Api/Modules.Accounting.Api/"]
RUN dotnet restore "API/Bootstrapper.csproj"
COPY . .
WORKDIR "/src/API"
RUN dotnet build "Bootstrapper.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Bootstrapper.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Bootstrapper.dll"]