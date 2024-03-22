using PipelineApp2._0.Domain;
using PipelineApp2._0.Persistence;

namespace PipelineApp2._0.Controllers;

public class SettingsController : ISettingsController
{
    private readonly PipelineDbContext _dbContext;
    private readonly ILogger<SettingsController> _logger;

    public SettingsController(PipelineDbContext dbContext, ILogger<SettingsController> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public Setting GetOrAddSettings()
    {
        try
        {
            var settings = _dbContext.Settings.FirstOrDefault();
            if (settings is null)
            {
                settings = new Setting
                {
                    LunchBreakInMinutes = 30,
                    AddLunchBreaks = false
                };
                int dayCounter = 1;
                foreach (var val in Enum.GetValues(typeof(DayOfWeek)))
                {
                    if ((DayOfWeek)val == DayOfWeek.Sunday) continue;
                    settings.WeekDays.Add(new()
                    {
                        Id = dayCounter++,
                        DayOfWeek = (int)val,
                        IsWorkDay = (DayOfWeek)val != DayOfWeek.Saturday,
                        Name = Enum.GetName(typeof(DayOfWeek), val),
                        Hours = (DayOfWeek)val != DayOfWeek.Saturday ? 8 : 0
                    });
                }
                settings.WeekDays.Add(new()
                {
                    Id = dayCounter,
                    DayOfWeek = (int)DayOfWeek.Sunday,
                    IsWorkDay = false,
                    Name = DayOfWeek.Sunday.ToString()
                });

                settings.Tags = new List<Tag>
                {
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Break",
                        Colour = "#FFDE59"
                    },
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Development",
                        Colour = "#FFD088"
                    },
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Learning",
                        Colour = "#EFAD84"
                    },
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Meeting",
                        Colour = "#92D1B3"
                    }
                };

                var savedEntity = _dbContext.Add(settings);
                _dbContext.SaveChanges();
                settings = savedEntity.Entity;
            }

            settings.WeekDays = _dbContext.WeekDays.OrderBy(x => x.Id).ToList();
            settings.Tags = _dbContext.Tags.OrderBy(x => x.Name).ToList();

            _logger.LogDebug("Getting settings successful.");
            return settings;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Something went wrong in the {method}.", nameof(GetOrAddSettings));
            return new Setting();
        }
    }

    /*public async Task ToggleDay(DayOfWeek dayOfWeek)
    {
        var settings = _dbContext.Settings.FirstOrDefault();
        if (!settings!.WeekDays.Contains((int)dayOfWeek))
        {
            settings.WeekDays.Add((int)dayOfWeek);
        }
        else
        {
            settings.WeekDays.Remove((int)dayOfWeek);
        }
        _dbContext.Update(settings);
        await _dbContext.SaveChangesAsync();
    }

    public async Task SetWorkingHoursPerDay(int workingHoursPerDay)
    {
        var settings = _dbContext.Settings.FirstOrDefault();
        settings!.WorkingHoursPerDay = workingHoursPerDay;
        _dbContext.Update(settings);
        await _dbContext.SaveChangesAsync();
    }

    public void SaveSettings(Setting setting)
    {
        var savedSetting = _dbContext.Settings.FirstOrDefault();
        savedSetting!.WorkingHoursPerDay = setting.WorkingHoursPerDay;
        savedSetting.WeekDays = setting.WeekDays;
        _dbContext.Update(savedSetting);
        _dbContext.SaveChanges();
    }*/
}