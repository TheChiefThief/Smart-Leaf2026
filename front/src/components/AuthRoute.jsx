import React from 'react';
import { Navigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';

function AuthRoute({ children }) {
  const { isAuthenticated, loading } = useAuth();

  if (loading) return null; // espera a que el contexto verifique el token

  if (!isAuthenticated) {
    return <Navigate to="/login" replace />;
  }

  return children;
}

export default AuthRoute;