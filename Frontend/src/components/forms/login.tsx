// src/components/LoginForm.tsx

import React, { useState, ChangeEvent, FormEvent } from "react";
import { login } from "../../services/AuthService"; // Ensure getVoterByCode is imported
import { getVoterByCode } from "../../services/VoterService"; // Import getVoterByCode
import { useNavigate } from "react-router-dom";

interface LoginFormState {
  username: string;
  password: string;
  uniquecode: string; // Add uniquecode to the form state
}

const LoginForm: React.FC = () => {
  const [form, setForm] = useState<LoginFormState>({
    username: "",
    password: "",
    uniquecode: "", // Initialize uniquecode in the state
  });
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate(); // Use the useNavigate hook

  const handleInputChange = (e: ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setForm((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async (e: FormEvent) => {
    e.preventDefault();

    if (form.uniquecode) {
      // If the user enters a uniquecode, use it for authentication
      try {
        const voter = await getVoterByCode(form.uniquecode); // Validate the uniquecode
        if (voter) {
          // Redirect to the voting page with voterId and uniquecode in the URL
          navigate(`/voting/${voter.id}/${form.uniquecode}`);
        } else {
          setError("Invalid code. Please try again.");
        }
      } catch (err: unknown) {
        if (err instanceof Error) {
          setError(err.message);
        } else {
          setError("An unknown error occurred");
        }
      }
    } else {
      // If no uniquecode, proceed with traditional username/password login
      try {
        const token = await login(form); // Log in using username and password
        localStorage.setItem("authToken", token);
        navigate("/polls"); // Redirect to the polls page after successful login
      } catch (err: unknown) {
        if (err instanceof Error) {
          setError(err.message);
        } else {
          setError("An unknown error occurred");
        }
      }
    }
  };

  const handleRegisterRedirect = () => {
    navigate("/register"); // Redirect to the register page
  };

  return (
    <div className="bg-gray-800 p-8 shadow-md rounded-lg max-w-md mx-auto">
      <h2 className="text-gray-100 text-2xl font-bold mb-4">Login</h2>
      {error && <p className="text-red-500 mb-4">{error}</p>}
      <form onSubmit={handleSubmit}>
        {/* Username & Password Fields (only visible if uniquecode is empty) */}
        {!form.uniquecode && (
          <>
            <div className="mb-4">
              <label className="block text-gray-300 mb-2">Username</label>
              <input
                type="text"
                name="username"
                value={form.username}
                onChange={handleInputChange}
                className="w-full p-2 border rounded-lg"
                required
              />
            </div>
            <div className="mb-4">
              <label className="block text-gray-300 mb-2">Password</label>
              <input
                type="password"
                name="password"
                value={form.password}
                onChange={handleInputChange}
                className="w-full p-2 border rounded-lg"
                required
              />
            </div>
          </>
        )}

        {/* Unique Code Field (visible if uniquecode is entered or empty) */}
        <div className="mb-4">
          <label className="block text-gray-300 mb-2">Unique Code</label>
          <input
            type="text"
            name="uniquecode"
            value={form.uniquecode}
            onChange={handleInputChange}
            className="w-full p-2 border rounded-lg"
            placeholder="Enter unique code or login with username/password"
          />
        </div>

        <button
          type="submit"
          className="w-full bg-blue-500 text-white py-2 rounded-lg hover:bg-blue-600"
        >
          Login
        </button>

        <button
          type="button"
          className="w-full bg-blue-800 text-white py-2 rounded-lg hover:bg-blue-900 mt-3"
          onClick={handleRegisterRedirect}
        >
          Register
        </button>
      </form>
    </div>
  );
};

export default LoginForm;
