interface LoginCredentials {
  username: string;
  password: string;
}

interface RegisterDetails {
  username: string;
  email: string;
  password: string;
  role: "admin" | "user";
}

interface ApiResponse<T> {
  data: T;
  message?: string;
  error?: string;
}

const BASE_URL = "http://localhost:5166/api";

// Function to login and store the JWT token in localStorage
export const login = async (credentials: LoginCredentials): Promise<string> => {
  try {
    const response = await fetch(`${BASE_URL}/auth/login`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(credentials),
    });

    // Check if the response is not ok (error status)
    if (!response.ok) {
      const error = await response.json();
      throw new Error(error.message || "Failed to login");
    }

    const contentType = response.headers.get("Content-Type") || "";

    // If the response is JSON, get the token from it
    let token = "";
    if (contentType.includes("application/json")) {
      const result = await response.json();
      token = result.token; // Assuming the response has a 'token' property
    } else {
      // If the response is plain text
      token = await response.text();
    }

    // Store the token in localStorage for persistence
    localStorage.setItem("auth_token", token);

    // Return the token (optional)
    return token;
  } catch (error: unknown) {
    console.error("Login error:", error);
    if (error instanceof Error) {
      throw error; // rethrow error to be handled in component
    } else {
      throw new Error("An unknown error occurred during login");
    }
  }
};

// Function to register a new user
export const register = async (userDetails: RegisterDetails): Promise<ApiResponse<RegisterDetails>> => {
  try {
    const response = await fetch(`${BASE_URL}/auth/register`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(userDetails),
    });

    // Handle if registration fails
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

    // Assume successful response contains JSON data (like user info or confirmation)
    const data = await response.json();
    return { data };
  } catch (error: unknown) {
    console.error("Registration error:", error);
    if (error instanceof Error) {
      throw error;
    } else {
      throw new Error("An unknown error occurred during registration");
    }
  }
};

// Function to check if user is already logged in based on token in localStorage
export const isAuthenticated = (): boolean => {
  const token = localStorage.getItem("auth_token");
  return token !== null; // Return true if token exists, meaning user is logged in
};

// Function to logout the user
export const logout = (): void => {
  localStorage.removeItem("auth_token");
  // Optionally, you can redirect to login page after logging out
  window.location.href = "/login";
};
