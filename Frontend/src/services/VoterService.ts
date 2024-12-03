const API_URL = "http://localhost:5166/api";

export interface Voter {
  	id: number;
	pollId: number;
	name: string;
  	email: string;
}

export const getAllVoters = async () => {
  const response = await fetch(`${API_URL}/voters`);
  if (!response.ok) {
    throw new Error("Failed to fetch voters");
  }
  return response.json();
};

export const getVotersByPollId = async (id: number): Promise<Voter> => {
  const response = await fetch(`${API_URL}/voters/poll/${id}`);
  if (!response.ok) {
	throw new Error("Failed to fetch voters");
  }
  return response.json();
}

export const getVoterById = async (id: number): Promise<Voter> => {
  const response = await fetch(`${API_URL}/voters/${id}`);
  if (!response.ok) {
	throw new Error("Failed to fetch voters");
  }
  return response.json();
};

export const createVoter = async (voter: Voter): Promise<Voter> => {
  const response = await fetch(`${API_URL}/voters`, {
	method: "POST",
	headers: {
	  "Content-Type": "application/json",
	},
	body: JSON.stringify(voter),
  });
  if (!response.ok) {
	throw new Error("Failed to create voters");
  }
  return response.json();
};

export const updateVoter = async (voter: Voter): Promise<Voter> => {
  const response = await fetch(`${API_URL}/voters/${voter.id}`, {
	method: "PUT",
	headers: {
	  "Content-Type": "application/json",
	},
	body: JSON.stringify(voter),
  });
  if (!response.ok) {
	throw new Error("Failed to update voters");
  }
  return response.json();
};

export const deleteVoter = async (id: number): Promise<void> => {
  const response = await fetch(`${API_URL}/voters/${id}`, {
	method: "DELETE",
  });
  if (!response.ok) {
	throw new Error("Failed to delete voter");
  }
};
