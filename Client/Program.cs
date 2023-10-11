using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using PollaEngendrilClientHosted.Client;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using PollaEngendrilClientHosted.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("ServerAPI",
    client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
  .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
  .CreateClient("ServerAPI"));

builder.Services.AddAuth0OidcAuthentication(options =>
{
  builder.Configuration.Bind("Auth0", options.ProviderOptions);
  options.ProviderOptions.ResponseType = "code";
  options.ProviderOptions.AdditionalProviderParameters.Add("audience", builder.Configuration["Auth0:Audience"]);

    var authority = builder.Configuration["Auth0:Authority"];
    var clientId = builder.Configuration["Auth0:ClientId"];
    options.ProviderOptions.MetadataSeed.EndSessionEndpoint = $"{authority}/v2/logout?client_id={clientId}&returnTo={builder.HostEnvironment.BaseAddress}";

});

builder.Services.AddMudServices();

builder.Services.AddScoped<IPredictionApiService, PredictionApiService>();
builder.Services.AddScoped<IFixturesApiService, FixturesApiService>();


await builder.Build().RunAsync();
