import { useState } from "react";
import { createPoll, Poll } from "../services/PollsService";
import { createQuestion, Question } from "../services/QuestionService";
import { Answer, createAnswer } from "../services/AnswerService";
import { getUserIdFromToken } from "./User";
import { createVoter, Voter } from "../services/VoterService";

export interface PollBuffer {
  poll: Poll;
  questions: {
    question: Question;
    answers: Answer[];
  }[];
  voters: Voter[];
}

export const useAddPoll = () => {
  const [step, setStep] = useState(1);
  const [pollBuffer, setPollBuffer] = useState<PollBuffer>({
    poll: {
      id: 0,
      userId: getUserIdFromToken() || 0,
      title: "",
      description: "",
      created_at: new Date(),
      updated_at: new Date(),
    },
    questions: [],
    voters: [],
  });
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const updatePollInfo = (field: "title" | "description", value: string) => {
    setPollBuffer((prevBuffer) => ({
      ...prevBuffer,
      poll: { ...prevBuffer.poll, [field]: value },
    }));
  };

  const addQuestion = (newQuestion: Question) => {
    setPollBuffer((prev) => ({
      ...prev,
      questions: [
        ...prev.questions,
        {
          question: newQuestion,
          answers: [],
        },
      ],
    }));
  };

  const updateQuestion = (questionId: number, newTitle: string) => {
    setPollBuffer((prev) => ({
      ...prev,
      questions: prev.questions.map((q) =>
        q.question.id === questionId
          ? { ...q, question: { ...q.question, title: newTitle } }
          : q
      ),
    }));
  };

  const removeQuestion = (questionId: number) => {
    setPollBuffer((prev) => ({
      ...prev,
      questions: prev.questions.filter((q) => q.question.id !== questionId),
    }));
  };

  const addAnswer = (questionId: number, newAnswer: Answer) => {
    setPollBuffer((prev) => ({
      ...prev,
      questions: prev.questions.map((q) =>
        q.question.id === questionId
          ? { ...q, answers: [...q.answers, newAnswer] }
          : q
      ),
    }));
  };

  const updateAnswer = (
    questionId: number,
    answerId: number,
    newText: string
  ) => {
    setPollBuffer((prev) => ({
      ...prev,
      questions: prev.questions.map((q) =>
        q.question.id === questionId
          ? {
              ...q,
              answers: q.answers.map((a) =>
                a.id === answerId ? { ...a, title: newText } : a
              ),
            }
          : q
      ),
    }));
  };

  const removeAnswer = (questionId: number, answerId: number) => {
    setPollBuffer((prev) => ({
      ...prev,
      questions: prev.questions.map((q) =>
        q.question.id === questionId
          ? {
              ...q,
              answers: q.answers.filter((a) => a.id !== answerId),
            }
          : q
      ),
    }));
  };

  const addVoter = (newVoter: Voter) => {
    setPollBuffer((prev) => ({
      ...prev,
      voters: [...prev.voters, newVoter],
    }));
  };

  const updateVoter = (voterId: number, newName: string, newEmail: string) => {
    setPollBuffer((prev) => ({
      ...prev,
      voters: prev.voters.map((v) =>
        v.id === voterId ? { ...v, name: newName, email: newEmail } : v
      ),
    }));
  };

  const removeVoter = (voterId: number) => {
    setPollBuffer((prev) => ({
      ...prev,
      voters: prev.voters.filter((v) => v.id !== voterId),
    }));
  };

  const goNext = () => {
    if (step < 4) {
      setStep(step + 1);
    }
  };

  const goBack = () => {
    if (step > 1) {
      setStep(step - 1);
    }
  };

  const submitPoll = async () => {
    try {
      setLoading(true);
      setError(null);

      if (!pollBuffer.poll.title || !pollBuffer.poll.description) {
        alert("Poll title and description are required.");
        return;
      }

      if (!pollBuffer.questions || pollBuffer.questions.length === 0) {
        alert("Poll has no questions.");
        return;
      }

      if (pollBuffer.questions.length === 0) {
        setError("Poll has no questions.");
        alert("Poll has no questions.");
        return;
      }

      for (const { question, answers } of pollBuffer.questions) {
        if (answers.length === 0) {
          setError(`Question "${question.title}" has no answers.`);
          alert(`Question "${question.title}" has no answers.`);
          return;
        }
      }

      for (const { question, answers } of pollBuffer.questions) {
        if (!question.title || !question.description) {
          setError("Every question must have a title and description.");
          alert("Every question must have a title and description.");
          return;
        }

        for (const answer of answers) {
          if (!answer.title) {
            setError("Every answer must have a title.");
            alert("Every answer must have a title.");
            return;
          }
        }
      }

      if (pollBuffer.voters.length === 0) {
        setError("Poll must have at least one voter.");
        alert("Poll must have at least one voter.");
        return;
      }

      const pollResponse = await createPoll(pollBuffer.poll);

      const createdQuestions = await Promise.all(
        pollBuffer.questions.map(async ({ question, answers }) => {
          const questionResponse = await createQuestion({
            ...question,
            pollId: pollResponse.id,
          });

          const createdAnswers = await Promise.all(
            answers.map((answer) =>
              createAnswer({ ...answer, questionId: questionResponse.id })
            )
          );

          return { ...questionResponse, answers: createdAnswers };
        })
      );

      const createdVoters = await Promise.all(
        pollBuffer.voters.map(async (voter) => {
          const voterResponse = await createVoter({
            ...voter,
            pollId: pollResponse.id,
          });

          return voterResponse;
        })
      );

      alert("Poll successfully created!");
      console.log("Poll successfully created:", {
        poll: pollResponse,
        questions: createdQuestions,
        voters: createdVoters,
      });

      setPollBuffer({
        poll: {
          id: 0,
          userId: 0,
          title: "",
          description: "",
          created_at: new Date(),
          updated_at: new Date(),
        },
        questions: [],
        voters: [],
      });
      setStep(1);
    } catch (err) {
      console.error("Error during poll creation:", err);
      setError("An error occurred while submitting the poll.");
    } finally {
      setLoading(false);
    }
  };

  return {
    step,
    pollBuffer,
    updatePollInfo,
    addQuestion,
    updateQuestion,
    removeQuestion,
    addAnswer,
    updateAnswer,
    removeAnswer,
    addVoter,
    updateVoter,
    removeVoter,
    goNext,
    goBack,
    submitPoll,
    loading,
    error,
  };
};
