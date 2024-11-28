using System.Text;

namespace Oscars.Backend.Model
{
    public class Voter
    {
        public required int Id { get; set; }
        public required int PollId { get; set; }
        public required string Name { get; set; }
    }

    public class UniqueCode
    {
        public required int Id { get; set; }
        public required int VoterId { get; set; }
        public required string Code { get; set; }

        public string Generate()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-*/.:;[]{}()";
            StringBuilder result = new(6);

            Random random = new();
            for (int i = 0; i < 6; i++)
            {
                result.Append(chars[random.Next(chars.Length)]);
            }

            return result.ToString();
        }
    }
}
