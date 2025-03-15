import React, { useState, useEffect } from "react";
import { useParams } from "react-router-dom";
import Header from "../components/header";
import Footer from "../components/footer";
import { getResultsByQuestionId, Result } from "../services/VoteService";
import { getQuestionsByPollId, Question } from "../services/QuestionService";
import { getPollById } from "../services/PollsService";
import { getVotersByPollId, Voter } from "../services/VoterService";
import { getAnswersByQuestionId, Answer } from "../services/AnswerService";

const ResultsPage: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const [, setPoll] = useState<unknown>(null);
  const [questions, setQuestions] = useState<Question[]>([]);
  const [results, setResults] = useState<Record<number, Result[]>>({});
  const [answers, setAnswers] = useState<Record<number, Answer[]>>({});
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const [, setVoters] = useState<Voter[]>([]);

  useEffect(() => {
    const fetchPollData = async () => {
      setLoading(true);
      try {
        const pollData = await getPollById(Number(id));
        setPoll(pollData);

        const fetchedQuestions = await getQuestionsByPollId(Number(id));
        setQuestions(fetchedQuestions);

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

  useEffect(() => {
    const fetchResults = async () => {
      if (!questions || questions.length === 0) {
        console.log("No questions to fetch results for.");
        return;
      }

      try {
        const fetchedResults: Record<number, Result[]> = {};
        const fetchedAnswers: Record<number, Answer[]> = {};

        for (const question of questions) {
          const resultsForQuestion = await getResultsByQuestionId(question.id);
          const answersForQuestion = await getAnswersByQuestionId(question.id);

          fetchedAnswers[question.id] = answersForQuestion;

          if (resultsForQuestion && resultsForQuestion.length > 0) {
            fetchedResults[question.id] = resultsForQuestion;
          } else {
            const emptyResults = answersForQuestion.map((answer) => ({
              answerId: answer.id,
              answer: answer.title,
              voteCount: 0,
            }));

            fetchedResults[question.id] = emptyResults;
          }
        }

        setResults(fetchedResults);
        setAnswers(fetchedAnswers);
      } catch (err) {
        if (err instanceof Error) {
          console.error("Error fetching results:", err.message);
          setError(err.message);
        } else {
          console.error("Unknown error occurred while fetching results");
          setError("An unknown error occurred");
        }
      } finally {
        setLoading(false);
      }
    };

    if (questions.length > 0) {
      fetchResults();
    }
  }, [questions]);

  if (loading) {
    return (
      <div className="bg-gray-200 min-h-screen flex flex-col justify-center items-center">
        <Header />
        <div className="flex flex-col justify-center items-center">
          <div className="w-16 h-16 border-t-4 border-blue-500 border-solid rounded-full animate-spin"></div>
          <p className="mt-4 text-lg text-gray-800">Loading results...</p>
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
        <h2 className="text-4xl font-bold text-center text-gray-800 mb-8">
          Poll Results 📊
        </h2>
        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-8">
          {/* Render questions in a grid */}
          {questions.map((question) => (
            <div
              key={question.id}
              className="bg-white p-6 rounded-lg shadow-md"
            >
              <h3 className="text-2xl font-semibold text-gray-800 mb-4">
                {question.title}
              </h3>
              {results[question.id] && results[question.id].length > 0 ? (
                <div className="space-y-4">
                  {results[question.id].map((answer) => {
                    const totalVotes = results[question.id].reduce(
                      (sum, ans) => sum + (ans.voteCount || 0),
                      0
                    );
                    const percentage =
                      totalVotes > 0
                        ? Math.round((answer.voteCount / totalVotes) * 100)
                        : 0;

                    return (
                      <div
                        key={answer.answerId}
                        className="p-4 bg-gray-100 rounded-lg shadow-md"
                      >
                        <h4 className="text-lg font-semibold text-gray-700">
                          {answer.answer}
                        </h4>
                        <div className="relative w-full h-4 bg-gray-200 rounded mt-2">
                          <div
                            className="absolute top-0 left-0 h-4 bg-blue-500 rounded"
                            style={{ width: `${percentage}%` }}
                          ></div>
                        </div>
                        <p className="text-sm text-gray-500 mt-2">
                          {`${answer.voteCount || 0} votes (${percentage}%)`}
                        </p>
                      </div>
                    );
                  })}
                </div>
              ) : (
                <div className="space-y-4">
                  {/* No votes for this question */}
                  {answers[question.id]?.map((answer) => (
                    <div
                      key={answer.id}
                      className="p-4 bg-gray-100 rounded-lg shadow-md"
                    >
                      <h4 className="text-lg font-semibold text-gray-700">
                        {answer.title}
                      </h4>
                      <div className="relative w-full h-4 bg-gray-200 rounded mt-2">
                        <div
                          className="absolute top-0 left-0 h-4 bg-blue-500 rounded"
                          style={{ width: "0%" }}
                        ></div>
                      </div>
                      <p className="text-sm text-gray-500 mt-2">0 votes (0%)</p>
                    </div>
                  ))}
                </div>
              )}
            </div>
          ))}
        </div>
      </main>
      <Footer />
    </div>
  );
};

export default ResultsPage;
