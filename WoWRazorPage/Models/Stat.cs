using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace WoWRazorPage.Models
{
    public class Stat
    {
        [JsonProperty("stat")]
        public int id;
        [JsonProperty("amount")]
        public int amount;
    }
}