// app.js

// When the user scrolls down 20px from the top of the document, show the button
window.onscroll = function () {
  scrollFunction();
};

function scrollFunction() {
  const backToTopButton = document.getElementById("btn-back-to-top");
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

function showModal(modalId) {
  const modal = new bootstrap.Modal(document.getElementById(modalId), {});
  modal.show(); 
}

function hideModal(modalId) {
  const modal = bootstrap.Modal.getInstance(document.getElementById(modalId));
  modal.hide();
}

function setInnerHtml(id, html) {
  let container = document.getElementById(id);
  container.innerHTML = html; 
}

// Methods for Icons page.
// TODO: CLEAN UP THESE METHODS
function extractClasses(lib) {
  const deviconClassRegex = /\.devicon-([a-z0-9-]+)-plain/g;
  const fontawesomeClassRegex = /\.fa-([a-z0-9-]+)/g;
  const bootstrapClassRegex = /\.bi-([a-z0-9-]+)-fill/g;
  const boxiconsClassRegex = /\.bxs-([a-z0-9-]+)/g;
  const iconoirClassRegex = /\.iconoir-([a-z0-9-]+)/g;

  let rx = deviconClassRegex; // devicon
  if (lib == 'fa') {
    rx = fontawesomeClassRegex; // fontawesome
  }
  else if (lib == 'bi') {
    rx = bootstrapClassRegex; // bootstrap
  }
  else if (lib == 'bx') {
    rx = boxiconsClassRegex; // boxicons
  }
  else if (lib == 'iconoir') {
    rx = iconoirClassRegex; // iconoir
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
  const container = document.getElementById("iconsDiv");
  container.innerHTML = "";
  const iconClasses = extractClasses(lib);
  iconClasses.forEach(iconClass => {
    const iconBox = document.createElement("div");
    iconBox.classList.add("icon-box");
    iconBox.classList.add("text-bg-light");
    //iconBox.classList.add("d-flex");
    //iconBox.classList.add("align-items-center");
    //iconBox.classList.add("justify-content-center");

    const iconElement = document.createElement("i");
    let cn = iconClass + " colored"; // devicon
    if (lib == "fa") {
      cn = "fas " + iconClass; // fontawesome
    }
    else if (lib == "bi") {
      cn = "bi " + iconClass; // bootstrap
    }
    else if (lib == "bx") {
      cn = "bx " + iconClass; // boxicons
    }
    else if (lib == "iconoir") {
      cn = "" + iconClass; // iconoir
      iconElement.style.maxWidth = "38px";
      iconElement.style.marginLeft = "30px";
    }
    iconElement.className = cn;

    iconBox.onclick = function () {
      IconClassToClipboard(cn);
    };

    const nameElement = document.createElement("div");
    nameElement.classList.add("icon-name");
    nameElement.textContent = iconClass.replace("devicon-", "").replace(" colored", "").replace("-plain", "").replace("bi-", "").replace("-fill", "").replace("fa-", "").replace("bxs-", "").replace("iconoir-", "");

    iconBox.appendChild(iconElement);
    iconBox.appendChild(nameElement);
    container.appendChild(iconBox);
  });
}

function IconClassToClipboard(iconClass) {
  if (iconClass) {
    /* Alert the copied text */
    const toastDiv = document.querySelector('.toast');
    toastDiv.innerHTML = "<div class='" + iconClass + " fs-4 d-inline me-2'></div> <div class='fs-4 d-inline'>Copied to clipboard!</div>"
    const toast = new bootstrap.Toast(toastDiv);

    const iconname = iconClass.replace("devicon-", "").replace("-plain", "").replace("bi-", "").replace("fa-", "").replace(" colored", "");
    iconClass = "<i class='" + iconClass + "'></i> " + iconname 
    navigator.clipboard.writeText(iconClass);

    toast.show();
  }
} 

function getIconsClasses() {
  //const fa = extractClasses("fa");
  const devicon = extractClasses("devicon");
  const bi = extractClasses("bi");
  const iconClasses = [...bi, ...devicon]; // [...fa, ...bi, ...devicon];

  iconClasses.forEach((value, index, array) => {
    if (array[index].startsWith("fa")) {
      array[index] = "fas " + value;
    }
    else if (array[index].startsWith("bi")) {
      array[index] = "bi " + value;
    }
    else {
      array[index] = value + " colored";
    }
  });

  return iconClasses;
}
