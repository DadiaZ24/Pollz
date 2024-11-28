// src/pages/LoginPage.tsx

import React, { useEffect } from "react";
import LoginForm from "../../components/forms/login";
import { isAuthenticated } from "../../services/AuthService";

const LoginPage: React.FC = () => {
  useEffect(() => {
    if (isAuthenticated()) {
      window.location.href = "/polls";
    }
  }, []);
  return (
    <div className="min-h-screen bg-gray-700 flex items-center justify-center">
      <LoginForm />
    </div>
  );
};

export default LoginPage;
