using STIN_News_Module.Logic;
using STIN_News_Module.Logic.News;

var builder = WebApplication.CreateBuilder(args);

EnvLoad.Load();

News_Getting news = new News_Getting();
var articles = news.returnNews("Microsoft", 7);
foreach (var article in articles)
{
    Console.WriteLine(article.Title + "\n");
}

// Add services to the container.
builder.Services.AddRazorPages();

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
