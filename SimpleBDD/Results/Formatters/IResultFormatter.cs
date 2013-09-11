using System.Text;

namespace SimpleBDD.Results.Formatters
{
	/// <summary>
	/// Result formatter interface.
	/// </summary>
	public interface IResultFormatter
	{
		/// <summary>
		/// Formats story result.
		/// </summary>
		/// <param name="result">Result to format.</param>
		string Format(StoryResult result);
	}

	/// <summary>
	/// Formats story result as plain text.
	/// </summary>
	public class PlainTextResultFormatter : IResultFormatter
	{
		/// <summary>
		/// Formats story result.
		/// </summary>
		/// <param name="result">Result to format.</param>
		public string Format(StoryResult result)
		{
			var builder = new StringBuilder();
			foreach (var scenario in result.Scenarios)
				FormatScenario(builder, scenario);
			return builder.ToString();
		}

		private void FormatScenario(StringBuilder builder, ScenarioResult scenario)
		{
			if (builder.Length > 0)
				builder.AppendLine();
			builder.Append("Scenario: ").Append(scenario.ScenarioName).Append(" - ").AppendLine(scenario.Status.ToString());
			foreach (var step in scenario.Steps)
			{
				builder.Append("\tStep ")
					.Append(step.StepNumber).Append("/").Append(step.TotalStepsCount).Append(" ")
					.Append(step.Name).Append(" - ").AppendLine(step.Status.ToString());
			}
		}
	}
}