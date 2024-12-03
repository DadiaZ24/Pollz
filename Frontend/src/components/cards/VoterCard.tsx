import React from "react";

interface Voter {
  id: number;
  name: string;
  email: string;
}

interface VoterCardProps {
  voters: Voter[];
  updateVoter: (id: number, newName: string, newEmail: string) => void;
  removeVoter: (id: number) => void;
}

const VoterCard: React.FC<VoterCardProps> = ({
  voters,
  updateVoter,
  removeVoter,
}) => {
  const handleNameChange = (voter: Voter, newName: string) => {
    updateVoter(voter.id, newName, voter.email); // Only update name
  };

  const handleEmailChange = (voter: Voter, newEmail: string) => {
    updateVoter(voter.id, voter.name, newEmail); // Only update email
  };

  return (
    <div className="bg-white shadow-lg rounded-lg p-6">
      <h3 className="text-lg font-semibold mb-4"></h3>
      <div className="w-full border-t border-gray-300 gap-4">
        {/* Header Row */}
        <div className="flex items-center bg-gray-100 py-2 px-4 font-semibold text-gray-700">
          <div className="flex-1">Name</div>
          <div className="flex-1">Email</div>
          <div className="w-16 text-center">Remove</div>
        </div>
        {/* Voter Rows */}
        {voters.map((voter) => (
          <div
            key={voter.id}
            className="gap-4 mr-auto flex items-center py-2 px-4 odd:bg-white even:bg-gray-50 border-b border-gray-300"
          >
            {/* Name Field */}
            <div className="flex-1">
              <input
                type="text"
                value={voter.name}
                onChange={(e) => handleNameChange(voter, e.target.value)} // Update name only
                className="w-full p-1 border rounded"
              />
            </div>
            {/* Email Field */}
            <div className="flex-1">
              <input
                type="email"
                value={voter.email}
                onChange={(e) => handleEmailChange(voter, e.target.value)} // Update email only
                className="w-full p-1 border rounded"
              />
            </div>

            {/* Remove Button */}
            <div className="w-16 text-center">
              <button
                onClick={() => removeVoter(voter.id)}
                className="bg-red-500 text-white px-3 py-1 rounded hover:bg-red-600"
              >
                Remove
              </button>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
};

export default VoterCard;
