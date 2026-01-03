namespace PromptDiff.Models
{
    public class GenerationMetadata
    {
        public string Prompt { get; set; } = string.Empty;

        public string NegativePrompt { get; set; } = string.Empty;

        public int Steps { get; set; }

        public int CfgScale { get; set; }

        public long Seed { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public string Sampler { get; set; } = string.Empty;
    }
}