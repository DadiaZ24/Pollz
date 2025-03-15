import React from "react";
import RegisterForm from "../../components/forms/register";

const RegisterPage: React.FC = () => {
  return (
    <div className="min-h-screen bg-gray-700 flex items-center justify-center">
      <RegisterForm />
    </div>
  );
};

export default RegisterPage;
