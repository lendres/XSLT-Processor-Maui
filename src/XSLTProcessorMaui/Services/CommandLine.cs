using DigitalProduction.CommandLine;

namespace XSLTProcessorMaui;

[CommandLineManager(EnabledOptionStyles = OptionStyles.Unix | OptionStyles.Plus)]
[CommandLineOptionGroup("commands", Name = "Commands", Require = OptionGroupRequirement.AtMostOne)]
public class CommandLine : ICommandLine
{
	#region Properties

	/// <summary>
	/// The XML file the is the source material.
	/// </summary>
	[CommandLineOption(
		Name			= "inputfile",
		Description		= "XML file that is the source material."
	)]
	public string? InputFile { get; set; } = null;

	/// <summary>
	/// The transformation sheet.
	/// </summary>
	[CommandLineOption(
		Name			= "xsltfile",
		Aliases			= "f",
		Description		= "XSLT file."
	)]
	public string? XsltFile { get; set; } = null;

	/// <summary>
	/// The arguments passed to the transformation.
	/// </summary>
	[CommandLineOption(
		Name			= "xsltargs",
		Aliases			= "a", Description = "XSLT arguments."
	)]
	public string? XsltArguments { get; set; } = null;

	/// <summary>
	/// Output (destination) file.
	/// </summary>
	[CommandLineOption(
		Name			= "outputfile",
		Aliases			= "o",
		Description		= "Where the output is written to."
	)]
	public string? OutputFile { get; set; } = null;

	/// <summary>
	/// Specifies if the post processor should be run.
	/// +runpostprocessor
	///		Overries any saved values and specifies that the post processor should be run.
	/// -runpostprocessor
	///		Overries any saved values and specifies that the post processor should NOT be run.
	/// </summary>
	[CommandLineOption(
		Name			= "runpostprocessor",
		Aliases			= "rpp",
		BoolFunction	= BoolFunction.UsePrefix,
		Description		= "Specifies if the post processor should be run."
	)]
	public bool? RunPostProcessor { get; set; } = null;

	/// <summary>
	/// Postprocessing file or command.
	/// </summary>
	[CommandLineOption(
		Name			= "postprocessor",
		Aliases			= "pp",
		Description		= "Post processing routine to run after XSLT transformation is completed."
	)]
	public string? PostProcessor { get; set; } = null;

	/// <summary>
	/// Specifies that the translation should begin immediately.
	/// -run
	/// </summary>
	[CommandLineOption(
		Name			= "run",
		Aliases			= "r",
		BoolFunction	= BoolFunction.TrueIfPresent,
		GroupId			= "commands",
		Description		= "Specifies that the translation should begin immediately.  Cannot be combined with the exit command."
	)]
	public bool? Run { get; set; } = null;

	/// <summary>
	/// Specifies that the software should exit after running the transformation and launching the postprocessor (if specified).
	/// </summary>
	[CommandLineOption(
		Name			= "exit",
		Aliases			= "e",
		BoolFunction	= BoolFunction.TrueIfPresent,
		GroupId			= "commands",
		Description		= "Specifies that the software should run and then close after running.  Cannot be combined with the run command."
	)]
	public bool? Exit { get; set; } = null;

	public string Help { get; private set; } = string.Empty;
	
	public string? Errors { get; private set; } = null;

	#endregion

	#region Methods

	/// <summary>
	/// Parse the command line arguments.
	/// </summary>
	public void ParseCommandLine()
	{
		// Create the parser to handle the command line arguments.  See the properties to determine valid command line arguments.
		CommandLineParser parser = new(this);

		// We need to replace the existing quotation information because if we don't it replaces network address such as "\\computer" 
		// with "\computer" because it escapes the escape character.
		QuotationInfo quotationInfo = new('\"');
		quotationInfo.AddEscapeCode('\"', '\"');
		parser.AddQuotation(quotationInfo);

		// Parse the command line arguments.  The parser will call Properties for each command line argument it finds with a matching
		// attribute.
		parser.Parse();

		Help	= parser.UsageInfo.GetOptionsAsString(30, 5000);
		Errors	= parser.HasErrors ? parser.UsageInfo.GetErrorsAsString() : null;
	}

	#endregion
}