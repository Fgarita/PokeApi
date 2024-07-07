using System;
using System.Collections.Generic;

namespace PokeApi.Models
{
    public class Pokemon
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string Type { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }
        public List<string> Abilities { get; set; } = new List<string>();
        public string Description { get; set; }
        public int HP { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int Speed { get; set; }
        public List<string> Moves { get; set; } = new List<string>(); // Propiedad para almacenar movimientos
        public List<string> EvolutionChain { get; set; } = new List<string>();
        public List<int> BaseStats { get; set; } = new List<int>();
        public List<string> Locations { get; set; } = new List<string>();
    }
}

