using Oscars.Backend.Model;

namespace Oscars.Backend.Dtos
{
    public class QuestionDto
    {
        public required int Id { get; set; }
        public required int Poll_id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
    }
}
