using System.Globalization;

namespace PipelineApp2._0.Domain;

public class QuarterlyHourCount : IDbEntity
{
    public Guid Id { get; set; }
    public double Hours { get; set; }

    public override string ToString()
    {
        var hourString = Hours.ToString(CultureInfo.InvariantCulture);
        var decimalParts = hourString.Split(',');
        return $"{decimalParts[0]}:{decimalParts[1]}:00";
    }
}