using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pokemon_API.Models
{
    public class TranslateRequest
    {
        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

        public TranslateRequest(string text)
        {
            Text = text;
        }
    }
}
