import React from "react";
import { useLocation } from "react-router-dom";
import { BackButton, Dropdown} from "./";
import  "../models/PageHeader.css"

const titles = {
  "/home": "Home",
  "/huerta": "🌱 Tus plantas",
  "/calendario": "🗓️ Calendario ",
  "/camara": "🕵🏻 Analizador",
  "/biblioteca": "🌿 Botánica",
  "/consultor": "👩🏻‍💼 Asistente",
  "/perfil": "Perfil",
  "/settings": "Configuración",
  "/jardin": "Jardín",
  "/registrar": "Registro",
  "/login": "Iniciar sesión",
  "/add": "Nueva planta",
  "/planta/:name": "Detalles de la planta",
  "/not-found": "Página no encontrada",
};

function PageHead() {
  const location = useLocation();
  const pageTitle = titles[location.pathname] || "";

  return (
    <header className="app-header">
      <BackButton />
      <h1 className="header-title">{pageTitle}</h1>
      <Dropdown dropdownTitle="Menú" />
    </header>
  );
};

export default PageHead;  