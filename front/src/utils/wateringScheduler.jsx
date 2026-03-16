export function getNextWateringDate(plant) {
    const lastWatered = new Date(plant.lastWatered);
    return new Date(lastWatered.setDate(lastWatered.getDate() + plant.wateringFrequency));
  }
  