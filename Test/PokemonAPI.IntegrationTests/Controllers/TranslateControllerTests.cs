
using Microsoft.AspNetCore.Mvc.Testing;
using Pokemon_API;
using Pokemon_API.Data.Models;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace PokemonAPI.IntegrationTests.Controllers
{
    public class TranslatedControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _httpClient;
        public TranslatedControllerTests(WebApplicationFactory<Startup> factory)
        {
            // create http client to send a request to the test server
            //_httpClient = factory.CreateDefaultClient();

            _httpClient = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                BaseAddress = new Uri("http://localhost/pokemon/")
            });
        }

        /// <summary>
        /// Test to verify we get a success response when looking for a existing pokemon. 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetPokemonModel_ReturnsSuccessStatusCode()
        {
            var response = await _httpClient.GetAsync("");
            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Verify we are reciving some content in the body of the GET request. 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetPokemonModel_ReturnsContent()
        {
            var response = await _httpClient.GetAsync("");
            Assert.NotNull(response.Content);
        }

        /// <summary>
        /// Verify we are reciving the a response with the json media type. 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetPokemonModel_ReturnsExpectedPokemonModelMediaType()
        {
            var response = await _httpClient.GetAsync("");
            Assert.Equal("application/json", response.Content.Headers.ContentType.MediaType);
        }

        /// <summary>
        /// Verify we are receiving the expected PokemonModel. 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetPokemonModel_ReturnsExpectedPokemonModel()
        {
            var responseStream = await _httpClient.GetStreamAsync("");

            // de-serialize the response to a PokemonModel. Ensure is case insensitive by setting the properties
            var model = await JsonSerializer.DeserializeAsync<PokemonModel>(responseStream,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            
            // Assert that the fields are not null
           Assert.NotNull(model?.Description);
           Assert.NotNull(model?.Habitat);
           Assert.NotNull(model?.Is_legendary);
           Assert.NotNull(model?.Name);

            // Assert the contents of the RestAPI
        }

        /// <summary>
        /// Verify we are receiving the expected PokemonModel. 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetPokemonModel_ReturnsExpectedPokemonModelContent()
        {
            var responseStream = await _httpClient.GetStreamAsync("");

            // de-serialize the response to a PokemonModel. Ensure is case insensitive by setting the properties
            var pokemonModelActual = await JsonSerializer.DeserializeAsync<PokemonModel>(responseStream,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            // Assert the contents of the RestAPI - Because REST API guarantees idempotency, we know we will have the same
            // result if we apply the same call beyond the first try
           var pokemonModelExpected = new PokemonModel
            {
                Habitat = "rare",
                Name = "mewtwo",
                Is_legendary = true,
                Description = "It was created by a scientist after years of horrific gene splicing and DNA engineering experiments."
            };

            Assert.Equal(pokemonModelExpected.Habitat, pokemonModelActual.Habitat);
            Assert.Equal(pokemonModelExpected.Name, pokemonModelActual.Name);
            Assert.Equal(pokemonModelExpected.Is_legendary, pokemonModelActual.Is_legendary);
            Assert.Equal(pokemonModelExpected.Description, pokemonModelActual.Description);
        }


    }
}
