using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Pokemon_API.Data.Models;
using Pokemon_API.Entities;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Pokemon_API.Core;
using Pokemon_API.Utilities;

namespace Pokemon_API.Controllers
{
    [Route("pokemon/translated/{pokemon}")]
    [ApiController]
    public class TranslatedController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;
        private readonly IPokemonAsync _pokemonAsync;
        private readonly IPokemonTranslate _pokemonTranslate;

        public TranslatedController(
            IMapper mapper,
            IHttpClientFactory clientFactory,
            IConfiguration configuration,
            IPokemonAsync pokemonAsync,
            IPokemonTranslate pokemonTranslate)
        {
            this._mapper = mapper;
            this._clientFactory = clientFactory;
            this._configuration = configuration;
            this._pokemonAsync = pokemonAsync;
            this._pokemonTranslate = pokemonTranslate;
        }

        [HttpGet]
        public async Task<ActionResult<PokemonModel>> Get(string pokemon)
        { 

                pokemon = pokemon.ToLower();
                Pokemon entity = await _pokemonAsync.GetPokemonModelAsync(pokemon, _clientFactory);
                PokemonModel model = _pokemonAsync.MapPokemonModel(_mapper, entity);
                Utility.RemoveSpecialCharacters(model);

                // Perform translation:
                await _pokemonTranslate.TranslateModel(model);
                return model;
        }
       
    }
}
