using PipelineApp2._0.Contants;
using PipelineApp2._0.Domain;
using PipelineApp2._0.Extensions;
using PipelineApp2._0.Helpers;
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
    public DateEntry GetOrCreateToday()
    {
        try
        {  
            return _dbContext.DateEntries.FirstOrDefault(x => x.StartTime.Date.Equals(DateTime.Today.Date)) ?? CreateNewDay();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Something went wrong in the {method}.", nameof(GetOrCreateToday));
            return new DateEntry();
        }
    }

    public void AddMissingDays()
    {
        try
        {
            var lastEntry = _dbContext.DateEntries.Where(x => x.StartTime.Date != DateTime.Today.Date).OrderByDescending(x => x.StartTime).FirstOrDefault();
            if (lastEntry is null) return;
            var daysBetween = (DateTime.Today.Date - lastEntry.StartTime.Date).TotalDays;
            if (daysBetween < 1) return;
            
            for (var i = 1; i < daysBetween; i++)
            {
                var newDay = lastEntry.StartTime.Date.AddDays(i);
                var weekDay = _dbContext.WeekDays.FirstOrDefault(x => x.DayOfWeek == (int)newDay.DayOfWeek);
                if (weekDay is null) continue;
                if (weekDay.IsWorkDay)
                {
                    _dbContext.Add(new DateEntry { StartTime = newDay.AddHours(8).AddMinutes(0).AddSeconds(0), EndTime = newDay.AddHours(0).AddMinutes(1).AddSeconds(0) });
                }
                else
                {
                    _dbContext.Add(new DateEntry
                    {
                        StartTime = newDay.AddHours(0).AddMinutes(0).AddSeconds(0),
                        EndTime = newDay.AddHours(23).AddMinutes(59).AddSeconds(59),
                        Tags = new List<string> { PipelineConstants.DayOff },
                        Description = PipelineConstants.DayOffTitle
                    });
                }
            }
            _dbContext.SaveChanges();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Something went wrong in the {method}.", nameof(AddMissingDays));
        }
    }
    public DateEntry? AddNewStartTime(Guid dateId, DateTime startTime, string task, List<string> tags)
    {
        try
        {
            var savedTodayEntry = _dbContext.DateEntries.FirstOrDefault(x => x.Id == dateId);
            if (savedTodayEntry?.EndTime is not null)
            {
                var newTodayEntry = new DateEntry { StartTime = startTime, Description = task, Tags = tags };
                var savedEntity = _dbContext.Add(newTodayEntry);
                _dbContext.SaveChanges();
                _logger.LogDebug("Adding today entry successful.");
                newTodayEntry = savedEntity.Entity;
                return newTodayEntry;
            }

            if (savedTodayEntry is not null)
            {
                savedTodayEntry.StartTime = startTime;
                savedTodayEntry.Description = task;
                savedTodayEntry.Tags = tags;
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
        var settings = _dbContext.Settings.FirstOrDefault();
        var today = _dbContext.DateEntries.Where(x => x.StartTime.Date.Equals(DateTime.Today.Date) && x.EndTime.HasValue).OrderByDescending(x => x.StartTime);
        TimeSpan totalWorkedTime = default;
        if (!today.Any()) return totalWorkedTime;
        
        var dayOfWeek = _dbContext.WeekDays.FirstOrDefault(x => x.DayOfWeek == (int)DateTime.Today.Date.DayOfWeek);
        foreach (var todayEntry in today)
        {
            if (todayEntry.StartTime is { Hour: 0, Minute: 0, Second: 0 } && todayEntry.EndTime is { Hour: 23, Minute: 59, Second: 59 } && !dayOfWeek!.IsWorkDay) continue;
            if (todayEntry.Tags.CaseInsensitiveContains(PipelineConstants.Break)) continue;
            totalWorkedTime += todayEntry.EndTime!.Value - todayEntry.StartTime;
        }
        if (settings is not null && settings.AddLunchBreaks && dayOfWeek!.IsWorkDay && !today.Contains(x => x.Tags.Exists(t => t.CaseInsensitiveContains(PipelineConstants.Break) || t.CaseInsensitiveContains(PipelineConstants.DayOff))))
        {
            totalWorkedTime -= TimeSpan.FromMinutes(settings.LunchBreakInMinutes);
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
        try
        {
            var settings = _dbContext.Settings.FirstOrDefault();
            var result = new Dictionary<WeekDay, string>();
            var currentDay = DateTime.Now.DayOfWeek;
            var daysTillCurrentDay = currentDay - DayOfWeek.Monday;
            var workingDays = _dbContext.WeekDays;
            var previousDays = _dbContext.DateEntries
                .Where(x => x.StartTime.Date >= DateTime.Today.Date.AddDays(-daysTillCurrentDay) &&
                            x.StartTime.Date < DateTime.Today.Date).OrderByDescending(x => x.StartTime)
                .GroupBy(x => x.StartTime.Date);
            int missingDays;
            if (previousDays.Any())
            {
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
                        if (timeBlock.Tags.CaseInsensitiveContains(PipelineConstants.Break)) continue;
                        if (!timeBlock.EndTime.HasValue) continue;
                        timeWorked += timeBlock.EndTime.Value - timeBlock.StartTime;
                    }
                    if (settings is not null && settings.AddLunchBreaks && dayOfWeek.IsWorkDay && !dayTimeBlocks.Contains(x => x.Tags.Exists(t => t.CaseInsensitiveContains(PipelineConstants.Break) || t.CaseInsensitiveContains(PipelineConstants.DayOff))))
                    {
                        timeWorked -= TimeSpan.FromMinutes(settings.LunchBreakInMinutes);
                    }
                    result.Add(dayOfWeek, TimeSpanHelper.GetFormattedTimeSpan(timeWorked));
                }
                missingDays = 7 - result.Count;
            }
            else
            {
                missingDays = 7;
            }

            if (missingDays == 0) return result;

            for (var i = 1; i <= 7; i++)
            {
                if (result.Keys.Any(x => x.Id == i)) continue;

                var today = i;
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Something went wrong in the {method}.", nameof(GetThisWeekPreviousDaysWorkHoursAsString));
        }

        return new Dictionary<WeekDay, string>();
    }

    public void DeleteEntry(Guid id)
    {
        var entry = _dbContext.DateEntries.FirstOrDefault(x => x.Id == id);
        if (entry is null) return;

        _dbContext.Remove(entry);
        _dbContext.SaveChanges();
    }

    public List<Tag> GetAllTags() => _dbContext.Tags.ToList();
    public void UpdateDateEntry(Guid id, DateEntry entry)
    {
        var savedEntry = _dbContext.DateEntries.FirstOrDefault(x => x.Id == id);
        if (savedEntry is null) return;
        savedEntry.StartTime = entry.StartTime;
        savedEntry.EndTime = entry.EndTime;
        savedEntry.Description = entry.Description;
        savedEntry.Tags = entry.Tags;
        _dbContext.Update(savedEntry);
        _dbContext.SaveChanges();
    }

    public void CalculateQuarterlyHours()
    {
        try
        {
            var settings = _dbContext.Settings.FirstOrDefault();
            var previousDays = _dbContext.DateEntries.Where(x => x.StartTime.Date < DateTime.Today.Date && x.EndTime.HasValue).GroupBy(x => x.StartTime.Date);
            var workingDays = _dbContext.WeekDays;
            var savedQuarterlyHours = _dbContext.QuarterlyHours.FirstOrDefault();
            TimeSpan quarterlyHoursInTimeSpan = default;
            foreach (var day in previousDays)
            {
                var dayOfWeek = workingDays.FirstOrDefault(x => x.DayOfWeek == (int)day.Key.DayOfWeek);
                if (dayOfWeek is null) continue;
                var dayTimeBlocks = day.ToList();
                TimeSpan timeWorked = default;
                foreach (var timeBlock in dayTimeBlocks)
                {
                    if (timeBlock.StartTime is { Hour: 0, Minute: 0, Second: 0 } && timeBlock.EndTime is { Hour: 23, Minute: 59, Second: 59 } && !dayOfWeek.IsWorkDay) continue;
                    if (timeBlock.Tags.CaseInsensitiveContains(PipelineConstants.Break)) continue;
                    timeWorked += timeBlock.EndTime!.Value - timeBlock.StartTime;
                }
                if (dayOfWeek.IsWorkDay && settings is not null && settings.AddLunchBreaks && dayOfWeek.IsWorkDay && !dayTimeBlocks.Contains(x => x.Tags.Exists(t => t.CaseInsensitiveContains(PipelineConstants.Break) || t.CaseInsensitiveContains(PipelineConstants.DayOff))))
                {
                    timeWorked -= TimeSpan.FromMinutes(settings.LunchBreakInMinutes);
                }
                var timeHasToBeWorked = TimeSpan.FromHours(dayOfWeek.Hours) + TimeSpan.FromHours(dayOfWeek.Minutes);
                quarterlyHoursInTimeSpan += timeWorked - timeHasToBeWorked;
            }

            if (savedQuarterlyHours is null)
            {
                _dbContext.QuarterlyHours.Add(new QuarterlyHourCount
                {
                    Hours = quarterlyHoursInTimeSpan
                });
            }
            else
            {
                savedQuarterlyHours.Hours = quarterlyHoursInTimeSpan;
                _dbContext.QuarterlyHours.Update(savedQuarterlyHours);
            }

            _dbContext.SaveChanges();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in method {name}.", nameof(CalculateQuarterlyHours));
        }
    }

    private DateEntry CreateNewDay()
    {
        var weekDay = _dbContext.WeekDays.FirstOrDefault(x => x.DayOfWeek == (int)DateTime.Today.DayOfWeek); 
        DateEntry today;
        if (weekDay is not null && weekDay.IsWorkDay)
        {
            today = new DateEntry { StartTime = DateTime.Today };
        }
        else
        {
            today = new DateEntry
            {
                StartTime = DateTime.Today,
                EndTime = DateTime.Today.AddHours(23).AddMinutes(59).AddSeconds(59),
                Tags = new List<string> { PipelineConstants.DayOff },
                Description = PipelineConstants.DayOffTitle
            };
        }
        _dbContext.Add(today);
        _dbContext.SaveChanges();
        return today;
    }
}