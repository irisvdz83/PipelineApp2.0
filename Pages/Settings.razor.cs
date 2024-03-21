using Microsoft.AspNetCore.Components;
using PipelineApp2._0.Domain;
using PipelineApp2._0.ViewModels;

namespace PipelineApp2._0.Pages;

public partial class Settings : ComponentBase
{
    public SettingViewModel SettingViewModel { get; set; } = null!;
    public int WorkingHoursPerDay { get; set; }

    protected override void OnInitialized()
    {
        GetSetting();
    }

    private void GetSetting()
    {
        var settings = SettingsController.GetOrAddSettings();
        SettingViewModel = SettingViewModel.MapToDateEntry(settings);
    }

    private async Task ToggleDay(DayOfWeek dayOfWeek)
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
            WorkingDaysPerWeek = SettingViewModel.WorkingDaysPerWeek
        });
    }
}