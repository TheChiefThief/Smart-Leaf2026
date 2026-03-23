import React, { useEffect, useState } from "react";
import { BrowserRouter as Router, Routes, Route, Navigate } from "react-router-dom";

import { Home, NotFound, Garden, Calendar, PlantIdentifier, PlantsLibrary, PlantConsultantAI, Profile, Settings } from "../pages/";
import { Login, Registro, AddPlant, PlantDetails } from "../pages/";

import { useAuth } from "../context/AuthContext";
import { AuthRoute, BottomNav } from "../components/";
import "../models/App.css";

function App() {
  const { isAuthenticated, loading } = useAuth();

  const [theme, setTheme] = useState(localStorage.getItem("theme") || "light");

  useEffect(() => {
    document.body.classList.toggle("dark-theme", theme === "dark");
    localStorage.setItem("theme", theme);
  }, [theme]);

  // Mientras verifica el token, no renderiza nada para evitar parpadeos
  if (loading) return null;

  return (
    <Router>
      <Routes>
        <Route path="/login"     element={<Login />} />
        <Route path="/registrar" element={<Registro />} />
        <Route
          path="/"
          element={isAuthenticated ? <Navigate to="/home" /> : <Navigate to="/login" />}
        />

        <Route path="/home"       element={<AuthRoute><Home /></AuthRoute>} />
        <Route path="/huerta"     element={<AuthRoute><><Garden /><BottomNav /></></AuthRoute>} />
        <Route path="/calendario" element={<AuthRoute><><Calendar /><BottomNav /></></AuthRoute>} />
        <Route path="/camara"     element={<AuthRoute><><PlantIdentifier /><BottomNav /></></AuthRoute>} />
        <Route path="/biblioteca" element={<AuthRoute><><PlantsLibrary /><BottomNav /></></AuthRoute>} />
        <Route path="/consultor"  element={<AuthRoute><><PlantConsultantAI /><BottomNav /></></AuthRoute>} />
        <Route path="/perfil"     element={<AuthRoute><><Profile /><BottomNav /></></AuthRoute>} />
        <Route path="/settings"   element={<AuthRoute><><Settings setTheme={setTheme} /><BottomNav /></></AuthRoute>} />
        <Route path="/add"        element={<AuthRoute><><AddPlant /><BottomNav /></></AuthRoute>} />
        <Route path="/planta/:name" element={<AuthRoute><><PlantDetails /><BottomNav /></></AuthRoute>} />
        <Route path="*"           element={<AuthRoute><><NotFound /><BottomNav /></></AuthRoute>} />
      </Routes>
    </Router>
  );
}

export default App;