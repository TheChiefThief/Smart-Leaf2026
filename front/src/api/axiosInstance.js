import axios from "axios";

const BASE_URL = "http://localhost:5085/api";

const api = axios.create({
  baseURL: BASE_URL,
  headers: { "Content-Type": "application/json" },
});

// Inyecta el JWT en cada request automáticamente
api.interceptors.request.use((config) => {
  const token = localStorage.getItem("smartleaf_token");
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

// Si el servidor devuelve 401, limpia la sesión y redirige al login
api.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      localStorage.removeItem("smartleaf_token");
      window.location.href = "/login";
    }
    return Promise.reject(error?.response?.data ?? error);
  }
);

export default api;
