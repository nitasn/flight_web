using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace FlightControlWeb
{
    public static class FlightPlanExtensions
    {
        public static TimeSpan TotalTime(this FlightPlan plan)
        {
            double total_seconds = 0;

            foreach (var segment in plan.segments)
                total_seconds += segment.timespan_seconds;

            return TimeSpan.FromSeconds(total_seconds);
        }

        public static (double latitude, double longitude) InterpolateLocation
            (this FlightPlan plan, DateTime dateTime)
        {
            var startLoc = new Loc(
                plan.initial_location.latitude,
                plan.initial_location.longitude);

            var timeDiff = dateTime - plan.initial_location.date_time;
            var totalTime = timeDiff.TotalSeconds;

            var loc = LinearInterpolation(startLoc, plan.segments, totalTime);

            return (loc.latitude, loc.longitude);
        }

        private static Loc LinearInterpolation
            (Loc startLoc, List<FlightSegment> segments, double totalSeconds)
        {
            for (int i = 0; i < segments.Count && totalSeconds >= 0; i++)
            {
                var segment = segments[i];

                var segment_end_point = Loc.EndPointOf(segment);
                var segment_duration = segment.timespan_seconds;

                if (totalSeconds <= segment_duration)
                {
                    var delta = segment_end_point - startLoc;
                    var slope = totalSeconds / segment_duration;

                    return startLoc + (delta * slope);
                }

                startLoc = segment_end_point;
                totalSeconds -= segment_duration;
            }

            throw new Exception("flight isn't in the air at the time");
        }
    }

    internal struct Loc // yep, could have used Vector, but idk
    {
        public readonly double latitude, longitude;

        public static Loc EndPointOf(FlightSegment segment)
        {
            return new Loc(segment.latitude, segment.longitude);
        }

        public Loc(double latitude, double longitude)
        {
            this.latitude = latitude;
            this.longitude = longitude;
        }

        public static Loc operator -(Loc locA, Loc locB)
        {
            return new Loc(
                    locA.latitude - locB.latitude,
                    locA.longitude - locB.longitude
                );
        }

        public static Loc operator +(Loc locA, Loc locB)
        {
            return new Loc(
                    locA.latitude + locB.latitude,
                    locA.longitude + locB.longitude
                );
        }

        public static Loc operator *(Loc loc, double scalar)
        {
            return new Loc(
                    loc.latitude * scalar,
                    loc.longitude * scalar
                );
        }
    }
}

// todo somewhere: can FlightSegment's time be 0? cause that's a problem.
//  server should reject and return some error code