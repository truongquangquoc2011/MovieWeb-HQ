using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using MovieWeb_HQ.Models;

var builder = WebApplication.CreateBuilder(args);

// Load cấu hình từ appsettings.json
var configuration = builder.Configuration;

// Thêm DbContext với ConnectionString từ appsettings.json
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Cấu hình pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
// giúp cho fancybox nhận được file TS
var provider = new FileExtensionContentTypeProvider();
provider.Mappings[".ts"] = "video/mp2t"; // MIME type chuẩn cho file TS

app.UseStaticFiles(new StaticFileOptions
{
    ContentTypeProvider = provider
});