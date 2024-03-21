using PipelineApp2._0.Domain;

namespace PipelineApp2._0.Controllers;

public interface IDateEntryController
{
    DateEntry GetToday();
    DateEntry? AddNewStartTime(DateTime startTime);
    bool UpdateEndTime(Guid dateId);
}