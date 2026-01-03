using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace PromptDiff.Core
{
    public static class StepDiffGenerator
    {
        private readonly static Regex StepsRegex = new Regex(@"--steps\s+(-?\d+)", RegexOptions.Compiled);

        public static IEnumerable<string> GenerateStepVariants(StepVariantRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.RequestLine) || request.Count <= 0)
            {
                yield break;
            }

            var match = StepsRegex.Match(request.RequestLine);
            if (!match.Success)
            {
                yield break;
            }

            var originalSteps = int.Parse(match.Groups[1].Value);

            for (int i = 0; i < request.Count; i++)
            {
                var newSteps = originalSteps + request.StartOffset + i;

                yield return StepsRegex.Replace(
                    request.RequestLine,
                    $"--steps {newSteps}",
                    1);
            }
        }
    }
}