using PromptDiff.Core;

namespace PromptDIff.Tests.Core
{
    [TestFixture]
    public class GenerationMetadataParserTests
    {
        private const string SampleMetadata = """
                                              masterpiece,
                                              BREAK,

                                              1girl, black hair, <lora-name:1.0>, (short hair:0.8),
                                              Negative prompt: worst quality,
                                              Steps: 12, Sampler: Euler, Schedule type: Automatic, CFG scale: 7,
                                              Seed: 3793887137, Size: 480x672,
                                              Model hash: adfsf010ff, Model: checkPointNameXX,
                                              VAE hash: b4958602ab, VAE: xlVAEC_c1.safetensors, Version: v1.10.1
                                              """;

        [Test]
        public void Parse_ShouldExtractPrompt()
        {
            var result = GenerationMetadataParser.Parse(SampleMetadata);

            Assert.That(result.Prompt, Does.Contain("masterpiece"));
            Assert.That(result.Prompt, Does.Contain("1girl"));
            Assert.That(result.Prompt, Does.Not.Contain("Negative prompt"));
        }

        [Test]
        public void Parse_ShouldExtractNegativePrompt()
        {
            var result = GenerationMetadataParser.Parse(SampleMetadata);

            Assert.That(result.NegativePrompt, Is.EqualTo("worst quality,"));
        }

        [Test]
        public void Parse_ShouldExtractStepsAndCfgScale()
        {
            var result = GenerationMetadataParser.Parse(SampleMetadata);

            Assert.That(result.Steps, Is.EqualTo(12));
            Assert.That(result.CfgScale, Is.EqualTo(7));
        }

        [Test]
        public void Parse_ShouldExtractSeed()
        {
            var result = GenerationMetadataParser.Parse(SampleMetadata);

            Assert.That(result.Seed, Is.EqualTo(3793887137));
        }

        [Test]
        public void Parse_ShouldExtractSampler()
        {
            var result = GenerationMetadataParser.Parse(SampleMetadata);

            Assert.That(result.Sampler, Is.EqualTo("Euler"));
        }

        [Test]
        public void Parse_ShouldExtractImageSize()
        {
            var result = GenerationMetadataParser.Parse(SampleMetadata);

            Assert.That(result.Width, Is.EqualTo(480));
            Assert.That(result.Height, Is.EqualTo(672));
        }

    }
}