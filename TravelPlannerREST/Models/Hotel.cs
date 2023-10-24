using System.ComponentModel.DataAnnotations;

namespace TravelPlannerREST.Models
{
    public class Hotel
    {
        [Key]
        public int HoteId { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public int CityId { get; set; }
    }
}
