using PipelineApp2._0.Domain;
using PipelineApp2._0.Helpers;
using PipelineApp2._0.Persistence;
using System;

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

    public Dictionary<WeekDay, string> GetThisWeekPreviousDaysWorkHoursAsString()
    {
        var result = new Dictionary<WeekDay, string>();
        var currentDay = DateTime.Now.DayOfWeek;
        var daysTillCurrentDay = currentDay - DayOfWeek.Monday;
        var workingDays = _dbContext.WeekDays;
        var previousDays = _dbContext.DateEntries
            .Where(x => x.StartTime.Date >= DateTime.Today.Date.AddDays(-daysTillCurrentDay) &&
                        x.StartTime.Date < DateTime.Today.Date).OrderByDescending(x => x.StartTime)
            .GroupBy(x => x.StartTime.Date);
        foreach (var day in previousDays)
        {
            var dayOfWeek = workingDays.FirstOrDefault(x => x.DayOfWeek == (int)day.Key.DayOfWeek);
            if (dayOfWeek is null) continue;
            var dayTimeBlocks = day.ToList();
            TimeSpan timeWorked = default;
            foreach (var timeBlock in dayTimeBlocks)
            {
                if (timeBlock.StartTime is { Hour: 0, Minute: 0, Second: 0 } && timeBlock.EndTime is { Hour: 23, Minute: 59, Second: 59 } && !dayOfWeek.IsWorkDay) 
                {
                    result.Add(dayOfWeek, TimeSpanHelper.GetFormattedTimeSpan(timeWorked));
                }
                timeWorked += timeBlock.EndTime!.Value - timeBlock.StartTime;
            }
            result.Add(dayOfWeek, TimeSpanHelper.GetFormattedTimeSpan(timeWorked));
        }
        var missingDays = 7 - daysTillCurrentDay;
        
        for (var i = 1; i <= missingDays; i++)
        {
            var today = daysTillCurrentDay + i;
            var dayOfWeek = workingDays.FirstOrDefault(x => x.Id == today);
            if (dayOfWeek is null) continue;
            result.Add(new WeekDay
            {
                Id = today,
                DayOfWeek = today == 7 ? 0 : today,
                Hours = dayOfWeek.Hours, 
                Minutes = dayOfWeek.Minutes, 
                Name = dayOfWeek.Name, 
                IsWorkDay = dayOfWeek.IsWorkDay
                
            }, TimeSpanHelper.GetFormattedTimeSpan(new TimeSpan(0, 0, 0)));
        }

        return result;
    }
}