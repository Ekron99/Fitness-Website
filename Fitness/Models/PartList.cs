using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fitness.Models
{
    public class PartList
    {
        public Part part { get; set; }
        public List<Part> createdParts { get; set; }
    }
}