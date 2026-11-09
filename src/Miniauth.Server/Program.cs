using Miniauth.Server.Authentication;
using Miniauth.Server.Pages;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDataProtection();
builder.Services.AddAntiforgery();
builder.Services.AddRazorComponents();
builder.Services.AddAuthentication(builder.Environment);

var app = builder.Build();
app.MapStaticAssets();
app.UseRouting();
app.UseAntiforgery();
app.UseAuthentication();
app.MapRazorComponents<App>();

app.Run();
