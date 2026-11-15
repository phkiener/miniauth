using System.Reflection;
using Microsoft.AspNetCore.Components;

namespace MiniAuth.Host.Pages;

public sealed partial class App : ComponentBase
{
    private static readonly Assembly AppAssembly = typeof(App).Assembly;
}
