namespace Oscars.Backend.Dtos
{
    public class UniqueCodeDto
    {
        public required string Code { get; set; }
        public required int CategoryId { get; set; }
        public required int NomineeId { get; set; }
    }
}
