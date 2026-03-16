import React from "react";
import { NavLink } from "react-router-dom";
import { FaHome, FaSeedling, FaSearch, FaUser } from "react-icons/fa";
import "../models/BottomNav.css";

function BottomNav() {
  return (
    <nav className="bottom-nav">
      <NavLink to="/home" className="nav-item">
        <FaHome size={20} />
        <span>Home</span>
      </NavLink>
      <NavLink to="/huerta" className="nav-item">
        <FaSeedling size={20} />
        <span>My plants</span>
      </NavLink>
      <NavLink to="/biblioteca" className="nav-item">
        <FaSearch size={20} />
        <span>Explore</span>
      </NavLink>
      <NavLink to="/perfil" className="nav-item">
        <FaUser size={20} />
        <span>Profile</span>
      </NavLink>
    </nav>
  );
}

export default BottomNav;