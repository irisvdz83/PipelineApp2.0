using System.ComponentModel.DataAnnotations.Schema;

namespace PipelineApp2._0.Domain
{
    public class Tag : IDbEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Colour { get; set; } = null!;
        public Guid SettingId { get; set; }
        
        [NotMapped]
        public bool Selected { get; set; }
    }
}
