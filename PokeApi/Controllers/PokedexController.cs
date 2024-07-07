using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PokeApi.Models;

namespace Pokedex.Controllers
{
    public class PokedexController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Search(string query)
        {
            if (query == null)
            {
                ViewBag.ErrorMessage = "El nombre o ID del Pokémon no puede estar vacío.";
                return View("Error");
            }

            var pokemon = await GetPokemonAsync(query);
            if (pokemon == null)
            {
                ViewBag.ErrorMessage = $"No se encontró ningún Pokémon con el nombre o ID ({query}).";
                return View("Error");
            }
            return View("Details", pokemon);
        }

        private async Task<Pokemon> GetPokemonAsync(string query)
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    // Obtener datos básicos del Pokémon
                    var response = await httpClient.GetStringAsync($"https://pokeapi.co/api/v2/pokemon/{query.ToLower()}");
                    var json = JObject.Parse(response);

                    if (json["detail"] != null)
                    {
                        // Si la API devuelve un detalle de error, no se encontró el Pokémon
                        return null;
                    }

                    var pokemon = new Pokemon
                    {
                        Id = (int)json["id"],
                        Name = (string)json["name"],
                        ImageUrl = (string)json["sprites"]["front_default"],
                        Type = string.Join(", ", json["types"].Select(t => (string)t["type"]["name"])),
                        Height = (int)json["height"],
                        Weight = (int)json["weight"],
                        HP = (int)json["stats"][0]["base_stat"],
                        Attack = (int)json["stats"][1]["base_stat"],
                        Defense = (int)json["stats"][2]["base_stat"],
                        Speed = (int)json["stats"][5]["base_stat"]
                    };

                    // Obtener habilidades en español
                    var abilitiesUrls = json["abilities"].Select(a => (string)a["ability"]["url"]);
                    foreach (var abilityUrl in abilitiesUrls)
                    {
                        var abilityResponse = await httpClient.GetStringAsync(abilityUrl);
                        var abilityJson = JObject.Parse(abilityResponse);
                        var abilityName = (string)abilityJson["names"].First(n => (string)n["language"]["name"] == "es")["name"];
                        pokemon.Abilities.Add(abilityName);
                    }

                    // Obtener estadísticas base
                    pokemon.BaseStats.AddRange(json["stats"].Select(s => (int)s["base_stat"]));

                    // Obtener descripción (de otro endpoint)
                    var speciesUrl = (string)json["species"]["url"];
                    var speciesResponse = await httpClient.GetStringAsync(speciesUrl);
                    var speciesJson = JObject.Parse(speciesResponse);
                    var flavorTextEntries = speciesJson["flavor_text_entries"];
                    foreach (var entry in flavorTextEntries)
                    {
                        var language = (string)entry["language"]["name"];
                        if (language == "es") // Suponiendo que queremos la descripción en inglés
                        {
                            pokemon.Description = (string)entry["flavor_text"];
                            break;
                        }
                    }

                    // Obtener movimientos en español (limitado a los primeros 5 movimientos en este ejemplo)
                    var movesUrls = json["moves"].Select(m => (string)m["move"]["url"]).Take(5);
                    foreach (var moveUrl in movesUrls)
                    {
                        var moveResponse = await httpClient.GetStringAsync(moveUrl);
                        var moveJson = JObject.Parse(moveResponse);
                        var moveName = (string)moveJson["names"].First(n => (string)n["language"]["name"] == "es")["name"];
                        pokemon.Moves.Add(moveName);
                    }

                    // Obtener evoluciones (si hay información de cadena de evolución)
                    var evolutionChainUrl = (string)speciesJson["evolution_chain"]["url"];
                    var evolutionChainResponse = await httpClient.GetStringAsync(evolutionChainUrl);
                    var evolutionChainJson = JObject.Parse(evolutionChainResponse);
                    var evolutionChain = GetEvolutionChain(evolutionChainJson);
                    pokemon.EvolutionChain.AddRange(evolutionChain);

                    // Obtener información de localización
                    var locationAreaEncounters = (string)json["location_area_encounters"];
                    if (!string.IsNullOrEmpty(locationAreaEncounters))
                    {
                        var locationResponse = await httpClient.GetStringAsync(locationAreaEncounters);
                        var locationJson = JArray.Parse(locationResponse);
                        var locations = locationJson.Select(l => (string)l["location_area"]["name"]).Distinct().ToList();
                        pokemon.Locations.AddRange(locations);
                    }

                    return pokemon;
                }
                catch (HttpRequestException)
                {
                    // Manejo de error de solicitud HTTP (por ejemplo, conexión fallida)
                    ViewBag.ErrorMessage = "Error al conectar con la API de Pokémon.";
                    return null;
                }
                catch (Exception ex)
                {
                    // Otros errores inesperados
                    ViewBag.ErrorMessage = $"Error: {ex.Message}";
                    return null;
                }
            }
        }

        private List<string> GetEvolutionChain(JObject evolutionChainJson)
        {
            List<string> evolutionChain = new List<string>();

            // Recorrer la cadena de evolución y obtener los nombres de los Pokémon involucrados
            var chain = evolutionChainJson["chain"];
            while (chain != null)
            {
                var speciesName = (string)chain["species"]["name"];
                evolutionChain.Add(speciesName);

                // Avanzar al siguiente Pokémon en la cadena de evolución
                chain = chain["evolves_to"].FirstOrDefault();
            }

            return evolutionChain;
        }



    }
}



