import React, { createContext, useContext, useState, useEffect } from "react";
import { authApi } from "../api/authApi";

const AuthContext = createContext(null);

export function AuthProvider({ children }) {
  const [user, setUser]           = useState(null);
  const [loading, setLoading]     = useState(true);

  // Al montar, si hay token intenta recuperar el usuario actual
  useEffect(() => {
    const init = async () => {
      if (authApi.isAuthenticated()) {
        try {
          const me = await authApi.getMe();
          setUser(me);
        } catch {
          // Token inválido o expirado → limpia sesión
          localStorage.removeItem("smartleaf_token");
        }
      }
      setLoading(false);
    };
    init();
  }, []);

  const login = async (email, password) => {
    const data = await authApi.login(email, password);
    const me   = await authApi.getMe();
    setUser(me);
    return data;
  };

  const logout = () => {
    authApi.logout();
    setUser(null);
  };

  const value = {
    user,
    loading,
    isAuthenticated: !!user,
    login,
    logout,
  };

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}

// Hook de acceso rápido: const { user, login, logout } = useAuth();
export const useAuth = () => {
  const ctx = useContext(AuthContext);
  if (!ctx) throw new Error("useAuth debe usarse dentro de <AuthProvider>");
  return ctx;
};
