import { Navigate } from "react-router-dom";

interface LoginCredentials {
  username: string;
  password: string;
}

interface RegisterDetails {
  username: string;
  email: string;
  password: string;
  role: "Admin" | "User";
}

interface ApiResponse<T> {
  data: T;
  message?: string;
  error?: string;
}

const BASE_URL = "http://localhost:5166/api";

export const login = async (credentials: LoginCredentials): Promise<string> => {
  try {
    const response = await fetch(`${BASE_URL}/auth/login`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(credentials),
    });

    if (!response.ok) {
      const error = await response.json();
      throw new Error(error.message || "Failed to login");
    }

    const contentType = response.headers.get("Content-Type") || "";

    if (contentType.includes("application/json")) {
      const result = await response.json();
      return result.token;
    } else {
      const token = await response.text();
      return token;
    }
  } catch (error: unknown) {
    console.error("Login error:", error);
    if (error instanceof Error) {
      throw error;
    } else {
      throw new Error("An unknown error occurred during login");
    }
  }
};


export const register = async (userDetails: RegisterDetails) => {
  try {
    const response = await fetch(`${BASE_URL}/auth/register`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(userDetails),
    });

    if (!response.ok) {
      const contentType = response.headers.get("Content-Type") || "";

      // Handle JSON error responses
      if (contentType.includes("application/json")) {
        const error = await response.json();
        throw new Error(error.message || "Registration failed");
      } else {
        const errorText = await response.text();
        throw new Error(errorText || "Unknown error occurred during registration");
      }
    }

    // Assume successful response contains JSON
    return await response.json();
  } catch (error: unknown) {
    console.error("Registration error:", error);
    if (error instanceof Error) {
      throw error;
    } else {
      throw new Error("An unknown error occurred during registration");
    }
  }
};
