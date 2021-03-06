﻿using System;
using LightBDD.Core.Results;
using LightBDD.UnitTests.Helpers;

namespace LightBDD.Framework.Reporting.UnitTests.Formatters
{
    internal static class ReportFormatterTestData
    {
        private static DateTimeOffset _startDate = new DateTimeOffset(2014, 09, 23, 19, 21, 57, 55, TimeSpan.Zero);

        public static IFeatureResult GetFeatureResultWithDescription()
        {
            return TestResults.CreateFeatureResult("My feature", $"My feature{Environment.NewLine}long description", "Label 1",
                TestResults.CreateScenarioResult("name", "Label 2", _startDate.AddSeconds(1), new TimeSpan(0, 0, 1, 2, 100), new[] { "categoryA" },
                    TestResults.CreateStepResult(ExecutionStatus.Passed)
                        .WithExecutionTime(_startDate.AddSeconds(2), new TimeSpan(0, 1, 1))
                        .WithStepNameDetails(1, "call step1 \"arg1\"", "step1 \"{0}\"", "call", "arg1")
                        .WithComments($"multiline{Environment.NewLine}comment", "comment 2"),
                    TestResults.CreateStepResult(2, "step2", ExecutionStatus.Ignored, _startDate.AddSeconds(3), new TimeSpan(0, 0, 0, 1, 100), "Not implemented yet")
                    .WithSubSteps(
                            TestResults.CreateStepResult(1, "substep 1", ExecutionStatus.Passed, _startDate.AddSeconds(3), new TimeSpan(0, 0, 0, 0, 100)).WithGroupPrefix("2."),
                            TestResults.CreateStepResult(2, "substep 2", ExecutionStatus.Passed, _startDate.AddSeconds(3).AddMilliseconds(100), new TimeSpan(0, 0, 0, 1, 0)).WithGroupPrefix("2."),
                            TestResults.CreateStepResult(3, "substep 3", ExecutionStatus.Ignored, _startDate.AddSeconds(4).AddMilliseconds(100), TimeSpan.Zero, "Not implemented yet").WithGroupPrefix("2.").WithComments("sub-comment")
                            .WithSubSteps(TestResults.CreateStepResult(1, "sub-substep 1", ExecutionStatus.NotRun).WithGroupPrefix("2.3.").WithComments($"sub-sub-multiline{Environment.NewLine}comment"))
                        )),

                TestResults.CreateScenarioResult(TestResults.CreateNameInfo("name2 \"arg1\"", "name2 \"{0}\"", "arg1"), null, _startDate.AddSeconds(4), new TimeSpan(0, 0, 0, 2, 157), new[] { "categoryB", "categoryC" },
                    TestResults.CreateStepResult(1, "step3", ExecutionStatus.Bypassed, _startDate.AddSeconds(5), new TimeSpan(0, 0, 0, 2, 107), "bypass reason"),
                    TestResults.CreateStepResult(2, "step4", ExecutionStatus.Failed, _startDate.AddSeconds(6), new TimeSpan(0, 0, 0, 0, 50),
                        $"  Expected: True{Environment.NewLine}  But was: False"),
                    TestResults.CreateStepResult(3, "step5", ExecutionStatus.NotRun)));
        }
        public static IFeatureResult[] GetMultipleFeatureResults()
        {
            var feature1 = TestResults.CreateFeatureResult("My feature", null, null,
            TestResults.CreateScenarioResult("scenario1", null, _startDate.AddSeconds(1), TimeSpan.FromMilliseconds(20), new[] { "categoryA" },
                TestResults.CreateStepResult(1, "step1", ExecutionStatus.Passed, _startDate.AddSeconds(2), TimeSpan.FromMilliseconds(20))));

            var feature2 = TestResults.CreateFeatureResult("My feature2", null, null,
                 TestResults.CreateScenarioResult("scenario1", null, _startDate.AddSeconds(4), TimeSpan.FromMilliseconds(20), new[] { "categoryB" },
                 TestResults.CreateStepResult(1, "step1", ExecutionStatus.Passed, _startDate.AddSeconds(5), TimeSpan.FromMilliseconds(20))));

            return new[] { feature1, feature2 };
        }

        public static IFeatureResult GetFeatureResultWithoutDescriptionNorLabelNorDetails()
        {
            return TestResults.CreateFeatureResult("My feature", null, null,
                TestResults.CreateScenarioResult("name", null, _startDate.AddSeconds(1), TimeSpan.FromMilliseconds(25), null,
                    TestResults.CreateStepResult(1, "step1", ExecutionStatus.Passed, _startDate.AddSeconds(2), TimeSpan.FromMilliseconds(20)),
                    TestResults.CreateStepResult(2, "step2", ExecutionStatus.Ignored, _startDate.AddSeconds(3), TimeSpan.FromMilliseconds(5))));
        }
    }
}