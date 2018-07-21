using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EndApi.Data
{
    public class PlanNutritional
    {
        [Key]
        public string Id { get; set; }
        [Required]
        public DateTime? CreateAt { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string Title { get; set; }
        public int? PeriodDays { get; set; }
        [Required]
        public bool IsReleased { get; set; }
        public DateTime? ReleasedAt { get; set; }
        public List<Food> Foods { get; set; }
        public List<PlanProperty> PlanProperties { get; set; }
    }
}