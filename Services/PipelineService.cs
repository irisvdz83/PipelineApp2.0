using Microsoft.EntityFrameworkCore;
using PipelineApp2._0.Contants;
using PipelineApp2._0.Domain;
using PipelineApp2._0.Extensions;
using PipelineApp2._0.Persistence;

namespace PipelineApp2._0.Services;

public sealed class PipelineService : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<PipelineService> _logger;

    public PipelineService(IServiceScopeFactory serviceScopeFactory, ILogger<PipelineService> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }
    private const string ClassName = nameof(PipelineService);
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            "{Name} is running.", ClassName);

        await DoWorkAsync(stoppingToken);
    }

    private async Task DoWorkAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("{Name} is working.", ClassName);

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<PipelineDbContext>();
                GetOrCreateEntryForToday(dbContext);

                await Task.Delay(TimeSpan.FromMinutes(30), cancellationToken);
            }
            catch (OperationCanceledException ex) // includes TaskCanceledException
            {
                _logger.LogError(ex, "OperationCanceledException Error occurred in method {name}.", nameof(DoWorkAsync));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Generic Error occurred in method {name}.", nameof(DoWorkAsync));
            }
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "{Name} is stopping.", ClassName);

        await base.StopAsync(cancellationToken);
    }
    public DateEntry? GetOrCreateEntryForToday(PipelineDbContext dbContext)
    {
        try
        {
            
            var today = dbContext.DateEntries.FirstOrDefault(x => x.StartTime.Date.Equals(DateTime.Today.Date));
            if (today != null) return today;
            CalculateQuarterlyHours(dbContext);
            today = CreateNewDay(dbContext);

            return today;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in method {name}.", nameof(GetOrCreateEntryForToday));
            return null;
        }
    }

    private static DateEntry CreateNewDay(PipelineDbContext dbContext)
    {
        var weekDay = dbContext.WeekDays.FirstOrDefault(x => x.DayOfWeek.Equals(DateTime.Today.DayOfWeek));
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
                Tags = new List<string>{PipelineConstants.DayOff},
                Description = PipelineConstants.DayOffTitle
            };
        }
        dbContext.Add(today);
        dbContext.SaveChanges();
        return today;
    }

    private void CalculateQuarterlyHours(PipelineDbContext dbContext)
    {
        try
        {
            var settings = dbContext.Settings.FirstOrDefault();
            var previousDays = dbContext.DateEntries.Where(x => x.StartTime.Date < DateTime.Today.Date && x.EndTime.HasValue).GroupBy(x => x.StartTime.Date);
            var workingDays = dbContext.WeekDays;
            var savedQuarterlyHours = dbContext.QuarterlyHours.FirstOrDefault();
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
                dbContext.QuarterlyHours.Add(new QuarterlyHourCount
                {
                    Hours = quarterlyHoursInTimeSpan
                });
            }
            else
            {
                savedQuarterlyHours.Hours = quarterlyHoursInTimeSpan;
                dbContext.QuarterlyHours.Update(savedQuarterlyHours);
            }

            dbContext.SaveChanges();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in method {name}.", nameof(CalculateQuarterlyHours));
        }
    }
}