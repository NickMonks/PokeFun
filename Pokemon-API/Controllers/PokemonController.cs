using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Pokemon_API.Data.Models;
using Pokemon_API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Pokemon_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly LinkGenerator linkGenerator;
        private readonly IHttpClientFactory clientFactory;

        public PokemonController(IMapper mapper, 
            LinkGenerator linkGenerator,
            IHttpClientFactory clientFactory)
        {
            this.mapper = mapper;
            this.linkGenerator = linkGenerator;
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

            string uri = "https://pokeapi.co/api/v2/pokemon-species/" + pokemon;
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            var client = clientFactory.CreateClient();

            try
            {
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
