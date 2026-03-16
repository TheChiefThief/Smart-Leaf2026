import React from "react";

function recuperacion() {
  return (
    <div className="recuperacion">
      <h1>Recuperación de Contraseña</h1>
      <p>Por favor, ingresa tu correo electrónico para recuperar tu contraseña.</p>
      <input type="email" placeholder="Correo electrónico" required />
      <button type="submit">Enviar</button>
    </div>
  );
}

export default recuperacion;