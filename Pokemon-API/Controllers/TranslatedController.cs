using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Pokemon_API.Data.Models;
using Pokemon_API.Entities;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System.Text.Json;
using Pokemon_API.Models;
using System.Text;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using Pokemon_API.Resources;
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

        public TranslatedController(
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

        [HttpGet]
        public async Task<ActionResult<PokemonModel>> Get(string pokemon)
        {
            try
            {

                pokemon = pokemon.ToLower();
                Pokemon entity = await _pokemonAsync.GetPokemonModelAsync(pokemon, _clientFactory);
                PokemonModel model = _pokemonAsync.MapPokemonModel(_mapper, entity);
                Utility.RemoveSpecialCharacters(model);
                await TranslateModel(model);

                ///TODO: check if it fails in case is empty

                return model;

            }
            catch (Exception ex)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Cannot translate the pokemon description");

            }
        }

        private async Task TranslateModel(PokemonModel model)
        {
            // perform business logic of the API - If the habitat is cave, or is_legendary returns true, then we use the yoga translation.
            // otherwise, we use the shakespeare translation:
            if (model.Habitat == "cave" || model.Is_legendary)
            {
                var translatedDescription = await Translator(model.Description, "yoda", _clientFactory);
                model.Description = translatedDescription;
            }
            else
            {
                var translatedDescription = await Translator(model.Description, "shakespeare", _clientFactory);
                model.Description = translatedDescription;
            }
        }

        public static async Task<string> Translator(string description, string translationType, IHttpClientFactory clientFactory)
        {
            try
            {
                var requestDescription = new TranslateRequest(description);

                //Serialise request Description Entity:
                string json = JsonConvert.SerializeObject(requestDescription);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                //Define client and send POST request with Description payload:
                string url = "https://api.funtranslations.com/translate/" + translationType;
                HttpClient client = clientFactory.CreateClient();

                var response = await client.PostAsync(url, data);

                var result = await response.Content.ReadFromJsonAsync<TranslateResponse>();
                string newDescription = result.contents.translated;

                return newDescription;
            }
            catch (Exception ex)
            {

                // If method fails (for whatever reason :-) ), then do not perform a translation at all
                // and return normal description
                return description;
            }



        }

       
    }
}
