using PipelineApp2._0.Domain;
using PipelineApp2._0.Pages;
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
                    WorkingDaysPerWeek = new List<int>{1,2,3,4,5},
                    WorkingHoursPerDay = 8
                };
                var savedEntity = _dbContext.Add(settings);
                _dbContext.SaveChanges();
                settings = savedEntity.Entity;
            }
            _logger.LogDebug("Getting settings successful.");
            return settings;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Something went wrong in the {method}.", nameof(GetOrAddSettings));
            return new Setting();
        }
    }

    public async Task ToggleDay(DayOfWeek dayOfWeek)
    {
        var settings = _dbContext.Settings.FirstOrDefault();
        if (!settings!.WorkingDaysPerWeek.Contains((int)dayOfWeek))
        {
            settings.WorkingDaysPerWeek.Add((int)dayOfWeek);
        }
        else
        {
            settings.WorkingDaysPerWeek.Remove((int)dayOfWeek);
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
        savedSetting.WorkingDaysPerWeek = setting.WorkingDaysPerWeek;
        _dbContext.Update(savedSetting);
        _dbContext.SaveChanges();
    }
}