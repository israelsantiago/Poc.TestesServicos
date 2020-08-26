﻿using System;

namespace PoC.TestesServicos.Data.Models
{
    public class UserTeam
    {
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid TeamId { get; set; }
        public Team Team { get; set; }
    }
}