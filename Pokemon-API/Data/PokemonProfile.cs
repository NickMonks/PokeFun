using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pokemon_API.Data.Models;
using Pokemon_API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pokemon_API.Data
{
    public class PokemonProfile : Profile
    {
        public PokemonProfile()
        {
            this.CreateMap<Pokemon, PokemonModel>()
                .ForMember(c => c.Description, o => o.MapFrom(m => m.flavor_text_entries[0].flavor_text))
                .ForMember(c => c.Habitat, o => o.MapFrom(m => m.habitat.name));
                
        }
    }
}
