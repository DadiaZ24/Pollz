import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { getCategory } from "../services/categoryService";
import Header from "../components/header";
import Footer from "../components/footer";
import CategoryCard from "../components/category";

interface Category {
  id: number;
  name: string;
  description: string;
}

const CategoriesPage: React.FC = () => {
  const [categories, setCategories] = useState<Category[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchCategories = async () => {
      try {
        const data = await getCategory();
        setCategories(data);
      } catch (error) {
        console.error("Failed to fetch categories:", error);
      } finally {
        setLoading(false);
      }
    };

    fetchCategories();
  }, []);

  const handleViewNominees = (categoryId: number) => {
    navigate(`/categories/${categoryId}`);
  };

  return (
    <div className="bg-gray-700 min-h-screen">
      <Header />

      <main className="container mx-auto p-8">
        <h2 className="text-3xl font-bold text-center text-gray-200">
          🎬 Oscars Categories 🏆
        </h2>
        <p className="text-center text-gray-200 mt-2">
          Choose your favorite nominee from each category below. 🌟
        </p>

        {loading ? (
          <div className="text-center mt-10">
            <p className="text-lg text-gray-200">🔄 Loading categories...</p>
          </div>
        ) : (
          <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-8 mt-10">
            {categories.length === 0 ? (
              <p className="text-center text-gray-600">
                🚫 No categories available. Please try again later. 🙇
              </p>
            ) : (
              categories.map((category) => (
                <CategoryCard
                  key={category.id}
                  id={category.id}
                  name={category.name}
                  description={category.description}
                  onViewNominees={handleViewNominees}
                />
              ))
            )}
          </div>
        )}
      </main>

      <Footer />
    </div>
  );
};

export default CategoriesPage;
