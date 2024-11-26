using Oscars.Backend.Model;

namespace Oscars.Backend.Dtos
{
    public class NomineeDto
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
    }
}
