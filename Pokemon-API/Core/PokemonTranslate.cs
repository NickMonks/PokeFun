using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Pokemon_API.Data.Models;
using Pokemon_API.Core;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon_API.Core
{
    public class PokemonTranslate : IPokemonTranslate
    {

        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;
        private readonly IPokemonAsync _pokemonAsync;

        public PokemonTranslate(
            IHttpClientFactory clientFactory,
            IConfiguration configuration,
            IPokemonAsync pokemonAsync)
        {
            this._clientFactory = clientFactory;
            this._configuration = configuration;
            this._pokemonAsync = pokemonAsync;
        }

        public async Task TranslateModel(PokemonModel model)
        {
            // Hard requirement - the client forces this business logic, so we can hard-code these fields.
            // When the habitat is cave, or the pokemon is a legendary one, call yoda translation. Otherwise, call
            // the shakespeare one. 
            string translatedDescription;

            if (model.Habitat == "cave" || model.Is_legendary)
            {
                translatedDescription = await Translation(model.Description, "yoda", _clientFactory);
            }
            else
            {
                translatedDescription = await Translation(model.Description, "shakespeare", _clientFactory);
            }

            model.Description = translatedDescription;
        }

        public async Task<string> Translation(string description, string translationType, IHttpClientFactory clientFactory)
        {
            try
            {
                // The API needs a body request with just one field: Text. We will create an instance
                // of the Translate Request in case we need in the future to extend this:
                var requestDescription = new TranslateRequest(description);

                //Serialise request Description Entity:
                string json = JsonConvert.SerializeObject(requestDescription);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                //Define client and send POST request with Description payload:
                string uri = _configuration.GetValue<string>("translation") + translationType;
                HttpClient client = clientFactory.CreateClient();
                var response = await client.PostAsync(uri, data);

                // wait for response from the server:
                var result = await response.Content.ReadFromJsonAsync<TranslateResponse>();
                string newDescription = result.contents.translated;

                return newDescription;
            }
            catch (Exception)
            {
                // If method fails then do not perform a translation at all and return normal description
                return description;
            }



        }
    }
}