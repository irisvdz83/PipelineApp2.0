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
            var today = _dbContext.DateEntries.Where(x => x.StartTime.Date.Equals(DateTime.Today.Date)).OrderByDescending(x => x.StartTime).FirstOrDefault();
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
    public DateEntry? AddNewStartTime(Guid dateId, DateTime startTime)
    {
        try
        {
            var savedTodayEntry = _dbContext.DateEntries.FirstOrDefault(x => x.Id == dateId);
            if (savedTodayEntry?.EndTime is not null)
            {
                var newTodayEntry = new DateEntry { StartTime = startTime };
                var savedEntity = _dbContext.Add(newTodayEntry);
                _dbContext.SaveChanges();
                _logger.LogDebug("Adding today entry successful.");
                newTodayEntry = savedEntity.Entity;
                return newTodayEntry;
            }

            if(savedTodayEntry is not null)
            {
                savedTodayEntry.StartTime = startTime;
                _dbContext.Update(savedTodayEntry);
                _dbContext.SaveChanges();
                _logger.LogDebug("Updating today entry successful.");
               return savedTodayEntry;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Something went wrong in the {method}.", nameof(AddNewStartTime));
        }

        return null;
    }
    public bool UpdateEndTime(Guid dateId, DateTime endTime)
    {
        try
        {
            var savedTodayEntry = _dbContext.DateEntries.FirstOrDefault(x => x.Id == dateId);
            if (savedTodayEntry is not null)
            {
                savedTodayEntry.EndTime = endTime;
                _dbContext.Update(savedTodayEntry);
                _dbContext.SaveChanges();
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

    public TimeSpan GetTotalWorkedTimeToday()
    {
        var today = _dbContext.DateEntries.Where(x => x.StartTime.Date.Equals(DateTime.Today.Date) && x.EndTime.HasValue).OrderByDescending(x => x.StartTime);
        TimeSpan totalWorkedTime = default;
        foreach (var todayEntry in today)
        {
            totalWorkedTime += todayEntry.EndTime!.Value - todayEntry.StartTime;
        }
        return totalWorkedTime;
    }

    public List<DateEntry> GetAllEntriesForToday()
    {
        var today = _dbContext.DateEntries.Where(x => x.StartTime.Date.Equals(DateTime.Today.Date) && x.EndTime.HasValue).OrderByDescending(x => x.StartTime);
        return today.ToList();
    }
}