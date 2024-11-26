using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices.Marshalling;

namespace Oscars.Backend.Model
{
    public class Vote
    {
        public required string UniqueCode { get; set; }
        public required int NomineeId { get; set; }
        public required int CategoryId { get; set; }
    }
}
