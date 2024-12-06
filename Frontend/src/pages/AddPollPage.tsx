import React from "react";
import Header from "../components/header";
import Footer from "../components/footer";
import { useAddPoll } from "../hooks/AddPoll";
import QuestionCard from "../components/cards/QuestionCard";
import AddQuestionCard from "../components/cards/AddQuestionCard";
import VoterCard from "../components/cards/VoterCard";

const AddPollPage: React.FC = () => {
  const [showAddQuestionCard, setShowAddQuestionCard] = React.useState(false);
  const {
    step,
    pollBuffer,
    addQuestion,
    updatePollInfo,
    updateQuestion,
    removeQuestion,
    addAnswer,
    updateAnswer,
    removeAnswer,
    addVoter,
    removeVoter,
    updateVoter,
    goNext,
    goBack,
    submitPoll,
  } = useAddPoll();

  return (
    <div className="bg-gray-200 min-h-screen flex flex-col">
      <Header />
      <main className="container mx-auto p-8 flex-grow">
        <h2 className="text-4xl font-bold text-center text-gray-800 mb-4">
          {step === 1
            ? "Create Poll"
            : step === 2
            ? "Insert Questions"
            : step === 3
            ? "Create Voters"
            : "Finish"}
        </h2>
        {/* Step 1: Create Poll */}
        {step === 1 && (
          <div className="bg-white shadow-lg rounded-lg p-6">
            <label className="block mb-2">Title</label>
            <input
              type="text"
              name="title"
              required
              value={pollBuffer.poll.title}
              onChange={(e) => updatePollInfo("title", e.target.value)}
              className="w-full p-2 mb-4 border border-gray-300 rounded-lg"
            />
            <label className="block mb-2">Description</label>
            <input
              type="text"
              name="description"
              value={pollBuffer.poll.description}
              onChange={(e) => updatePollInfo("description", e.target.value)}
              className="w-full p-2 mb-4 border border-gray-300 rounded-lg"
            />
          </div>
        )}
        {/* Step 2: Insert Questions */}
        {step === 2 && (
          <div className="bg-white shadow-lg rounded-lg p-6">
            <h3 className="text-lg font-semibold mb-4"></h3>
            <div className="grid grid-cols-3 gap-4">
              {/* Show "Add Question" button when there are no questions */}
              {pollBuffer.questions.length === 0 && !showAddQuestionCard && (
                <button
                  onClick={() => setShowAddQuestionCard(true)}
                  className="bg-blue-500 text-white rounded-lg mx-auto py-2 px-6"
                >
                  <div className="mr-2">+</div> Add Question
                </button>
              )}

              {/* Show AddQuestionCard directly if no questions */}
              {pollBuffer.questions.length === 0 && showAddQuestionCard && (
                <AddQuestionCard
                  pollBuffer={pollBuffer}
                  addQuestion={addQuestion}
                />
              )}

              {/* Render questions if they exist */}
              {pollBuffer.questions.length > 0 &&
                pollBuffer.questions.map(({ question, answers }) => (
                  <div key={question.id} className="mb-4">
                    {/* Pass question details and related answers to QuestionCard */}
                    <QuestionCard
                      question={question}
                      answers={answers} // Pass answers directly
                      updateQuestion={updateQuestion}
                      removeQuestion={removeQuestion}
                      updateAnswer={updateAnswer}
                      addAnswer={addAnswer}
                      removeAnswer={removeAnswer}
                    />
                  </div>
                ))}

              {/* Show "Add Question" button when there are questions */}
              {pollBuffer.questions.length > 0 && !showAddQuestionCard && (
                <button
                  onClick={() => setShowAddQuestionCard(true)}
                  className="bg-blue-500 text-white py-2 px-6 rounded-lg flex items-center"
                >
                  <div className="mr-2">+</div> Add Question
                </button>
              )}

              {/* Show AddQuestionCard when toggled and questions exist */}
              {showAddQuestionCard && pollBuffer.questions.length > 0 && (
                <AddQuestionCard
                  pollBuffer={pollBuffer}
                  addQuestion={addQuestion}
                />
              )}
            </div>
          </div>
        )}

        {/* Step 3: Create Voters */}
        {step === 3 && (
          <div className="bg-white shadow-lg rounded-lg p-6">
            <h3 className="text-lg font-semibold mb-4">Create Voters</h3>
            {/* Voter Card */}
            <VoterCard
              voters={pollBuffer.voters} // Pass current voters list
              updateVoter={(id, newName, newEmail) => {
                updateVoter(id, newName, newEmail); // Trigger update
              }}
              removeVoter={(id) => {
                removeVoter(id); // Trigger remove
              }}
            />

            {/* Add Voter Button */}
            <button
              onClick={() => {
                addVoter({
                  id: Math.floor(Math.random() * 10000) + 1, // Use timestamp for unique ID
                  pollId: pollBuffer.poll.id,
                  name: "",
                  email: "",
                  code: "",
                  used: false,
                });
              }}
              className="mt-4 bg-blue-500 text-white py-2 px-4 rounded-lg hover:bg-blue-600"
            >
              + Add Voter
            </button>
          </div>
        )}

        {/* Step 4: Finish */}
        {step === 4 && (
          <div className="bg-white shadow-lg rounded-lg p-6 text-center">
            <p>Your poll has been successfully created!</p>
            <button
              onClick={submitPoll} // Call the function that creates the poll
              className="mt-4 bg-blue-500 text-white py-2 px-6 rounded-lg"
            >
              Go to Polls
            </button>
          </div>
        )}
        {/* Navigation Buttons */}
        <div className="flex justify-between mt-6">
          <button
            onClick={goBack}
            disabled={step === 1}
            className="bg-gray-300 py-2 px-4 rounded-lg"
          >
            Back
          </button>
          <button
            onClick={goNext}
            disabled={step === 4}
            className="bg-blue-500 text-white py-2 px-4 rounded-lg"
          >
            Next
          </button>
        </div>
      </main>
      <Footer />
    </div>
  );
};

export default AddPollPage;
