import React, { useState } from 'react';
import { Link } from 'react-router-dom';
import "../models/Chatbot.css";

function PlantConsultantAI() {
  const [messages, setMessages] = useState([]);
  const [input, setInput] = useState('');

  const sendMessage = async () => {
    if (!input.trim()) return;

    // Agregar el mensaje del usuario al chat
    const newMessages = [...messages, { text: input, sender: 'user' }];
    setMessages(newMessages);

    try {
      const response = await fetch('http://localhost:5085/api/chatbot/ask?accessToken=AIzaSyC28Qojz6ws3C972l-tjQADeWUZ_YatSX0', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({
          contents: [
            {
              parts: [{ text: input }]
            }
          ]
        }) // Enviar el texto de entrada al backend en el formato esperado
      });

      const data = await response.json();
      console.log("Respuesta de la API:", data);

      // Verificar si hay candidatos en la respuesta
      if (data.candidates && data.candidates.length > 0) {
        const firstCandidate = data.candidates[0]; // Tomar el primer candidato
        const answer = firstCandidate.content.parts.map(part => part.text).join(' '); // Combinar los textos de las partes

        setMessages(prev => [...prev, { text: answer, sender: 'bot' }]);
      } else {
        setMessages(prev => [...prev, { text: 'No se encontró una respuesta del bot.', sender: 'bot' }]);
      }
    } catch (error) {
      console.error("Error al conectar con la API:", error);
      setMessages(prev => [...prev, { text: 'Error al conectar con la IA.', sender: 'bot' }]);
    }

    setInput('');
  };

  return (
    <div>
      <div className='calendar-container'>
        <header className="calendar-header">
          <Link to="/"><button className="back-button">←</button></Link>
          <h2>Consultor Plantie</h2>
          <button className="settings-button">⚙️</button>
        </header>
      </div>

      <div className="chatbot-container">
        <div className="chatbot-messages">
          {messages.map((msg, idx) => (
            <div key={idx} className={`message ${msg.sender}`}>
              {msg.text}
            </div>
          ))}
        </div>

        <div className="chatbot-input">
          <input
            type="text"
            placeholder="Preguntá algo sobre tus plantas..."
            value={input}
            onChange={(e) => setInput(e.target.value)}
            onKeyDown={(e) => e.key === "Enter" && sendMessage()}
          />
          <button onClick={sendMessage}>Enviar</button>
        </div>
      </div>
    </div>
  );
}

export default PlantConsultantAI;