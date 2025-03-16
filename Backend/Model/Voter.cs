using System.Text;

namespace Oscars.Backend.Model
{
    /// <summary>
    /// Represents a voter in a poll.
    /// </summary>
    public class Voter
    {
        public required int Id { get; set; }
        public required int PollId { get; set; }
        public required string Name { get; set; }
        public string? Email { get; set; }
    }
    /// <summary>
    /// Represents a unique code associated with a voter.
    /// </summary>
    /// <remarks>
    /// This class is used to generate and manage unique codes for voters. 
    /// Each unique code has an identifier, a voter identifier, the code itself, 
    /// and a flag indicating whether the code has been used.
    /// </remarks>
    public class UniqueCode
    {
        public required int Id { get; set; }
        public required int VoterId { get; set; }
        public required string Code { get; set; }
        public required bool Used { get; set; }
        /// <summary>
        /// Generates a random 6-character string consisting of uppercase letters, 
        /// lowercase letters, and digits.
        /// </summary>
        /// <returns>A randomly generated 6-character string.</returns>
        public string Generate()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
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
