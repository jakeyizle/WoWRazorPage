using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;


namespace WoWRazorPage.Models
{
    public class Zone
    {
        public string Name;
        public int Id;
        public List<Boss> Bosses;
        public bool IsRaid;
    }
}