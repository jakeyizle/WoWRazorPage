using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WoWRazorPage.Models;

namespace WoWRazorPage.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlizzardController : ControllerBase
    {
        HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("https://us.api.blizzard.com/wow/")
        };
        string token;
        readonly string baseDir = Directory.GetCurrentDirectory();

        [HttpGet]
        public async Task<List<Zone>> GetFullAsync(double mainStatWeight, double critWeight, double hasteWeight, double masteryWeight, double versatilityWeight, string mainStatName)
        {
            List<Item> characterItems = await CharacterItems("Peevo", "Emerald Dream");
            characterItems.RemoveAll(x => x.id == 2 || x.id == 12);

            List<Item> items;
            //fix this shit lul
            Dictionary<int, double> statWeightDict = new Dictionary<int, double>
            {
                { 32, critWeight },
                { 36, hasteWeight },
                { 49, masteryWeight },
                { 40, versatilityWeight },
                { 71, mainStatWeight }
            };
            //main stats can have multiple ids
            switch (mainStatName.ToLower())
            {
                case "intellect":
                    statWeightDict.Add(5, mainStatWeight);
                    statWeightDict.Add(73, mainStatWeight);
                    statWeightDict.Add(74, mainStatWeight);
                    break;
                case "agility":
                    statWeightDict.Add(3, mainStatWeight);
                    statWeightDict.Add(72, mainStatWeight);
                    statWeightDict.Add(73, mainStatWeight);
                    break;
                case "strength":
                    statWeightDict.Add(4, mainStatWeight);
                    statWeightDict.Add(72, mainStatWeight);
                    statWeightDict.Add(74, mainStatWeight);
                    break;
                default:
                    throw new Exception();
            }
            //get all items from local file
            using (StreamReader r = new StreamReader(Path.Combine(baseDir, "Resources/items.json")))
            {
                string json = r.ReadToEnd();
                items = JsonConvert.DeserializeObject<List<Item>>(json);
            }

            foreach (Item item in items)
            {
                foreach (Stat stat in item.Stats)
                {
                    if (statWeightDict.TryGetValue(stat.id, out double statWeight))
                    {
                        item.statValue += stat.amount * statWeight;
                    }
                }
            }

            foreach (Item item in characterItems)
            {
                foreach (Stat stat in item.Stats)
                {
                    if (statWeightDict.TryGetValue(stat.id, out double statWeight))
                    {
                        item.statValue += stat.amount * statWeight;
                    }
                }
            }

            List<Item> upgradeItems = new List<Item>();

            foreach (Item item in characterItems)
            {
                var tempItems = items.Where(x => x.inventoryType == item.inventoryType && x.statValue > item.statValue);
                foreach (Item tempItem in tempItems)
                {
                    tempItem.statImprovement = tempItem.statValue - item.statValue;
                    upgradeItems.Add(tempItem);
                }
            }

            List<Boss> bosses = new List<Boss>();
            using (StreamReader r = new StreamReader(Path.Combine(baseDir, "Resources/bosses.json")))
            {
                string json = r.ReadToEnd();
                bosses = JsonConvert.DeserializeObject<List<Boss>>(json);
            }

            List<Zone> zones = new List<Zone>();
            using (StreamReader r = new StreamReader(Path.Combine(baseDir, "Resources/zones.json")))
            {
                string json = r.ReadToEnd();
                zones = JsonConvert.DeserializeObject<List<Zone>>(json);
            }

            foreach (Zone zone in zones)
            {
                var zoneBosses = bosses.Where(x => x.zoneId == zone.id).ToList();
                foreach (Boss boss in zoneBosses)
                {
                    boss.items = upgradeItems.Where(y => y.sourceId == boss.id).Distinct().ToList();                    
                }
                zone.bosses = zoneBosses.Where(x => x.items.Count() > 0).OrderByDescending(y => y.items.Count()).ToList();                
            }

            return zones.Where(x=>x.bosses.Count() > 0).OrderByDescending(x => x.bosses.Sum(y => y.items.Count())).ToList();
        }

        //public async Task<Object> GetWeightsAsync(double mainStatWeight, double critWeight, double hasteWeight, double masteryWeight, double versatilityWeight, string mainStatName)
        //{
        //    List<Item> items;
        //    //fix this shit lul
        //    Dictionary<int, double> dict = new Dictionary<int, double>
        //    {
        //        { 32, critWeight },
        //        { 36, hasteWeight },
        //        { 49, masteryWeight },
        //        { 40, versatilityWeight },
        //        { 71, mainStatWeight }
        //    };
        //    //main stats can have multiple ids
        //    switch (mainStatName.ToLower())
        //    {
        //        case "intellect":
        //            dict.Add(5, mainStatWeight);
        //            dict.Add(73, mainStatWeight);
        //            dict.Add(74, mainStatWeight);
        //            break;
        //        case "agility":
        //            dict.Add(3, mainStatWeight);
        //            dict.Add(72, mainStatWeight);
        //            dict.Add(73, mainStatWeight);
        //            break;
        //        case "strength":
        //            dict.Add(4, mainStatWeight);
        //            dict.Add(72, mainStatWeight);
        //            dict.Add(74, mainStatWeight);
        //            break;
        //        default:
        //            throw new Exception();
        //    }
        //    //get all items from local file
        //    using (StreamReader r = new StreamReader("C:/Development/WebAPI/WebAPI/Resources/items.json"))
        //    {
        //        string json = r.ReadToEnd();
        //        items = JsonConvert.DeserializeObject<List<Item>>(json);
        //    }

        //    foreach (Item item in items)
        //    {
        //        foreach (Stat stat in item.Stats)
        //        {
        //            if (dict.TryGetValue(stat.id, out double statWeight))
        //            {
        //                item.statValue += stat.amount * statWeight;
        //            }
        //        }
        //    }
        //    return items;
        //}

        public async Task<List<Item>> CharacterItems(string characterName, string realm)
        {
            if (String.IsNullOrEmpty(token))
            {
                token = await TokenAsync();
            }

            //get character data, just need the item from the json
            var response = await client.GetAsync($"character/{realm}/{characterName}?fields=items&locale=en_US&access_token={token}");
            var jsonContent = response.Content.ReadAsStringAsync();

            JObject test = JObject.Parse(await jsonContent);
            IList<JToken> results = test["items"].Children().ToList();

            IList<Item> characterItems = new List<Item>();
            int i = 0;
            foreach (JToken result in results)
            {
                //first 2 entries are junk
                i++;
                if (i < 3)
                {
                    continue;
                }

                Item item = result.First.ToObject<Item>();
                characterItems.Add(item);
            }

            //character item data doesn't contain info about what slot it is, so we call the blizzard api to get it
            foreach (Item item in characterItems)
            {
                var itemResponse = await client.GetAsync($"item/{item.id}?locale=en_US&access_token={token}");
                var itemJsonContent = await itemResponse.Content.ReadAsStringAsync();
                //lazy
                dynamic itemContent = JObject.Parse(itemJsonContent);
                item.inventoryType = itemContent.inventoryType;
            }

            return characterItems.ToList();
        }


        public async Task<string> TokenAsync()
        {
            HttpClient tokenClient = new HttpClient
            {
                BaseAddress = new Uri("https://us.battle.net/")
            };

            var values = new Dictionary<string, string>
            {
                {"client_id", "94ba1f222c7b40a5b9dc009527df9de0" },
                {"client_secret", "8jE8d4fakM2TvVNYTUiabNlCfuwsvcbT" },
                {"grant_type", "client_credentials" }
            };
            var content = new FormUrlEncodedContent(values);

            var response = await tokenClient.PostAsync("oauth/token", content);
            var jsonContent = await response.Content.ReadAsStringAsync();

            var token = JsonConvert.DeserializeObject<Token>(jsonContent);
            tokenClient.Dispose();
            return token.AccessToken;
        }
    }




    public class Token
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
    }
}