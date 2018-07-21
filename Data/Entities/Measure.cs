using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;

namespace EndApi.Data
{
    public class Measure
    {
        [Key]
        public string Key { get; set; }
         [Required]
        [StringLength(50)]
        public string Name { get; set; }
        
        [Required]
        public string Unit { get; set; }
        
        [Required]
        public bool IsActive { get; set; }
        [Required]
        public bool IsPeriodic { get; set; }
        public bool TakeLast { get; set; }
        [Required]
        public int Order { get; set; }
    }
}