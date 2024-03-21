namespace PipelineApp2._0.Domain;

public class Setting
{
    public Guid Id { get; set; }
    public List<int> WorkingDaysPerWeek { get; set; } = new();
    public int WorkingHoursPerDay { get; set; }
    //should be set to ignored in the db
    public int TotalHoursPerWeek => WorkingDaysPerWeek.Count * WorkingHoursPerDay;

}