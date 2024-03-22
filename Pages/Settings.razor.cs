using Microsoft.AspNetCore.Components;
using PipelineApp2._0.ViewModels;

namespace PipelineApp2._0.Pages;

public partial class Settings : ComponentBase
{
    public SettingViewModel SettingViewModel { get; set; } = null!;
    public string Output = string.Empty;
    public int TotalHours;
    public int TotalMinutes;

    protected override void OnInitialized()
    {
        GetSetting();
    }

    private void GetSetting()
    {
        var settings = SettingsController.GetOrAddSettings();
        SettingViewModel = SettingViewModel.MapToDateEntry(settings);
        SetTotals();
    }

    public void WeekDayChange(int id)
    {
        var day = SettingViewModel.WeekDays.First(x => x.Id == id);
        Output = id + "  " + day.IsWorkDay + " " + day.Hours + " " + day.Minutes;
        SetTotals();
        StateHasChanged();
    }

    public void LunchBreakChange()
    {
        Output = SettingViewModel.AddLunchBreaks + " " + SettingViewModel.LunchBreakInMinutes;
        StateHasChanged();
    }

    private void SetTotals()
    {
        TotalHours = SettingViewModel.WeekDays.Where(x => x.IsWorkDay).Sum(x => x.Hours);
        TotalMinutes = SettingViewModel.WeekDays.Where(x => x.IsWorkDay).Sum(x => x.Minutes);
    }

    /*private async Task ToggleDay(DayOfWeek dayOfWeek)
    {
        await SettingsController.ToggleDay(dayOfWeek);
        GetSetting();
    }

    private async Task SetWorkingHoursPerDay(ChangeEventArgs ev)
    {
        SettingViewModel.WorkingHoursPerDay = WorkingHoursPerDay;
        await SettingsController.SetWorkingHoursPerDay(SettingViewModel.WorkingHoursPerDay);
        GetSetting();
    }

    private void SaveChanges()
    {
        SettingsController.SaveSettings(new Setting
        {
            WorkingHoursPerDay = SettingViewModel.WorkingHoursPerDay,
            WeekDays = SettingViewModel.WeekDays
        });
    }*/
}