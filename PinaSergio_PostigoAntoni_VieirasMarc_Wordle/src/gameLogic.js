let target;
let attempts = 0;

export async function fetchTargetCharacter() {
  try {
    const response = await fetch('https://localhost:7003/api/Character');
    if (!response.ok) {
      throw new Error('Error fetching target character');
    }
    const characters = await response.json();
    target = characters[Math.floor(Math.random() * characters.length)];
    console.log("Personaje objetivo:", target); // Solo para pruebas
  } catch (error) {
    console.error('Error:', error);
  }
}



export function compareAttributes(guess, target) {
  const feedback = [];
  const keys = ["name", "gender", "game", "origin", "race", "type"];
  keys.forEach((key) => {
    if (guess[key] === target[key]) {
      feedback.push("green"); // Coincidencia exacta
    } else {
      feedback.push("red"); // Sin coincidencia
    }
  });
  return feedback;
}

export function getTarget() {
  return target;
}

export function incrementAttempts() {
  attempts++;
}

export function getAttempts() {
  return attempts;
}