using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;
using EBidderWeb.Client;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ContextMenuService>();
builder.Services.AddTransient(sp => new HttpClient{BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)});
builder.Services.AddScoped<EBidderWeb.Client.EBidderService>();
builder.Services.AddAuthorizationCore();
builder.Services.AddHttpClient("EBidderWeb.Server", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
builder.Services.AddTransient(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("EBidderWeb.Server"));
builder.Services.AddScoped<EBidderWeb.Client.SecurityService>();
builder.Services.AddScoped<AuthenticationStateProvider, EBidderWeb.Client.ApplicationAuthenticationStateProvider>();
var host = builder.Build();
await host.RunAsync();