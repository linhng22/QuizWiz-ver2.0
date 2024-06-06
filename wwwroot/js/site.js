var currentCardIndex = 0;
var flashcards = window.flashcards || [];

var currentFlashcardElement = document.querySelector(
  "#flashcard-count #current-flashcard-id"
);
var headerElement = document.querySelector("#flashcardHeader");

function displayFlashcard(index) {
  var flashcard = flashcards[index];
  var questionElement = document.querySelector("#flashcard .question");
  var answerElement = document.querySelector("#flashcard .answer");

  questionElement.textContent = flashcard.question;
  answerElement.textContent = flashcard.answer;

  // Initially show the question and hide the answer
  headerElement.innerText = "Question";
  answerElement.style.display = "none";
  questionElement.style.display = "block";
  currentFlashcardElement.innerText = currentCardIndex + 1;
}

function toggleFlashcard() {
  var headerElement = document.querySelector("#flashcardHeader");
  var questionElement = document.querySelector("#flashcard .question");
  var answerElement = document.querySelector("#flashcard .answer");

  // If the answer is hidden, show it and hide the question
  if (answerElement.style.display === "none") {
    headerElement.innerText = "Answer";
    answerElement.style.display = "block";
    questionElement.style.display = "none";
  }
  // If the answer is shown, hide it and show the question
  else {
    headerElement.innerText = "Question";
    answerElement.style.display = "none";
    questionElement.style.display = "block";
  }
}

function nextFlashcard() {
  console.log("Next flashcard called");
  if (currentCardIndex < flashcards.length - 1) {
    currentCardIndex++;
    displayFlashcard(currentCardIndex);
  }
}

function previousFlashcard() {
  console.log("Previous flashcard called");
  if (currentCardIndex > 0) {
    currentCardIndex--;
    displayFlashcard(currentCardIndex);
  }
}

// Set the focus on the flashcard div element
window.onload = function () {
  document.getElementById("flashcard").focus();
};

// Handle keydown events on `flashcard` div
function checkKey(event) {
  if (event.key === "ArrowLeft") {
    previousFlashcard();
  } else if (event.key === "ArrowRight") {
    nextFlashcard();
  } else if (event.key === " ") {
    event.preventDefault(); // avoid page scrolling
    toggleFlashcard();
  }
}

// Display the first flashcard initially
displayFlashcard(currentCardIndex);
