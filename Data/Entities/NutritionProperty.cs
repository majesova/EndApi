using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;

namespace EndApi.Data
{
    public class NutritionProperty
    {
        [Key]
        public string Key { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(30)]
        public string Unit { get; set; }
        [Required]
        public bool IsActive { get; set; }
    }
}