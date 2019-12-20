using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Incite.Models
{
    public class EpgpStanding : BaseModel
    {
        public int EffortPoints { get; set; }

        public int GearPoints { get; set; }

        public int SnapshotId { get; set; }
        public virtual EpgpSnapshot Snapshot { get; set; }

        public int WowCharacterId { get; set; }
        public virtual WowCharacter WowCharacter { get; set; }

        [NotMapped]
        public float Priority => MathF.Round(EffortPoints / (float)GearPoints, 2);
    }
}
