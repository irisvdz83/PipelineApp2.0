using Microsoft.EntityFrameworkCore;
using PipelineApp2._0.Contants;
using PipelineApp2._0.Controllers;
using PipelineApp2._0.Domain;
using PipelineApp2._0.Extensions;
using PipelineApp2._0.Persistence;

namespace PipelineApp2._0.Services;

public sealed class PipelineService : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<PipelineService> _logger;

    public PipelineService(IServiceScopeFactory serviceScopeFactory, ILogger<PipelineService> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }
    private const string ClassName = nameof(PipelineService);
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            "{Name} is running.", ClassName);

        await DoWorkAsync(stoppingToken);
    }

    private async Task DoWorkAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("{Name} is working.", ClassName);

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var controller= scope.ServiceProvider.GetRequiredService<IDateEntryController>();
                GetOrCreateEntryForToday(controller);

                await Task.Delay(TimeSpan.FromMinutes(30), cancellationToken);
            }
            catch (OperationCanceledException ex) // includes TaskCanceledException
            {
                _logger.LogError(ex, "OperationCanceledException Error occurred in method {name}.", nameof(DoWorkAsync));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Generic Error occurred in method {name}.", nameof(DoWorkAsync));
            }
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "{Name} is stopping.", ClassName);

        await base.StopAsync(cancellationToken);
    }
    public DateEntry? GetOrCreateEntryForToday(IDateEntryController controller)
    {
        try
        {

            var today = controller.GetOrCreateToday();
            controller.CalculateQuarterlyHours();
            return today;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in method {name}.", nameof(GetOrCreateEntryForToday));
            return null;
        }
    }

    
}