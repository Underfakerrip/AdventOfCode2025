using SimplexMethod;

namespace Test
{
    public class Excercise11
    {
        private static readonly string path = "input.txt";
        private static Dictionary<string, string[]> _deviceOutputs;
        private static long _counter = 0;
        private static long _SecondToOutCounter = 0;
        private static long _middleCounter = 0;
        private static HashSet<string> _invalidPathDevices = new HashSet<string>();
        private static bool _stop = false;
        private static bool _fftBeforeDac = false;

        public static void Main2(string[] args)
        {
            _deviceOutputs = RetrieveDeviceOutputs();
            SolveTask1();
            SolveTask2();
        }

        private static void SolveTask1()
        {
            CheckPathsRecursive("you");
            Console.WriteLine(_counter);
        }

        private static void SolveTask2()
        {
            // Should not be done like this!
          
            BackWards("dac", true); // Only to set _fftBeforeDac

            // Get all paths from latter device to out
            HashSet<string> lastDevices = new HashSet<string>();
            CheckPathsRecursive(_fftBeforeDac ? "dac" : "fft", "out", lastDevices, false);
            _invalidPathDevices.UnionWith(lastDevices); // Eliminate found devices as there are not valid for next steps (svr -> first device and first device -> second device) and reduces computation time

            // Get all devices from first device to latter device
            HashSet<string> middleDevices = new HashSet<string>();
            CheckPathsRecursive(_fftBeforeDac ? "fft" : "dac", "dac", middleDevices, false);
            _invalidPathDevices.UnionWith(middleDevices); // Eliminate found devices as there are not valid for next step (svr -> first device) and reduces computation time

            CheckPathsRecursive("svr", _fftBeforeDac ? "fft" : "dac", lastDevices, true);
            Console.WriteLine(_counter * _SecondToOutCounter * _middleCounter);
        }

        private static void CheckPathsRecursive(string key)
        {
            if (_deviceOutputs[key].Count() == 1 && _deviceOutputs[key][0] == "out")
            {
                _counter++;
                return;
            }
            else
            {
                foreach (var item in _deviceOutputs[key])
                {
                    CheckPathsRecursive(item);
                }
            }
        }

        private static void CheckPathsRecursive(string key, string target, HashSet<string> pathDevices, bool svrStart)
        {
            if (_invalidPathDevices.Contains(key))
            {
                return;
            }

            if (!target.Equals("out") && key.Equals("out")) return;

            if (key == target || (_deviceOutputs[key].Count() == 1 && _deviceOutputs[key][0] == target))
            {
                if (target.Equals("out")) _SecondToOutCounter++;
                else if (svrStart) _counter++;
                else _middleCounter++;
                return;
            }
            else
            {
                foreach (var item in _deviceOutputs[key])
                {
                    pathDevices.Add(item);
                    CheckPathsRecursive(item, target, pathDevices, svrStart);
                }
            }
        }

        private static void BackWards(string key, bool fft)
        {
            if (_stop) return;
            // Look for all elements having output == key
            List<string> keys = _deviceOutputs
                .Where(x => x.Value.Any(y => y.Equals(key)))
                .Select(x => x.Key)
                .ToList();

            if (keys.Count == 0) return;

            foreach (string item in keys)
            {
                if (fft && item.Equals("fft") || !fft && item.Equals("dac"))
                {
                    if (fft)
                    {
                        _fftBeforeDac = true;
                        _stop = true;
                        return;
                    }
                    else
                    {
                        // dac before fft found
                        _stop = true;
                        return;
                    }
                }
                if (item.Equals("svr")) return;
                BackWards(item, fft);
            }
        }

        private static Dictionary<string, string[]> RetrieveDeviceOutputs()
        {
            return File.ReadAllLines(path)
                .Select(x => x.Split(":"))
                .ToDictionary(x => x[0], x => x[1].Trim().Split(' '));
        }
    }
}
