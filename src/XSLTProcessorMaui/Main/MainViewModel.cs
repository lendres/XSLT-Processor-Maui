using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DigitalProduction.Validation;

namespace XSLTProcessorMaui.ViewModels;

public partial class MainViewModel : ObservableObject
{
	#region Fields

	[ObservableProperty]
	private ValidatableObject<string>			_xmlInputFile					= new();

	[ObservableProperty]
	private ValidatableObject<string>			_xsltFile						= new();

	[ObservableProperty]
	private ValidatableObject<string>			_xsltArguments					= new();

	[ObservableProperty]
	private ValidatableObject<string>			_outputFile						= new();

	[ObservableProperty]
	private bool								_runPostprocessing;

	[ObservableProperty]
	private ValidatableObject<string>			_postprocessor					= new();

	[ObservableProperty]
	private bool								_isSubmittable;

	#endregion

	#region Construction

	public MainViewModel()
    {
		InitializeValues();
		AddValidations();
		ValidateSubmittable();
	}

	private void InitializeValues()
	{
		XmlInputFile.Value		= Preferences.XmlInputFile;
		XsltFile.Value			= Preferences.XsltFile;
		XsltArguments.Value		= Preferences.XsltArguments;
		OutputFile.Value		= Preferences.OutputFile;
		RunPostprocessing		= Preferences.RunPostprocessor;
		Postprocessor.Value		= Preferences.Postprocessor;
	}

	private void AddValidations()
	{

		XmlInputFile.Validations.Add(new IsNotNullOrEmptyRule	{ ValidationMessage = "A file name is required." });
		XmlInputFile.Validations.Add(new FileExistsRule		{ ValidationMessage = "The file does not exist." });
		XmlInputFile.Validate();

		XsltFile.Validations.Add(new IsNotNullOrEmptyRule	{ ValidationMessage = "A file name is required." });
		XsltFile.Validations.Add(new FileExistsRule		{ ValidationMessage = "The file does not exist." });
		XsltFile.Validate();

		OutputFile.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = "A file name is required." });
		OutputFile.Validate();

		Postprocessor.Validations.Add(new IsNotNullOrEmptyRule	{ ValidationMessage = "A file name is required." });
		Postprocessor.Validations.Add(new FileExistsRule		{ ValidationMessage = "The file does not exist." });
		Postprocessor.Validate();

		ValidateSubmittable();
	}

	#endregion

	#region Properties
	public ProcessingResult ProcessingResult { get; set; }

	#endregion

	#region Validation

	[RelayCommand]
	private void ValidateXmlInputFile()
	{
		XmlInputFile.Validate();
		ValidateSubmittable();
	}

	[RelayCommand]
	private void ValidateXsltFile()
	{
		XsltFile.Validate();
		ValidateSubmittable();
	}

	[RelayCommand]
	private void ValidateOutputFile()
	{
		OutputFile.Validate();
		ValidateSubmittable();
	}

	[RelayCommand]
	private void ValidatePostprocessor()
	{
		Postprocessor.Validate();
		ValidateSubmittable();
	}

	public bool ValidateSubmittable() => IsSubmittable =
		XmlInputFile.IsValid &&
		XsltFile.IsValid &&
		OutputFile.IsValid &&
		Postprocessor.IsValid;

	#endregion

	#region Commands

	[RelayCommand]
	private void ClearXsltArguments()
	{
		XsltArguments.Value = "";
	}


	private void SaveSettings()
	{
		Preferences.XmlInputFile		= XmlInputFile.Value!.Trim();
		Preferences.XsltFile			= XsltFile.Value!.Trim();
		Preferences.XsltArguments		= XsltArguments.Value!.Trim();
		Preferences.OutputFile			= OutputFile.Value!.Trim();
		Preferences.RunPostprocessor	= RunPostprocessing;
		Preferences.Postprocessor		= Postprocessor.Value!.Trim();
		ValidateSubmittable();
	}

	[RelayCommand]
	private void Process()
	{
		SaveSettings();
		ProcessingResult = XsltProcessor.Transform(XmlInputFile.Value!, XsltFile.Value!, XsltArguments.Value!, OutputFile.Value!, RunPostprocessing, Postprocessor.Value!);

	}

	#endregion

} // End class.