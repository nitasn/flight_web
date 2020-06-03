using System;
using System.Collections.Generic;

namespace FlightControlWeb.Tests
{
    public class Test_Interpolation
    {
        private static bool DoublesEqual(double d1, double d2)
        {
            const double EPSILON = 0.0000001;

            return Math.Abs(d1 - d2) < EPSILON;
        }

        public void TestInterpolateLocation()
        {
            var startTime = new DateTime(year: 2000, month: 1, day: 1);

            FlightPlan plan = new FlightPlan()
            {
                initial_location = new InitialPosition()
                {
                    date_time = startTime,
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

            var (result_lat, result_lng) =
                plan.InterpolateLocation(startTime + TimeSpan.FromSeconds(15));

            (double correct_lat, double corrent_lng) =
                (116.66666666666667, 166.66666666666667);

            if (!DoublesEqual(result_lat, correct_lat) ||
                !DoublesEqual(result_lng, corrent_lng))
                throw new Exception("TestInterpolateLocation Failed");
        }
    }
}
