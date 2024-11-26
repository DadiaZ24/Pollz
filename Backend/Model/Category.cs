using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Oscars.Backend.Model;

namespace Oscars.Backend.Model
{
    public class Category
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required List<string> Nominees { get; set; }
    }
}
