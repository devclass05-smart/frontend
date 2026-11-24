using DevOpsTasks.Frontend;
using DevOpsTasks.Frontend.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Local: http://localhost:5200/  |  Azure Nginx proxy: /
// Set ApiBaseUrl in wwwroot/appsettings.json before publishing or after copying.
var configuredBase = builder.Configuration["ApiBaseUrl"] ?? "/";
var apiBaseUri = new Uri(new Uri(builder.HostEnvironment.BaseAddress), configuredBase);
builder.Services.AddScoped(_ => new HttpClient { BaseAddress = apiBaseUri });
builder.Services.AddScoped<TaskApiClient>();

await builder.Build().RunAsync();
