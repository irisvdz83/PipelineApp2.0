using Microsoft.EntityFrameworkCore;
using PipelineService.Domain;
using PipelineService.Persistence;

namespace Pipeline.UI.Controllers;

public class DateController
{
    private readonly IDbContextFactory<PipelineContext> _contextFactory;
    private readonly ILogger<DateController> _logger;

    public DateController(IDbContextFactory<PipelineContext> contextFactory, ILogger<DateController> logger)
    {
        _contextFactory = contextFactory;
        _logger = logger;
    }

    public DateEntry GetToday()
    {
        try
        {
            var dbContext = _contextFactory.CreateDbContext();
            var today = dbContext.DateEntries.FirstOrDefault(x => x.StartTime.Date.Equals(DateTime.Today.Date));
            if (today is null)
            {
                today = new DateEntry { StartTime = DateTime.Now };
                var savedEntity = dbContext.Add(today);
                dbContext.SaveChanges();
                today = savedEntity.Entity;
            }
            _logger.LogDebug("Getting today entry successful.");
            return today;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Something went wrong in the {method}.", nameof(GetToday));
            return new DateEntry();
        }
    }
    public DateEntry? AddNewStartTime(DateTime startTime)
    {
        try
        {
            var dbContext = _contextFactory.CreateDbContext();
            var newTodayEntry = new DateEntry { StartTime = startTime };
            var savedEntity = dbContext.Add(newTodayEntry);
            dbContext.SaveChanges();
            _logger.LogDebug("Adding or updating today entry successful.");
            newTodayEntry = savedEntity.Entity;
            return newTodayEntry;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Something went wrong in the {method}.", nameof(AddNewStartTime));
        }

        return null;
    }
    public bool UpdateEndTime(Guid dateId)
    {
        try
        {
            var dbContext = _contextFactory.CreateDbContext();
            var savedTodayEntry = dbContext.DateEntries.FirstOrDefault(x => x.Id == dateId);
            if (savedTodayEntry is not null)
            {
                savedTodayEntry = new DateEntry { StartTime = DateTime.Now };
                dbContext.Update(savedTodayEntry);
            }
            
            _logger.LogDebug("Updating today end time successful.");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Something went wrong in the {method}.",nameof(UpdateEndTime));
            return false;
        }
    }
}