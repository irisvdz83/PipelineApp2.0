using PipelineApp2._0.Domain;
using PipelineApp2._0.ViewModels;

namespace PipelineApp2._0.Controllers;

public interface ISettingsController
{
    Setting GetOrAddSettings();
    void UpdateWeekdays(List<WeekDay> weekDays);
    void AddNewTag(Tag newTag);
    void SaveSettings(SettingViewModel settings);
    void DeleteTag(Guid tagId);

}