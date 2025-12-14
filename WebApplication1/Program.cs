using Microsoft.EntityFrameworkCore;
using System;
// Hata vermemesi için gerekli olan satýrý ekledim:
using EvimCebim.Data;

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

        // Render'ýn verdiði URL'yi C#'ýn anlayacaðý formata çeviriyoruz
        connectionString = $"Host={databaseUri.Host};Port={databaseUri.Port};Database={databaseUri.AbsolutePath.TrimStart('/')};Username={userInfo[0]};Password={userInfo[1]};Ssl Mode=Require;Trust Server Certificate=true;";

        // DÜZELTME: Context adýnýn tam yolunu yazdýk (EvimCebim.Data.ApplicationDbContext)
        builder.Services.AddDbContext<EvimCebim.Data.ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString));

        Console.WriteLine("--> Render PostgreSQL baðlantýsý yapýlandýrýldý.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"--> Baðlantý hatasý: {ex.Message}");
    }
}
else
{
    // Lokal bilgisayardayýz, eski SQL Server devam
    // DÜZELTME: Context adýnýn tam yolunu yazdýk
    builder.Services.AddDbContext<EvimCebim.Data.ApplicationDbContext>(options =>
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
        var context = services.GetRequiredService<EvimCebim.Data.ApplicationDbContext>();
        context.Database.Migrate(); // Veritabaný yoksa oluþturur, tablolarý ekler.
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Migration Hatasý: {ex.Message}");
    }
}

app.Run();