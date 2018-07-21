using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.Design;

namespace EndApi.Data
{
    public class Revision
    {
        [Key]
        public string Id { get; set; }
        [Required]
        public DateTime? Date { get; set; }
        [StringLength(100)]
        public string Concept { get; set; }
        [Required]
        public DateTime? NextRevisionDate { get; set; }

        //relations
        public EndUser CreatedBy { get; set; }
        public string CreatedById { get; set; }
        public List<MeasurementRevision> Measurements { get; set; }
        public EndUser AssignedUser { get; set; }
        public string AssignedUserId { get; set; }
    }
}