import React, { useState } from "react";
import { PageHead } from "../components";
import "../models/Settings.css";

function Settings({ setTheme }) {
  const [language, setLanguage] = useState("es");
  const [notifications, setNotifications] = useState(false);
  const [theme, setThemeState] = useState("light");

  const handleSave = () => {
    console.log("Idioma:", language);
    console.log("Notificaciones:", notifications);
    console.log("Tema:", theme);
    // Aquí podrías guardar en localStorage, context, etc.
    alert("¡Configuraciones guardadas!");
  };

  return (
    <div className="settings-page">
      <PageHead />
      <div className="settings-card">
        <h2>Configuraciones</h2>

        <div className="settings-option">
          <label htmlFor="language">Idioma</label>
          <select
            id="language"
            value={language}
            onChange={(e) => setLanguage(e.target.value)}
          >
            <option value="es">Español</option>
            <option value="en">English</option>
          </select>
        </div>

        <div className="settings-option">
          <label htmlFor="notifications">Notificaciones</label>
          <input
            type="checkbox"
            id="notifications"
            checked={notifications}
            onChange={() => setNotifications(!notifications)}
          />
        </div>

        <div className="settings-option">
          <label htmlFor="theme">Tema</label>
          <select
            id="theme"
            name="theme"
            value={theme}
            onChange={(e) => {
              setTheme(e.target.value);
              setThemeState(e.target.value); // tu propio estado local si lo tienes
            }}
          >
            <option value="light">Claro</option>
            <option value="dark">Oscuro</option>
          </select>
        </div>

        <button className="settings-save-button" onClick={handleSave}>
          Guardar Cambios
        </button>
      </div>
    </div>
  );
}

export default Settings;
