#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["WebApiExample/WebApiExample.csproj", "WebApiExample/"]
COPY ["BackEndInfrsastructure/BackEndInfrastructure.csproj", "BackEndInfrsastructure/"]
COPY ["Migrations.Oracle/Migrations.Oracle.csproj", "Migrations.Oracle/"]
COPY ["WebApiExample.DB/WebApiExample.DB.csproj", "WebApiExample.DB/"]
COPY ["Migrations.SQL/Migrations.SQL.csproj", "Migrations.SQL/"]
RUN dotnet restore "WebApiExample/WebApiExample.csproj"
COPY . .
WORKDIR "/src/WebApiExample"
RUN dotnet build "WebApiExample.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebApiExample.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApiExample.dll"]