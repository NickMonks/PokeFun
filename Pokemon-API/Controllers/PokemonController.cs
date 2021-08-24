using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Pokemon_API.Data.Models;
using Pokemon_API.Entities;
using Pokemon_API.Resources;
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

            //try
            //{
                // Ensure that it works even if the user uses uppercase
                pokemon = pokemon.ToLower();
                Pokemon entity = await _pokemonAsync.GetPokemonModelAsync(pokemon, _clientFactory);
                PokemonModel model = _pokemonAsync.MapPokemonModel(_mapper, entity);
                Utility.RemoveSpecialCharacters(model);

                return model;
            //}
            //catch (Exception ex)
            //{
            //    return this.StatusCode(StatusCodes.Status500InternalServerError, $"Cannot find pokemon {pokemon} - Did you type it wrong?: {ex.Message}");

            //}


        }

        /// <summary>
        /// Creates the Pokemon Model object from a 3rd party API. Uses the HttpClient to send a GET request and retrieving the body response. 
        /// </summary>
        /// <param name="pokemon"></param>
        /// <returns></returns>
        private async Task<Pokemon> GetPokemonAsync(string pokemon)
        {
            string uri = _configuration.GetValue<string>("pokemon") + pokemon;

            var request = new HttpRequestMessage(HttpMethod.Get, uri);

            var client = _clientFactory.CreateClient(pokemon);

            HttpResponseMessage response = await client.SendAsync(request);

            var result = await response.Content.ReadFromJsonAsync<Pokemon>();
            return result;
        }

        private PokemonModel MapPokemonModel(Pokemon result)
        {
            return _mapper.Map<PokemonModel>(result);
        }
    }
}
