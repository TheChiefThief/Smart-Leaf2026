import axios from "axios";

export const getPlantImageByName = async (plantName) => {
  try {
    const res = await axios.get(`http://localhost:5085/api/plantsearch/${encodeURIComponent(plantName)}`);
    console.log("Respuesta de la API:", res.data); // <-- Aquí ves el JSON en la consola
    return res.data;
  } catch (error) {
    throw new Error('No se encontró la planta o no tiene imagen.');
  }
};

// Busca la planta y retorna el primer resultado con todos los datos relevantes
export const getPlantInfoByName = async (plantName) => {
  try {
    const res = await axios.post(
      "http://localhost:5085/api/plant/search",
      plantName,
      { headers: { "Content-Type": "application/json" } }
    );
    // res.data debe ser un array de objetos [{ name, scientificName, imageUrl, description }]
    if (Array.isArray(res.data) && res.data.length > 0) {
      // Devuelve el primer resultado
      return res.data[0];
    } else {
      throw new Error("No se encontró la planta.");
    }
  } catch (error) {
    throw new Error("No se encontró la planta o no tiene imagen.");
  }
};