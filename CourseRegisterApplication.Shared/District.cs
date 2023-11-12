using System.ComponentModel.DataAnnotations.Schema;

namespace CourseRegisterApplication.Shared
{
    public class District
    {
        public int Id { get; set; }
        public string? DistrictName { get; set; }
        public bool IsPriority { get; set; }

        [ForeignKey("Province")]
        public int ProvinceId { get; set; }
        public Province? Province { get; set; } 
    }
}
