namespace PipelineApp2._0.Domain;

public class QuarterlyHourCount : IDbEntity
{
    public Guid Id { get; set; }
    public TimeSpan Hours { get; set; }
}