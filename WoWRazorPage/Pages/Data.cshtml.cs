using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WoWRazorPage.Service;
using WoWRazorPage.Models;

namespace WoWRazorPage.Pages.Blizzard
{
    [BindProperties]
    public class DataModel : PageModel
    {
        public List<Zone> zones = new List<Zone>();
        private readonly IBlizzardService _blizzardController;

        public DataModel(IBlizzardService blizzardController)
        {
            _blizzardController = blizzardController;

        }
        public void OnGet()
        {
            zones = _blizzardController.Zones;
        }
    }
}