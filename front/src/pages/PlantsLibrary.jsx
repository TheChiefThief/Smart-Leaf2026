import React, { useState } from 'react';
import { Link } from 'react-router-dom';
import '../models/PlantLibrary.css';
import { getPlantImageByName } from '../services/plantDataService';

// Plantas precargadas por categorías
const defaultCategories = [
  {
    category: "Árboles",
    plants: [
      { name: "Jacarandá", imageUrl: "https://upload.wikimedia.org/wikipedia/commons/8/8e/Jacaranda_mimosifolia.jpg" },
      { name: "Roble", imageUrl: "https://upload.wikimedia.org/wikipedia/commons/6/6e/Quercus_robur.jpg" },
    ]
  },
  {
    category: "Flores",
    plants: [
      { name: "Rosa", imageUrl: "https://upload.wikimedia.org/wikipedia/commons/b/bf/Red_rose.jpg" },
      { name: "Girasol", imageUrl: "https://upload.wikimedia.org/wikipedia/commons/4/40/Sunflower_sky_backdrop.jpg" },
    ]
  },
  {
    category: "Suculentas",
    plants: [
      { name: "Aloe Vera", imageUrl: "https://upload.wikimedia.org/wikipedia/commons/7/7a/Aloe_vera_flower.JPG" },
      { name: "Cactus", imageUrl: "https://upload.wikimedia.org/wikipedia/commons/5/5b/Cactus.jpg" },
    ]
  }
];

function PlantLibrary() {
  const [searchTerm, setSearchTerm] = useState('');
  const [plantCard, setPlantCard] = useState(null);
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);

  const handleInputChange = (e) => {
    setSearchTerm(e.target.value);
    setError('');
    setPlantCard(null);
  };

  const handleSearch = async () => {
    setError('');
    setPlantCard(null);

    if (searchTerm.trim() === '') return;

    setLoading(true);
    try {
      const imageUrl = await getPlantImageByName(searchTerm);
      setPlantCard({
        name: searchTerm,
        imageUrl,
      });
    } catch (err) {
      setError(err.message);
    }
    setLoading(false);
  };

  return (
    <div className="app">
      <h2>Buscador de Plantas</h2>
      <input
        type="text"
        value={searchTerm}
        onChange={handleInputChange}
        placeholder="Nombre común de la planta"
        className="search-bar"
      />
      <button onClick={handleSearch} className="search-btn">Buscar</button>

      {loading && <p>Buscando...</p>}
      {error && <p style={{ color: 'red' }}>{error}</p>}

      {/* Mostrar carta de búsqueda si hay resultado */}
      {plantCard && (
        <Link
          to={`/descripcion/${encodeURIComponent(plantCard.name)}`}
          state={{
            imageUrl: plantCard.imageUrl,
            commonName: plantCard.commonName,
            scientificName: plantCard.scientificName,
            description: plantCard.description
          }}
          style={{ textDecoration: "none", color: "inherit" }}
        >
          <div className="plant-card-lib">
            <img src={plantCard.imageUrl} alt={plantCard.name} className="plant-image-lib" />
            <p className="plant-name-lib">{plantCard.name}</p>
          </div>
        </Link>
      )}

      {/* Mostrar categorías por defecto si no hay búsqueda ni resultado */}
      {!plantCard && !searchTerm && (
        <div>
          {defaultCategories.map((cat) => (
            <div key={cat.category} className="category-section">
              <h3 className="category-title">{cat.category}</h3>
              <div className="plant-list-lib">
                {cat.plants.map((plant) => (
                  <Link
                    key={plant.name}
                    to={`/descripcion/${encodeURIComponent(plant.name)}`}
                    state={{
                      imageUrl: plant.imageUrl,
                      commonName: plant.commonName,
                      scientificName: plant.scientificName,
                      description: plant.description
                    }}
                    style={{ textDecoration: "none", color: "inherit" }}
                  >
                    <div className="plant-card-lib">
                      <img src={plant.imageUrl} alt={plant.name} className="plant-image-lib" />
                      <p className="plant-name-lib">{plant.name}</p>
                    </div>
                  </Link>
                ))}
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
}

export default PlantLibrary;