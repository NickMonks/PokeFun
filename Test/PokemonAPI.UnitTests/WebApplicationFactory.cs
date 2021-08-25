using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Net.Http;

namespace PokemonAPI.UnitTests
{
    internal class WebApplicationFactory<T>
    {
        internal HttpClient CreateClient(WebApplicationFactoryClientOptions webApplicationFactoryClientOptions)
        {
            throw new NotImplementedException();
        }
    }
}