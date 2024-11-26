using Oscars.Backend.Model;

namespace Oscars.Backend.Dtos
{
    public class CategoryDto
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required List<string> Nominees { get; set; }
    }
}
