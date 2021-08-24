using AutoMapper;
using Pokemon_API.Data.Models;
using Pokemon_API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Pokemon_API.Resources
{
    public interface IPokemonAsync
    {

        /// <summary>
        /// Creates the Pokemon Model object, using the IHttpClientFactory interface to access and request the body response from a 3rd party API
        /// And also needs AutoMapper to map the Pokemon Entity to our Pokemon Model. 
        /// </summary>
        /// <param name="pokemon"></param>
        /// <param name="mapper"></param>
        /// <param name="clientFactory"></param>
        /// <returns></returns>
        Task<Pokemon> GetPokemonModelAsync(string pokemon, IHttpClientFactory clientFactory);
        PokemonModel MapPokemonModel(IMapper mapper, Pokemon result);
    }
}
