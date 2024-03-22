using PipelineApp2._0.Domain;

namespace PipelineApp2._0.Controllers;

public interface IDateEntryController
{
    DateEntry GetToday();
    DateEntry? AddNewStartTime(Guid dateId, DateTime startTime);
    bool UpdateEndTime(Guid dateId, DateTime endTime);
    TimeSpan GetTotalWorkedTimeToday();
    List<DateEntry> GetAllEntriesForToday();
    Dictionary<WeekDay, string> GetThisWeekPreviousDaysWorkHoursAsString();

    void DeleteEntry(Guid id);
}