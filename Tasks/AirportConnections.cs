using System;
using System.Collections.Generic;
using System.Linq;

namespace CabaVS.MiscellaneousTasks.Tasks
{
    /// <summary>
    /// Challenge: Find the minimum number of new routes from Starting Airport in such a way, to have any airport available from Starting Airport. By default, starting airport doesn't have any
    /// outbound connections.
    /// 
    /// Input parameters:
    /// -> Airports - list of all airports
    /// -> Connections - connections between airports
    /// -> Starting airport - airport from which you're able to "build" new routes
    /// </summary>
    public static class AirportConnections
    {
        private static readonly (string[] Airports, string[][] Connections, string StartingAirport, int ExpectedOutput)[]
            InputData = {
                (new[] { "LGA", "BPI", "CNG", "CTP", "PST", "ARC" },
                new[]
                {
                    new [] {"BPI", "CNG"}, new [] {"CNG", "CTP"}, new []{"CNG","LGA"}, new []{"PST","CNG"}, new []{"PST", "ARC"}
                }, "LGA", 2),
                (new[]
                {
                    "BGI", "CDG", "DEL", "DOH", "DSM", "EWR", "EYW", "HND", "ICN", "JFK", "LGA", "LHR", "ORD", "SAN",
                    "SFO", "SIN", "TLV", "BUD"
                }, new[]
                {
                    new[] {"DSM", "ORD"}, new[] {"ORD", "BGI"}, new[] {"BGI", "LGA"},
                    new[] {"SIN", "CDG"}, new[] {"CDG", "SIN"}, new[] {"CDG", "BUD"},
                    new[] {"DEL", "DOH"}, new[] {"DEL", "CDG"}, new[] {"TLV", "DEL"},
                    new[] {"EWR", "HND"}, new[] {"HND", "ICN"}, new[] {"HND", "JFK"},
                    new[] {"ICN", "JFK"}, new[] {"JFK", "LGA"}, new[] {"EYW", "LHR"},
                    new[] {"LHR", "SFO"}, new[] {"SFO", "SAN"}, new[] {"SFO", "DSM"},
                    new[] {"SAN", "EYW"}
                }, "LGA", 3)
            };

        public static void Execute()
        {
            foreach (var (airports, connections, startingAirport, expectedOutput) in InputData)
            {
                Console.WriteLine($"Airports: [{string.Join(',', airports)}]");
                //Console.WriteLine("Connections:");
                Console.WriteLine($"Starting airport: {startingAirport}");
                Console.WriteLine($"Expected output: {expectedOutput}");

                var output = FindNumberOfConnections(airports, connections, startingAirport);
                Console.WriteLine($"Actual output: {output}");

                Console.WriteLine(new string('-', 80));
            }
        }

        private static int FindNumberOfConnections(string[] airports, string[][] connections, string startingAirport)
        {
            // TODO: Convert to better structure first?

            var calculatedWeight = new List<(string Airport, List<string> PotentialDestinations)>(airports.Length - 1);
            var excludeStartingAirport = new List<string> {startingAirport};

            foreach (var airport in airports)
            {
                if (airport == startingAirport) continue;

                var potentialDestinations = GetPotentialDestinations(airport, connections, excludeStartingAirport);
                calculatedWeight.Add((airport, potentialDestinations));
            }

            var numberOfConnections = 0;
            var resolvedAirports = new List<string>(airports.Length) {startingAirport};

            do
            {
                // get airports with zero inbound flights
                var notResolvedAirportsZeroInbound = airports.Where(x => !resolvedAirports.Contains(x) && connections.All(y => y[1] != x)).ToList();

                // find the biggest weight
                var weightToLookup = calculatedWeight
                    .Where(x => notResolvedAirportsZeroInbound.Count == 0 ||
                                notResolvedAirportsZeroInbound.Contains(x.Airport)).ToList();

                weightToLookup.Sort(delegate((string Airport, List<string> PotentialDestinations) tuple,
                    (string Airport, List<string> PotentialDestinations) valueTuple)
                {
                    var count1 = tuple.PotentialDestinations.Count;
                    var count2 = valueTuple.PotentialDestinations.Count;

                    return count2.CompareTo(count1);
                });

                var biggestWeight = weightToLookup.First();

                // add to results
                numberOfConnections += 1;
                resolvedAirports.AddRange(biggestWeight.PotentialDestinations);

                if (resolvedAirports.Count == airports.Length)
                {
                    break;
                }

                // remove resolved airports from calculations
                calculatedWeight = calculatedWeight.Where(x => !biggestWeight.PotentialDestinations.Contains(x.Airport))
                    .ToList();
                calculatedWeight = calculatedWeight.Select(x => (x.Airport,
                    x.PotentialDestinations.Where(y => !biggestWeight.PotentialDestinations.Contains(y)).ToList())).ToList();

            } while (true);

            return numberOfConnections;
        }

        private static List<string> GetPotentialDestinations(string airport, string[][] connections, List<string> exclude)
        {
            var potentialDestinations = new List<string> {airport};

            // TODO: any way to omit this?
            var excludeClone = new List<string>(exclude.Count + 1);
            excludeClone.AddRange(exclude);
            excludeClone.Add(airport);

            var availableAirports = connections.Where(x => x[0] == airport && exclude.All(y => y != x[1])).Select(x => x[1]);
            foreach (var availableAirport in availableAirports)
            {
                var destinations = GetPotentialDestinations(availableAirport, connections, excludeClone);
                potentialDestinations.AddRange(destinations);
            }

            // TODO: any good way how to remove it in algorithmic way?
            potentialDestinations = potentialDestinations.Distinct().ToList();

            return potentialDestinations;
        }
    }
}