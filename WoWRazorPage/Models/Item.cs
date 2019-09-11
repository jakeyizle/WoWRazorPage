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
        public int bonusId;
        public int inventoryType;
        public int itemLevel;
        public int sourceId;
        public string name;
        public double statValue;
        public double statImprovement;
        public int forgeNumber;
        public string PercentImprovement()
        {
            var change = statImprovement / statValue;
            var percent = change * 100;
            return Math.Round(percent, 2).ToString() + "%";
        }
    }
}