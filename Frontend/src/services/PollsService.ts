const API_URL = "http://localhost:5166/api";

export interface Poll {
  id: number;
  userId: number;
  title: string;
  description?: string;
  created_at: Date;
  updated_at: Date;
}

export const getAllPolls = async () => {
  const response = await fetch(`${API_URL}/polls`);
  if (!response.ok) {
    throw new Error("Failed to fetch polls");
  }
  return response.json();
};

export const getPollById = async (id: number): Promise<Poll> => {
  const response = await fetch(`${API_URL}/polls/${id}`);
  if (!response.ok) {
    throw new Error("Failed to fetch poll");
  }
  return response.json();
};

export const createPoll = async (poll: Poll): Promise<Poll> => {
  const response = await fetch(`${API_URL}/polls`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(poll),
  });
  if (!response.ok) {
    throw new Error("Failed to create poll");
  }
  return response.json();
};
