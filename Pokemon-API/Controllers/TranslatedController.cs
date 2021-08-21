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

namespace Pokemon_API.Controllers
{
    [Route("pokemon/translated/{pokemon}")]
    [ApiController]
    public class TranslatedController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IHttpClientFactory clientFactory;

        public TranslatedController(IMapper mapper,
            IHttpClientFactory clientFactory)
        {
            this.mapper = mapper;
            this.clientFactory = clientFactory;
        }

        [HttpGet]
        public async Task<ActionResult<PokemonModel>> Get(string pokemon)
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
                string descriptionWithoutSymbol = model.Description.Replace("\n", " ").Replace("\f", " ");
                model.Description = descriptionWithoutSymbol;

                // perform business logic of the API - If the habitat is cave, or is_legendary returns true, then we use the yoga translation.
                // otherwise, we use the shakespeare translation:
                if (model.Habitat == "cave" || model.Is_legendary)
                {
                    var translatedDescription = await Translator(model.Description, "yoda", clientFactory);
                    model.Description = translatedDescription;
                } 
                else
                {
                    var translatedDescription = await Translator(model.Description, "shakespeare", clientFactory);
                    model.Description = translatedDescription;
                }

                ///TODO: check if it fails in case is empty
                
                return model;

            }
            catch (Exception)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Cannot translate the pokemon description");

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
            catch (Exception)
            {

                // If method fails (for whatever reason :-) ), then do not perform a translation at all
                // and return normal description
                return description;
            }



        }

       
    }
}
