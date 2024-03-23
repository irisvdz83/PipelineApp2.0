using PipelineApp2._0.Domain;
using PipelineApp2._0.Persistence;

namespace PipelineApp2._0.Controllers;

public class QuarterlyHoursController : IQuarterlyHoursController
{
    private readonly PipelineDbContext _dbContext;
    private readonly ILogger<QuarterlyHoursController> _logger;

    public QuarterlyHoursController(PipelineDbContext dbContext, ILogger<QuarterlyHoursController> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public QuarterlyHourCount GetQuarterlyHourCount()
    {
        return _dbContext.QuarterlyHours.FirstOrDefault() ?? new QuarterlyHourCount
        {
            Hours = new TimeSpan(0,0,0)
        };
    }
}