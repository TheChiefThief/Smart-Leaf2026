import axios from "axios";


export async function identifyPlant(base64Image) {
  try {
    const response = await axios.post("http://localhost:5085/api/plant/identify", base64Image, {
      headers: {
        "Content-Type": "application/json",
      },
    });
    return response.data;
  } catch (error) {
    console.error("Error al identificar planta:", error);
    throw error;
  }
}
