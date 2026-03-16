import React, { useEffect, useState } from "react";
import { BrowserRouter as Router, Routes, Route, Navigate } from "react-router-dom";

import { Home, NotFound, Garden, Calendar, PlantIdentifier, PlantsLibrary, PlantConsultantAI, Profile, Settings } from "../pages/";
import { Login, Registro, AddPlant, PlantDetails } from "../pages/";

import { getToken } from "../utils/token";
import { AuthRoute, BottomNav } from "../components/";
import { PlantProvider } from "../components/PlantProvider"; // Importar el contexto
import "../models/App.css";

function App() {
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [theme, setTheme] = useState(
    localStorage.getItem("theme") || "light"
  );

  useEffect(() => {
    const token = getToken();
    setIsAuthenticated(!!token);
  }, []);
  console.log("isAuthenticated:", isAuthenticated); // Depuración

  useEffect(() => {
    document.body.classList.toggle("dark-theme", theme === "dark");
    localStorage.setItem("theme", theme);
  }, [theme]);

  return (

    <PlantProvider> {/* Envolver las rutas con el proveedor */}
      <Router>
        <Routes>
          <Route path="/login" element={<Login />} />
          <Route
            path="/"
            element={isAuthenticated ? <Navigate to="/home" /> : <Navigate to="/login" />}
          />
          <Route path="/home" element={<AuthRoute> <Home /> </AuthRoute>} />
          <Route path="/huerta" element={<AuthRoute> <> <Garden /> <BottomNav /> </> </AuthRoute>} />
          <Route path="/calendario" element={<AuthRoute> <> <Calendar /> <BottomNav /> </> </AuthRoute>} />
          <Route path="/camara" element={<AuthRoute> <> <PlantIdentifier /> <BottomNav /> </> </AuthRoute>} />
          <Route path="/biblioteca" element={<AuthRoute> <> <PlantsLibrary /> <BottomNav /> </> </AuthRoute>} />
          <Route path="/consultor" element={<AuthRoute> <> <PlantConsultantAI /> <BottomNav /> </> </AuthRoute>} />
          <Route path="/perfil" element={<AuthRoute> <> <Profile /> <BottomNav /> </> </AuthRoute>} />
          <Route
            path="/settings"
            element={
              <AuthRoute>
                <>
                  <Settings setTheme={setTheme} /> {/* <-- pasa setTheme aquí */}
                  <BottomNav />
                </>
              </AuthRoute>
            }
          />
          <Route path="/registrar" element={<Registro />} />
          <Route path="/add" element={<AuthRoute> <> <AddPlant /> <BottomNav /> </> </AuthRoute>} />
          <Route path="/planta/:name" element={<AuthRoute> <> <PlantDetails /> <BottomNav /> </> </AuthRoute>} /> {/* Ruta dinámica */}
          <Route path="*" element={<AuthRoute> <> <NotFound /> <BottomNav /> </> </AuthRoute>} />
        </Routes>
      </Router>
    </PlantProvider>

  );
}

export default App;