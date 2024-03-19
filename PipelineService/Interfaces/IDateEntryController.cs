using PipelineService.Domain;

namespace PipelineService.Interfaces;

public interface IDateEntryController
{
    bool AddOrUpdateDateEntry(DateEntry dateEntry);
}