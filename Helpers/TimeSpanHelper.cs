namespace PipelineApp2._0.Helpers;

public static class TimeSpanHelper
{
    public static string GetFormattedTimeSpan(TimeSpan? timeSpan)
    {
        if(timeSpan is null) return string.Empty;
        var hours = timeSpan.Value.Hours < 10 ? $"0{timeSpan.Value.Hours}" : $"{timeSpan.Value.Hours}";
        var minutes = timeSpan.Value.Minutes < 10 ? $"0{timeSpan.Value.Minutes}" : $"{timeSpan.Value.Minutes}";
        var seconds = timeSpan.Value.Seconds < 10 ? $"0{timeSpan.Value.Seconds}" : $"{timeSpan.Value.Seconds}";
        return $"{hours}:{minutes}:{seconds}";
    }
}