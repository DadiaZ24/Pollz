using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Oscars.Backend.Model;

namespace Oscars.Backend.Model
{
    public class Question
    {
        public required int Id { get; set; }
        public required int pollId { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
    }
}
