using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace WoWRazorPage.Models
{
    public class Boss
    {
        public string Name;
        public int Id;
        public int ZoneId;
        public List<Item> Items;
    }
}