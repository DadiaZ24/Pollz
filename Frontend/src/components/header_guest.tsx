import React, { useEffect, useState } from "react";
import { useTranslation } from "react-i18next";
import { Link } from "react-router-dom";
import logo from "../assets/logo.png"; // Ensure the path is correct
import { getAllPolls } from "../services/PollsService";
import "../i18n";
import { getUserIdFromToken } from "../hooks/User";

const HeaderGuest: React.FC = () => {
  useTranslation("common");
  const [, setUser] = useState<{ id: number; name: string } | null>(null);
  interface Poll {
    id: string;
    title: string;
  }

  const [, setPolls] = useState<Poll[]>([]);

  useEffect(() => {
    const fetchPolls = async () => {
      try {
        const pollsData = await getAllPolls();
        setPolls(pollsData);
      } catch (error) {
        console.error("Error fetching polls:", error);
      }
    };

    fetchPolls();
  }, []);

  useEffect(() => {
    // Fetch user data from the decoded JWT token
    const userId = getUserIdFromToken();
    if (userId !== null) {
      // Example logic to fetch user name based on userId (replace with actual user data)
      const fetchedUser = { id: userId, name: `User ${userId}` }; // Replace with actual API call to fetch user info
      setUser(fetchedUser);
    }
  }, []);

  return (
    <header className="bg-gradient-to-r from-blue-500 to-purple-600 text-white shadow-lg">
      <nav className="container mx-auto flex justify-left items-center p-4">
        {/* Logo Section */}
        <div className="flex items-center space-x-8 cursor-pointer">
          <img
            src={logo}
            alt="logo"
            className="h-12 w-auto ml-8"
            onClick={() => (window.location.href = "/")}
          />
        </div>

        {/* Nav Links */}
        <ul className="ml-11 hidden md:flex space-x-4 justify-left">
          <li className="w-full relative">
            <Link
              to="/"
              className="px-6 py-2 text-white bg-transparent rounded-md hover:bg-white hover:text-gray-800 transition duration-300 block text-center transform hover:scale-105"
            >
              Home
            </Link>
          </li>
        </ul>
      </nav>
    </header>
  );
};

export default HeaderGuest;
