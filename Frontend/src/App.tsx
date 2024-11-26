import React from "react";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import LoginPage from "./pages/Auth/login";
import RegisterPage from "./pages/Auth/register";
import CategoriesPage from "./pages/categoryPage";

const App: React.FC = () => {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<LoginPage />} />
        <Route path="/register" element={<RegisterPage />} />
        <Route path="/categories" element={<CategoriesPage />} />
      </Routes>
    </Router>
  );
};

export default App;
