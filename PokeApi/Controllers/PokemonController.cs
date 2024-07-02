using Microsoft.AspNetCore.Mvc;
using PokeApi.Models;
using PokeApi.Services;

namespace PokeApi.Controllers
{
    public class PokemonController : Controller
    {
            private readonly PokemonService _pokemonService;

            public PokemonController(PokemonService pokemonService)
            {
                _pokemonService = pokemonService;
            }

            [HttpGet]
            public IActionResult Index()
            {
                return View();
            }

            [HttpPost]
            public async Task<IActionResult> Index(int id)
            {
                Pokemon pokemon = await _pokemonService.GetPokemonAsync(id);
                return View(pokemon);
            }
    }
}
