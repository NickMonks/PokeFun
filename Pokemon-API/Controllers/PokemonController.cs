using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Pokemon_API.Data.Models;
using Pokemon_API.Entities;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Pokemon_API.Controllers
{
    [Route("pokemon")]
    [ApiController]
    public class PokemonController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IHttpClientFactory clientFactory;

        public PokemonController(IMapper mapper,
            IHttpClientFactory clientFactory)
        {
            this.mapper = mapper;
            this.clientFactory = clientFactory;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="pokemon"></param>
        /// <returns></returns>
        [HttpGet("{pokemon}")]
        public async Task<ActionResult<PokemonModel>> GetPokemon(string pokemon)
        {

            try
            {
                string uri = "https://pokeapi.co/api/v2/pokemon-species/" + pokemon;

                var request = new HttpRequestMessage(HttpMethod.Get, uri);

                var client = clientFactory.CreateClient();

                HttpResponseMessage response = await client.SendAsync(request);

                var result = await response.Content.ReadFromJsonAsync<Pokemon>();

                PokemonModel model = mapper.Map<PokemonModel>(result);

                // remove the symbols in the Description property:
                string newDescription = model.Description.Replace("\n", " ").Replace("\f"," ");
                
                model.Description = newDescription;

                return model;
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Cannot find pokemon {pokemon} - Did you type it wrong?");

            }


        }

    }
}
