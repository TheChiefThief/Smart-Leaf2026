import React, { useState, useContext } from "react";
import { useNavigate } from "react-router-dom";
import { PageHead } from "../components";
import { PlantContext } from "../context/PlantContext";
import "../models/AddPlant.css";

const speciesOptions = [
  "Aloe Vera",
  "Basil",
  "Cactus",
  "Fern",
  "Rosemary",
  "Cebolla",
  "Tomate",
  "Lavanda",
  "Girasol",
  "Petunia",
  "Lechuga",
  "Zanahoria",
  "Rosa",
  "Orquídea",
  "Caléndula",
  "Espinaca",
  "Perejil",
  "Albahaca",
  "Dalia",
  "Cilantro",
  "Menta"
];

const developmentStages = ["Semilla", "Germinando", "Madura"];

const locationOptions = [
  "Argentina, Buenos Aires",
  "Argentina, Córdoba",
  "Argentina, Tucumán",
  "Argentina, Mendoza",
  "Argentina, Salta",
];

const placeTypeOptions = [
  "Maceta grande",
  "Maceta pequeña",
  "Tierra directa",
  "Cantero pequeño",
  "Cantero grande",
  "Huerta",
  "Jardín",
  "Balcón",
  "Terraza"
];

const lightOptions = [
  "Sol directo",
  "Media sombra",
  "Interior"
];

const soilOptions = [
  "Tierra negra",
  "Con compost",
  "Turba",
  "Arena",
  "Sustrato universal"
];

// Consejos por especie y campo
const advice = {
  "Cactus": {
    lightType: "Consejo: los cactus prefieren recibir constante sol.",
    placeType: "Consejo: los cactus crecen bien en macetas pequeñas con buen drenaje.",
    soilType: "Consejo: usa sustrato arenoso y bien drenado para cactus."
  },
  "Cebolla": {
    placeType: "Consejo: la cebolla crece mejor en tierra directa, en ambientes húmedos. Se recomienda no plantarla junto a legumbres o arvejas.",
    lightType: "Consejo: la cebolla debe recibir luz constante.",
    soilType: "Consejo: la cebolla puede crecer en cualquier sustrato, pero prefiere suelos sueltos y bien drenados."
  },
  "Tomate": {
    placeType: "Consejo: el tomate prefiere canteros grandes o huertas con espacio para crecer.",
    lightType: "Consejo: el tomate necesita al menos 6 horas de sol directo diario.",
    soilType: "Consejo: usa tierra rica en materia orgánica y bien drenada."
  },
  "Lavanda": {
    placeType: "Consejo: la lavanda crece bien en macetas exteriores o tierra directa.",
    lightType: "Consejo: la lavanda necesita mucho sol directo.",
    soilType: "Consejo: prefiere suelos arenosos y con buen drenaje."
  },
  "Lechuga": {
    placeType: "Consejo: la lechuga crece bien en huertas o canteros grandes.",
    lightType: "Consejo: la lechuga prefiere media sombra, especialmente en verano.",
    soilType: "Consejo: usa tierra fértil y húmeda, pero sin encharcar."
  },
  "Rosa": {
    placeType: "Consejo: las rosas prefieren canteros grandes o tierra directa.",
    lightType: "Consejo: las rosas necesitan sol directo al menos 5 horas al día.",
    soilType: "Consejo: usa tierra rica en nutrientes y bien drenada."
  },
  "Orquídea": {
    placeType: "Consejo: las orquídeas suelen ir en macetas especiales con sustrato de corteza.",
    lightType: "Consejo: prefieren luz indirecta brillante.",
    soilType: "Consejo: usa sustrato especial para orquídeas, nunca tierra común."
  },
  "Espinaca": {
    placeType: "Consejo: la espinaca crece bien en huertas o canteros grandes.",
    lightType: "Consejo: prefiere media sombra.",
    soilType: "Consejo: usa tierra fértil y húmeda."
  },
  "Girasol": {
    placeType: "Consejo: el girasol necesita tierra directa o canteros grandes.",
    lightType: "Consejo: requiere sol directo todo el día.",
    soilType: "Consejo: prefiere suelos profundos y bien drenados."
  }
  // Puedes agregar más consejos para otras especies si lo deseas
};

