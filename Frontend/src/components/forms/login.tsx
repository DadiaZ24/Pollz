// src/components/LoginForm.tsx

import React, { useState, ChangeEvent, FormEvent } from "react";
import { login } from "../../services/AuthService";
import { Navigate, useNavigate } from "react-router-dom";

interface LoginFormState {
  username: string;
  password: string;
}

const LoginForm: React.FC = () => {
  const [form, setForm] = useState<LoginFormState>({
    username: "",
    password: "",
  });
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate(); // Use the useNavigate hook

  const handleInputChange = (e: ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setForm((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async (e: FormEvent) => {
    e.preventDefault();
    try {
      const token = await login(form);
      navigate("/categories");

      localStorage.setItem("authToken", token);
    } catch (err: unknown) {
      if (err instanceof Error) {
        setError(err.message);
      } else {
        setError("An unknown error occurred");
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
