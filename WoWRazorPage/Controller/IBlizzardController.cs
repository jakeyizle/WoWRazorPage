﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WoWRazorPage.Models;

namespace WoWRazorPage.Controller
{
    public interface IBlizzardController
    {
        List<Zone> Zones { get; set; }
        Task<List<Zone>> GetZonesAsync(string characterName, string realm, double mainStatWeight, double critWeight, double hasteWeight, double masteryWeight, double versatilityWeight, string mainStatName);

    }
}
