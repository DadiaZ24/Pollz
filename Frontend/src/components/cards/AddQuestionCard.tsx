import React, { useState } from "react";
import { PollBuffer } from "../../hooks/AddPoll";
import { Question } from "../../services/QuestionService";

type AddQuestionCardProps = {
  pollBuffer: PollBuffer;
  addQuestion: (newQuestion: Question) => void;
};

const AddQuestionCard: React.FC<AddQuestionCardProps> = ({
  pollBuffer,
  addQuestion,
}) => {
  const [newQuestionTitle, setNewQuestionTitle] = useState("");
  const [newQuestionDescription, setNewQuestionDescription] = useState("");

  const handleSaveQuestion = () => {
    if (newQuestionTitle.trim() && newQuestionDescription.trim()) {
      const newQuestion: Question = {
        id: Math.floor(Math.random() * 100000) + 1,
        pollId: pollBuffer.poll.id,
        title: newQuestionTitle,
        description: newQuestionDescription,
      };

      addQuestion(newQuestion);
      setNewQuestionTitle("");
      setNewQuestionDescription("");
    }
  };

  return (
    <div className="h-full bg-gradient-to-r from-blue-600 to-purple-500 shadow-md rounded-lg p-6 flex flex-col">
      <div className="mb-4">
        <label className="text-gray-200 block mb-2">Title:</label>
        <input
          type="text"
          value={newQuestionTitle}
          onChange={(e) => setNewQuestionTitle(e.target.value)}
          className="w-full p-2 rounded-md border border-gray-300"
          placeholder="Enter question title"
        />
      </div>
      <div className="mb-4">
        <label className="text-gray-200 block mb-2">Description:</label>
        <input
          type="text"
          value={newQuestionDescription}
          onChange={(e) => setNewQuestionDescription(e.target.value)}
          className="w-full p-2 rounded-md border border-gray-300"
          placeholder="Enter question description"
        />
      </div>
      <button
        onClick={handleSaveQuestion}
        className="mt-4 bg-blue-500 text-white px-4 py-2 rounded-md hover:bg-blue-600"
      >
        Save Question
      </button>
    </div>
  );
};

export default AddQuestionCard;
