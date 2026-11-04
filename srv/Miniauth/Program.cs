using Microsoft.AspNetCore.DataProtection;
using Miniauth.Endpoints;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorComponents();
builder.Services.AddDataProtection();
builder.Services.AddTransient<IDataProtector>(
    static sp => sp.GetRequiredService<IDataProtectionProvider>().CreateProtector("default"));

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAntiforgery();
app.MapStaticAssets();

app.MapGet("/challenge", Challenge.HandleAsync);
app.MapGet("/login", Login.HandleAsync);
app.MapPost("/login", Login.HandlePostAsync);

if (app.Environment.IsDevelopment())
{
    app.MapGet("/inspect", Inspect.HandleAsync);
}

app.Run();
