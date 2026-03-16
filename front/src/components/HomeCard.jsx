import React from "react";
import "../models/home-card.css"; 
import { Link } from "react-router-dom";

function HomeCard({ nombre, descripcion, link, src1}) {
  return (
    <div className="home-card">
      <div className="home-card-text">
        <h3 className="home-card-title">{nombre}</h3>
        <p className="home-card-description">
          {descripcion}
        </p>
        <Link to={link}><button className="home-card-button">{nombre}</button></Link>
      </div>
      <img
        className="home-card-image"
        src={src1}
        alt="foto"
      />
    </div>
  );
}

export default HomeCard;