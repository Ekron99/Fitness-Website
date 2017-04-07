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
    using System.ComponentModel.DataAnnotations;

    public partial class AerobicGoal
    {
        public int GoalID { get; set; }
        public int UserID { get; set; }
        [Display(Name = "Exercise Name")]
        public int ExerciseListID { get; set; }
        public string Name { get; set; }
        [DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
        public System.TimeSpan Duration { get; set; }
        [Display(Name = "Distance")]
        public decimal Length { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.DateTime StartDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.DateTime EndDate { get; set; }
        public string Focus { get; set; }
    
        public virtual ExerciseList ExerciseList { get; set; }
        public virtual User User { get; set; }
    }
}
