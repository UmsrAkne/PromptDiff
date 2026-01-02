using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace PromptDiff.Core
{
    public static class PromptRequestFormatter
    {
        public static string ConvertToPromptRequest(string rawMetadata)
        {
            if (string.IsNullOrWhiteSpace(rawMetadata))
            {
                return string.Empty;
            }

            // 正規化
            var text = rawMetadata.Replace("\r", string.Empty).Trim();

            // 1. Prompt / Negative / Params に分割
            var negativeIndex = text.IndexOf("\nNegative prompt:", StringComparison.Ordinal);
            var stepsIndex = text.IndexOf("\nSteps:", StringComparison.Ordinal);

            var promptPart = negativeIndex >= 0
                ? text[..negativeIndex]
                : text;

            var negativePart = negativeIndex >= 0 && stepsIndex > negativeIndex
                ? text[(negativeIndex + "\nNegative prompt:".Length) ..stepsIndex]
                : string.Empty;

            var paramsPart = stepsIndex >= 0
                ? text[stepsIndex ..]
                : string.Empty;

            var args = new List<string>();

            // 2. prompt
            var prompt = CleanPrompt(promptPart);
            if (!string.IsNullOrWhiteSpace(prompt))
            {
                args.Add($"--prompt \"{prompt}\"");
            }

            // 3. negative prompt
            var negativePrompt = CleanPrompt(negativePart);
            if (!string.IsNullOrWhiteSpace(negativePrompt))
            {
                args.Add($"--negative_prompt \"{negativePrompt}\"");
            }

            // 4. parameters (Steps, Sampler, CFG, Seed, Size...)
            var paramMap = ParseParams(paramsPart);

            AddIfExists(args, paramMap, "steps", "--steps");
            AddIfExists(args, paramMap, "cfg scale", "--cfg_scale");
            AddIfExists(args, paramMap, "seed", "--seed");
            AddIfExists(args, paramMap, "sampler", "--sampler_name");

            if (paramMap.TryGetValue("size", out var size))
            {
                var match = Regex.Match(size, @"(\d+)\s*x\s*(\d+)");
                if (match.Success)
                {
                    args.Add($"--width {match.Groups[1].Value}");
                    args.Add($"--height {match.Groups[2].Value}");
                }
            }

            return string.Join(" ", args);
        }

        private static string CleanPrompt(string text)
        {
            return string.Join(", ", text.Split(',')
                    .Select(t => t.Trim())
                    .Where(t => !string.IsNullOrWhiteSpace(t)));
        }

        private static Dictionary<string, string> ParseParams(string text)
        {
            var dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            var parts = text.Split(',')
                .Select(p => p.Trim())
                .Where(p => p.Contains(':'));

            foreach (var part in parts)
            {
                var idx = part.IndexOf(':');
                var key = part[..idx].Trim();
                var value = part[(idx + 1) ..].Trim();

                dict[key] = value;
            }

            return dict;
        }

        private static void AddIfExists(
            List<string> args,
            Dictionary<string, string> map,
            string key,
            string optionName)
        {
            if (map.TryGetValue(key, out var value))
            {
                args.Add($"{optionName} {value}");
            }
        }
    }
}