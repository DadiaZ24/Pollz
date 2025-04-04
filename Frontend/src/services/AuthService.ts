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

const API_URL = "http://localhost:5166/api";

export const login = async (credentials: LoginCredentials): Promise<string> => {
  try {
    const response = await fetch(`${API_URL}/auth/login`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(credentials),
    });

    if (!response.ok) {
      const error = await response.json();
      throw new Error(error.message || "Failed to login");
    }

    const contentType = response.headers.get("Content-Type") || "";

    let token = "";
    if (contentType.includes("application/json")) {
      const result = await response.json();
      token = result.token;
    } else {
      token = await response.text();
    }

    localStorage.setItem("auth_token", token);

    return token;
  } catch (error: unknown) {
    console.error("Login error:", error);
    if (error instanceof Error) {
      throw error;
    } else {
      throw new Error("An unknown error occurred during login");
    }
  }
};

export const register = async (userDetails: RegisterDetails): Promise<ApiResponse<RegisterDetails>> => {
  try {
    const response = await fetch(`${API_URL}/auth/register`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(userDetails),
    });

    if (!response.ok) {
      const contentType = response.headers.get("Content-Type") || "";

      if (contentType.includes("application/json")) {
        const error = await response.json();
        throw new Error(error.message || "Registration failed");
      } else {
        const errorText = await response.text();
        throw new Error(errorText || "Unknown error occurred during registration");
      }
    }

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

export const isAuthenticated = (): boolean => {
  const token = localStorage.getItem("auth_token");
  return token !== null;
};

export const logout = (): void => {
  localStorage.removeItem("auth_token");
  window.location.href = "/login";
};
