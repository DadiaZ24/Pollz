using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Oscars.Backend.Model
{
    public class Nominee
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
    }
}
