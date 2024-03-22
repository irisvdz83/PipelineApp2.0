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
    public string NewTag { get; set; }
    public string NewTagColour;


    protected override void OnInitialized()
    {
        GetSetting();
    }

    private void GetSetting()
    {
        NewTagColour = "#EFAD84";
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
        SettingsController.UpdateWeekdays(TempWeekDays);

        SettingViewModel.AddLunchBreaks = TempAddLunchBreaks;
        SettingViewModel.LunchBreakInMinutes = TempLunchBreakInMinutes;
        SettingsController.UpdateWeekdays(SettingViewModel.WeekDays);

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
        SettingsController.DeleteTag(id);
    }

    private void CreateNewTag()
    {
        var newTag = new Tag
        {
            Name = NewTag,
            Colour = NewTagColour,
            SettingId = SettingViewModel.Id
        };
        SettingsController.AddNewTag(newTag);
        GetSetting();
        NewTag = string.Empty;
    }

}