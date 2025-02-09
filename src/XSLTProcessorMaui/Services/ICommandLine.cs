using DigitalProduction.CommandLine;

namespace XSLTProcessorMaui;

public interface ICommandLine
{
	/// <summary>
	/// The XML file the is the source material.
	/// </summary>
	public string? InputFile { get; set; }

	/// <summary>
	/// The transformation sheet.
	/// </summary>
	public string? XsltFile { get; set; }

	/// <summary>
	/// The transformation sheet.
	/// </summary>
	public string? XsltArguments { get; set; }

	/// <summary>
	/// Output (destination) file.
	/// </summary>
	public string? OutputFile { get; set; }

	/// <summary>
	/// Output (destination) file.
	/// </summary>
	public bool? RunPostProcessor { get; set; }

	/// <summary>
	/// Output (destination) file.
	/// </summary>
	public string? PostProcessor { get; set; }

	/// <summary>
	/// Specifies that the translation should begin immediately.
	/// -run
	/// </summary>
	public bool Run { get; set; }

	/// <summary>
	/// Specifies that the software should exit after running the transformation and launching the postprocessor (if specified).
	/// </summary>
	public bool Exit { get; set; }

	/// <summary>
	/// The help string.
	/// </summary>
	public string Help { get; }

	/// <summary>
	/// Any parsing errors, if they occured.
	/// </summary>
	public string? Errors { get; }

	/// <summary>
	/// Parse the command line arguments.
	/// </summary>
	public void ParseCommandLine();
}