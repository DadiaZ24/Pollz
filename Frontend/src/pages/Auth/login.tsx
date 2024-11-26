// src/pages/LoginPage.tsx

import React from "react";
import LoginForm from "../../components/forms/login";

const LoginPage: React.FC = () => {
  return (
    <div className="min-h-screen bg-gray-700 flex items-center justify-center">
      <LoginForm />
    </div>
  );
};

export default LoginPage;
