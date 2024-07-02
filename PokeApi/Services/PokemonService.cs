using Newtonsoft.Json;
using PokeApi.Models;

namespace PokeApi.Services
{
    public class PokemonService
    {
        private readonly HttpClient _httpClient;

        public PokemonService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Pokemon> GetPokemonAsync(int id)
        {
            var response = await _httpClient.GetAsync($"https://pokeapi.co/api/v2/pokemon/{id}");
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            var root = JsonConvert.DeserializeObject<Root>(responseBody);

            var pokemon = new Pokemon
            {
                Id = root.Id,
                Name = root.Name,
                Height = root.Height,
                Weight = root.Weight,
                BaseExperience = root.Base_Experience,
                Abilities = root.Abilities.Select(a => a.Ability.Name).ToList(),
                FrontDefaultImage = root.Sprites.Front_Default
            };

            return pokemon;
        }
    }
}
