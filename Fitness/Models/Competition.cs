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
    
    public partial class Competition
    {
        public int CompetitionID { get; set; }
        public int OwnerID { get; set; }
        public int StartDate { get; set; }
        public int EndDate { get; set; }
        public int ExerciseListID { get; set; }
    
        public virtual ExerciseList ExerciseList { get; set; }
        public virtual Participant Participant { get; set; }
    }
}
