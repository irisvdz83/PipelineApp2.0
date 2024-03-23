using PipelineApp2._0.Contants;
using PipelineApp2._0.Domain;
using PipelineApp2._0.Persistence;
using PipelineApp2._0.ViewModels;

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
                        Name = PipelineConstants.Break,
                        Colour = "#FFDE59",
                        IsDefault = true
                    },
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Name = PipelineConstants.Development,
                        Colour = "#FFD088",
                        IsDefault = true
                    },
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Name = PipelineConstants.Learning,
                        Colour = "#EFAD84",
                        IsDefault = true
                    },
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Name = PipelineConstants.Meeting,
                        Colour = "#92D1B3",
                        IsDefault = true
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

    public void UpdateWeekdays(List<WeekDay> weekDays)
    {
        var existingWeekDays = _dbContext.WeekDays.ToList();
        weekDays.ForEach(item =>
        {
            var existing = existingWeekDays.First(x => x.Id == item.Id);
            existing.IsWorkDay = item.IsWorkDay;
            existing.Hours = item.Hours;
            existing.Minutes = item.Minutes;
        });
        _dbContext.UpdateRange(existingWeekDays);
        _dbContext.SaveChanges();
    }

    public void AddNewTag(Tag newTag)
    {
        _dbContext.Tags.Add(newTag);
        _dbContext.SaveChanges();
    }

    public void DeleteTag(Guid tagId)
    {
        var savedTag =_dbContext.Tags.FirstOrDefault(x => x.Id == tagId);
        if (savedTag is null) return;
        _dbContext.Tags.Remove(savedTag);
        _dbContext.SaveChanges();
    }

    public void SaveSettings(SettingViewModel settings)
    {
        var savedSetting = _dbContext.Settings.FirstOrDefault();
        savedSetting!.AddLunchBreaks = settings.AddLunchBreaks;
        savedSetting.LunchBreakInMinutes = settings.LunchBreakInMinutes;
        _dbContext.Update(savedSetting);
        _dbContext.SaveChanges();
    }
}