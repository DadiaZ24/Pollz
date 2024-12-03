import { jwtDecode } from "jwt-decode";

export const getUserIdFromToken = (): number | null => {
  const token = localStorage.getItem("auth_token");
  if (!token) {
    return null;
  }

  try {
    const decodedToken: { Id: number } = jwtDecode(token);
    return decodedToken.Id;
  } catch (error) {
    console.error("Error decoding token:", error);
    return null;
  }
};
