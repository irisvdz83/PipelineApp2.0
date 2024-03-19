namespace PipelineService.Services;

public interface IScopedProcessingService
{
    Task DoWorkAsync(CancellationToken stoppingToken);
}