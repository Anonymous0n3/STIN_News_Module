using STIN_News_Module.Logic;
using STIN_News_Module.Logic.Filtering;
using STIN_News_Module.Logic.JsonModel;
using STIN_News_Module.Logic.Logging;
using System.Collections.Generic;

var builder = WebApplication.CreateBuilder(args);

EnvLoad.Load();

// Add services to the container.
builder.Services.AddRazorPages();

//Load .env file
EnvLoad.Load();

LoggingService.AddLog("Loading .env file");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.MapGet("/Api/Logs", () =>
{
    LoggingService.AddLog("Logs requested");
    return Results.Ok(LoggingService.GetLogs());
});

app.MapGet("/liststock", () =>
{
    LoggingService.AddLog("List stock requested");
});

app.MapGet("/salestock", () =>
{
    LoggingService.AddLog("Sale stock requested");
});

app.Run();