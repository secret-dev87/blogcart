﻿using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ECommerce_Client;
using MudBlazor.Services;
using MudBlazor;
using ECommerce_Client.Service;
using ECommerce_Client.Service.IService;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddMudServices();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.Configuration.GetValue<string>("BaseAPIUrL")) });
builder.Services.AddScoped<IProductService, ProductService>();
await builder.Build().RunAsync();

