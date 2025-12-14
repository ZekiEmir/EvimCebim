using Microsoft.EntityFrameworkCore;
using System;
using EvimCebim.Data; // Hata almamak için gerekli namespace

var builder = WebApplication.CreateBuilder(args);

// --- VERÝTABANI AYARI BAÞLANGIÇ ---

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Render'dan gelen DATABASE_URL var mý diye bakýyoruz
var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

if (!string.IsNullOrEmpty(databaseUrl))
{
    // Render üzerindeyiz, PostgreSQL kullanacaðýz
    try
    {
        var databaseUri = new Uri(databaseUrl);
        var userInfo = databaseUri.UserInfo.Split(':');

        // --- KRÝTÝK DÜZELTME BURADA ---
        // Eðer URL'den port okunamazsa (-1 gelirse), varsayýlan PostgreSQL portunu (5432) kullan.
        int port = databaseUri.Port > 0 ? databaseUri.Port : 5432;

        // Connection String'i oluþtururken artýk güvenli 'port' deðiþkenini kullanýyoruz
        connectionString = $"Host={databaseUri.Host};Port={port};Database={databaseUri.AbsolutePath.TrimStart('/')};Username={userInfo[0]};Password={userInfo[1]};Ssl Mode=Require;Trust Server Certificate=true;";

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString));

        Console.WriteLine("--> Render PostgreSQL baðlantýsý (Port Düzeltmeli) yapýlandýrýldý.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"--> Baðlantý hatasý: {ex.Message}");
    }
}
else
{
    // Lokal bilgisayardayýz, eski SQL Server devam
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connectionString));
}

// --- VERÝTABANI AYARI BÝTÝÞ ---


// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


// OTOMATÝK TABLO OLUÞTURMA (MIGRATION)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate(); // Veritabaný yoksa oluþturur.
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Migration Hatasý: {ex.Message}");
    }
}

app.Run();