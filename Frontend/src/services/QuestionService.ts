const API_URL = "http://localhost:5166/api";
export interface Question {
  id: number;
	pollId: number;
	title: string;
  description?: string;
}

export const getAllQuestions = async () => {
  const response = await fetch(`${API_URL}/question`);
  if (!response.ok) {
    throw new Error("Failed to fetch questions");
  }
  return response.json();
};

export const getQuestionsByPollId = async (id: number): Promise<Question[]> => {
  const response = await fetch(`${API_URL}/question/poll/${id}`);
  if (!response.ok) {
	throw new Error("Failed to fetch question");
  }
  return response.json();
}

export const getQuestionById = async (id: number): Promise<Question> => {
  const response = await fetch(`${API_URL}/question/${id}`);
  if (!response.ok) {
	throw new Error("Failed to fetch question");
  }
  return response.json();
};

export const createQuestion = async (question: Question): Promise<Question> => {
  const response = await fetch(`${API_URL}/question`, {
	method: "POST",
	headers: {
	  "Content-Type": "application/json",
	},
	body: JSON.stringify(question),
  });
  if (!response.ok) {
	throw new Error("Failed to create question");
  }
  return response.json();
};

export const updateQuestion = async (question: Question): Promise<Question> => {
  const response = await fetch(`${API_URL}/question/${question.id}`, {
	method: "PUT",
	headers: {
	  "Content-Type": "application/json",
	},
	body: JSON.stringify(question),
  });
  if (!response.ok) {
	throw new Error("Failed to update question");
  }
  return response.json();
};

export const deleteQuestion = async (id: number): Promise<void> => {
  const response = await fetch(`${API_URL}/question/${id}`, {
	method: "DELETE",
  });
  if (!response.ok) {
	throw new Error("Failed to delete question");
  }
};
