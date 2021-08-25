using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Pokemon_API.Data.Models;
using Pokemon_API.Entities;
using Pokemon_API.Core;
using Pokemon_API.Utilities;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon_API.Controllers
{

    [Route("pokemon")]
    [ApiController]
    public class PokemonController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;
        private readonly IPokemonAsync _pokemonAsync;

        public PokemonController(
            IMapper mapper,
            IHttpClientFactory clientFactory,
            IConfiguration configuration,
            IPokemonAsync pokemonAsync)
        {
            this._mapper = mapper;
            this._clientFactory = clientFactory;
            this._configuration = configuration;
            this._pokemonAsync = pokemonAsync;
        }


        /// <summary>
        /// Async method that runs an HTTP GET method to retrieve the relevant resources
        /// inside the pokemon API call.In order to use it, it needs a query parameter - the pokemon name.
        /// It uses an HttpClient object to call the 3rd party API. 
        /// </summary>
        /// <param name="pokemon">The name of the Pokemon the client wants to find</param>
        /// <returns>PokemonModel instance</returns>

        [HttpGet("{pokemon}")]
        public async Task<ActionResult<PokemonModel>> GetPokemon(string pokemon)
        {
            // Ensure that it works even if the user uses uppercase
            pokemon = pokemon.ToLower();
            Pokemon entity = await _pokemonAsync.GetPokemonModelAsync(pokemon, _clientFactory);
            PokemonModel model = _pokemonAsync.MapPokemonModel(_mapper, entity);
            Utility.RemoveSpecialCharacters(model);

            return model;
        }
    }
}
