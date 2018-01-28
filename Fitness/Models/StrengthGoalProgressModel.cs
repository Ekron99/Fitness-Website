using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fitness.Models
{
    public class StrengthGoalProgressModel
    {

        public StrengthGoal goal { get; set; }
        public IQueryable<StrengthExercise> progress { get; set; }


    }
}