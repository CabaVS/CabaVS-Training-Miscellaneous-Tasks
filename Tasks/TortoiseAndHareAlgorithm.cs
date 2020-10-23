using System;

namespace CabaVS.MiscellaneousTasks.Tasks
{
    public static class TortoiseAndHareAlgorithm
    {
        private static readonly (int[] Input, int ExpectedOutput)[] InputData = new[]
        {
            (new[] {1,3,2,4,2}, 2),
            (new[] {3,2,3,3,3}, 3),
            (new[] {1,2,3,4,2}, 2)
        };

        public static void Execute()
        {
            foreach (var (input, expectedOutput) in InputData)
            {
                Console.WriteLine($"Input: [{string.Join(',', input)}]");

                var output = RunSearch(input);

                Console.WriteLine($"Actual output: {output}");
                Console.WriteLine($"Expected output: {expectedOutput}");

                Console.WriteLine(new string('-', 80));
            }
        }

        private static int RunSearch(int[] input)
        {
            var tortoise = 0;
            var hare = 0;

            while (true)
            {
                tortoise = input[tortoise];
                hare = input[input[hare]];

                if (tortoise == hare)
                {
                    break;
                }
            }

            tortoise = 0;

            while (true)
            {
                tortoise = input[tortoise];
                hare = input[hare];

                if (input[tortoise] == input[hare])
                {
                    return input[tortoise];
                }
            }
        }
    }
}