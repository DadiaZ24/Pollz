import React, { useState, useEffect } from "react";
import { getAllPolls, Poll } from "../services/PollsService";
import PollCard from "../components/cards/PollCard";
import Header from "../components/header";
import Footer from "../components/footer";
import { Link } from "react-router-dom";
import AddCard from "../components/cards/AddCard";

const PollsPage: React.FC = () => {
  const [polls, setPolls] = useState<Poll[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    const fetchPolls = async () => {
      try {
        const data = await getAllPolls();
        setPolls(data);
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
    fetchPolls();
  }, []);

  if (loading) {
    return (
      <div className="bg-gray-200 min-h-screen flex flex-col justify-center items-center">
        <Header />
        <div className="flex flex-col justify-center items-center">
          <div className="w-16 h-16 border-t-4 border-blue-500 border-solid rounded-full animate-spin"></div>
          <p className="mt-4 text-lg text-gray-800">Loading your polls...</p>
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
        <h2 className="text-4xl font-bold text-center text-gray-800 mb-4">
          🎬 Your Polls 🏆
        </h2>
        <p className="text-xl text-center text-gray-600 mb-8">
          Here are your current polls. 🌟 Vote and make your voice heard!
        </p>

        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-8">
          {polls.length === 0 ? (
            <p className="col-span-full text-center text-gray-600">
              🚫 No polls available, please try again later. 🙇
            </p>
          ) : (
            polls.map((poll) => (
              <React.Fragment key={poll.id}>
                <Link
                  to={`/polls/${poll.id}`}
                  className="text-center block transform transition-all duration-300 ease-in-out hover:scale-105 hover:translate-y-2"
                >
                  <PollCard poll={poll} />
                </Link>
              </React.Fragment>
            ))
          )}
          <Link
            to={`/polls/add-pole`}
            className="block transform transition-all duration-300 ease-in-out hover:scale-105 hover:translate-y-2"
          >
            <AddCard onClick={() => console.log(`Add card clicked`)} />
          </Link>
        </div>
      </main>
      <Footer />
    </div>
  );
};

export default PollsPage;
