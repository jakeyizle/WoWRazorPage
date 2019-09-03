using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;


namespace WoWRazorPage.Models
{
    public class Zone
    {
        public string name;
        public int id;
        public List<Boss> bosses;

        //public int ItemCount()
        //{
        //    return bosses.Sum(x => x.items.Count());
        //}
    }
}