namespace PipelineApp2._0.Domain
{
    public class Category : IDbEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
