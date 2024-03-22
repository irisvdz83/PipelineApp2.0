using System.Globalization;

namespace PipelineApp2._0.Domain;

public class QuarterlyHourCount : IDbEntity
{
    public Guid Id { get; set; }
    public double Hours { get; set; }

    public override string ToString()
    {
        if(Hours == 0) return "00:00";
        var hourString = $"{Hours:f2}";
        var decimalParts = hourString.Split(',');

        var hours =int.Parse(decimalParts[0]) < 10 ? $"0{decimalParts[0]}" : $"{decimalParts[0]}";
        var minutes = int.Parse(decimalParts[1]) < 10 ? $"0{decimalParts[1]}" : $"{decimalParts[1]}";
        
        return $"{hours}:{minutes}";
    }
}