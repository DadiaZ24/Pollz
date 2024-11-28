import React, { useState, useEffect } from "react";
import { getAllPolls, Poll } from "../services/PollsService";
import PollCard from "../components/cards/PollCard";
import Header from "../components/header";
import Footer from "../components/footer";

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
    return <p>Loading...</p>;
  }
  if (error) {
    return <p>{error}</p>;
  }
  return (
    <div className="bg-gray-700 min-h-screen flex flex-col">
      <Header />

      <main className="container mx-auto p-8 flex-grow">
        <h2 className="text-3xl font-bold text-center text-gray-200">
          🎬 Your Polls 🏆
        </h2>
        <p className="text-center text-gray-200 mt-2">
          Here are you current polls 🌟
        </p>

        {loading ? (
          <div className="text-center mt-10">
            <p className="text-lg text-gray-200">🔄 Loading categories...</p>
          </div>
        ) : (
          <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-8 mt-10">
            {polls.length === 0 ? (
              <p className="text-center text-gray-600">
                🚫 No polls available, try again later. 🙇
              </p>
            ) : (
              polls.map((poll) => <PollCard key={poll.id} poll={poll} />)
            )}
          </div>
        )}
      </main>
      <Footer />
    </div>
  );
};

export default PollsPage;
