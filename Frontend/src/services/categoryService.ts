import axios from "axios";

const API_URL = "http://localhost:5166/api/";

export const getCategory = async () => {
  let response;
  try {
    response = await axios.get(`${API_URL}category`);
    return response.data;
  } catch (error) {
    console.error("Failed to fetch categories:", error);
    throw error;
  }
};
