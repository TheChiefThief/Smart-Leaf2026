import api from "./axiosInstance";

const TOKEN_KEY = "smartleaf_token";

export const authApi = {
  login: async (email, password) => {
    const { data } = await api.post("/auth/login", { email, password });
    // El backend devuelve { token: "..." }
    localStorage.setItem(TOKEN_KEY, data.token);
    return data;
  },

  register: async (fullName, userName, email, password) => {
    const { data } = await api.post("/auth/register", { fullName, userName, email, password });
    return data;
  },

  getMe: async () => {
    const { data } = await api.get("/auth/me");
    return data;
  },

  logout: () => {
    localStorage.removeItem(TOKEN_KEY);
    window.location.href = "/login";
  },

  getToken: () => localStorage.getItem(TOKEN_KEY),

  isAuthenticated: () => !!localStorage.getItem(TOKEN_KEY),
};
