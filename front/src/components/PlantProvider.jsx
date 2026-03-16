import React, { useState } from "react";
import { PlantContext } from "../context/PlantContext";
import { Jazmin, Albahaca } from "../assets/";

export function PlantProvider({ children }) {
  const preloadedPlants = [
    {
      species: "Solanum lycopersicum",
      name: "Jazmín",
      plantingDate: "2025-05-01",
      location: "Living Room",
      image: Jazmin,
      status: "¡Regar!",
      waterFrequency: 7,
    },
    {
      species: "Albahaca ocimum",
      name: "Albahaca",
      plantingDate: "2025-04-15",
      location: "Kitchen",
      image: Albahaca,
      status: "¡Cosechar!",
      waterFrequency: 5,
    },
  ];

  const [plants, setPlants] = useState(() => {
    const storedPlants = localStorage.getItem("plants");
    return storedPlants ? JSON.parse(storedPlants) : preloadedPlants;
  });

  const addPlant = (newPlant) => {
    const updatedPlant = {
      ...newPlant,
      image: newPlant.image || null, // Imagen predeterminada si no se proporciona
      status: newPlant.status || "Recién plantada", // Estado predeterminado
      waterFrequency: newPlant.waterFrequency || 7, // Frecuencia predeterminada
    };
    setPlants((prevPlants) => {
      const updatedPlants = [...prevPlants, updatedPlant];
      localStorage.setItem("plants", JSON.stringify(updatedPlants));
      return updatedPlants;
    });
  };

  const calculateWateringDates = (plant) => {
    const dates = [];
    const plantingDate = new Date(plant.plantingDate);

    // Validar si plantingDate es una fecha válida
    if (isNaN(plantingDate.getTime())) {
      console.error(`Invalid plantingDate for plant: ${plant.name}`);
      return dates; // Retornar un array vacío si la fecha no es válida
    }

    for (let i = 1; i <= 5; i++) {
      const nextDate = new Date(plantingDate);
      nextDate.setDate(plantingDate.getDate() + i * plant.waterFrequency);
      dates.push(nextDate.toISOString().split("T")[0]); // Formato YYYY-MM-DD
    }
    return dates;
  };

  return (
    <PlantContext.Provider value={{ plants, addPlant, calculateWateringDates }}>
      {children}
    </PlantContext.Provider>
  );
}