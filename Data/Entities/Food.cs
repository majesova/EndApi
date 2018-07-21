using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EndApi.Data
{
    public class Food
    {
        [Key]
        public string Id { get; set; }
        [Required]
        public int? FoodTime { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public int? FoodDay { get; set; }
        public PlanNutritional Plan { get; set; }
        public string PlanId { get; set; }
        public List<FoodPicture> Pictures { get; set; }
    }
}