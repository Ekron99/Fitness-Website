using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fitness.Models
{
    public class StrengthAerobicList
    {
        public List<ExerciseList> strengthExercise { get; set; }
        public List<ExerciseList> aerobicExercise { get; set; }
    }
}