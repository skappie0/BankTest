using BankTest.Client;
using BankTest.Client.Providers;
using BankTest.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<LoginAPI>();
builder.Services.AddScoped(sp =>
{
    var baseUrl = builder.Configuration.GetValue<string>("ApiUrl");
    var httpClient = new HttpClient { BaseAddress = new Uri(baseUrl) };
    return new LoginAPI(baseUrl, httpClient);
});
builder.Services.AddAuthorizationCore();
builder.Services.AddMudServices();
builder.Services.AddScoped<TokenAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<TokenAuthenticationStateProvider>());

await builder.Build().RunAsync();