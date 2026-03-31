using Radzen;
using Weltenretter.Systeminformationen.Host.Components;
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
var app = builder.Build();
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