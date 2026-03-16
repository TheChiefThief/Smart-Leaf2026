import React from "react";
import "../models/utilitiesbottoms.css";

function ButtonX({ text, onClick, type = "button" }) {
  return (
    <button className="login-button" onClick={onClick} type={type}>
      {text}
    </button>
  );
}

export default ButtonX;