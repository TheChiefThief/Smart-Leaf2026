import React from "react";
import { PageHead } from "../components";
import { useNavigate } from "react-router-dom";
import { Avatar } from "../assets/";
import "../models/Profile.css";

function Profile() {
  const navigate = useNavigate();
  const handleLogout = () => {
    navigate("/login"); // Redirigir a la página de inicio de sesión
  };
  const examplePeople = {
  id: 1,
  name: "Julián Ruíz López",
  age: 30,
  email: "johndoe@example.com",
  address: "123 Main Street, Springfield",
  phone: "+1 234 567 890",
  avatar: Avatar
};

  const { avatar, name, age, email, address, phone } = examplePeople;

  return (
    <div className="profile-page">
      <header className="calendar-header">
        <PageHead />
      </header>

      <div className="profile-card">
        <img src={avatar} alt={name} className="profile-avatar" />
        <h2>{name}</h2>
        <p><strong>Edad:</strong> {age}</p>
        <p><strong>Email:</strong> {email}</p>
        <p><strong>Dirección:</strong> {address}</p>
        <p><strong>Teléfono:</strong> {phone}</p>
      </div>

      <div className="profile-actions">
        <button className="btn-edit" >Editar Perfil</button>
        <button className="btn-logout" onClick={handleLogout}>Cerrar Sesión</button>
      </div>
    </div>
  );
}

export default Profile;
