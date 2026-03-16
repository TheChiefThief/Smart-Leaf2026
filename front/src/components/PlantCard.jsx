import React from "react";
import { Link } from "react-router-dom"; // Importar Link
import "../models/Garden.css";

const statusColors = {
  "¡Regar!": "#1c9c56",
  "¡Cosechar!": "#215ead",
  "Recién plantada": "#888",
};

const PlantCard = ({ name = "Sin nombre", image = null, status = "Recién plantada" }) => {
  const ruta = `/planta/${name.toLowerCase()}`;
  return (
    <div className="plant-card">
      <Link to={ruta}>
        <img
          src={image} // Mostrar imagen predeterminada si no hay una específica
          alt={name}
          className="plant-image"
        />
      </Link>
      <h3 className="plant-name">{name}</h3>
      <p
        className="plant-status"
        style={{ color: statusColors[status] || "#888" }} // Color según el estado
      >
        {status}
      </p>
    </div>
  );
};

export default PlantCard;