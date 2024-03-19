using PipelineService.Domain;
using PipelineService.Persistence;

namespace PipelineService.Services;

public sealed class PipelineService(IServiceScopeFactory serviceScopeFactory, ILogger<PipelineService> logger) : BackgroundService
{
    private const string ClassName = nameof(PipelineService);
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation(
            "{Name} is running.", ClassName);

        await DoWorkAsync(stoppingToken);
    }

    private async Task DoWorkAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "{Name} is working.", ClassName);

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                using var scope = serviceScopeFactory.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<PipelineContext>();
                GetOrCreateEntryForToday(dbContext);

                await Task.Delay(TimeSpan.FromHours(24), cancellationToken);
            }
            catch (OperationCanceledException ex) // includes TaskCanceledException
            {
                logger.LogError(ex, "OperationCanceledException Error occurred in method {name}.", nameof(DoWorkAsync));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Generic Error occurred in method {name}.", nameof(DoWorkAsync));
            }
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "{Name} is stopping.", ClassName);

        await base.StopAsync(cancellationToken);
    }
    public DateEntry? GetOrCreateEntryForToday(PipelineContext dbContext)
    {
        try
        {
            var today = dbContext.DateEntries.FirstOrDefault(x => x.StartTime.Date.Equals(DateTime.Today.Date));
            if (today != null) return today;
            today = new DateEntry { StartTime = DateTime.Today };
            dbContext.Add(today);
            dbContext.SaveChanges();

            return today;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred in method {name}.", nameof(GetOrCreateEntryForToday));
            return null;
        }
    }
}