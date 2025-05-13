using STIN_News_Module.Logic;
using STIN_News_Module.Logic.Filtering;
using STIN_News_Module.Logic.JsonModel;

var builder = WebApplication.CreateBuilder(args);

EnvLoad.Load();

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

// REST endpoint using the DataModel from a separate file
app.MapPost("/api/data", (DataModel data) =>
{
    // For demonstration, set the number of articles to 10.
    data.setarticleNum(10);

    return Results.Ok(new
    {
        message = "Data model received and processed",
        receivedData = data,
        articleCount = data.getarticleNum()
    });
});

app.Run();
