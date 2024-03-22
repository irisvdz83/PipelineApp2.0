namespace PipelineApp2._0.Domain;

public class Setting
{
    public Guid Id { get; set; }
    public List<WeekDay> WeekDays { get; set; } = new();
    public int LunchBreakInMinutes { get; set; } = 0;
    public bool AddLunchBreaks { get; set; }
    public List<Tag> Tags { get; set; } = new();
}