using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WoWRazorPage.Models;
using WoWRazorPage.Controller;
using System.ComponentModel.DataAnnotations;

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

        public List<Zone> zones = new List<Zone>();
        readonly BlizzardController controller = new BlizzardController();
        public void OnGet()
        {

        }

        public async Task<PageResult> OnPostAsync()
        {            
            if (ModelState.IsValid)
            { 
            zones = await controller.GetZonesAsync(CharacterName, Realm, MainStatWeight, CritWeight, HasteWeight, MasteryWeight, VersatilityWeight, MainStatName);
            }
            return Page();
        }
    }
}