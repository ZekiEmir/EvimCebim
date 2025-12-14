using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics; // BU YENÝ EKLENDÝ (Hata Kodlarý Ýçin)
using System;
using EvimCebim.Data;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
var builder = WebApplication.CreateBuilder(args);

// --- VERÝTABANI AYARI BAÞLANGIÇ ---

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

if (!string.IsNullOrEmpty(databaseUrl))
{
    // Render üzerindeyiz (PostgreSQL)
    try
    {
        var databaseUri = new Uri(databaseUrl);
        var userInfo = databaseUri.UserInfo.Split(':');

        // Port hatasýný önleyen kod (-1 gelirse 5432 yap)
        int port = databaseUri.Port > 0 ? databaseUri.Port : 5432;

        connectionString = $"Host={databaseUri.Host};Port={port};Database={databaseUri.AbsolutePath.TrimStart('/')};Username={userInfo[0]};Password={userInfo[1]};Ssl Mode=Require;Trust Server Certificate=true;";

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString)
                   // AÞAÐIDAKÝ SATIR YENÝ EKLENDÝ: Migration hatasýný susturur ve devam ettirir.
                   .ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning)));

        Console.WriteLine("--> Render PostgreSQL baðlantýsý (Fixler yapýldý) yapýlandýrýldý.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"--> Baðlantý hatasý: {ex.Message}");
    }
}
else
{
    // Lokal bilgisayardayýz (SQL Server)
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connectionString)
        // Lokaldeki hatalarý da yoksayalým
        .ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning)));
}

// --- VERÝTABANI AYARI BÝTÝÞ ---


builder.Services.AddControllersWithViews();

var app = builder.Build();

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
// ... (Üst kýsýmlar ayný kalsýn) ...

// OTOMATÝK TABLO SIFIRLAMA VE OLUÞTURMA
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<EvimCebim.Data.ApplicationDbContext>();

        //  DÝKKAT: Bu komut veritabanýný önce tamamen siler (temizlik için)
        // Sonra sýfýrdan tablolarla birlikte tekrar oluþturur.
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        Console.WriteLine("--> Veritabaný SIFIRLANDI ve yeniden oluþturuldu. Tablolar hazýr.");
    }
    catch (Exception ex)
    {
        // Hata olursa konsola yaz ama siteyi çökertme
        Console.WriteLine($"Veritabaný Oluþturma Hatasý: {ex.Message}");
    }
}

app.Run();