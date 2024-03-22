using PipelineApp2._0.Domain;

namespace PipelineApp2._0.ViewModels;

public class SettingViewModel
{
    public Guid Id { get; set; }
    public List<WeekDay> WeekDays { get; set; } = new();
    public int LunchBreakInMinutes { get; set; } = 0;
    public bool AddLunchBreaks { get; set; }
    public List<Tag> Tags { get; set; } = new();

    public static SettingViewModel MapToDateEntry(Setting setting)
    {
        return new SettingViewModel
        {
            Id = setting.Id,
            LunchBreakInMinutes = setting.LunchBreakInMinutes,
            AddLunchBreaks = setting.AddLunchBreaks,
            WeekDays = setting.WeekDays,
            Tags = setting.Tags
        };
    }
}