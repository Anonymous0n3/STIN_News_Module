using STIN_News_Module.Logic;
using STIN_News_Module.Logic.Filtering;
using STIN_News_Module.Logic.JsonModel;
using System.Collections.Generic;

var builder = WebApplication.CreateBuilder(args);

EnvLoad.Load();

// Add services to the container.
builder.Services.AddRazorPages();

//Load .env file
EnvLoad.Load();

Utils utils = new Utils();
List<DataModel> data = JSONLogic.Instance.deserializeJSON("[\r\n{\"name\": \"Microsoft\", \"date\": 12345678, \"rating\": -10, \"sale\": 1},\r\n{\"name\": \"Google\", \"date\": 12345678, \"rating\": 10, \"sale\": 0},\r\n{\"name\": \"OpenAI\", \"date\": 12345678, \"rating\": 2, \"sale\": 0}\r\n]\r\n");
//List<DataModel> logickedData = utils.doAllLogic(data, 2);

//List<DataModel> logickedData = FilterManager.Instance.ExecuteAllFilters(data);
List < DataModel> logickedData = utils.saleRating(data, 5);
String json = JSONLogic.Instance.serializeJSON(logickedData);
Console.WriteLine(json);
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

app.Run();
