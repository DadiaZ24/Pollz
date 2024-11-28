import React, { useEffect } from "react";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import LoginPage from "./pages/Auth/login";
import RegisterPage from "./pages/Auth/register";
import PollsPage from "./pages/PollsPage";
import { isAuthenticated } from "./services/AuthService";
import ProtectedRoute from "./components/ProtectedRoute";

const App: React.FC = () => {
  const [isAuth, setIsAuth] = React.useState<boolean>(false);

  useEffect(() => {
    setIsAuth(isAuthenticated());
  }, []);

  return (
    <Router>
      <Routes>
        <Route path="/login" element={<LoginPage />} />
        <Route path="/register" element={<RegisterPage />} />
        <Route element={<ProtectedRoute />}>
          <Route path="/" element={isAuth ? <PollsPage /> : <LoginPage />} />
          <Route path="/polls" element={<PollsPage />} />
        </Route>
      </Routes>
    </Router>
  );
};

export default App;
