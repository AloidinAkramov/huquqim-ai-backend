FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# csproj fayllarni avval ko'chirish (Docker layer cache uchun)
COPY src/Huquqim.Domain/Huquqim.Domain.csproj src/Huquqim.Domain/
COPY src/Huquqim.Application/Huquqim.Application.csproj src/Huquqim.Application/
COPY src/Huquqim.Infrastructure/Huquqim.Infrastructure.csproj src/Huquqim.Infrastructure/
COPY src/Huquqim.Api/Huquqim.Api.csproj src/Huquqim.Api/
COPY global.json .
RUN dotnet restore src/Huquqim.Api/Huquqim.Api.csproj

# Qolgan manbani ko'chirish va publish
COPY src/ src/
RUN dotnet publish src/Huquqim.Api/Huquqim.Api.csproj \
    -c Release \
    -o /app/publish \
    --no-restore \
    /p:UseAppHost=false

# ----------------- Runtime image -----------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://0.0.0.0:8080
ENV ASPNETCORE_ENVIRONMENT=Production
EXPOSE 8080

ENTRYPOINT ["dotnet", "Huquqim.Api.dll"]
