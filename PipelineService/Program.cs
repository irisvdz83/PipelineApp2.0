using Microsoft.EntityFrameworkCore;
using PipelineService.Persistence;

const string serviceName = "Pipeline Service";
var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddWindowsService(options =>
{
    options.ServiceName = serviceName;
});
builder.Services.AddLogging();
builder.Services.AddDbContext<PipelineContext>(options => options.UseSqlite(builder.Configuration["ConnectionStrings:SqliteConnection"]));

builder.Services.AddHostedService<PipelineService.Services.PipelineService>();

var host = builder.Build();
host.Run();
