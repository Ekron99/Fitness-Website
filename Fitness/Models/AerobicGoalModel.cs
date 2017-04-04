using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fitness.Models
{
    public class AerobicGoalModel
    {
        public AerobicGoal goal { get; set; }
        public IQueryable<AerobicExercise> progress { get; set; }
    }
}