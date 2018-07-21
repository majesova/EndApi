using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EndApi.Data
{
    public class PlanProperty
    {
        [Key]
        public string Key { get; set; }
        [Required]
        public string Value { get; set; }
        [Required]
        public string Name { get; set; }
        public PlanNutritional Plan { get; set; }
        public string PlanId { get; set; }
    }
}