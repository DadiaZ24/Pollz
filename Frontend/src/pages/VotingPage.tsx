import React, { useState, useEffect } from "react";
import { useParams } from "react-router-dom";
import { getQuestionsByPollId, Question } from "../services/QuestionService";
import { createVote } from "../services/VoteService";
import { getVoterByCode } from "../services/VoterService"; // Assuming you have a service for this
import { getAnswersByQuestionId, Answer } from "../services/AnswerService";
import Footer from "../components/footer";
import HeaderGuest from "../components/header_guest";

const VotingPage: React.FC = () => {
  const { voterId, uniqueCode } = useParams<{
    voterId: string;
    uniqueCode: string;
  }>();
  const [questions, setQuestions] = useState<Question[]>([]);
  const [answersByQuestion, setAnswersByQuestion] = useState<
    Record<number, Answer[]>
  >({});
  const [selectedAnswers, setSelectedAnswers] = useState<
    Record<number, number>
  >({});
  const [error, setError] = useState<string | null>(null);
  const [successMessage, setSuccessMessage] = useState<string | null>(null);
  const [, setVoter] = useState<unknown>(null);
  const [, setIsFormValid] = useState<boolean>(true);

  useEffect(() => {
    const fetchVotingData = async () => {
      try {
        setSelectedAnswers({});

        if (!uniqueCode) {
          throw new Error("Unique code is undefined");
        }
        const fetchedVoter = await getVoterByCode(uniqueCode);
        setVoter(fetchedVoter);

        const fetchedQuestions = await getQuestionsByPollId(
          fetchedVoter.pollId
        );
        setQuestions(fetchedQuestions);

        const fetchedAnswersByQuestion: Record<number, Answer[]> = {};
        for (const question of fetchedQuestions) {
          const answers = await getAnswersByQuestionId(question.id);
          fetchedAnswersByQuestion[question.id] = answers;
        }
        setAnswersByQuestion(fetchedAnswersByQuestion);
      } catch (err) {
        console.error("Error fetching voting data:", err);
        setError("Error fetching voting data.");
      }
    };

    if (uniqueCode) {
      fetchVotingData();
    }
  }, [voterId, uniqueCode]);

  const handleSubmitVote = async () => {
    if (!voterId) {
      setError("Please provide a voter ID.");
      return;
    }
    const isValid = Object.keys(selectedAnswers).length === questions.length;
    setIsFormValid(isValid);
    if (!isValid) {
      setError("Please answer all the questions before submitting your vote.");
      return;
    }
    try {
      const votePromises = questions.map(async (question) => {
        const answerId = selectedAnswers[question.id];
        if (answerId) {
          const vote = {
            answerId,
            questionId: question.id,
            voterId: parseInt(voterId),
          };
          await createVote(voterId, selectedAnswers, vote);
        }
      });
      await Promise.all(votePromises);
      setError(""); // Clear any existing error
      setSuccessMessage("Your vote has been successfully submitted!");
    } catch (err) {
      console.error("Error during vote submission:", err);
      setError("An error occurred while submitting your vote.");
    }
  };

  return (
    <div className="bg-gray-200 min-h-screen flex flex-col">
      <HeaderGuest />
      <main className="container mx-auto p-8 flex-grow">
        <h2 className="text-4xl font-bold text-center text-gray-800 mb-8">
          <h2>🎄✨ Poll Page ✨🎄</h2>
        </h2>
        {successMessage && (
          <div className="text-green-500 mb-4">{successMessage}</div>
        )}
        {error && <div className="text-red-500 mb-4">{error}</div>}
        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-8">
          {questions.map((question) => (
            <div
              key={question.id}
              className="bg-white p-6 rounded-lg shadow-md"
            >
              <h3 className="text-2xl font-semibold text-gray-800 mb-4">
                {question.title}
              </h3>
              {answersByQuestion[question.id]?.map((answer) => (
                <div
                  key={answer.id}
                  className="flex items-center space-x-4 p-4 bg-gray-100 rounded-lg shadow-md"
                >
                  <input
                    type="radio"
                    id={`answer-${answer.id}`}
                    name={`question-${question.id}`}
                    value={answer.id}
                    checked={selectedAnswers[question.id] === answer.id}
                    onChange={() =>
                      setSelectedAnswers((prev) => ({
                        ...prev,
                        [question.id]: answer.id,
                      }))
                    }
                  />
                  <label
                    htmlFor={`answer-${answer.id}`}
                    className="text-lg text-gray-700"
                  >
                    {answer.title}
                  </label>
                </div>
              ))}
            </div>
          ))}
        </div>
        <div className="mt-8 text-center">
          <button
            onClick={handleSubmitVote}
            className={`px-6 py-2 bg-blue-500 text-white rounded-lg hover:bg-blue-700
            }`}
          >
            Submit Your Vote
          </button>
        </div>
      </main>
      <Footer />
    </div>
  );
};

export default VotingPage;
