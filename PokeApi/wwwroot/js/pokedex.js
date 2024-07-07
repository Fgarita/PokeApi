document.getElementById('search-btn').addEventListener('click', function () {
    const pokemonId = document.getElementById('pokemon-id').value;
    if (pokemonId) {
        window.location.href = `/Home/Search/${pokemonId}`;
    }
});

function toggleLights() {
    const lights = document.querySelectorAll('.light');
    lights.forEach(light => light.classList.toggle('active'));
}

setInterval(toggleLights, 1000);
