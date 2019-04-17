using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using System;

namespace techtalk
{

    public static class Maths
    {
        public static double GetDistanceInMiles(double lat1, double long1, double lat2, double long2)
        {
            if (lat1 == lat2 && long1 == long2)
                return 0;

            var rlat1 = lat1 * Math.PI / 180;
            var rlat2 = lat2 * Math.PI / 180;

            var acosParam = Math.Min(1.0d, Math.Sin(rlat1) * Math.Sin(rlat2) + Math.Cos(rlat1) * Math.Cos(rlat2) * Math.Cos((long2 - long1) * Math.PI / 180));
            var distance = Math.Acos(acosParam) * (6371 / 1.609);

            return distance;
        }

    }

    public class Vendor
    {
        public double Latitude;
        public double Longitude;
        public bool inChannel;

        public Vendor(double la, double lo)
        {
            Latitude = la;
            Longitude = lo;
            inChannel = true;
        }

    }

    public class SearchResult
    {
        Vendor v;
        double distance;

        public SearchResult(Vendor v, double distance)
        {
            this.v = v;
            this.distance = distance;
        }


    }


    public static class VendorExtensions
    {



        public static IEnumerable<Vendor> NearList(this List<Vendor> vendors, double latitude, double longitude, double radius)
        {
            //First pass: filter using a bounding box of 1 degree (+-69 miles)
            var minLat = latitude - 1;
            var maxLat = latitude + 1;
            var minLong = longitude - 1;
            var maxLong = longitude + 1;

            return vendors
                    .FindAll(v => v.Latitude > minLat
                                && v.Latitude < maxLat
                                && v.Longitude > minLong
                                && v.Longitude < maxLong)
                    .FindAll(v => Maths.GetDistanceInMiles(v.Latitude, v.Longitude, latitude, longitude) <= radius)
                    .OrderBy(v => Maths.GetDistanceInMiles(v.Latitude, v.Longitude, latitude, longitude));
        }

        public static IEnumerable<Vendor> NearEnumerable(this IEnumerable<Vendor> vendors, double latitude, double longitude, double radius)
        {
            //First pass: filter using a bounding box of 1 degree (+-69 miles)
            var minLat = latitude - 1;
            var maxLat = latitude + 1;
            var minLong = longitude - 1;
            var maxLong = longitude + 1;

            return vendors
                    .Where(v => v.Latitude > minLat
                                && v.Latitude < maxLat
                                && v.Longitude > minLong
                                && v.Longitude < maxLong)
                    .Where(v => Maths.GetDistanceInMiles(v.Latitude, v.Longitude, latitude, longitude) <= radius)
                    .OrderBy(v => Maths.GetDistanceInMiles(v.Latitude, v.Longitude, latitude, longitude));
        }

        public static IEnumerable<Vendor> NearEnumerableOneWhere(this IEnumerable<Vendor> vendors, double latitude, double longitude, double radius)
        {
            //First pass: filter using a bounding box of 1 degree (+-69 miles)
            var minLat = latitude - 1;
            var maxLat = latitude + 1;
            var minLong = longitude - 1;
            var maxLong = longitude + 1;

            return vendors
                    .Where(v => v.Latitude > minLat
                                && v.Latitude < maxLat
                                && v.Longitude > minLong
                                && v.Longitude < maxLong
                                && Maths.GetDistanceInMiles(v.Latitude, v.Longitude, latitude, longitude) <= radius)
                    .OrderBy(v => Maths.GetDistanceInMiles(v.Latitude, v.Longitude, latitude, longitude));
        }

        public static IEnumerable<(Vendor, double)> NearEnumerableOneWhereDistance(this IEnumerable<Vendor> vendors, double latitude, double longitude, double radius)
        {
            //First pass: filter using a bounding box of 1 degree (+-69 miles)
            var minLat = latitude - 1;
            var maxLat = latitude + 1;
            var minLong = longitude - 1;
            var maxLong = longitude + 1;

            return vendors
                    .Where(v => v.Latitude > minLat
                                && v.Latitude < maxLat
                                && v.Longitude > minLong
                                && v.Longitude < maxLong)
                    .Select(v => (v, Maths.GetDistanceInMiles(v.Latitude, v.Longitude, latitude, longitude)))
                    .Where(vendorAndDistance => vendorAndDistance.Item2 <= radius)
                    .OrderBy(vendorAndDistance => vendorAndDistance.Item2);
        }


    }

    [SimpleJob(launchCount: 1, warmupCount: 1, targetCount: 3)]
    [MemoryDiagnoser]
    public class LinqTips
    {
        const int TEST_SIZE = 10000;
        List<Vendor> vendors = new List<Vendor>(TEST_SIZE);
        public LinqTips()
        {
            Random r = new Random();
            for (int i = 0; i < TEST_SIZE; i++)
            {
                vendors.Add(new Vendor(r.NextDouble() * 10, r.NextDouble() * 10));
            }
        }

        public SearchResult CreateSearchResult(Vendor v, double lattitude, double longitude)
        {
            return new SearchResult(v, Maths.GetDistanceInMiles(v.Latitude, v.Longitude, lattitude, longitude));

        }

        [Benchmark]
        public List<SearchResult> WithListFilter()
        {

            return vendors
                    .Where(v => v.inChannel)
                    .ToList()
                    .NearList(5.0, 5.0, 100)
                    .Take(10)
                    .Select(v => CreateSearchResult(v, 5.0, 5.0))
                    .ToList();
        }


        /*
        [Benchmark]
        public List<SearchResult> WithEnumerableFilter()
        {

            return vendors
                    .Where(v => v.inChannel)
                    .NearEnumerable(5.0, 5.0, 100)
                    .Take(10)
                    .Select(v => CreateSearchResult(v, 5.0, 5.0))
                    .ToList();
        }
        
        [Benchmark]
        public List<SearchResult> WithEnumerableFilterOneWhere()
        {

            return vendors
                    .Where(v => v.inChannel)
                    .NearEnumerableOneWhere(5.0, 5.0, 100)
                    .Take(10)
                    .Select(v => CreateSearchResult(v, 5.0, 5.0))
                    .ToList();
        }
        
        [Benchmark]
        public List<SearchResult> WithEnumerableFilterOneWhereDistance()
        {

            return vendors
                    .Where(v => v.inChannel)
                    .NearEnumerableOneWhereDistance(5.0, 5.0, 100)
                    .Take(10)
                    .Select(v => new SearchResult(v.Item1, v.Item2))
                    .ToList();
        }
        */
    }
}
