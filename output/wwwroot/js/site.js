document.getElementById('search-btn').addEventListener('click', function () {
    const pokemonId = document.getElementById('pokemon-id').value;
    if (pokemonId) {
        fetch(`https://pokeapi.co/api/v2/pokemon/${pokemonId}`)
            .then(response => response.json())
            .then(data => {
                document.getElementById('pokemon-name').textContent = data.name.toUpperCase();
                document.getElementById('pokemon-image').src = data.sprites.front_default;
                document.getElementById('pokemon-type').textContent = `Tipo: ${data.types.map(type => type.type.name).join(', ')}`;
                document.querySelector('.pokemon-info').style.display = 'block';
            })
            .catch(error => {
                alert('Pokémon no encontrado');
                console.error(error);
            });
    }
});

function toggleLights() {
    const lights = document.querySelectorAll('.light');
    lights.forEach(light => light.classList.toggle('active'));
}

setInterval(toggleLights, 1000);
