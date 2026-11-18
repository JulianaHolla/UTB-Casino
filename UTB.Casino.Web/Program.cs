using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Serilog;
using Serilog.Events;
using UTB.Casino.Application;
using UTB.Casino.Infrastructure;
using UTB.Casino.Web.Validations.Adapters.Providers;

// Decimals always use "." so <input type="number"> works on any OS locale.
CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;
CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((_, cfg) => cfg
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
    .WriteTo.Console()
    .WriteTo.File("logs/casino-.log", rollingInterval: RollingInterval.Day));

builder.Services.AddControllersWithViews(o => o.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()));

//client side validation of our own validation attributes
builder.Services.AddSingleton<ValidationAttributeAdapterProvider>();
builder.Services.AddSingleton<IValidationAttributeAdapterProvider, ClientValidationAttributeAdapterProvider>();

//Infrastructure and Application do not know each other - only the web project connects them
builder.Services.AddInfrastructure(builder.Configuration, builder.Environment.WebRootPath);
builder.Services.AddApplication();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// The database is NOT created or migrated here. Apply migrations separately (see README):
//   dotnet ef database update -p UTB.Casino.Infrastructure -s UTB.Casino.Web

app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(name: "areas", pattern: "{area:exists}/{controller=Game}/{action=Select}/{id?}");
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
