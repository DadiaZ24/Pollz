const API_URL = "http://localhost:5166/api";

export interface Answer {
  id: number;
  questionId: number;
  title: string;
}

export const getAllAnswers = async () => {
  const response = await fetch(`${API_URL}/answer`);
  if (!response.ok) {
	throw new Error("Failed to fetch answers");
  }
  return response.json();
};

export const getAnswersByQuestionId = async (id: number): Promise<Answer[]> => {
  const response = await fetch(`${API_URL}/answer/question/${id}`);
  if (!response.ok) {
	throw new Error("Failed to fetch answer");
  }
  return response.json();
}

export const getAnswerById = async (id: number): Promise<Answer> => {
  const response = await fetch(`${API_URL}/answer/${id}`);
  if (!response.ok) {
	throw new Error("Failed to fetch answer");
  }
  return response.json();
};

export const createAnswer = async (answer: Answer): Promise<Answer> => {
  const response = await fetch(`${API_URL}/answer`, {
	method: "POST",
	headers: {
	  "Content-Type": "application/json",
	},
	body: JSON.stringify(answer),
  });
  if (!response.ok) {
	throw new Error("Failed to create answer");
  }
  return response.json();
};

export const updateAnswer = async (answer: Answer): Promise<Answer> => {
	  const response = await fetch(`${API_URL}/answer/${answer.id}`, {
	method: "PUT",
	headers: {
	  "Content-Type": "application/json",
	},
	body: JSON.stringify(answer),
  });
  if (!response.ok) {
	throw new Error("Failed to update answer");
  }
  return response.json();
}

export const deleteAnswer = async (id: number): Promise<boolean> => {
  const response = await fetch(`${API_URL}/answer/${id}`, {
	method: "DELETE",
  });
  if (!response.ok) {
	throw new Error("Failed to delete answer");
  }
  return true;
};
