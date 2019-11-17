using System;
using System.Collections.Generic;
using System.Text;

namespace Incite.Models
{
    public class MemberRole : BaseModel
    {
        public int MemberId { get; set; }
        public virtual Member Member { get; set; }

        public int RoleId { get; set; }
        public virtual Role Role { get; set; }
    }
}
