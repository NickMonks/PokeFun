using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pokemon_API.Exceptions
{
    // Defining a series of classes which are custom exceptions for our API. 
    public class PokemonNotFoundException : Exception
    {
        public PokemonNotFoundException()
        {
        }

        public PokemonNotFoundException(string message)
            : base(message)
        {
        }

        public PokemonNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
    
    public class PokemonTranslateAPIExhaustion : Exception
    {
        public PokemonTranslateAPIExhaustion()
        {
        }

        public PokemonTranslateAPIExhaustion(string message)
            : base(message)
        {
        }

        public PokemonTranslateAPIExhaustion(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
