import React from "react";
import addIcon from "../../assets/add.png";

interface AddCardProps {
  onClick: () => void;
}

const AddCard: React.FC<AddCardProps> = ({ onClick }) => {
  return (
    <div
      onClick={onClick}
      className="h-full bg-transparent shadow-md rounded-lg p-6 flex justify-center items-center cursor-pointer"
    >
      <img src={addIcon} alt="Add" className="w-6 h-6 opacity-50" />
    </div>
  );
};

export default AddCard;
