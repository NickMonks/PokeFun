using AutoMapper;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using Pokemon_API;
using Pokemon_API.Core;
using Pokemon_API.Data.Models;
using Pokemon_API.Entities;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace PokemonAPI.UnitTests
{
    class PokemonResultTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        public readonly HttpClient _httpClient;
        public PokemonResultTests(WebApplicationFactory<Startup> factory)
        {
            // create http client to send a request to the test server
            //_httpClient = factory.CreateDefaultClient();

            _httpClient = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                BaseAddress = new Uri("http://localhost/pokemon/mewtwo")
            });
        }


        public static Mock<IHttpClientFactory> CreatePokemonClientMock(HttpResponseMessage httpResponseMessage)
        {
            // first, we create a mock HttpClientFactory, and setup the methods used: SendAsync, CreateClient and PostAsync
            var mockFactory = new Mock<IHttpClientFactory>();

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();


            // Setup SendAsync response; we define the kind of arguments we could expect 
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponseMessage);


            var client = new HttpClient(mockHttpMessageHandler.Object);
            mockFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);
            return mockFactory;
        }

        public static Mock<IMapper> CreatePokemonMapperMock(PokemonModel pokemonModelExpected)
        {
            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(x => x.Map<PokemonModel>(It.IsAny<Pokemon>()))
                .Returns(pokemonModelExpected);

            return mockMapper;
        }

        public static IConfiguration CreatePokemonConfiguration(string key, string value)
        {
            var inMemorySettings = new Dictionary<string, string> {
                {key, value}};

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            return configuration;
        }



        [Fact]
        public async Task PokemonControllerAsync_ReturnsExpectedResult()
        {

            var response = @"{""habitat"": ""rare"",""is_legendary"": true, ""name"": ""mewtwo2"",""description"": ""It was created by a scientist after years of horrific gene splicing and DNA engineering experiments.""}";
            var httpResponseExpected = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(response)
            };

            var pokemonModelExpected = new PokemonModel
            {
                Habitat = "rare",
                Name = "mewtwo",
                Is_legendary = true,
                Description = "It was created by a scientist after years of horrific gene splicing and DNA engineering experiments."
            };

            var clientMock  = PokemonResultTests.CreatePokemonClientMock(httpResponseExpected).Object;
            var mapperMock  = PokemonResultTests.CreatePokemonMapperMock(pokemonModelExpected).Object;
            var confMock    = PokemonResultTests.CreatePokemonConfiguration("pokemon", "https://pokeapi.co/api/v2/pokemon-species/");

            var pokemonResultMock = new PokemonResult(confMock);

            var result = await pokemonResultMock.GetPokemonModelAsync("mewtwo", clientMock);
            var model = pokemonResultMock.MapPokemonModel(mapperMock, result);





        }

    }
}
