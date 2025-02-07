using DigitalProduction.CommandLine;

namespace XSLTProcessorMaui;

[CommandLineManager(ApplicationName = "XSLT Transformer", Copyright = "Copyright (c) Lance A. Endres.")]
public class CommandLine : ICommandLine
{
	#region Properties

	/// <summary>
	/// The XML file the is the source material.
	/// </summary>
	[CommandLineOption(Name = "inputfile", Description = "XML file that is the source material.")]
	public string? InputFile { get; set; } = null;

	/// <summary>
	/// The transformation sheet.
	/// </summary>
	[CommandLineOption(Name = "xsltfile", Description = "XSLT file.")]
	public string? XsltFile { get; set; } = null;

	/// <summary>
	/// The transformation sheet.
	/// </summary>
	[CommandLineOption(Name = "xsltargs", Description = "XSLT arguments.")]
	public string? XsltArguments { get; set; } = null;

	/// <summary>
	/// Output (destination) file.
	/// </summary>
	[CommandLineOption(Name = "outputfile", Description = "Where the output is written to.")]
	public string? OutputFile { get; set; } = null;

	/// <summary>
	/// Output (destination) file.
	/// </summary>
	[CommandLineOption(Name = "runpostprocessor", BoolFunction = BoolFunction.TrueIfPresent, Description = "Specifies if the post processor should be run.")]
	public bool? RunPostProcessor { get; set; } = null;

	/// <summary>
	/// Output (destination) file.
	/// </summary>
	[CommandLineOption(Name = "postprocessor", Description = "Post processing routine to run after XSLT transformation is completed.")]
	public string? PostProcessor { get; set; } = null;

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
	}

	#endregion
}