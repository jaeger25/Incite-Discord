using Incite.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Incite.Discord.ApiModels
{
    public class QualifiedWowCharacter
    {
        public string Name { get; set; }

        public WowServer Server { get; set; }
    }
}
