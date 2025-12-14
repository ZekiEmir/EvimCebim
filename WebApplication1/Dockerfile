# 1. Aşama: İnşaat (Build)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Dosyaları kopyala
COPY . .

# Projeyi derle
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

# 2. Aşama: Çalıştırma (Run)
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

# Port ayarı (Render için gerekli)
ENV ASPNETCORE_URLS=http://+:8080

# Başlatma komutu
ENTRYPOINT ["dotnet", "EvimCebim.dll"]