using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Incite.Models
{
    public class GuildOptions : BaseModel
    {
        public char CommandPrefix { get; set; } = '!';
    }
}
