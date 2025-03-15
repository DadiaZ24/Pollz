import React, { useEffect, useState } from "react";
import { useTranslation } from "react-i18next";
import { Link } from "react-router-dom";
import logo from "../assets/logo.png";
import { getAllPolls } from "../services/PollsService";
import "../i18n";
import { getUserIdFromToken } from "../hooks/User";

const Header: React.FC = () => {
  useTranslation("common");
  const [isDropdownOpen, setIsDropdownOpen] = useState(false);
  const [user, setUser] = useState<{ id: number; name: string } | null>(null);
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
    const userId = getUserIdFromToken();
    if (userId !== null) {
      const fetchedUser = { id: userId, name: `User ${userId}` };
      setUser(fetchedUser);
    }
  }, []);

  const toggleDropdown = () => {
    setIsDropdownOpen(!isDropdownOpen);
  };

  const handleMouseEnter = () => {
    setIsDropdownOpen(true);
  };

  const handleMouseLeave = () => {
    setIsDropdownOpen(false);
  };

  return (
    <header className="bg-gradient-to-r from-blue-500 to-purple-600 text-white shadow-lg">
      <nav className="container mx-auto flex justify-left items-center p-4">
        <div className="flex items-center space-x-8 cursor-pointer">
          <img
            src={logo}
            alt="logo"
            className="h-12 w-auto ml-8"
            onClick={() => (window.location.href = "/")}
          />
        </div>
        <ul className="ml-11 hidden md:flex space-x-4 justify-left">
          <li className="w-full relative">
            <Link
              to="/"
              className="px-6 py-2 text-white bg-transparent rounded-md hover:bg-white hover:text-gray-800 transition duration-300 block text-center transform hover:scale-105"
            >
              Home
            </Link>
          </li>
          <li
            className="w-full h-full  relative"
            onMouseEnter={handleMouseEnter}
            onMouseLeave={handleMouseLeave}
          >
            <button
              onClick={toggleDropdown}
              className={`px-6 py-2 text-white bg-transparent transition duration-300 block text-center
                ${
                  isDropdownOpen
                    ? "bg-white text-gray-800 scale-105 shadow-lg"
                    : "hover:bg-white hover:text-gray-800"
                } 
                rounded-md transform ease-in-out transition-all`}
            >
              Polls
            </button>
          </li>
          <li className="w-full">
            <Link
              to="/polls/add-pole"
              className="px-6 py-2 text-white bg-transparent rounded-md hover:bg-white hover:text-gray-800 transition duration-300 block text-center transform hover:scale-105"
            >
              Create
            </Link>
          </li>
          <li className="w-full">
            <Link
              to="/about"
              className="px-6 py-2 text-white bg-transparent rounded-md hover:bg-white hover:text-gray-800 transition duration-300 block text-center transform hover:scale-105"
            >
              About
            </Link>
          </li>
          <li className="w-full">
            <Link
              to="/contact"
              className="px-6 py-2 text-white bg-transparent rounded-md hover:bg-white hover:text-gray-800 transition duration-300 block text-center transform hover:scale-105"
            >
              Contact
            </Link>
          </li>
        </ul>
        {user && (
          <div className="ml-auto text-white text-sm">
            <span>{user.name}</span>
            <span className="ml-2">({user.id})</span>
          </div>
        )}
      </nav>
    </header>
  );
};

export default Header;
