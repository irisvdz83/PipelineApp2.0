using PipelineService.Domain;

namespace Pipeline.UI.ViewModels;

public class DateEntryViewModel
{
    public Guid Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }

    public static DateEntryViewModel MapToDateEntry(DateEntry date)
    {
        return new DateEntryViewModel
        {
            Id = date.Id,
            StartTime = date.StartTime,
            EndTime = date.EndTime
        };
    }
}

