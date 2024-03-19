using PipelineService.Domain;
using PipelineService.Interfaces;
using PipelineService.Persistence;

namespace PipelineService.Controllers;

public class DateEntryController : IDateEntryController
{
    private readonly PipelineContext _pipelineContext;
    private readonly ILogger<DateEntryController> _logger;

    public DateEntryController(PipelineContext pipelineContext, ILogger<DateEntryController> logger)
    {
        _pipelineContext = pipelineContext;
        _logger = logger;
    }

    public bool AddOrUpdateDateEntry(DateEntry dateEntry)
    {
        try
        {

            if (dateEntry.Id == Guid.Empty)
            {
                _pipelineContext.Add(dateEntry);
            }
            else
            {
                _pipelineContext.Update(dateEntry);
            }
            _pipelineContext.SaveChanges();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An exception occurred in {method}.", nameof(AddOrUpdateDateEntry));
            return false;
        }
    }
}