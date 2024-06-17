using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FilmShelf.Areas.Identity.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext
var connectionString = builder.Configuration.GetConnectionString("FilmShelfContextConnection") ?? throw new InvalidOperationException("Connection string 'FilmShelfContextConnection' not found.");
builder.Services.AddDbContext<FilmShelfContext>(options => options.UseSqlServer(connectionString));

// Add Identity
builder.Services.AddDefaultIdentity<FilmShelfUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<FilmShelfContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();

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
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();

