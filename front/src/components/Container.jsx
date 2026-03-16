import React from "react";
import '../models/Login.css';

function Container({ children }) {
  return (
    <div className="container">
      {children}
    </div>
  );
}

export default Container;