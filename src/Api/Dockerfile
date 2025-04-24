# 📦 Base image with .NET 9.0
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

# 📑 Copy project files
COPY ["Directory.Packages.props", "."]
COPY ["OPS.Api/OPS.Api.csproj", "OPS.Api/"]
COPY ["OPS.Infrastructure/OPS.Infrastructure.csproj", "OPS.Infrastructure/"]
COPY ["OPS.Application/OPS.Application.csproj", "OPS.Application/"]
COPY ["OPS.Persistence/OPS.Persistence.csproj", "OPS.Persistence/"]
COPY ["OPS.Domain/OPS.Domain.csproj", "OPS.Domain/"]

# 🔧 Restore
RUN dotnet restore "./OPS.Api/OPS.Api.csproj"

# 📥 Copy everything and build
COPY . .
WORKDIR "/OPS.Api"
RUN dotnet build "OPS.Api.csproj" -c Release -o /app/build

# 🚀 Publish
FROM build AS publish
RUN dotnet publish "OPS.Api.csproj" -c Release -o /app/publish

# 🎯 Final stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

# 🔐 Create a non-root user and group
RUN addgroup --system --gid 1001 opsapi
RUN adduser --system --uid 1001 opsuser

# 🔒 Switch to the non-root user
USER opsuser

# 🌍 Set & expose ports
ENV ASPNETCORE_HTTP_PORTS=9998
EXPOSE 9998

# 📋 Copy published output
COPY --from=publish /app/publish .

# ▶️ Start the app
ENTRYPOINT ["dotnet", "OPS.Api.dll"]