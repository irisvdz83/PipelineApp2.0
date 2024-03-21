using PipelineApp2._0.Domain;

namespace PipelineApp2._0.ViewModels;

public class SettingViewModel
{
    public Guid Id { get; set; }
    public List<int> WorkingDaysPerWeek { get; set; } = new();
    public int WorkingHoursPerDay { get; set; }
    public int TotalHoursPerWeek { get; set; }

    public static SettingViewModel MapToDateEntry(Setting setting)
    {
        return new SettingViewModel
        {
            Id = setting.Id,
            WorkingDaysPerWeek = setting.WorkingDaysPerWeek,
            TotalHoursPerWeek = setting.TotalHoursPerWeek,
            WorkingHoursPerDay = setting.WorkingHoursPerDay
        };
    }
}