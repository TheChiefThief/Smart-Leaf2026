import React, { useContext, useState } from "react";
import "../models/Calendar.css";
import { PageHead } from "../components/";
import { PlantContext } from "../context/PlantContext";

function Calendar() {
  const { plants, calculateWateringDates } = useContext(PlantContext);
  const [selectedDate, setSelectedDate] = useState(null); // Estado para el día seleccionado

  const today = new Date();
  const currentMonth = today.toLocaleString("default", { month: "long" });
  const currentYear = today.getFullYear();
  const daysInMonth = new Date(currentYear, today.getMonth() + 1, 0).getDate();
  const firstDayOfMonth = new Date(currentYear, today.getMonth(), 1).getDay();
  const blankDays = Array.from({ length: firstDayOfMonth });
  const monthDays = Array.from({ length: daysInMonth }, (_, i) => i + 1);

  const events = plants.flatMap((plant) => {
    const plantingEvent = {
      name: plant.name,
      action: "Plantada",
      date: plant.plantingDate,
      icon: "🌱",
    };

    const wateringEvents = calculateWateringDates(plant)
      .filter((date) => date) // Filtrar fechas válidas
      .map((date) => ({
        name: plant.name,
        action: "Regar",
        date,
        icon: "💧",
      }));

    return [plantingEvent, ...wateringEvents];
  });

  // Filtrar eventos según el día seleccionado
  const filteredEvents = selectedDate
    ? events.filter((event) => event.date === selectedDate)
    : [];

  const handleDayClick = (day) => {
    const formattedDate = `${currentYear}-${String(today.getMonth() + 1).padStart(2, "0")}-${String(day).padStart(2, "0")}`;
    setSelectedDate(formattedDate); // Actualizar el día seleccionado
  };

  return (
    <div className="calendar-container">
      <PageHead />
      <div className="calendar-month">
        <p>{`${currentMonth} ${currentYear}`}</p>
        <div className="calendar-grid">
          {["S", "M", "T", "W", "T", "F", "S"].map((day, idx) => (
            <div key={idx} className="calendar-day-label">
              {day}
            </div>
          ))}
          {blankDays.map((_, i) => (
            <div key={`blank-${i}`} className="calendar-day empty"></div>
          ))}
          {monthDays.map((day, i) => (
            <div
              key={`day-${i}`}
              className={`calendar-day ${
                selectedDate === `${currentYear}-${String(today.getMonth() + 1).padStart(2, "0")}-${String(day).padStart(2, "0")}` ? "selected" : ""
              }`}
              onClick={() => handleDayClick(day)} // Seleccionar el día
            >
              {day}
            </div>
          ))}
        </div>
      </div>

      <h3 className="plans-title">
        {selectedDate
          ? `Eventos para el ${selectedDate}`
          : "Selecciona un día para ver los eventos"}
      </h3>
      <div className="plans-list">
        {filteredEvents.length > 0 ? (
          filteredEvents.map((event, i) => (
            <div key={i} className="plan-card">
              <span className="icon">{event.icon}</span>
              <div className="plan-details">
                <strong>{event.name}</strong>
                <p>
                  {event.action}: {event.date}
                </p>
              </div>
              <span className="options">⋮</span>
            </div>
          ))
        ) : (
          selectedDate && <p>No hay eventos para este día.</p>
        )}
      </div>
    </div>
  );
}

export default Calendar;

