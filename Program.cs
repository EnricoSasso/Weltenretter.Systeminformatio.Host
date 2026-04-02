using Radzen;
using Weltenretter.Systeminformationen.Data;
using Weltenretter.Systeminformationen.Host.Components;
using Weltenretter.Systeminformationen.Host.Services.Systeminformationen;
using Weltenretter.Systeminformationen.Models;
using Weltenretter.Systeminformationen.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddRazorComponents().AddInteractiveServerComponents().AddHubOptions(options => options.MaximumReceiveMessageSize = 10 * 1024 * 1024);
builder.Services.AddControllers();
builder.Services.AddRadzenComponents();
builder.Services.AddHttpClient();
builder.Services.AddDbContext<Weltenretter.Systeminformationen.Host.Data.z_Systeminformationen_HostContext>(options =>
{
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    options.UseSqlServer(builder.Configuration.GetConnectionString("z_Systeminformationen_HostConnection"));
});
builder.Services.AddScoped<Weltenretter.Systeminformationen.Host.z_Systeminformationen_HostService>();

// Modul-Konfiguration binden
builder.Services.Configure<SysteminformationenOptionen>(
    builder.Configuration.GetSection("Systeminformationen"));

// DbContext des Moduls registrieren
builder.Services.AddDbContext<SysteminformationenDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration["Systeminformationen:ConnectionString"],
        sql => sql.MigrationsAssembly("Weltenretter.Systeminformationen")));

// Modul-Services registrieren
builder.Services.AddSysteminformationenServices();
builder.Services.AddScoped<IProjektSchluesselProvider, ProjektSchluesselProvider>();

var app = builder.Build();

// Migrationen des Moduls beim Start anwenden
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SysteminformationenDbContext>();
    db.Database.Migrate();
}

var forwardingOptions = new ForwardedHeadersOptions()
{
    ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedFor | Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedProto
};
forwardingOptions.KnownIPNetworks.Clear();
forwardingOptions.KnownProxies.Clear();
app.UseForwardedHeaders(forwardingOptions);
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found");
app.UseHttpsRedirection();
app.MapControllers();
app.MapStaticAssets();
app.UseAntiforgery();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();
app.Run();
