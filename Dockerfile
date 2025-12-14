# 1. Aşama: .NET 9 SDK (GÜNCELLENDİ)
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Dosyaları kopyala
COPY . .

# Projeyi derle
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

# 2. Aşama: .NET 9 Runtime (GÜNCELLENDİ)
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .

# Port ayarı
ENV ASPNETCORE_URLS=http://+:8080

# Başlatma komutu
ENTRYPOINT ["dotnet", "EvimCebim.dll"]