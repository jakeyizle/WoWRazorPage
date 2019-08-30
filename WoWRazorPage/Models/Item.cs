using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace WoWRazorPage.Models
{
    public class Item
    {
        [JsonProperty("id")]
        public int id;
        [JsonProperty("stats")]
        public List<Stat> Stats;
        public int inventoryType;
        public int itemLevel;
        public int sourceId;
        public string name;
        public double statValue;
        public double statImprovement;
    }
}