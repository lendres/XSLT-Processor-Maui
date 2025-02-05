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
	/// Parse the command line arguments.
	/// </summary>
	public void ParseCommandLine();
}