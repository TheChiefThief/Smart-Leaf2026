import api from "./axiosInstance";

export const gardenApi = {
  getMyGardens: async () => {
    const { data } = await api.get("/garden");
    return data;
  },

  getById: async (id) => {
    const { data } = await api.get(`/garden/${id}`);
    return data;
  },

  create: async ({ name, climateZoneId, soilTypeId, sunExposure }) => {
    const { data } = await api.post("/garden", { name, climateZoneId, soilTypeId, sunExposure });
    return data;
  },

  update: async (id, fields) => {
    await api.put(`/garden/${id}`, fields);
  },

  delete: async (id) => {
    await api.delete(`/garden/${id}`);
  },
};
