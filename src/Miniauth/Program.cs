using Microsoft.AspNetCore.DataProtection;
using Miniauth.Authentication;
using Miniauth.Endpoints;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDataProtection();
builder.Services.AddTransient<IAuthenticationProvider, PamAuthenticationProvider>();
builder.Services.AddTransient<IDataProtector>(
    static sp => sp.GetRequiredService<IDataProtectionProvider>().CreateProtector("default"));

var app = builder.Build();

app.MapGet("/", Inspect.HandleAsync);
app.MapGet("/login", Login.HandleAsync);
app.MapPost("/login", Login.HandlePostAsync);
app.MapGet("/logout", Logout.HandleAsync);
app.MapGet("/challenge", Challenge.HandleAsync);

app.Run();
