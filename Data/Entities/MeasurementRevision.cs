using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EndApi.Data
{
    [Table("MeasurementRevision")]
    public class MeasurementRevision
    {
        //composite key
        public string Key { get; set; }
        [Required]
        public string Value { get; set; }        
        [Required]
        public int? Order { get; set; }
        public Revision Revision { get; set; }
        public string  RevisionId { get; set; }
    }
}