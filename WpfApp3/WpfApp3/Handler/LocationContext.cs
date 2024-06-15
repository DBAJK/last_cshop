using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp3.DB;

namespace WpfApp3.Handler
{
    internal class LocationContext : DbContext
    {
        public LocationContext() { }

    }

    public class Location
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class LocationService
    {
        private readonly KakaoMapService kakaoMapService;
        private readonly LocationContext dbContext;

        public LocationService()
        {
            kakaoMapService = new KakaoMapService();
            dbContext = new LocationContext();
        }

        public async Task SaveLocationAsync(string address)
        {
            var locationData = await kakaoMapService.GetLocationDataAsync(address);
            var documents = locationData["documents"];

            if (documents.HasValues)
            {
                var firstDocument = documents[0];
                var location = new Location
                {
                    Address = address,
                    Latitude = (double)firstDocument["y"],
                    Longitude = (double)firstDocument["x"]
                };

              //  dbContext.Locations.Add(location);
                await dbContext.SaveChangesAsync();
            }
        }

    }
}
