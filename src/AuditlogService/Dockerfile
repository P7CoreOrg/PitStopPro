FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["src/AuditlogService/AuditlogService.csproj", "src/AuditlogService/"]
RUN dotnet restore "src/AuditlogService/AuditlogService.csproj"
COPY . .
WORKDIR "/src/src/AuditlogService"
RUN dotnet build "AuditlogService.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "AuditlogService.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "AuditlogService.dll"]