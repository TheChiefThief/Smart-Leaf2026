import React, { createContext, useState } from 'react';

export const PlantContext = createContext();

function PlantProvider  ({ children })  {
  const [plants, setPlants] = useState([]);

  const addPlant = (newPlant) => {
    setPlants((prevPlants) => [...prevPlants, newPlant]);
  };

  return (
    <PlantContext.Provider value={{ plants, addPlant }}>
      {children}
    </PlantContext.Provider>
  );
};
export default PlantProvider;

