using PipelineApp2._0.Domain;

namespace PipelineApp2._0.Controllers;

public interface IDateEntryController
{
    DateEntry GetOrCreateToday();
    DateEntry? AddNewStartTime(Guid dateId, DateTime startTime, string task, List<string> tags);
    bool UpdateEndTime(Guid dateId, DateTime endTime);
    TimeSpan GetTotalWorkedTimeToday();
    List<DateEntry> GetAllEntriesForToday();
    Dictionary<WeekDay, string> GetThisWeekPreviousDaysWorkHoursAsString();
    void DeleteEntry(Guid id);
    List<Tag> GetAllTags();
    void UpdateDateEntry(Guid id, DateEntry entry);
    void CalculateQuarterlyHours();
    void AddMissingDays();
}