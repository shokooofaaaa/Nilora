using System;
using System.Collections.Generic;
using System.Text;

namespace Nilora.Domain.Entities
{
    public class TechnicianSkill
    {
        public int TechnicianId { get; private set; }

        public int SkillId { get; private set; }

        public Technician Technician { get; private set; } = null!;

        public Skill Skill { get; private set; } = null!;

        private TechnicianSkill() { }

        public TechnicianSkill(int technicianId, int skillId)
        {
            if (technicianId <= 0 || skillId <= 0)
                throw new ArgumentException("Invalid relationship IDs.");

            TechnicianId = technicianId;
            SkillId = skillId;
        }
    }
}
