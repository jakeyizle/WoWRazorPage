using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace WoWRazorPage.Models
{
    public class Boss
    {
        public string name;
        public int id;
        public int zoneId;
        public List<Item> items;
    }
}