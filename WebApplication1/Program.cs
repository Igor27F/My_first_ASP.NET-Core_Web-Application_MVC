using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;
using WebApplication1.Data;
using WebApplication1.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<WebApplication1Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("WebApplication1Context") ?? throw new InvalidOperationException("Connection string 'WebApplication1Context' not found.")));

builder.Services.AddScoped<SeedingService>();
builder.Services.AddScoped<SellerService>();
builder.Services.AddScoped<DepartmentService>();
builder.Services.AddScoped<SalesRecordService>();
//builder.Services.AddTransient<SeedingService>();
// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

/*if(args.Length == 1 && args[0].ToLower() == "seeddata") {
    SeedData(app);
}

void SeedData(IHost app) {
    var scopedData = app.Services.GetService<IServiceScopeFactory>();

    using (var scope = scopedData.CreateScope()) {
        var service = scope.ServiceProvider.GetService<SeedingService>();
        service.Seed();
    }
}*/

var enUS = new CultureInfo("en-US");
var localizationOptions = new RequestLocalizationOptions {
    DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture(enUS),
    SupportedCultures = new List<CultureInfo> { enUS },
    SupportedUICultures = new List<CultureInfo> { enUS }
};

app.UseRequestLocalization(localizationOptions);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//SeedingService seedingService = app.Services.GetRequiredService<SeedingService>();
//seedingService.Seed();
//app.Services.GetRequiredService<SeedingService>().Seed();

/*Seed(app);

void Seed(WebApplication app) {
    using var scope = app.Services.CreateScope();
    SeedingService seedingService = scope.ServiceProvider.GetRequiredService<SeedingService>();
    seedingService.Seed();
}*/

app.Services.CreateScope().ServiceProvider.GetRequiredService<SeedingService>().Seed();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
