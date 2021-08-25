
using Microsoft.AspNetCore.Mvc.Testing;
using Pokemon_API;
using Pokemon_API.Data.Models;
using Pokemon_API.Exceptions;
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
            
            _httpClient = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                BaseAddress = new Uri("http://localhost/pokemon/translated")
            });
        }


        /// <summary>
        /// Test to verify we get a success response when looking for a existing pokemon. 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetPokemonModel_ReturnsSuccessStatusCode()
        {
            // This just confirms we can send a message and get a 200 status code
            var response = await _httpClient.GetAsync("mewtwo");
            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Verify we are reciving some content in the body of the GET request. 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetPokemonModel_ReturnsContent()
        {
            var response = await _httpClient.GetAsync("mewtwo");
            Assert.NotNull(response.Content);
        }

        /// <summary>
        /// Verify we are reciving the a response with the json media type. 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetPokemonModel_ReturnsExpectedPokemonModelMediaType()
        {
            var response = await _httpClient.GetAsync("mewtwo");
            Assert.Equal("application/json", response.Content.Headers.ContentType.MediaType);
        }

        /// <summary>
        /// Verify we are receiving the expected PokemonModel. 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetPokemonModel_ReturnsNotNullResponse()
        {
            var responseStream = await _httpClient.GetStreamAsync("mewtwo");

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

        }

        /// <summary>
        /// Verify we are receiving the expected PokemonModel. 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetPokemonModel_ReturnsYodaTranslationWhenHabitatIsCave()
        {
            // introduce the most typical cave pokemon - zubat!
            var responseStream = await _httpClient.GetStreamAsync("zubat");

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
                Habitat = "cave",
                Name = "zubat",
                Is_legendary = false,
                Description = ""
            };

            Assert.Equal(pokemonModelExpected.Habitat, pokemonModelActual.Habitat);
            Assert.Equal(pokemonModelExpected.Name, pokemonModelActual.Name);
            Assert.Equal(pokemonModelExpected.Is_legendary, pokemonModelActual.Is_legendary);
            Assert.Equal(pokemonModelExpected.Description, pokemonModelActual.Description);
        }

        /// <summary>
        /// Verify we are receiving the expected PokemonModel. 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetPokemonModel_ReturnsShakespeareTranslationWhenHabitatIsNotCaveAndIsLegendary()
        {
            // introducing our fav pokemon legendary - mewtwo!
            var responseStream = await _httpClient.GetStreamAsync("mewtwo");

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
                Description = ""
            };

            Assert.Equal(pokemonModelExpected.Habitat, pokemonModelActual.Habitat);
            Assert.Equal(pokemonModelExpected.Name, pokemonModelActual.Name);
            Assert.Equal(pokemonModelExpected.Is_legendary, pokemonModelActual.Is_legendary);
            Assert.Equal(pokemonModelExpected.Description, pokemonModelActual.Description);
        }

        /// <summary>
        /// Verify we are receiving the expected PokemonModel. 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetPokemonModel_ReturnsShakespeareTranslationWhenHabitatIsNotCaveOrIsNotLegendary()
        {
            // we select a random pokemon - geodude, great dude!
            var responseStream = await _httpClient.GetStreamAsync("geodude");

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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetPokemonModel_ReturnsStatusCodeNotFound()
        {
            var responseStream = await _httpClient.GetStreamAsync("mewtwo_TEST");

            // de-serialize the response to a PokemonModel. Ensure is case insensitive by setting the properties
            var pokemonResponseActual = await JsonSerializer.DeserializeAsync<ExceptionPokemon>(responseStream,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            var pokemonResponseExpected = new ExceptionPokemon("Not Found");
            pokemonResponseExpected.StatusCode = 404;

            Assert.Equal(pokemonResponseExpected.Message, pokemonResponseActual.Message);
            Assert.Equal(pokemonResponseExpected.StatusCode, pokemonResponseActual.StatusCode);

        }

        /// <summary>
        /// The translate API exhausts after a few requests. This test ensures that, when we exhaust our API calls, we get a
        /// normal description of the pokemon.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetPokemonModel_ReturnsTooManyRequestCode()
        {
            var responseStream = await _httpClient.GetStreamAsync("mewtwo_TEST");

            // de-serialize the response to a PokemonModel. Ensure is case insensitive by setting the properties
            var pokemonResponseActual = await JsonSerializer.DeserializeAsync<ExceptionPokemon>(responseStream,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            var pokemonResponseExpected = new ExceptionPokemon("Not Found");
            pokemonResponseExpected.StatusCode = 404;

            Assert.Equal(pokemonResponseExpected.Message, pokemonResponseActual.Message);
            Assert.Equal(pokemonResponseExpected.StatusCode, pokemonResponseActual.StatusCode);

        }


    }
}
