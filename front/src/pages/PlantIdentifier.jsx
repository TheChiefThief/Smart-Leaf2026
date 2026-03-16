import { useState } from 'react';
import { identifyPlant } from '../services/plantIdAPI';
import { Camera } from '../assets/';
import { PageHead } from '../components';
import '../models/PlantIdentifier.css'; 

const PlantIdentifier = () => {
  const [image, setImage] = useState(null);
  const [result, setResult] = useState(null);
  const [loading, setLoading] = useState(false);

  const handleImageUpload = (e) => {
    const file = e.target.files[0];
    if (!file) return;

    const reader = new FileReader();
    reader.onloadend = () => {
      setImage(reader.result.split(',')[1]);
    };
    reader.readAsDataURL(file);
  };

  const handleIdentify = async () => {
    if (!image) return alert("Por favor, sube una imagen antes de identificar.");

    setLoading(true);
    try {
      const data = await identifyPlant(image);
      setResult(data);
    } catch (err) {
      console.error("Error al identificar la planta:", err);
      alert('Hubo un error al identificar la planta. Intenta nuevamente.');
    } finally {
      setLoading(false);
    }
  };

  const handleReset = () => {
    setImage(null);
    setResult(null);
  };

  const suggestion = result?.suggestions?.[0];
  const details = suggestion?.plant_details;

  return (
    <div className="app">
      <PageHead />
      {!result ? (
        <>
          <div className="plant-id-header-container">
            {!image ? (
              <img className="plant-id-header-image" src={Camera} alt="Encabezado" />
            ) : (
              <img
                src={`data:image/jpeg;base64,${image}`}
                alt="Vista previa"
                className="plant-id-header-image"
              />
            )}
          </div>

          <div className="plant-id-guide-text">
            <p>
              Por favor, toma una fotografía de tu planta o carga una imagen para 
              que podamos analizarla.
            </p>
          </div>

          <div className="plant-id-upload">
            {!image ? (
              <label htmlFor="plant-upload" className="pretty-upload-btn">
                📷 Subir foto
                <input
                  id="plant-upload"
                  type="file"
                  accept="image/*"
                  onChange={handleImageUpload}
                  style={{ display: "none" }}
                />
              </label>
            ) : (
              <button
                onClick={handleIdentify}
                disabled={loading}
                className="pretty-upload-btn"
                style={{ marginTop: "1rem" }}
              >
                {loading ? 'Identificando...' : 'Identificar planta'}
              </button>
            )}
          </div>
        </>
      ) : (
        <div className="plant-id-result">
          <h3>Resultado:</h3>
          {/* Mostrar la imagen subida en el resultado */}
          <img
            src={`data:image/jpeg;base64,${image}`}
            alt="Planta identificada"
            className="plant-id-result-image"
            style={{ maxWidth: 250, borderRadius: 12, margin: "1rem auto" }}
          />
          <p><strong>Nombre común:</strong> {details?.common_names?.join(', ') || 'No disponible'}</p>
          <p><strong>Nombre científico:</strong> {suggestion?.plant_name || 'No disponible'}</p>
          <p><strong>Descripción:</strong> {details?.wiki_description?.value || 'No disponible'}</p>
          <button className="add-to-garden-btn" onClick={() => {/* lógica para agregar */}}>
            Agregar al jardín
          </button>
          <button onClick={handleReset} className="reset-button" style={{ marginTop: "1.5rem" }}>
            Nueva identificación
          </button>
        </div>
      )}
    </div>
  );
};

export default PlantIdentifier;