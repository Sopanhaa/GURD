const api = {
  games: "/games",
  genres: "/genres"
};

const elements = {
  form: document.querySelector("#gameForm"),
  formTitle: document.querySelector("#formTitle"),
  gameId: document.querySelector("#gameId"),
  name: document.querySelector("#nameInput"),
  genre: document.querySelector("#genreInput"),
  price: document.querySelector("#priceInput"),
  releaseDate: document.querySelector("#releaseDateInput"),
  saveButton: document.querySelector("#saveButton"),
  cancelEditButton: document.querySelector("#cancelEditButton"),
  refreshButton: document.querySelector("#refreshButton"),
  tableBody: document.querySelector("#gamesTableBody"),
  gameCount: document.querySelector("#gameCount"),
  status: document.querySelector("#statusMessage")
};

let games = [];
let genres = [];

function setStatus(message, isError = false) {
  elements.status.textContent = message;
  elements.status.style.color = isError ? "#b42335" : "";
}

function formatPrice(value) {
  return new Intl.NumberFormat("en-US", {
    style: "currency",
    currency: "USD"
  }).format(value);
}

function formatDate(value) {
  return new Intl.DateTimeFormat("en-US", {
    year: "numeric",
    month: "short",
    day: "numeric"
  }).format(new Date(`${value}T00:00:00`));
}

async function requestJson(url, options = {}) {
  const response = await fetch(url, {
    headers: {
      "Content-Type": "application/json",
      ...options.headers
    },
    ...options
  });

  if (!response.ok) {
    throw new Error(`Request failed with status ${response.status}`);
  }

  if (response.status === 204) {
    return null;
  }

  return response.json();
}

function renderGenres() {
  const currentValue = elements.genre.value;
  elements.genre.innerHTML = '<option value="">Choose a genre</option>';

  for (const genre of genres) {
    const option = document.createElement("option");
    option.value = genre.name;
    option.textContent = genre.name;
    elements.genre.append(option);
  }

  elements.genre.value = currentValue;
}

function renderGames() {
  elements.tableBody.innerHTML = "";
  elements.gameCount.textContent = `${games.length} ${games.length === 1 ? "game" : "games"}`;

  if (games.length === 0) {
    const row = document.createElement("tr");
    row.innerHTML = '<td class="empty-state" colspan="5">No games yet.</td>';
    elements.tableBody.append(row);
    return;
  }

  for (const game of games) {
    const row = document.createElement("tr");

    row.innerHTML = `
      <td>${game.name}</td>
      <td>${game.genre}</td>
      <td>${formatPrice(game.price)}</td>
      <td>${formatDate(game.releaseDate)}</td>
      <td>
        <div class="row-actions">
          <button class="table-action" type="button" data-action="edit" data-id="${game.id}">Edit</button>
          <button class="table-action danger" type="button" data-action="delete" data-id="${game.id}">Delete</button>
        </div>
      </td>
    `;

    elements.tableBody.append(row);
  }
}

function resetForm() {
  elements.form.reset();
  elements.gameId.value = "";
  elements.formTitle.textContent = "Add game";
  elements.saveButton.textContent = "Save game";
  elements.cancelEditButton.classList.add("hidden");
}

function editGame(game) {
  elements.gameId.value = game.id;
  elements.name.value = game.name;
  elements.genre.value = game.genre;
  elements.price.value = game.price;
  elements.releaseDate.value = game.releaseDate;
  elements.formTitle.textContent = "Edit game";
  elements.saveButton.textContent = "Update game";
  elements.cancelEditButton.classList.remove("hidden");
  elements.name.focus();
}

async function loadData() {
  setStatus("Loading...");

  try {
    const [loadedGames, loadedGenres] = await Promise.all([
      requestJson(api.games),
      requestJson(api.genres)
    ]);

    games = loadedGames;
    genres = loadedGenres;
    renderGenres();
    renderGames();
    setStatus("Ready");
  } catch (error) {
    setStatus("Could not reach the API", true);
  }
}

async function saveGame(event) {
  event.preventDefault();

  const id = elements.gameId.value;
  const payload = {
    name: elements.name.value.trim(),
    genre: elements.genre.value,
    price: Number(elements.price.value),
    releaseDate: elements.releaseDate.value
  };

  if (!payload.name || !payload.genre || !payload.releaseDate) {
    setStatus("Complete the form first", true);
    return;
  }

  try {
    setStatus(id ? "Updating..." : "Saving...");

    await requestJson(id ? `${api.games}/${id}` : api.games, {
      method: id ? "PUT" : "POST",
      body: JSON.stringify(payload)
    });

    resetForm();
    await loadData();
    setStatus(id ? "Game updated" : "Game added");
  } catch (error) {
    setStatus("Save failed", true);
  }
}

async function deleteGame(id) {
  try {
    setStatus("Deleting...");
    await requestJson(`${api.games}/${id}`, { method: "DELETE" });
    await loadData();
    setStatus("Game deleted");
  } catch (error) {
    setStatus("Delete failed", true);
  }
}

elements.form.addEventListener("submit", saveGame);
elements.cancelEditButton.addEventListener("click", resetForm);
elements.refreshButton.addEventListener("click", loadData);

elements.tableBody.addEventListener("click", (event) => {
  const button = event.target.closest("button[data-action]");

  if (!button) {
    return;
  }

  const id = Number(button.dataset.id);
  const game = games.find((item) => item.id === id);

  if (button.dataset.action === "edit" && game) {
    editGame(game);
  }

  if (button.dataset.action === "delete") {
    deleteGame(id);
  }
});

loadData();
