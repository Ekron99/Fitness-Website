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

    public partial class StrengthExercise
    {
        public StrengthExercise()
        {
            DateRecorded = DateTime.Now;
        }

        public int ExerciseID { get; set; }
        public int UserID { get; set; }
        public int Rep { get; set; }
        public int Sets { get; set; }
        public decimal Weight { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.DateTime DateRecorded { get; set; }
        [Display(Name = "Exercise Name")]
        public int ExerciseListID { get; set; }
    
        public virtual User User { get; set; }
        public virtual ExerciseList ExerciseList { get; set; }
    }
}
