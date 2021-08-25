using Pokemon_API.Data.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace Pokemon_API.Core
{
    public interface IPokemonTranslate
    {
        Task TranslateModel(PokemonModel model);
        Task<string> Translation(string description, string translationType, IHttpClientFactory clientFactory);
    }
}