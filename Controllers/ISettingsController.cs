using PipelineApp2._0.Domain;

namespace PipelineApp2._0.Controllers;

public interface ISettingsController
{
    Setting GetOrAddSettings();
    /*Task ToggleDay(DayOfWeek dayOfWeek);
    Task SetWorkingHoursPerDay(int workingHoursPerDay);
    void SaveSettings(Setting setting);*/
}