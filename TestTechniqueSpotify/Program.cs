using Microsoft.EntityFrameworkCore;
using MvcLibrary.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MvcLibraryContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("MvcLibraryContext") ?? throw new InvalidOperationException("Connection string 'MvcLibraryContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Library}/{action=Index}/{id?}");

app.Run();
