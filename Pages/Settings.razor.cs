using Microsoft.AspNetCore.Components;
using PipelineApp2._0.Domain;
using PipelineApp2._0.ViewModels;

namespace PipelineApp2._0.Pages;

public partial class Settings : ComponentBase
{
    public SettingViewModel SettingViewModel { get; set; } = null!;
    public string Output = string.Empty;
    public int TotalHours;
    public int TotalMinutes;
    public bool Editing;
    public List<WeekDay> TempWeekDays;
    public bool TempAddLunchBreaks;
    public int TempLunchBreakInMinutes;

    protected override void OnInitialized()
    {
        GetSetting();
    }

    private void GetSetting()
    {
        var settings = SettingsController.GetOrAddSettings();
        SettingViewModel = SettingViewModel.MapToDateEntry(settings);
        TempWeekDays = new();
        SettingViewModel.WeekDays.ForEach(item =>
        {
            TempWeekDays.Add((WeekDay)item.Clone());
        });
        TempAddLunchBreaks = SettingViewModel.AddLunchBreaks;
        TempLunchBreakInMinutes = SettingViewModel.LunchBreakInMinutes;
        SetTotals();
    }

    private void SetTotals()
    {
        TotalHours = SettingViewModel.WeekDays.Where(x => x.IsWorkDay).Sum(x => x.Hours);
        TotalMinutes = SettingViewModel.WeekDays.Where(x => x.IsWorkDay).Sum(x => x.Minutes);
    }

    public void EditWeekDays()
    {
        Editing = true;
    }

    public void SaveWeekDays()
    {
        SettingViewModel.WeekDays = new();
        TempWeekDays.ForEach(item =>
        {
            SettingViewModel.WeekDays.Add((WeekDay)item.Clone());
        });
        SettingsController.SaveSettings(TempWeekDays);

        SettingViewModel.AddLunchBreaks = TempAddLunchBreaks;
        SettingViewModel.LunchBreakInMinutes = TempLunchBreakInMinutes;
        SettingsController.SaveSettings(SettingViewModel);

        SetTotals();
        Editing = false;
    }

    public void CancelWeekDays()
    {
        TempWeekDays = new();
        SettingViewModel.WeekDays.ForEach(item =>
        {
            TempWeekDays.Add((WeekDay)item.Clone());
        });
        TempAddLunchBreaks = SettingViewModel.AddLunchBreaks;
        TempLunchBreakInMinutes = SettingViewModel.LunchBreakInMinutes;
        Editing = false;
    }

    public void TagDelete(Guid id)
    {
        throw new NotImplementedException();
    }
}