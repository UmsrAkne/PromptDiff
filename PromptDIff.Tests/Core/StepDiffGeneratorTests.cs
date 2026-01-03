using PromptDiff.Core;

namespace PromptDIff.Tests.Core
{
    [TestFixture]
    public class StepDiffGeneratorTests
    {
        [Test]
        public void GenerateStepVariants_ReplacesOnlyStepsValue()
        {
            // Arrange
            const string input = "--prompt \"masterpiece, BREAK, 1girl, black hair, <lora-name:1.0>, (short hair:0.8)\" " +
                                 "--negative_prompt \"worst quality\" " +
                                 "--steps 12 " +
                                 "--cfg_scale 7 " +
                                 "--seed 3793887137 " +
                                 "--sampler_name Euler " +
                                 "--width 480 " +
                                 "--height 672";

            // Act
            var results = StepDiffGenerator
                .GenerateStepVariants(new StepVariantRequest
                {
                    RequestLine = input,
                    StartOffset = 0,
                    Count = 5
                })
                .ToList();

            // Assert
            Assert.That(results, Has.Count.EqualTo(5));

            Assert.Multiple(() =>
            {
                Assert.That(results[0], Does.Contain("--steps 12"));
                Assert.That(results[1], Does.Contain("--steps 13"));
                Assert.That(results[2], Does.Contain("--steps 14"));
                Assert.That(results[3], Does.Contain("--steps 15"));
                Assert.That(results[4], Does.Contain("--steps 16"));
            });

            // 他の引数が変わっていないこと
            foreach (var result in results)
            {
                Assert.That(result, Does.Contain("--cfg_scale 7"));
                Assert.That(result, Does.Contain("--seed 3793887137"));
                Assert.That(result, Does.Contain("--sampler_name Euler"));
                Assert.That(result, Does.Contain("--width 480"));
                Assert.That(result, Does.Contain("--height 672"));
                Assert.That(result, Does.Contain(
                    "--prompt \"masterpiece, BREAK, 1girl, black hair, <lora-name:1.0>, (short hair:0.8)\""));
                Assert.That(result, Does.Contain("--negative_prompt \"worst quality\""));

                TestContext.WriteLine(result);
            }
        }
    }
}