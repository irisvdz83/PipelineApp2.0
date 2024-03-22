using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace PipelineApp2._0.Domain
{
    public class Tag : IDbEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Colour { get; set; } = null!;
        [NotMapped]
        public bool Selected { get; set; }
    }
}
