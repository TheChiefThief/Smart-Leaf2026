import React, { useContext, useState } from "react";
import { PlantContext } from "../context/PlantContext";
import { PlantCard, PageHead } from "../components/";
import { useNavigate } from "react-router-dom";
import "../models/Garden.css";

export const Garden = () => {
  const { plants } = useContext(PlantContext);
  const [open, setOpen] = useState(false);
  const navigate = useNavigate();

  return (
    <div className="app">
      <PageHead />

      <div className="plant-list">
        {plants.map((plant, index) => (
          <PlantCard
            key={index}
            name={plant.name}
            image={plant.image}
            status={plant.status}
          />
        ))}
      </div>

      {/* Botón flotante */}
      <div className="fab-container">
        <button
          className={`fab-main ${open ? "open" : ""}`}
          onClick={() => setOpen((prev) => !prev)}
          aria-label="Agregar"
        >
          +
        </button>
        <div className={`fab-options ${open ? "show" : ""}`}>
          <button
            className="fab-option"
            title="Agregar por formulario"
            onClick={() => navigate("/add")}
          >
            <span role="img" aria-label="Formulario">📝</span>
          </button>
          <button
            className="fab-option"
            title="Agregar por cámara"
            onClick={() => navigate("/camara")}
          >
            <span role="img" aria-label="Cámara">📷</span>
          </button>
        </div>
      </div>
    </div>
  );
};

export default Garden;

