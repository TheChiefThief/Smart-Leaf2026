const API_URL = "http://localhost:5085/api/auth"; // <-- Corrige aquí

import Cookies from 'js-cookie';

export async function login(email, password) {
  const response = await fetch(`${API_URL}/login`, { // <-- Corrige aquí
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify({ email, password }),
  });

  if (!response.ok) {
    throw new Error("Login fallido");
  }

  const data = await response.json();
  Cookies.set('userData', JSON.stringify(data.userData));
  return data.token;
}

export async function register(fullName, userName, email, password) {
  const response = await fetch(`${API_URL}/register`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ fullName, userName, email, password }),
  });
  if (!response.ok) {
    const error = await response.json();
    throw new Error(error.message || "Error al registrar usuario");
  }
  return await response.json();
}

export async function getCurrentUser(token) {
  const response = await fetch(`${API_URL}/me`, {
    method: "GET",
    headers: { "Authorization": `Bearer ${token}` }
  });
  if (!response.ok) {
    throw new Error("No autenticado");
  }
  return await response.json();
}

