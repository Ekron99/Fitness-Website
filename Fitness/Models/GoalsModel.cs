using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fitness.Models
{
    public class GoalsModel
    {
        public List<StrengthGoal> StrengthGoals { get; set; }
        public List<AerobicGoal> AerobicGoals { get; set; }
    }
}