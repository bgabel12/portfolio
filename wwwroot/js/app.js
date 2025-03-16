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

// Methods for Icons page.
function extractClasses(lib) {
  const deviconClassRegex = /\.devicon-([a-z0-9-]+)-plain/g;
  const fontawesomeClassRegex = /\.fa-([a-z0-9-]+)/g;
  const bootstrapClassRegex = /\.bi-([a-z0-9-]+)/g;

  var rx = deviconClassRegex;
  if (lib == 'fa') {
    rx = fontawesomeClassRegex;
  }
  else if (lib == 'bi') {
    rx = bootstrapClassRegex;
  }

  // Iterate through all stylesheets
  const classes = new Set();
  for (let sheet of document.styleSheets) {
    try {
      for (let rule of sheet.cssRules) {
        let matches = [...rule.cssText.matchAll(rx)];
        matches.forEach(match => classes.add(match[0].replace('.', '')));
      }
    } catch (e) {
      // Ignore cross-origin stylesheet errors
    }
  }

  return Array.from(classes).sort();
}

function displayIcons(lib) {
  document.getElementById("diContainer").innerHTML = "";
  document.getElementById("faContainer").innerHTML = "";
  document.getElementById("biContainer").innerHTML = "";
  var containerName = lib + "Container";

  const container = document.getElementById(containerName);
  const iconClasses = extractClasses(lib);
  iconClasses.forEach(iconClass => {
    const iconBox = document.createElement("div");
    iconBox.classList.add("icon-box");
    iconBox.classList.add("text-bg-light");

    const iconElement = document.createElement("i");
    var cn = iconClass + " colored"; // fs-1
    if (lib == "fa") {
      cn = "fas " + iconClass + ""; // fs-1
    }
    else if (lib == "bi") {
      cn = "bi " + iconClass + ""; // fs-1
    }
    iconElement.className = cn;

    iconBox.onclick = function () {
      IconClassToClipboard(cn);
    };

    const nameElement = document.createElement("div");
    nameElement.classList.add("icon-name");
    nameElement.textContent = iconClass.replace("devicon-", "").replace("-plain", "").replace("bi-", "").replace("fa-", "");

    iconBox.appendChild(iconElement);
    iconBox.appendChild(nameElement);
    container.appendChild(iconBox);
  });
}

function IconClassToClipboard(iconClass) {
  if (iconClass) {
    var iconname = iconClass.replace("devicon-", "").replace("-plain", "").replace("bi-", "").replace("fa-", "");
    iconClass = "<i class='" + iconClass + "'></i> " + iconname 
    navigator.clipboard.writeText(iconClass);

    /* Alert the copied text */
    const toastDiv = document.querySelector('.toast');
    //toastDiv.innerHTML = "" + iconClass + "<br/>" + "Copied to clipboard!";
    toastDiv.innerHTML = "<div class='" + iconClass +" fs-4 d-inline me-2'></div> <div class='fs-4 d-inline'>Copied to clipboard!</div>"
    const toast = new bootstrap.Toast(toastDiv);
    toast.show();
  }
} 
