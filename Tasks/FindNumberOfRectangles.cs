using System;

namespace CabaVS.MiscellaneousTasks.Tasks
{
    public static class FindNumberOfRectangles
    {
        private static Point[] Points = new Point[]
        {
            new Point { X = 0, Y = 0 },
            new Point { X = 0, Y = 1 },
            new Point { X = 1, Y = 0 },
            new Point { X = 1, Y = 1 },
            new Point { X = 2, Y = 0 },
            new Point { X = 2, Y = 1 },

            new Point { X = 1, Y = 7 },
            new Point { X = 6, Y = 7 },
            new Point { X = 6, Y = 1 },

            new Point { X = 3, Y = 2 },
            new Point { X = 1, Y = 4 },
            new Point { X = 4, Y = 7 },
            new Point { X = 6, Y = 5 },
        };

        public static void Execute()
        {
            Console.WriteLine($"Input points: [{string.Join<Point>(", ", Points)}]");

            var output = CalculateNumberOfRectangles(Points);

            Console.WriteLine($"Number of rectangles: {output}");
        }

        private static int CalculateNumberOfRectangles(Point[] points)
        {
            if (points == null) throw new ArgumentNullException(nameof(points));
            if (points.Length < 4) return 0;

            var result = 0;

            for (var i = 0; i < points.Length - 3; i++)
            {
                for (var j = i + 1; j < points.Length - 2; j++)
                {
                    for (var k = j + 1; k < points.Length - 1; k++)
                    {
                        for (var l = k + 1; l < points.Length; l++)
                        {
                            if (IsRectangle(points[i], points[j], points[k], points[l])) result += 1;
                        }
                    }
                }
            }

            return result;
        }

        private static bool IsRectangle(Point p1, Point p2, Point p3, Point p4)
        {
            // diagonal 1-2
            var length12 = LineLength(p1, p2);
            var length13 = LineLength(p1, p3);
            var length23 = LineLength(p2, p3);
            var length24 = LineLength(p2, p4);
            var length14 = LineLength(p1, p4);
            if (length12 == Math.Sqrt(Math.Pow(length13, 2) + Math.Pow(length23, 2))
                && length13 == length24 && length14 == length23) return true;

            // diagonal 1-3
            var length34 = LineLength(p3, p4);
            if (length13 == Math.Sqrt(Math.Pow(length12, 2) + Math.Pow(length23, 2))
                && length12 == length34 && length23 == length14) return true;

            // diagonal 1-4
            if (length14 == Math.Sqrt(Math.Pow(length12, 2) + Math.Pow(length24, 2))
                && length12 == length34 && length13 == length24) return true;

            return false;
        }

        private static double LineLength(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2));
        }

        public class Point
        {
            public int X { get; set; }
            public int Y { get; set; }

            public override string ToString()
            {
                return $"({X}; {Y})";
            }
        }
    }
}