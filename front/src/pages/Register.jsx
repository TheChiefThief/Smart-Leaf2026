import React, { useState } from "react";
import '../models/Register.css';
import { PageHead } from "../components/";
import Container from "../components/Container";
import ButtonX from "../components/ButtonX";
import { Eye, EyeOff } from "lucide-react";
import { register } from "../services/authServices";
import { useNavigate } from "react-router-dom";
function Register() {
  const [formData, setFormData] = useState({
    fullName: '',
    username: '',
    email: '',
    password: '',
    confirmPassword: ''
  });

  const [showPassword, setShowPassword] = useState(false);
  const [showConfirm, setShowConfirm] = useState(false);
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();
 
  // Manejar cambios en los campos del formulario
  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  // Función para enviar datos a la base de datos
  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      await register(formData.fullName, formData.username, formData.email, formData.password);
      alert("¡Registro exitoso!");
      navigate("/login"); // <-- Redirige al login
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  return (
    <Container>
      <div className="app">
        <PageHead  />

        {error && <p className="error-message">{error}</p>}

        <form className="register-form" onSubmit={handleSubmit}>
          <input
            type="text"
            name="fullName"
            placeholder="Full Name"
            value={formData.fullName}
            onChange={handleChange}
            className="form-input"
          />

          <input
            type="text"
            name="username"
            placeholder="Username"
            value={formData.username}
            onChange={handleChange}
            className="form-input"
          />

          <input
            type="email"
            name="email"
            placeholder="Email"
            value={formData.email}
            onChange={handleChange}
            className="form-input"
          />

          <div className="form-input password-field">
            <input
              type={showPassword ? "text" : "password"}
              name="password"
              placeholder="Password"
              value={formData.password}
              onChange={handleChange}
            />
            <button
              type="button"
              onClick={() => setShowPassword(!showPassword)}
              className="eye-button"
            >
              {showPassword ? <EyeOff size={18} /> : <Eye size={18} />}
            </button>
          </div>

          <div className="form-input password-field">
            <input
              type={showConfirm ? "text" : "password"}
              name="confirmPassword"
              placeholder="Confirm Password"
              value={formData.confirmPassword}
              onChange={handleChange}
            />
            <button
              type="button"
              onClick={() => setShowConfirm(!showConfirm)}
              className="eye-button"
            >
              {showConfirm ? <EyeOff size={18} /> : <Eye size={18} />}
            </button>
          </div>

          <button
            type="submit"
            className="register-button"
            disabled={loading}
          >
            {loading ? "Registrando..." : "Register"}
          </button>
        </form>

        <p className="alt-login">O continúa de otra manera</p>

        <div className="button-group">
          <ButtonX text="Continuar con Google" />
          <ButtonX text="Continuar con Apple" />
          <ButtonX text="Continuar con Facebook" />
        </div>

        <p className="privacy-text">
          Al continuar, aceptas los{" "}
          <a href="#" className="link">Términos de servicio</a> y la{" "}
          <a href="#" className="link">Política de privacidad</a>. Lee nuestro{" "}
          <a href="#" className="link">Aviso de Privacidad</a>.
        </p>
      </div>
    </Container>
  );
}

export default Register;