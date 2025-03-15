export interface Result {
	answerId: number;
	answer: string;
	voteCount: number;
}

export interface Vote {
	questionId: number;
	answerId: number;
	voterId: number;
}

interface VoteRequestDto {
	answerId: number;
	questionId: number;
	voterId: number;
  }

const API_URL = "http://localhost:5166/api";
  
export const getResultsByQuestionId = async (questionId: number): Promise<Result[]> => {
	console.log("Fetching results for question ID:", questionId); 
	const response = await fetch(`${API_URL}/vote/${questionId}`);
	
	if (!response.ok) {
	  console.error("API call failed with status:", response.status); 
	  throw new Error("No results available");
	}
  
	const data = await response.json();
	console.log("Fetched results:", data); 
	return data;
  };
  
export const getResultsByPollId = async (pollId: number): Promise<Result[]> => {
	console.log("Fetching results for poll ID:", pollId); 
  
	const response = await fetch(`${API_URL}/vote/poll/${pollId}`);
	
	if (!response.ok) {
		console.error("API call failed with status:", response.status); 
		throw new Error("No results available");
	}
  
	const data = await response.json();
	console.log("Fetched results:", data); 
	return data;
};

export const createVote = async (
	voterId: string | undefined,
	answers: Record<number, number>, 
	vote: Vote
  ): Promise<Vote> => {
	if (!voterId || !answers || Object.keys(answers).length === 0) {
	  throw new Error("Invalid data. Please ensure voterId and answers are provided.");
	}
  
	const voteRequestDto: VoteRequestDto = {
	  answerId: answers[vote.questionId], 
	  questionId: vote.questionId,        
	  voterId: parseInt(voterId),          
	};
  
	console.log("Sending vote data to backend:", JSON.stringify(voteRequestDto));
  
	try {
	  const response = await fetch(`${API_URL}/vote`, {
		method: "POST",
		headers: {
		  "Content-Type": "application/json",
		},
		body: JSON.stringify(voteRequestDto), 
	  });
  
	  console.log("Response Status:", response.status);
	  const responseText = await response.text(); 
  
	  if (!response.ok) {
		throw new Error(`Failed to create vote. Server responded with status: ${response.status}. Message: ${responseText}`);
	  }
  
	  console.log("Vote successfully created or updated!");
  
	  return {
		answerId: voteRequestDto.answerId,
		questionId: voteRequestDto.questionId,
		voterId: voteRequestDto.voterId,
	  };
	} catch (error) {
	  console.error("Error while submitting vote:", error);
	  throw new Error("An error occurred while submitting your vote.");
	}
  };
  