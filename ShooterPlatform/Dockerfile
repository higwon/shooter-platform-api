FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# csproj 먼저 복사 (캐시 최적화)
COPY ./ShooterPlatform.Api/ShooterPlatform.Api.csproj ./ShooterPlatform.Api/
RUN dotnet restore ./ShooterPlatform.Api/ShooterPlatform.Api.csproj

# 전체 소스 복사
COPY . .

# publish
WORKDIR /src/ShooterPlatform.Api
RUN dotnet publish -c Release -o /app/publish


# =========================
# 2. Runtime Stage
# =========================
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

# 포트 설정 (ASP.NET Core 기본)
EXPOSE 8080

# 실행
ENTRYPOINT ["dotnet", "ShooterPlatform.Api.dll"]