import React from "react";
import logo from "../assets/logo.png";

const Header: React.FC = () => (
  <header className="bg-blue-600 text-white p-10 shadow-md">
    <div className="container mx-auto flex justify-between items-center">
      <img src={logo} alt="logo" />
    </div>
  </header>
);

export default Header;
