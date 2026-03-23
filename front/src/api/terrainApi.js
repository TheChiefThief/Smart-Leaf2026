import api from "./axiosInstance";

export const terrainApi = {
  // ── Canvas / Terrain ─────────────────────────────────────────────────────
  // Un solo llamado carga el lienzo COMPLETO con todos los sprouts embebidos

  getByGarden: async (gardenId) => {
    const { data } = await api.get(`/terrain/garden/${gardenId}`);
    return data; // { id, gardenId, shapeType, dimensions, sprouts: [...] }
  },

  create: async ({ gardenId, shapeType, dimensions }) => {
    const { data } = await api.post("/terrain", { gardenId, shapeType, dimensions });
    return data;
  },

  update: async (id, fields) => {
    await api.put(`/terrain/${id}`, fields);
  },

  delete: async (id) => {
    await api.delete(`/terrain/${id}`);
  },

  // ── Sprouts ───────────────────────────────────────────────────────────────

  addSprout: async ({ terrainId, plantId, label, color, form, x, y, z }) => {
    const { data } = await api.post("/terrain/sprout", {
      terrainId, plantId, label, color, form, x, y, z,
    });
    return data;
  },

  updateSprout: async (sproutId, fields) => {
    // fields puede incluir: label, color, form, x, y, z
    await api.put(`/terrain/sprout/${sproutId}`, fields);
  },

  deleteSprout: async (sproutId) => {
    await api.delete(`/terrain/sprout/${sproutId}`);
  },
};
