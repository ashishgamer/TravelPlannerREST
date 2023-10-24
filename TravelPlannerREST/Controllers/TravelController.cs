using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using TravelPlannerREST.Models;

namespace TravelPlannerREST.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TravelController : ControllerBase
    {
        private List<Flight> flights = new List<Flight>();
        private List<Hotel> hotels = new List<Hotel>();
        private List<Activity> activities = new List<Activity>();

        SqlConnection conn = new SqlConnection();

        private readonly ILogger<TravelController> _logger;

        public TravelController(ILogger<TravelController> logger, IConfiguration configuration)
        {
            _logger = logger;
            string connectionString = configuration.GetConnectionString("SqlServer");
            conn = new SqlConnection(connectionString);
        }

        [HttpGet("GetFlights")]
        public IEnumerable<Flight> GetFlights(string source, string destination)
        {
            if (!flights.Where(f => f.DCity == destination && f.SCity == source).Any())
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetFlightsToDestination", conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add("@destination", System.Data.SqlDbType.VarChar).Value = destination;
                    cmd.Parameters.Add("@source", System.Data.SqlDbType.VarChar).Value = source;
                    conn.Open();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        flights.Add(new Flight()
                        {
                            SAirport = reader[0].ToString(),
                            SCity = reader[1].ToString(),
                            SCountry = reader[2].ToString(),
                            DAirport = reader[3].ToString(),
                            DCity = reader[4].ToString(),
                            DCountry = reader[5].ToString(),
                            Airline = reader[6].ToString()
                        });
                    }
                }
                conn.Close();
            }
            return flights.Where(f => f.DCity == destination && f.SCity == source);
        }

        [HttpGet("GetHotels")]
        public IEnumerable<Hotel> GetHotels(string destination)
        {
            if (!hotels.Where(h => h.City == destination).Any())
            {
                using (SqlCommand cmd = new SqlCommand("select * from Hotels where City = '" + destination + "'", conn))
                {
                    conn.Open();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        hotels.Add(new Hotel()
                        {
                            Name = reader[1].ToString(),
                            City = reader[2].ToString()
                        });
                    }
                }
                conn.Close();
            }

            return hotels.Where(h => h.City == destination);
        }

        [HttpGet("GetActivities")]
        public IEnumerable<Activity> GetActivities(string destination)
        {
            if (!activities.Where(a => a.City == destination).Any())
            {
                using (SqlCommand cmd = new SqlCommand("select * from Activities where City = '" + destination + "'", conn))
                {
                    conn.Open();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        activities.Add(new Activity()
                        {
                            ActivityName = reader[1].ToString(),
                            City = reader[2].ToString()
                        });
                    }
                }
                conn.Close();
            }

            return activities.Where(a => a.City == destination);
        }
    }
}