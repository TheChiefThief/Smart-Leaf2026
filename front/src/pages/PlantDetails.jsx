import React from "react";
import { useParams } from "react-router-dom";
import { useContext } from "react";
import { PlantContext } from "../context/PlantContext";
import { PageHead } from "../components";
import "../models/PlantDetails.css";

function PlantDetails() {
  const { name } = useParams(); // Obtener el nombre de la planta desde la URL
  const { plants } = useContext(PlantContext); // Obtener las plantas del contexto

  // Buscar la planta por nombre
  const plant = plants.find((p) => p.name.toLowerCase() === name.toLowerCase());

  if (!plant) {
    return (
      <div className="plant-details">
        <PageHead />
        <h2>Planta no encontrada</h2>
      </div>
    );
  }

  return (
    <div className="plant-details">
      <PageHead />
      <h2>{plant.name}</h2>
      <img src={plant.image || ""} alt={plant.name} className="plant-image" />
      <p><strong>Especie:</strong> {plant.species}</p>
      <p><strong>Fecha de plantación:</strong> {plant.plantingDate}</p>
      <p><strong>Ubicación:</strong> {plant.location}</p>
      <p><strong>Estado:</strong> {plant.status}</p>
    </div>
  );
};

export default PlantDetails;