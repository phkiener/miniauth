using Miniauth.Server;
using Miniauth.Server.Authentication;
using Miniauth.Server.Handlers;
using Miniauth.Server.Pages;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAntiforgery();
builder.Services.AddRazorComponents();
builder.Services.AddAuthentication(builder.Environment);
builder.Services.AddOptions<MiniauthOptions>()
    .BindConfiguration(string.Empty)
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Logging.AddSystemdConsole(static opt => opt.UseUtcTimestamp = true);
builder.Logging.AddFilter(static level => level >= LogLevel.Warning);
builder.Logging.AddFilter("Microsoft.Hosting.Lifetime", static level => level >= LogLevel.Information);
builder.Logging.AddFilter("Miniauth", static level => level >= LogLevel.Information);

var app = builder.Build();
app.MapStaticAssets();
app.UseRouting();
app.UseAntiforgery();
app.UseAuthentication();

app.MapGet("/challenge", Challenge.Handle);
app.MapRazorComponents<App>();

app.Run();
