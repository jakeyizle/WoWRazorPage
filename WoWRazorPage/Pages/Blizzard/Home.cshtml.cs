using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WoWRazorPage.Models;
using WoWRazorPage.Controller;
namespace WoWRazorPage.Pages.Blizzard
{
    public class HomeModel : PageModel
    {
        [BindProperty]
        public List<double> Weights { get; set; }
        [BindProperty]
        public string MainStatName { get; set; }

        public List<Zone> zones = new List<Zone>();
        readonly BlizzardController controller = new BlizzardController();
        public void OnGet()
        {

        }

        public async Task<PageResult> OnPostAsync(string name, string realm)
        {
            Weights.AddRange(new List<double> { 1, 2, 3, 5, 6 });
            MainStatName = "Intellect";
            zones = await controller.GetZonesAsync(Weights[0], Weights[1], Weights[2], Weights[3], Weights[4], MainStatName);
            return Page();
        }
    }
}