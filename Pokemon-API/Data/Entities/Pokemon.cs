using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pokemon_API.Entities
{
    public class Pokemon
    {
        public Flavor_Text_Entries[] flavor_text_entries { get; set; }
        public Habitat habitat { get; set; }
       
        public bool is_legendary { get; set; }
        
        public string name { get; set; }
       
    }



}

