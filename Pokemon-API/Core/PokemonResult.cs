using AutoMapper;
using Microsoft.Extensions.Configuration;
using Pokemon_API.Data.Models;
using Pokemon_API.Entities;
using Pokemon_API.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon_API.Core
{
    public class PokemonResult : IPokemonAsync
    {

        private readonly IConfiguration _configuration;

        public PokemonResult(
            IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public async Task<Pokemon> GetPokemonModelAsync(string pokemon, IHttpClientFactory _clientFactory)
        {
            string uri = _configuration.GetValue<string>("pokemon") + pokemon;
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            var client = _clientFactory.CreateClient("pokemon");
            HttpResponseMessage response = await client.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                throw new PokemonNotFoundException(await response.Content.ReadAsStringAsync());

            var result = await response.Content.ReadFromJsonAsync<Pokemon>();
            return result;
        }

        

        public PokemonModel MapPokemonModel(IMapper _mapper, Pokemon result)
        {
            return _mapper.Map<PokemonModel>(result);
        }
    }
}
