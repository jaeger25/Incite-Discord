using Incite.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Incite.Discord.ApiModels
{
    public class UserWowCharacter
    {
        public WowCharacter Character { get; set; }
    }
}
