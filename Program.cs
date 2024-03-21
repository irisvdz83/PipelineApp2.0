using Microsoft.EntityFrameworkCore;
using PipelineApp2._0.Controllers;
using PipelineApp2._0.Persistence;
using PipelineApp2._0.Services;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("PipelineDb");
// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddDbContextFactory<PipelineDbContext>(options => options.UseSqlite(connectionString));

builder.Services.AddTransient<IDateEntryController, DateEntryController>();
builder.Services.AddTransient<ISettingsController, SettingsController>();
builder.Services.AddHostedService<PipelineService>();

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

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
