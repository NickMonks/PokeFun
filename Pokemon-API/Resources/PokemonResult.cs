using AutoMapper;
using Microsoft.Extensions.Configuration;
using Pokemon_API.Data.Models;
using Pokemon_API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon_API.Resources
{
    public class PokemonResult : IPokemonAsync
    {

        private readonly IMapper _mapper;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;

        public PokemonResult(
            IMapper mapper,
            IHttpClientFactory clientFactory,
            IConfiguration configuration)
        {
            this._mapper = mapper;
            this._clientFactory = clientFactory;
            this._configuration = configuration;
        }

        public async Task<Pokemon> GetPokemonModelAsync(string pokemon, IHttpClientFactory _clientFactory)
        {
            string uri = _configuration.GetValue<string>("pokemon") + pokemon;
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            var client = _clientFactory.CreateClient("pokemon");
            HttpResponseMessage response = await client.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                throw new HttpRequestException();

            var result = await response.Content.ReadFromJsonAsync<Pokemon>();
            return result;
        }

        

        public PokemonModel MapPokemonModel(IMapper _mapper, Pokemon result)
        {
            return _mapper.Map<PokemonModel>(result);
        }
    }
}
