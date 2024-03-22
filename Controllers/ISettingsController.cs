using PipelineApp2._0.Domain;
using PipelineApp2._0.ViewModels;

namespace PipelineApp2._0.Controllers;

public interface ISettingsController
{
    Setting GetOrAddSettings();
    void SaveSettings(List<WeekDay> weekDays);
    void SaveSettings(SettingViewModel settings);
    /*Task ToggleDay(DayOfWeek dayOfWeek);
    Task SetWorkingHoursPerDay(int workingHoursPerDay);
    void SaveSettings(Setting setting);*/
}