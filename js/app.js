// app.js

// When the user scrolls down 20px from the top of the document, show the button
window.onscroll = function () {
  scrollFunction();
};

function scrollFunction() {
  var backToTopButton = document.getElementById("btn-back-to-top");
  if (document.body.scrollTop > 20 || document.documentElement.scrollTop > 20) {
    backToTopButton.style.display = "block";
  } else {
    backToTopButton.style.display = "none";
  }
}

function backToTop() {
  document.body.scrollTop = 0;
  document.documentElement.scrollTop = 0;
}

// sets the given bootstrap theme
function setTheme(theme) {
  if (theme != 'light' && theme != 'dark') {
    console.error("Invalid theme: " + theme);
    theme = 'light';
  }

  document.documentElement.setAttribute("data-bs-theme", theme);
}

function showModal(name) {
  var modal = new bootstrap.Modal(document.getElementById(name), {});
  modal.show(); 
}
