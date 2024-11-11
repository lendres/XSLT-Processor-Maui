using GotDotNet.XInclude;
using System.Diagnostics;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace XSLTProcessorMaui;

public static class XsltProcessor
{
	/// <summary>
	/// Perform the transformation.
	/// </summary>
	/// <param name="inputFile">Input (XML) file.</param>
	/// <param name="xsltFile">Transformation (XSLT) file.</param>
	/// <param name="xsltArguments">
	/// Arguments passed to the XSLT processor.
	/// Each set of arguments should be separated by a semicolon.  Each parameter has three parts, separated by a comma:
	/// name, namespaceUri, parameter
	/// So the string:
	/// foo,,bar;usegeneric,,no
	/// Gets turned into two parameters:
	/// foo=bar
	/// usegeneric=no
	/// </param>
	/// <param name="outputFile">Output file full path.</param>
	/// <param name="runPostprocessor">If true, the postprocessing will be run.</param>
	/// <param name="postprocessor">Postprocessing command to run.  This should be something that can run stand alone, e.g. a batch file or executable.</param>
	public static ProcessingResult Transform(string inputFile, string xsltFile, string xsltArguments, string outputFile, bool runPostprocessor, string postprocessor)
	{
		ProcessingResult result = new();

		try
		{
			XIncludingReader xIncludingReader   = new(inputFile);
			XPathDocument xPathDocument         = new(xIncludingReader);

			XslCompiledTransform xslTransform   = new(true);
			xslTransform.Load(xsltFile);

			XsltArgumentList xsltArgumentList   = GetArgumentList(xsltArguments);

			System.IO.StreamWriter streamWriter = new(outputFile, false, System.Text.Encoding.ASCII);
			xslTransform.Transform(xPathDocument, xsltArgumentList, streamWriter);

			xIncludingReader.Close();
			streamWriter.Close();
		}
		catch (Exception exception)
		{
			result.Message = "Error running XSLT transformation.\n\n" + exception.Message;
			return result;
		}

		try
		{
			if (runPostprocessor)
			{
				ProcessStartInfo startinfo = new()
				{
					FileName          = postprocessor,
					WorkingDirectory  = Path.GetDirectoryName(postprocessor)
				};
				//startinfo.Arguments			= "";

				Process? process = Process.Start(startinfo);
			}
		}
		catch (Exception exception)
		{
			result.Message = "Error running post processor.\n\n" + exception.Message;
			return result;
		}

		// No errors.
		result.Success = true;
		result.Message = "The processing completed successfully.";
		return result;
	}

	/// <summary>
	/// Splits the string in the argument list textbox into a set of arguments to pass to the XSLT processor.
	/// </summary>
	/// <returns>XsltArgumentList.</returns>
	private static XsltArgumentList GetArgumentList(string xsltArguments)
	{
		XsltArgumentList argumentList = new();

		// For an empty string, we just return the default XsltArgumentList object.
		string arguments = xsltArguments.Trim();

		if (arguments == "")
		{
			return argumentList;
		}

		// Each set of arguments should be separated by a semicolon.  Each parameter has three parts, separated by a comma:
		// name, namespaceUri, parameter
		// So the string:
		// foo,,bar;usegeneric,,no
		// Gets turned into two parameters:
		// foo=bar
		// usegeneric=no
		string[] splitArguments = arguments.Split(';');

		foreach (string argumentLine in splitArguments)
		{
			char[]   delimiterChars = [','];
			string[] splitArgumentLine = argumentLine.Split(delimiterChars);

			if (splitArgumentLine.Length != 3)
			{
				throw new Exception("Invalid XSLT argument specified.");
			}

			argumentList.AddParam(splitArgumentLine[0], splitArgumentLine[1], splitArgumentLine[2]);
		}

		return argumentList;
	}
}