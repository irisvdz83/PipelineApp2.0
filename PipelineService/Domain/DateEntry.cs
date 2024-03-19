namespace PipelineService.Domain;

public class DateEntry
{
    public Guid Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
}