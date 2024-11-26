using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Oscars.Backend.Model
{
    public class UniqueCode
    {
        public required string Code { get; set; }
        public required int CategoryId { get; set; }
        public required int NomineeId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
