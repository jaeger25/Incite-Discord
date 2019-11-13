using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Incite.Models
{
    public abstract class BaseModel
    {
        [Key]
        public int Id { get; set; }
    }
}
