namespace PipelineApp2._0.Domain;

public class DateEntry : IDbEntity
{
    public Guid Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public List<string> Tags { get; set; } = new();
    public string? Description { get; set; } 
}