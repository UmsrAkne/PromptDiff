using Prism.Mvvm;

namespace PromptDiff.Core
{
    /// <summary>
    /// Parameters for generating step variants.
    /// </summary>
    public class StepVariantRequest : BindableBase
    {
        private int startOffset;
        private int count;

        public string RequestLine { get; set; } = string.Empty;

        public int StartOffset { get => startOffset; set => SetProperty(ref startOffset, value); }

        public int Count { get => count; set => SetProperty(ref count, value); }
    }
}