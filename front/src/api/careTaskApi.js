import api from "./axiosInstance";

export const careTaskApi = {
  getByPlant: async (plantId) => {
    const { data } = await api.get(`/caretask/plant/${plantId}`);
    return data;
  },

  getById: async (id) => {
    const { data } = await api.get(`/caretask/${id}`);
    return data;
  },

  create: async ({ plantId, taskType, scheduledDate, status }) => {
    const { data } = await api.post("/caretask", { plantId, taskType, scheduledDate, status });
    return data;
  },

  update: async (id, fields) => {
    await api.put(`/caretask/${id}`, fields);
  },

  updateStatus: async (id, status) => {
    await api.patch(`/caretask/${id}/status`, JSON.stringify(status));
  },

  delete: async (id) => {
    await api.delete(`/caretask/${id}`);
  },
};
