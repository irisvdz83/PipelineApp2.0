using PipelineApp2._0.Domain;
using PipelineApp2._0.Persistence;

namespace PipelineApp2._0.Controllers;

public class DateEntryController : IDateEntryController
{
    private readonly PipelineDbContext _dbContext;
    private readonly ILogger<DateEntryController> _logger;

    public DateEntryController(PipelineDbContext dbContext, ILogger<DateEntryController> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
    public DateEntry GetToday()
    {
        try
        {
            var today = _dbContext.DateEntries.FirstOrDefault(x => x.StartTime.Date.Equals(DateTime.Today.Date));
            if (today is null)
            {
                today = new DateEntry { StartTime = DateTime.Now };
                var savedEntity = _dbContext.Add(today);
                _dbContext.SaveChanges();
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
            var newTodayEntry = new DateEntry { StartTime = startTime };
            var savedEntity = _dbContext.Add(newTodayEntry);
            _dbContext.SaveChanges();
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
            var savedTodayEntry = _dbContext.DateEntries.FirstOrDefault(x => x.Id == dateId);
            if (savedTodayEntry is not null)
            {
                savedTodayEntry = new DateEntry { StartTime = DateTime.Now };
                _dbContext.Update(savedTodayEntry);
            }

            _logger.LogDebug("Updating today end time successful.");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Something went wrong in the {method}.", nameof(UpdateEndTime));
            return false;
        }
    }
}