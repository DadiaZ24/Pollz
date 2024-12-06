import React, { useState, useEffect } from "react";
import { useParams, Link } from "react-router-dom";
import { getPollById, Poll } from "../services/PollsService";

import { getQuestionsByPollId, Question } from "../services/QuestionService";
import Header from "../components/header";
import Footer from "../components/footer";
import { getVotersByPollId, Voter } from "../services/VoterService";

const PollInfoPage: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const [, setPoll] = useState<Poll | null>(null);
  const [questions, setQuestions] = useState<Question[]>([]);
  const [voters, setVoters] = useState<Voter[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    const fetchPollData = async () => {
      try {
        const pollData = await getPollById(Number(id));
        setPoll(pollData);

        // Fetch questions for the poll
        const fetchedQuestions = await getQuestionsByPollId(Number(id));
        setQuestions(fetchedQuestions);

        // Fetch answers for each question
        const fetchedVoters = await getVotersByPollId(Number(id));
        setVoters(fetchedVoters);
      } catch (err: unknown) {
        if (err instanceof Error) {
          setError(err.message);
        } else {
          setError("An unknown error occurred");
        }
      } finally {
        setLoading(false);
      }
    };
    fetchPollData();
  }, [id]);

  if (loading) {
    return (
      <div className="bg-gray-200 min-h-screen flex flex-col justify-center items-center">
        <Header />
        <div className="flex flex-col justify-center items-center">
          <div className="w-16 h-16 border-t-4 border-blue-500 border-solid rounded-full animate-spin"></div>
          <p className="mt-4 text-lg text-gray-800">Loading poll info...</p>
        </div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="bg-gray-200 min-h-screen flex flex-col justify-center items-center">
        <Header />
        <div className="text-center">
          <p className="text-xl text-red-500 font-semibold">{error}</p>
        </div>
      </div>
    );
  }

  return (
    <div className="bg-gray-200 min-h-screen flex flex-col">
      <Header />
      <main className="container mx-auto p-8 flex-grow">
        <nav className="flex justify-center space-x-4 mb-8">
          <Link
            to={`/polls/${id}`}
            className="text-lg font-medium text-blue-500 hover:underline"
          >
            Info
          </Link>
          <Link
            to={`/polls/${id}/results`}
            className="text-lg font-medium text-blue-500 hover:underline"
          >
            Results
          </Link>
        </nav>

        <h2 className="text-4xl font-bold text-center text-gray-800 mb-8">
          Poll Details 📝
        </h2>

        <div className="grid grid-cols-1 md:grid-cols-2 gap-8">
          {/* Questions Section */}
          <div className="bg-white p-6 rounded-lg shadow-md">
            <h3 className="text-2xl font-semibold text-gray-800 mb-4">
              Questions
            </h3>
            {questions.length ? (
              <ul className="space-y-4">
                {questions.map((question) => (
                  <li
                    key={question.id}
                    className="text-lg text-gray-600 bg-gray-100 p-4 rounded"
                  >
                    {question.title}
                  </li>
                ))}
              </ul>
            ) : (
              <p className="text-gray-600">No questions available.</p>
            )}
          </div>

          {/* Voters Section */}
          <div className="bg-white p-6 rounded-lg shadow-md">
            <h3 className="text-2xl font-semibold text-gray-800 mb-4 text-center">
              Voters
            </h3>
            {voters.length ? (
              <div>
                <div className="grid grid-cols-3 bg-gray-200 p-6 font-semibold">
                  <span>Name</span>
                  <span>Code</span>
                  <span>Used</span>
                </div>
                <ul className="space-y-4">
                  {voters.map((voter, index) => (
                    <div
                      className="bg-gray-100 p-6 grid grid-cols-3"
                      key={index}
                    >
                      <span>{voter.name}</span>
                      <span>{voter.code}</span>
                      <span>{voter.used ? "✔️" : "❌"}</span>
                    </div>
                  ))}
                </ul>
              </div>
            ) : (
              <p className="text-gray-600">No voters yet.</p>
            )}
          </div>
        </div>
      </main>
      <Footer />
    </div>
  );
};

export default PollInfoPage;
