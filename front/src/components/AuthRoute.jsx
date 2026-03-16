import React from 'react';
import { Navigate } from 'react-router-dom';
import Cookies from 'js-cookie';
import { getToken } from '../utils/token';

function AuthRoute ({ children }){
  const token = getToken(); // Verifica si hay un token en localStorage
  const userData = Cookies.get('userData'); // Verifica si hay datos de usuario en las cookies

  // Si no hay token ni cookies, redirige al inicio de sesión
  if (!token || !userData) {
    return <Navigate to="/login" replace />;
  }

  // Si ambos están presentes, permite el acceso
  return children;
};

export default AuthRoute;