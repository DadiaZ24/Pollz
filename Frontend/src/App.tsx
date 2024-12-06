import React, { useEffect } from "react";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import LoginPage from "./pages/Auth/login";
import RegisterPage from "./pages/Auth/register";
import PollsPage from "./pages/PollsPage";
import { isAuthenticated } from "./services/AuthService";
import ProtectedRoute from "./components/ProtectedRoute";
import AddPollPage from "./pages/AddPollPage";
import PollsInfoPage from "./pages/PollInfoPage";
import ResultsPage from "./pages/ResultPage";
import VotingPage from "./pages/VotingPage";

const App: React.FC = () => {
  const [isAuth, setIsAuth] = React.useState<boolean>(false);

  useEffect(() => {
    setIsAuth(isAuthenticated());
  }, []);

  fetch("http://100.42.185.156:5000/api/auth/test", {
    method: "GET",
  })
    .then((response) => response.text())
    .then((data) => console.log("Backend Response:", data))
    .catch((error) => console.error("Error connecting to backend:", error));

  return (
    <Router>
      <Routes>
        <Route path="/login" element={<LoginPage />} />
        <Route path="/register" element={<RegisterPage />} />
        <Route element={<ProtectedRoute />}>
          <Route path="/" element={isAuth ? <PollsPage /> : <LoginPage />} />
          <Route path="/polls" element={<PollsPage />} />
          <Route path="/polls/add-pole" element={<AddPollPage />} />
          <Route path="polls/:id" element={<PollsInfoPage />} />
          <Route path="polls/:id/results" element={<ResultsPage />} />
        </Route>
        <Route path="voting/:voterId/:uniqueCode" element={<VotingPage />} />
      </Routes>
    </Router>
  );
};

export default App;
