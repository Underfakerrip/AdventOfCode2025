using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    internal class Excercise9
    {
        private static readonly string path = "input.txt";

        private static void SolveTask1()
        {
            List<Coordinate> coordinates = RetrieveCoordinates();
            long maxSize = 0;
            for (int i = 0; i < coordinates.Count; i++)
            {
                for (int j = i + 1; j < coordinates.Count; j++)
                {
                    long size = CalculateRectangle(coordinates[i], coordinates[j]);
                    maxSize = size > maxSize ? size : maxSize;
                }
            }
            Console.WriteLine(maxSize);
        }

        public static void SolveTask2()
        {
            List<Coordinate> coordinates = RetrieveCoordinates()
                .OrderBy(c => c.Y)
                .ToList();

            List<Boundary> boundaries = GetBoundaries(coordinates);
            // Check if valid rectangle and calculate
            long maxSize = 0;
            for (int i = 0; i < coordinates.Count; i++)
            {
                for (int j = i + 1; j < coordinates.Count; j++)
                {
                    Coordinate firstCoordinate = coordinates[i];
                    Coordinate secondCoordinate = coordinates[j];

                    if (!ValidateRectangle(firstCoordinate, secondCoordinate, boundaries))
                    {
                        continue;
                    }

                    long size = CalculateRectangle(firstCoordinate, secondCoordinate);
                    maxSize = size > maxSize ? size : maxSize;
                }
            }
            Console.WriteLine(maxSize);
        }

        private static List<Coordinate> RetrieveCoordinates()
        {
            return File.ReadAllLines(path)
                .Select(x => x.Split(','))
                .Select(x => new Coordinate(int.Parse(x[0]) + 1, int.Parse(x[1]) + 1))
                .ToList();
        }

        private static long CalculateRectangle(Coordinate co1, Coordinate co2)
        {
            long width = 1 + (co1.X > co2.X ? (co1.X - co2.X) : (co2.X - co1.X));
            long height = 1 + (co1.Y > co2.Y ? (co1.Y - co2.Y) : (co2.Y - co1.Y));
            return width * height;
        }


        /// <summary>
        /// Gets boundaries by moving around the surface while alternating beetween horizontal and vertical edges
        /// </summary>
        /// <param name="coordinates">List of coordinates</param>
        /// <returns>List of boundaries with starting and ending coordinate and if its horizontal or vertical</returns>
        private static List<Boundary> GetBoundaries(List<Coordinate> coordinates)
        {
            List<Boundary> boundaries = new();

            // Use groupings for faster execution time instead of repeating searches over all coordinates
            List<IGrouping<int, Coordinate>> groupedYCoordinates = coordinates
                .GroupBy(c => c.Y)
                .ToList();

            List<IGrouping<int, Coordinate>> groupedXCoordinates = coordinates
                .GroupBy(c => c.X)
                .ToList();

            bool leftRight = false; // State of alternating horizontal and vertical boundary

            // First coordinate of boundary. Start with smallest Y value and (if more than one) smallest X value
            Coordinate currentCoordinate = groupedYCoordinates
                .First()
                .MinBy(c => c.X);

            Coordinate partnerCoordinate = null; // Second coordinate of boundary

            while (boundaries.Count == 0 || !(partnerCoordinate.X == boundaries[0].FirstCoordinate.X && partnerCoordinate.Y == boundaries[0].FirstCoordinate.Y)) // Stop if boundaries exist and we are back at the first coordinate
            {
                if (leftRight) // Horizontal boundary
                {
                    partnerCoordinate = groupedYCoordinates
                        .First(c => c.Key == currentCoordinate.Y) // Get all coordinates with same Y value
                        .MaxBy(c => Math.Abs(currentCoordinate.X - c.X)); // Most distant X value
                }
                else // Vertical boundary
                {
                    partnerCoordinate = groupedXCoordinates
                        .First(c => c.Key == currentCoordinate.X) // Get all coordinates with same X value
                        .MaxBy(c => Math.Abs(currentCoordinate.Y - c.Y)); // Most distant Y value
                }

                boundaries.Add(new Boundary()
                {
                    FirstCoordinate = currentCoordinate,
                    SecondCoordinate = partnerCoordinate,
                    Horizontal = leftRight
                });

                currentCoordinate = partnerCoordinate;
                leftRight = !leftRight; // Switch as horitontal boundary comes after vertical and otherwise
            }

            return boundaries;
        }


        /// <summary>
        /// Check if no boundary is beetween opposite rectangle sides and <br/>
        /// if all coordinates are polygon corners at at most one is not but contains boundaries
        /// </summary>
        /// <param name="co1">First red tile coordinate</param>
        /// <param name="co2">Opposite corner red tile coordinate</param>
        /// <param name="boundaries">All boundaries of the surface</param>
        /// <returns>True if valid rectangle and false otherwise</returns>
        private static bool ValidateRectangle(Coordinate co1, Coordinate co2, List<Boundary> boundaries)
        {
            // Get the outer most X and Y values
            int higherY = co1.Y > co2.Y ? co1.Y : co2.Y;
            int higherX = co1.X > co2.X ? co1.X : co2.X;
            int lowerY = co1.Y < co2.Y ? co1.Y : co2.Y;
            int lowerX = co1.X < co2.X ? co1.X : co2.X;

            Coordinate co3 = new Coordinate(co1.X, co2.Y);
            Coordinate co4 = new Coordinate(co2.X, co1.Y);

            // Helper for check of adjacent rectangle side of coordinates
            bool firstCoordinatesSide = false; // co1 <-> co3
            bool secondCoordinatesSide = false; // co3 <-> co2
            bool thirdCoordinatesSide = false; // co2 <-> co4
            bool fourthCoordinatesSide = false; // co4 <-> co1

            // Helper for polygon corner check 
            bool firstCoordinate = false;
            bool secondCoordinate = false;
            bool thirdCoordinate = false;
            bool forthCoordinate = false;

            foreach (Boundary boundary in boundaries)
            {
                int boundaryStart = -1;
                int boundaryEnd = -1;
                if (boundary.Horizontal) // Different X values 
                {
                    if (boundary.FirstCoordinate.Y >= higherY || boundary.FirstCoordinate.Y <= lowerY) // Y in rectangle? If not, boundary can not be within rectangle
                    {
                        continue;
                    }

                    boundaryStart = boundary.FirstCoordinate.X > boundary.SecondCoordinate.X ? boundary.SecondCoordinate.X : boundary.FirstCoordinate.X;
                    boundaryEnd = boundary.FirstCoordinate.X > boundary.SecondCoordinate.X ? boundary.FirstCoordinate.X : boundary.SecondCoordinate.X;

                    // Is boundary entirely or partially in rectangle (vertically)?
                    if (boundaryStart > lowerX && boundaryStart < higherX || boundaryEnd > lowerX && boundaryEnd < higherX || boundaryStart <= lowerX && boundaryEnd >= higherX)
                    {
                        return false;
                    }
                }
                else // Different Y values
                {
                    if (boundary.FirstCoordinate.X >= higherX || boundary.FirstCoordinate.X <= lowerX) // X in rectangle? If not, boundary can not be within rectangle
                    {
                        continue;
                    }

                    boundaryStart = boundary.FirstCoordinate.Y > boundary.SecondCoordinate.Y ? boundary.SecondCoordinate.Y : boundary.FirstCoordinate.Y;
                    boundaryEnd = boundary.FirstCoordinate.Y > boundary.SecondCoordinate.Y ? boundary.FirstCoordinate.Y : boundary.SecondCoordinate.Y;

                    // Is boundary entirely or partially in rectangle (vertically)?
                    if (boundaryStart > lowerY && boundaryStart < higherY || boundaryEnd > lowerY && boundaryEnd < higherY || boundaryStart <= lowerY && boundaryEnd >= higherY)
                    {
                        return false;
                    }
                }

                // Check if coordinates are polygon corners or the rectangle sides contain partially a boundary
                firstCoordinate = firstCoordinate || IsCooridnateOnBoundary(co1, co3, boundaryStart, boundaryEnd, boundary);
                secondCoordinate = secondCoordinate || IsCooridnateOnBoundary(co3, co2, boundaryStart, boundaryEnd, boundary);
                thirdCoordinate = thirdCoordinate || IsCooridnateOnBoundary(co2, co4, boundaryStart, boundaryEnd, boundary);
                forthCoordinate = forthCoordinate || IsCooridnateOnBoundary(co4, co1, boundaryStart, boundaryEnd, boundary);

                firstCoordinatesSide = firstCoordinatesSide || SideContainsBoundary(co1, co3, boundaryStart, boundaryEnd, boundary);
                secondCoordinatesSide = secondCoordinatesSide || SideContainsBoundary(co3, co2, boundaryStart, boundaryEnd, boundary);
                thirdCoordinatesSide = thirdCoordinatesSide || SideContainsBoundary(co2, co4, boundaryStart, boundaryEnd, boundary);
                fourthCoordinatesSide = fourthCoordinatesSide || SideContainsBoundary(co4, co1, boundaryStart, boundaryEnd, boundary);
            }

            List<bool> coordinateResult = new List<bool>() { firstCoordinate, secondCoordinate, thirdCoordinate, forthCoordinate }; // Easyer to code the following checks if in list

            if (coordinateResult.Count == 4) return true; // All coordinates are polygon corners and no boundaries within rectangle
            
            if (coordinateResult.Count == 3) // 3 coordinates are corners. Check if last one has both adjacent rectangle sides containing partially a boundary
            {
                int coordinateNumber = coordinateResult
                    .Select((value, index) => new { value, index })
                    .FirstOrDefault(x => !x.value).index + 1;
                switch (coordinateNumber)
                {
                    case 1: return firstCoordinatesSide && fourthCoordinatesSide;
                    case 2: return firstCoordinatesSide && secondCoordinatesSide;
                    case 3: return secondCoordinatesSide && thirdCoordinatesSide;
                    default: return thirdCoordinatesSide && forthCoordinate;
                }
            }
            return false;
        }


        private static bool SideContainsBoundary(Coordinate co1, Coordinate co2, int boundaryStart, int boundaryEnd, Boundary boundary)
        {
            if (boundary.Horizontal) // Different X values
            {
                if (co1.Y == boundary.FirstCoordinate.Y && co2.Y == boundary.FirstCoordinate.Y)
                {
                    return boundaryStart > co1.X && boundaryStart < co2.X || boundaryStart > co2.X && boundaryStart < co1.X ||
                        boundaryEnd > co1.X && boundaryEnd < co2.X || boundaryEnd > co2.X && boundaryEnd < co1.X;
                }
            }
            else // Different Y values
            {
                if (co1.X == boundary.FirstCoordinate.X && co2.X == boundary.FirstCoordinate.X)
                {
                    return boundaryStart > co1.Y && boundaryStart < co2.Y || boundaryStart > co2.Y && boundaryStart < co1.Y ||
                        boundaryEnd > co1.Y && boundaryEnd < co2.Y || boundaryEnd > co2.Y && boundaryEnd < co1.Y;
                }
            }
            return false;
        }

        private static bool IsCooridnateOnBoundary(Coordinate co1, Coordinate co2, int start, int end, Boundary boundary)
        {
            if (boundary.Horizontal) // Different X values
            {
                if (co1.Y == boundary.FirstCoordinate.Y && co2.Y == boundary.FirstCoordinate.Y && co1.X >= start && co1.X <= end && co2.X >= start && co2.X <= end)
                {
                    return true;
                }
            }
            else // Different Y values
            {
                if (co1.X == boundary.FirstCoordinate.X && co2.X == boundary.FirstCoordinate.X && co1.Y >= start && co1.Y <= end && co2.Y >= start && co2.Y <= end)
                {
                    return true;
                }
            }
            return false;
        }
    }

    public class Coordinate
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Coordinate(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    public class Boundary
    {
        public Coordinate FirstCoordinate { get; set; }
        public Coordinate SecondCoordinate { get; set; }
        public bool Horizontal { get; set; }
    }
}
