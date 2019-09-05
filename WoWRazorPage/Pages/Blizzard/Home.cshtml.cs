using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WoWRazorPage.Models;
using WoWRazorPage.Service;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace WoWRazorPage.Pages.Blizzard
{
    [BindProperties]
    public class HomeModel : PageModel
    {       
        [Required]
        public double MainStatWeight { get; set; }
        [Required]
        public double CritWeight { get; set; }
        [Required]
        public double HasteWeight { get; set; }
        [Required]
        public double MasteryWeight { get; set; }
        [Required]
        public double VersatilityWeight { get; set; }
        [Required]
        public string MainStatName { get; set; }
        [Required]
        public string CharacterName { get; set; }
        [Required]
        public string Realm { get; set; }

        public List<Zone> Zones = new List<Zone>();
        private readonly IBlizzardService _blizzardController;

        public HomeModel(IBlizzardService blizzardController)
        {
            _blizzardController = blizzardController;
        }
        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {            
            if (ModelState.IsValid)
            { 
                Zones = await _blizzardController.GetZonesAsync(CharacterName, Realm, MainStatWeight, CritWeight, HasteWeight, MasteryWeight, VersatilityWeight, MainStatName);
                _blizzardController.Zones = Zones;                
                return RedirectToPage("Data");
            }
            return Page();
        }
    }
}