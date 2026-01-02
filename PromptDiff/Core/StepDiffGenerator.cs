using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace PromptDiff.Core
{
    public static class StepDiffGenerator
    {
        private readonly static Regex StepsRegex = new Regex(@"--steps\s+(-?\d+)", RegexOptions.Compiled);

        public static IEnumerable<string> GenerateStepVariants(
            string requestLine,
            int startOffset,
            int count)
        {
            if (string.IsNullOrWhiteSpace(requestLine) || count <= 0)
            {
                yield break;
            }

            var match = StepsRegex.Match(requestLine);
            if (!match.Success)
            {
                yield break;
            }

            var originalSteps = int.Parse(match.Groups[1].Value);

            for (int i = 0; i < count; i++)
            {
                var newSteps = originalSteps + startOffset + i;

                yield return StepsRegex.Replace(
                    requestLine,
                    $"--steps {newSteps}",
                    1);
            }
        }
    }
}