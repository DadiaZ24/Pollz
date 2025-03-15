import { Answer } from "../../services/AnswerService";
import { Question } from "../../services/QuestionService";

interface QuestionCardProps {
  question: Question;
  answers: Answer[];
  updateQuestion: (id: number, newTitle: string) => void;
  removeQuestion: (id: number) => void;
  updateAnswer: (questionId: number, answerId: number, newText: string) => void;
  addAnswer: (questionId: number, newAnswer: Answer) => void;
  removeAnswer: (questionId: number, answerId: number) => void;
}

const QuestionCard: React.FC<QuestionCardProps> = ({
  question,
  answers,
  removeQuestion,
  updateAnswer,
  addAnswer,
  removeAnswer,
}) => {
  const handleAnswerUpdate = (answerId: number, newText: string) => {
    updateAnswer(question.id, answerId, newText);
  };

  const handleAnswerAdd = () => {
    const newAnswer: Answer = {
      id: Math.floor(Math.random() * 100000) + 1,
      questionId: question.id,
      title: "",
    };
    addAnswer(question.id, newAnswer);
  };

  const handleAnswerRemove = (answerId: number) => {
    removeAnswer(question.id, answerId);
  };

  return (
    <div className="h-full bg-gradient-to-r from-blue-600 to-purple-500 shadow-md rounded-lg p-6 flex flex-col">
      <h3 className="text-gray-200 text-3xl block mb-2 font-bold">
        {question.title}
      </h3>
      <p className="text-gray-200 block mb-2 text-l">{question.description}</p>
      <div className="mt-4">
        <h5 className="text-gray-200 text-3xl block mb-2 font-bold">Answers</h5>
        {answers.map((answer) => (
          <div key={answer.id} className="flex items-center mb-2">
            <input
              type="text"
              value={answer.title}
              onChange={(e) => handleAnswerUpdate(answer.id, e.target.value)}
              className="flex-1 border p-2 rounded mr-2"
            />
            <button
              onClick={() => handleAnswerRemove(answer.id)}
              className=" bg-red-500 text-white px-4 py-2 rounded-md hover:bg-red-600"
            >
              Remove
            </button>
          </div>
        ))}
        <button
          onClick={handleAnswerAdd}
          className="mt-4 bg-green-600 text-white px-4 py-2 rounded-md hover:bg-green-700"
        >
          Add Answer
        </button>
      </div>
      <button
        onClick={() => removeQuestion(question.id)}
        className="mt-4 bg-red-500 text-white px-4 py-2 rounded-md hover:bg-red-600"
      >
        Remove Question
      </button>
    </div>
  );
};

export default QuestionCard;
