//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Fitness.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class StrengthGoal
    {
        public int GoalID { get; set; }
        public int UserID { get; set; }
        public int ExerciseListID { get; set; }
        public string Name { get; set; }
        public decimal TargetWeight { get; set; }
        public System.DateTime EndDate { get; set; }
        public System.DateTime StartDate { get; set; }
    
        public virtual ExerciseList ExerciseList { get; set; }
        public virtual User User { get; set; }
    }
}
