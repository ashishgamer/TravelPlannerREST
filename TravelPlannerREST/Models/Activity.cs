using System.ComponentModel.DataAnnotations;

namespace TravelPlannerREST.Models
{
    public class Activity
    {
        [Key]
        public int ActivityId { get; set; }
        public string ActivityName { get; set; }
        public string City { get; set; }
        public int CityID { get; set; }
    }
}
