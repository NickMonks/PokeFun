using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Pokemon_API.Exceptions
{
    public class ExceptionPokemon
    {
        public string Message { get; set; }

        public int? StatusCode { get; set; }

        public ExceptionPokemon(string message)
        {
            Message = message;
        }

      
    }
}
