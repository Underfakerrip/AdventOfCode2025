using SimplexMethod;

namespace Test
{
    public class Program
    {
        private static readonly string path = "input.txt";

        public static void Main(string[] args)
        {
            SolveTask();
        }


        /// <summary>
        /// Ignore if their could possibly an arrangement in tetris style.
        /// Simply consider each present needs 9 blocks of the space.
        /// </summary>
        private static void SolveTask()
        {
            int counter = 0;
            List<Input> input = RetrieveInput();
            foreach(var item in input)
            {
                int size = item.Width * item.Height;
                int neededSize = item.Presents.Sum(x => x) * 9;
                if (neededSize <= size)
                {
                    counter++;
                }
            }
            Console.WriteLine(counter);
        }


        private static List<Input> RetrieveInput()
        {
            return File.ReadAllLines(path)
                .Where(x => x.Contains("x"))
                .Select(x => x.Split(":"))
                .Select(x => new Input
                {
                    Width = int.Parse(x[0].Split("x")[0]),
                    Height = int.Parse(x[0].Split("x")[1]),
                    Presents = x[1].Trim().Split(" ").Select(y => int.Parse(y)).ToList()
                })
                .ToList();
        }

    }

    public class Input
    {
        public int Height { get; set; }
        public int Width { get; set; }
        public List<int> Presents { get; set; } 
    }
}
