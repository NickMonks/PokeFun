

using Pokemon_API.Entities;

namespace Pokemon_API.Data.Models
{
   
    public class PokemonModel
    {
        public string Habitat { get; set; }

        public bool Is_legendary { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
