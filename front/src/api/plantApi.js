import api from "./axiosInstance";

export const plantApi = {
  // ── Biblioteca de plantas del jardín ─────────────────────────────────────

  getByGarden: async (gardenId) => {
    const { data } = await api.get(`/plantlibrary/garden/${gardenId}`);
    return data;
  },

  getById: async (id) => {
    const { data } = await api.get(`/plantlibrary/${id}`);
    return data;
  },

  add: async ({ gardenId, speciesId, nickname, initialQuantity, plantingDate, status, notes }) => {
    const { data } = await api.post("/plantlibrary", {
      gardenId, speciesId, nickname, initialQuantity, plantingDate, status, notes,
    });
    return data;
  },

  update: async (id, fields) => {
    await api.put(`/plantlibrary/${id}`, fields);
  },

  updateStatus: async (id, status) => {
    await api.patch(`/plantlibrary/${id}/status`, JSON.stringify(status));
  },

  delete: async (id) => {
    await api.delete(`/plantlibrary/${id}`);
  },

  // ── Identificación por imagen ─────────────────────────────────────────────

  identify: async (base64Image) => {
    const { data } = await api.post("/plant/identify", JSON.stringify(base64Image));
    return data;
  },

  // ── Búsqueda de imagen por nombre científico (iNaturalist) ─────────────────

  searchImage: async (scientificName) => {
    const { data } = await api.get(`/plantsearch/${encodeURIComponent(scientificName)}`);
    return data; // { imageUrl: "https://..." }
  },
};
