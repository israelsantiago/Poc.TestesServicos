using System;
using System.Collections.Generic;

namespace PoC.TestesServicos.Data.Models
{
    public class Team
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<UserTeam> UserTeams { get; set; }
    }
}