using PromptDiff.Core;

namespace PromptDIff.Tests.Core
{
    [TestFixture]
    public class PromptRequestFormatterTests
    {
        [Test]
        public void ConvertToPromptRequest_ReturnsString_ForAnyInput()
        {
            // Arrange
            var metadata = """
                           masterpiece, 
                           BREAK,

                           1girl, black hair, <lora-name:1.0>, (short hair:0.8),
                           Negative prompt: worst quality, 
                           Steps: 12, Sampler: Euler, Schedule type: Automatic, CFG scale: 7, Seed: 3793887137, Size: 480x672, Model hash: adfsf010ff, Model: checkPointNameXX, VAE hash: b4958602ab, VAE: xlVAEC_c1.safetensors, Version: v1.10.1
                           """;

            // Act
            var result = PromptRequestFormatter.ConvertToPromptRequest(metadata);

            TestContext.WriteLine(result);

            // Assert
            Assert.That(result, Does.Contain("--prompt \"masterpiece, BREAK, 1girl, black hair, <lora-name:1.0>, (short hair:0.8)\""));
            Assert.That(result, Does.Contain("--negative_prompt \"worst quality\""));
            Assert.That(result, Does.Contain("--steps 12"));
            Assert.That(result, Does.Contain("--cfg_scale 7"));
            Assert.That(result, Does.Contain("--seed 3793887137"));
            Assert.That(result, Does.Contain("--sampler_name Euler"));
            Assert.That(result, Does.Contain("--width 480"));
            Assert.That(result, Does.Contain("--height 672"));
        }
    }
}