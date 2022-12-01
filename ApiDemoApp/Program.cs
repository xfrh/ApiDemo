using ApiDemoApp.Data;
using ApiDemoApp.Models;
using ApiDemoApp.Services;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services;
using Quartz.Impl;
using Quartz;
using Quartz.Spi;
using BlazorDownloadFile;
using ApiDemoApp.Events;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSignalR();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddSingleton<AppState>();
builder.Services.AddMudServices();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddHostedService<StatuService>();
builder.Services.AddHostedService<NavTaskService>();
builder.Services.AddHttpClient();
builder.Services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

builder.Services.AddHttpClient("cmd", c => {
    c.BaseAddress = new Uri(builder.Configuration["cmd"]);
    });
builder.Services.AddHttpClient("nav", c =>
{
    c.BaseAddress = new Uri(builder.Configuration["nav"]);
});

builder.Services.AddSingleton<AuthenticationStateProvider, TestAuthenticationStateProvider>();
//builder.Services.AddCors();
builder.Services.AddBlazorDownloadFile();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(b =>
    {
        b.WithOrigins("http://localhost:5173")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStaticFiles();
app.UseHttpsRedirection();

app.UseCors();
app.UseRouting();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
app.Run();
