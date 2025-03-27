using STIN_News_Module.Logic;
using STIN_News_Module.Logic.Filtering;
using STIN_News_Module.Logic.JsonModel;

var builder = WebApplication.CreateBuilder(args);

EnvLoad.Load();

//DeSerialization
List<DataModel> data = JSONLogic.Instance.deserializeJSON("[{\"name\": \"Microsoft\", \"date\": 12345678, \"rating\": -10, \"sale\": 1}, {\"name\": \"Google\", \"date\": 12345678, \"rating\": 10, \"sale\": 0}, {\"name\": \"OpenAI\", \"date\": 12345678, \"rating\": 2, \"sale\": 0}]");

//Filtering
Random rnd = new Random();
foreach (var item in data) {
    item.setarticleNum(rnd.Next(0,20));
}

FilterManager filterManager = new FilterManager();
data = filterManager.ExecuteAllFilters(data);

foreach (var item in data)
{
    Console.WriteLine(item.getarticleNum());
}
// Add services to the container.
builder.Services.AddRazorPages();

//Load .env file
EnvLoad.Load();

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
