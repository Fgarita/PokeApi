namespace PokeApi.Models
{
    public class Pokemon
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }
        public int BaseExperience { get; set; }
        public List<string> Abilities { get; set; }
        public string FrontDefaultImage { get; set; }
    }

    public class PokemonAbility
    {
        public AbilityDetail Ability { get; set; }
    }

    public class AbilityDetail
    {
        public string Name { get; set; }
    }

    public class Sprites
    {
        public string Front_Default { get; set; }
    }

    public class Root
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }
        public int Base_Experience { get; set; }
        public List<PokemonAbility> Abilities { get; set; }
        public Sprites Sprites { get; set; }
    }

}
