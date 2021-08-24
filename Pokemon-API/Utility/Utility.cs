using Pokemon_API.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pokemon_API.Utilities
{
    public class Utility
    {

        /// <summary>
        /// Removes some of the control characters (i.e. /n, /r) found in the API response. 
        /// </summary>
        /// <param name="model"></param>
        public static void RemoveSpecialCharacters(PokemonModel model)
        {
            string newDescription = model.Description.Replace("\n", " ").Replace("\f", " ");

            model.Description = newDescription;
        }
    }
}
