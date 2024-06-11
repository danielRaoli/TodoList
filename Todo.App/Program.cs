using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Todo.App;
using MudBlazor.Services;
using Microsoft.Extensions.Configuration;
using TodoList.Core;
using TodoList.Core.Repositories;
using Todo.App.Handlers;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddMudServices();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddHttpClient(WebConfiguration.HttpClientName, opt =>
{
    opt.BaseAddress = new Uri(AppConfiguration.BackEnd);
});

builder.Services.AddScoped<ITodoRepository, TodoHandler>();
await builder.Build().RunAsync();
