using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WoWRazorPage.Controller;
using WoWRazorPage.Models;

namespace WoWRazorPage.Pages.Blizzard
{
    [BindProperties]
    public class DataModel : PageModel
    {
        public List<Zone> zones = new List<Zone>();
        private readonly IBlizzardController _blizzardController;

        public DataModel(IBlizzardController blizzardController)
        {
            _blizzardController = blizzardController;

        }
        public void OnGet()
        {
            zones = _blizzardController.Zones;
        }
    }
}