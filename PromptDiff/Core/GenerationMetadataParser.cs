using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using PromptDiff.Models;

namespace PromptDiff.Core
{
    public static class GenerationMetadataParser
    {
        private readonly static Regex KeyValueRegex =
            new (@"(?<key>[^:,]+):\s*(?<value>[^,]+)", RegexOptions.Compiled);

        public static GenerationMetadata Parse(string raw)
        {
            var parsed = ParseRaw(raw);
            return MapToGenerationMetadata(parsed);
        }

        private static MetadataParseResult ParseRaw(string raw)
        {
            var result = new MetadataParseResult();

            var normalized = raw.Replace("\r", string.Empty);
            var lines = normalized.Split('\n', StringSplitOptions.RemoveEmptyEntries)
                .Select(l => l.Trim())
                .ToList();

            // Negative prompt 行を探す
            var negativeIndex =
                lines.FindIndex(l => l.StartsWith("Negative prompt:", StringComparison.OrdinalIgnoreCase));

            if (negativeIndex >= 0)
            {
                result.NegativePrompt = lines[negativeIndex]
                    .Substring("Negative prompt:".Length)
                    .Trim();
            }

            // Prompt = Negative prompt より前の部分
            var promptLines = lines
                .Take(negativeIndex >= 0 ? negativeIndex : lines.Count);

            result.Prompt = string.Join("\n", promptLines).Trim();

            // key:value 群を全部拾う
            foreach (var line in lines)
            {
                foreach (Match match in KeyValueRegex.Matches(line))
                {
                    var key = match.Groups["key"].Value.Trim();
                    var value = match.Groups["value"].Value.Trim();
                    result.Parameters[key] = value;
                }
            }

            return result;
        }

        private static GenerationMetadata MapToGenerationMetadata(MetadataParseResult parsed)
        {
            var meta = new GenerationMetadata
            {
                Prompt = parsed.Prompt,
                NegativePrompt = parsed.NegativePrompt,
                Steps = GetInt(parsed, "Steps"),
                CfgScale = GetInt(parsed, "CFG scale"),
                Seed = GetLong(parsed, "Seed"),
                Sampler = GetString(parsed, "Sampler"),
            };

            // Size: 480x672
            if (parsed.Parameters.TryGetValue("Size", out var size))
            {
                var parts = size.Split('x');
                if (parts.Length == 2 && int.TryParse(parts[0], out var w) && int.TryParse(parts[1], out var h))
                {
                    meta.Width = w;
                    meta.Height = h;
                }
            }

            return meta;
        }

        private static int GetInt(MetadataParseResult parsed, string key)
        {
            return parsed.Parameters.TryGetValue(key, out var v) && int.TryParse(v, out var i) ? i : 0;
        }

        private static long GetLong(MetadataParseResult parsed, string key)
        {
            return parsed.Parameters.TryGetValue(key, out var v) && long.TryParse(v, out var l) ? l : 0;
        }

        private static string GetString(MetadataParseResult parsed, string key)
        {
            return parsed.Parameters.TryGetValue(key, out var v) ? v : string.Empty;
        }

        private class MetadataParseResult
        {
            public string Prompt { get; set; } = string.Empty;

            public string NegativePrompt { get; set; } = string.Empty;

            // 将来 VAE / Model / Version 等を拾うための生データ
            public Dictionary<string, string> Parameters { get; } = new (StringComparer.OrdinalIgnoreCase);
        }
    }
}