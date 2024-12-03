import React from "react";
import { Poll } from "../../services/PollsService";

interface PollCardProps {
  poll: Poll;
}

const PollCard: React.FC<PollCardProps> = ({ poll }) => {
  return (
    <div className="h-full bg-gradient-to-r from-blue-600 to-purple-500 shadow-md rounded-lg p-6 flex flex-col justify-between">
      <h3 className="text-xl font-semibold text-gray-100 mb-2">{poll.title}</h3>
      <p className="text-gray-200 mb-4">{poll.description}</p>
      <p className="text-gray-300">
        Start Date: {new Date(poll.created_at).toLocaleDateString()}
      </p>
      <p className="text-gray-300">
        End Date: {new Date(poll.updated_at).toLocaleDateString()}
      </p>
    </div>
  );
};

export default PollCard;
