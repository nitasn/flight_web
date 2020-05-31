using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FlightControlWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var start = new DateTime(year: 2000, month: 1, day: 1);

            FlightPlan plan = new FlightPlan()
            {
                initial_location = new InitialPosition()
                {
                    date_time = start,
                    latitude = 100,
                    longitude = 100,
                },

                segments = new List<FlightSegment>()
                {
                    new FlightSegment()
                    {
                        latitude = 110,
                        longitude = 150,
                        timespan_seconds = 10
                    },
                    new FlightSegment()
                    {
                        latitude = 130,
                        longitude = 200,
                        timespan_seconds = 15
                    }
                },
            };

            var loc = plan.InterpolateLocation(start + TimeSpan.FromSeconds(15));

            Console.WriteLine("********************************");
            Console.WriteLine("********************************");
            Console.WriteLine(loc.latitude + ", " + loc.longitude);
            Console.WriteLine("********************************");
            Console.WriteLine("********************************");

            //CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
