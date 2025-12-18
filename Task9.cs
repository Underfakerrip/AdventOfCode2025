namespace AdventOfCode
{
    public class Program
    {
        private static readonly string path = "input.txt";

        public static void Main(string[] args)
        {
            SolveTask2();
        }

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

        private static void SolveTask2()
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

                    if (!ValidateRectangle(firstCoordinate, secondCoordinate, boundaries)) continue;
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
        /// Check if no boundary is beetween opposite rectangle sides
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

            foreach (Boundary boundary in boundaries)
            {
                if (boundary.Horizontal) // Different X values 
                {
                    if (boundary.FirstCoordinate.Y >= higherY || boundary.FirstCoordinate.Y <= lowerY) // Y in rectangle? If not, boundary can not be within rectangle
                    {
                        continue;
                    }
                    int start = boundary.FirstCoordinate.X > boundary.SecondCoordinate.X ? boundary.SecondCoordinate.X : boundary.FirstCoordinate.X;
                    int end = boundary.FirstCoordinate.X > boundary.SecondCoordinate.X ? boundary.FirstCoordinate.X : boundary.SecondCoordinate.X;

                    // Is boundary entirely or partially in rectangle (horizontally)?
                    if (start <= higherX && end >= lowerX || start <= lowerX && end >= lowerX || start <= higherX && end >= higherX) return false;
                }
                else // Different Y values
                {
                    if (boundary.FirstCoordinate.X >= higherX || boundary.FirstCoordinate.X <= lowerX) // X in rectangle? If not, boundary can not be within rectangle
                    {
                        continue;
                    }
                    int start = boundary.FirstCoordinate.Y > boundary.SecondCoordinate.Y ? boundary.SecondCoordinate.Y : boundary.FirstCoordinate.Y;
                    int end = boundary.FirstCoordinate.Y > boundary.SecondCoordinate.Y ? boundary.FirstCoordinate.Y : boundary.SecondCoordinate.Y;

                    // Is boundary entirely or partially in rectangle (vertically)?
                    if (start <= higherY && end >= lowerY || start <= lowerY && end >= lowerY || start <= higherY && end >= higherY) return false;
                }
            }
            return true;
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
