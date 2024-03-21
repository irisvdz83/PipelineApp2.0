namespace PipelineApp2._0.Services;

public interface IScopedProcessingService
{
    Task DoWorkAsync(CancellationToken stoppingToken);
}