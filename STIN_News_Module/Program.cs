using STIN_News_Module.Logic;
using STIN_News_Module.Logic.Filtering;

var builder = WebApplication.CreateBuilder(args);

EnvLoad.Load();

//DeSerialization
List<DataModel> data = JsonManager.DeserializeJson();

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