function AddPlant() {
  const { addPlant } = useContext(PlantContext);
  const navigate = useNavigate();
  const [formData, setFormData] = useState({
    species: "",
    developmentStage: "",
    plantingDate: "",
    name: "",
    location: "",
    placeType: "",
    lightType: "",
    soilType: "",
    notes: "",
  });

  const [step, setStep] = useState(0); // 0 = solo especie, 1 = developmentStage, 2 = resto

  const handleChange = (e) => {
    const { name, value } = e.target;

    setFormData((prev) => ({ ...prev, [name]: value }));

    if (name === "species" && value) setStep(1);
    if (name === "developmentStage" && value) setStep(2);
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    const plantingDate = new Date(formData.plantingDate);
    if (isNaN(plantingDate.getTime())) {
      alert("Por favor, ingresa una fecha válida.");
      return;
    }
    addPlant(formData);
    navigate("/huerta");
  };

  const handleCancel = () => {
    setFormData({
      species: "",
      developmentStage: "",
      plantingDate: "",
      name: "",
      location: "",
      placeType: "",
      lightType: "",
      soilType: "",
      notes: "",
    });
    setStep(0);
    navigate(-1);
  };

  // Para mostrar consejos según la especie seleccionada
  const selectedAdvice = advice[formData.species] || {};

  return (
    <div className="addplant-container">
      <PageHead />
      <h2 className="addplant-title">Agregar una planta</h2>
      <form onSubmit={handleSubmit} className="addplant-form">

        {/* Selector de especie */}
        <select
          name="species"
          value={formData.species}
          onChange={handleChange}
          className="addplant-input"
          required
        >
          <option value="" disabled>Seleccionar especie</option>
          {speciesOptions.map((sp) => <option key={sp} value={sp}>{sp}</option>)}
        </select>

        {/* Estado de desarrollo */}
        {step >= 1 && (
          <select
            name="developmentStage"
            value={formData.developmentStage}
            onChange={handleChange}
            className="addplant-input"
            required
          >
            <option value="" disabled>Estado de desarrollo</option>
            {developmentStages.map((st) => <option key={st} value={st}>{st}</option>)}
          </select>
        )}

        {/* Campos restantes */}
        {step === 2 && (
          <>
            {/* Único campo de fecha */}
            <input
              type="date"
              name="plantingDate"
              value={formData.plantingDate}
              onChange={handleChange}
              className="addplant-input"
              required
              readOnly={formData.developmentStage === "Semilla"}
            />

            <input
              type="text"
              name="name"
              placeholder="Nombre"
              value={formData.name}
              onChange={handleChange}
              className="addplant-input"
            />

            {/* Ubicación real */}
            <select
              name="location"
              value={formData.location}
              onChange={handleChange}
              className="addplant-input"
              required
            >
              <option value="" disabled>Ubicación (Provincia, Ciudad, etc.)</option>
              {locationOptions.map((loc) => <option key={loc} value={loc}>{loc}</option>)}
            </select>

            {/* Tipo de lugar */}
            <select
              name="placeType"
              value={formData.placeType}
              onChange={handleChange}
              className="addplant-input"
              required
            >
              <option value="" disabled>¿En qué tipo de lugar la tienes plantada?</option>
              {placeTypeOptions.map((pt) => <option key={pt} value={pt}>{pt}</option>)}
            </select>
            <div className="addplant-advice">{selectedAdvice.placeType}</div>

            {/* Tipo de luz */}
            <select
              name="lightType"
              value={formData.lightType}
              onChange={handleChange}
              className="addplant-input"
              required
            >
              <option value="" disabled>Tipo de luz</option>
              {lightOptions.map((l) => <option key={l} value={l}>{l}</option>)}
            </select>
            {/* Consejo para tipo de luz */}
            <div className="addplant-advice">{selectedAdvice.lightType}</div>

            {/* Tipo de sustrato */}
            <select
              name="soilType"
              value={formData.soilType}
              onChange={handleChange}
              className="addplant-input"
              required
            >
              <option value="" disabled>Tipo de sustrato</option>
              {soilOptions.map((s) => <option key={s} value={s}>{s}</option>)}
            </select>
            {/* Consejo para tipo de sustrato */}
            {selectedAdvice.soilType && (
              <div className="addplant-advice">{selectedAdvice.soilType}</div>
            )}

            <textarea
              name="notes"
              placeholder="Notas adicionales"
              value={formData.notes}
              onChange={handleChange}
              className="addplant-input"
            />
          </>
        )}

        <div className="addplant-buttons">
          <button type="submit" className="addplant-upload">Agregar planta</button>
          <button type="button" className="addplant-cancel" onClick={handleCancel}>
            Cancelar
          </button>
        </div>
      </form>
    </div>
  );
}

export default AddPlant;
