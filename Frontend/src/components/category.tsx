import React from "react";

interface CategoryCardProps {
  id: number;
  name: string;
  description: string;
  onViewNominees: (id: number) => void;
}

const CategoryCard: React.FC<CategoryCardProps> = ({
  id,
  name,
  description,
  onViewNominees,
}) => (
  <div className="bg-gray-900 shadow-md rounded-lg p-6 flex flex-col justify-between">
    <h3 className="text-xl font-semibold text-gray-100 mb-2">{name}</h3>
    <p className="text-gray-200 mb-4">{description}</p>
    <button
      className="bg-blue-600 text-white px-4 py-2 rounded-lg shadow-md hover:bg-blue-700 w-full"
      onClick={() => onViewNominees(id)}
    >
      View Nominees
    </button>
  </div>
);

export default CategoryCard;
