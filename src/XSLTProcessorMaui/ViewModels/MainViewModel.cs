﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DigitalProduction.Maui.Validation;

namespace XSLTProcessorMaui;

public partial class MainViewModel : ObservableObject
{
	#region Fields

	private readonly ICommandLine	_commandLineArguments;
	private Timer?					_timer							= null;

	#endregion

	#region Construction

	public MainViewModel(ICommandLine commandLineArguments)
    {
		_commandLineArguments = commandLineArguments;

		InitializeValues();
		AddValidations();
		ValidateSubmittable();

		if (!ShowErrors && IsSubmittable && _commandLineArguments.Run)
		{
			Process();
		}
	}

	private void InitializeValues()
	{
		_commandLineArguments.ParseCommandLine();

		// Set initial values.  The command line argument, if provided, takes priority.  Then the values are either restored
		// from memory (if the memory recovery is specified in the settings) or given a default value.
		XmlInputFile.Value		= _commandLineArguments.InputFile			??
			(Preferences.RestoreLastValuesAtStartup ? Preferences.XmlInputFile : string.Empty);

		XsltFile.Value			= _commandLineArguments.XsltFile			??
			(Preferences.RestoreLastValuesAtStartup ? Preferences.XsltFile : string.Empty);

		XsltArguments.Value		= _commandLineArguments.XsltArguments		??
			(Preferences.RestoreLastValuesAtStartup ? Preferences.XsltArguments : string.Empty);

		OutputFileFullPath		= _commandLineArguments.OutputFile			??
			(Preferences.RestoreLastValuesAtStartup ? Preferences.OutputFile : string.Empty);

		RunPostprocessing		= _commandLineArguments.RunPostProcessor	??
			(Preferences.RestoreLastValuesAtStartup ? Preferences.RunPostprocessor : false);

		Postprocessor.Value		= _commandLineArguments.PostProcessor		??
			(Preferences.RestoreLastValuesAtStartup ? Preferences.Postprocessor : string.Empty);

		CommandLineHelp         = _commandLineArguments.Help;
		CommandLineErrors		= _commandLineArguments.Errors;

		ShowErrors				= _commandLineArguments.Errors != null;
		CommandLineErrorMessage	= CommandLineErrors + Environment.NewLine + "Available options are:" + Environment.NewLine + CommandLineHelp;
	}

	private void AddValidations()
	{
		XmlInputFile.Validations.Add(new IsNotNullOrEmptyRule		{ ValidationMessage = "A file name is required." });
		XmlInputFile.Validations.Add(new FileExistsRule				{ ValidationMessage = "The file does not exist." });
		XmlInputFile.Validate();

		XsltFile.Validations.Add(new IsNotNullOrEmptyRule			{ ValidationMessage = "A file name is required." });
		XsltFile.Validations.Add(new FileExistsRule					{ ValidationMessage = "The file does not exist." });
		XsltFile.Validate();

		OutputFile.Validations.Add(new IsNotNullOrEmptyRule			{ ValidationMessage = "A file name is required." });
		OutputFile.Validate();

		OutputDirectory.Validations.Add(new IsNotNullOrEmptyRule	{ ValidationMessage = "A directory is required." });
		OutputDirectory.Validations.Add(new DirectoryExistsRule		{ ValidationMessage = "The directory does not exist." });
		OutputDirectory.Validate();

		OnRunPostprocessingChanged(RunPostprocessing);
	}

	#endregion

	#region Properties

	[ObservableProperty]
	public partial ValidatableObject<string>			XmlInputFile { get; set; }					= new();

	[ObservableProperty]
	public partial ValidatableObject<string>			XsltFile { get; set; }						= new();

	[ObservableProperty]
	public partial ValidatableObject<string>			XsltArguments { get; set; }					= new();

	[ObservableProperty]
	public partial ValidatableObject<string>			OutputFile { get; set; }					= new();

	[ObservableProperty]
	public partial ValidatableObject<string>			OutputDirectory { get; set; }				= new();

	[ObservableProperty]
	public partial bool									RunPostprocessing { get; set; }				= false;

	[ObservableProperty]
	public partial ValidatableObject<string>			Postprocessor { get; set; }					= new();

	[ObservableProperty]
	public partial bool									IsSubmittable { get; set; }

	[ObservableProperty]
	public partial bool									ShowErrors { get; set; }

	[ObservableProperty]
	public partial string								CommandLineHelp { get; set; }				= string.Empty;

	[ObservableProperty]
	public partial string?								CommandLineErrors { get; set; }				= null;

	[ObservableProperty]
	public partial string?								CommandLineErrorMessage { get; set; }		= null;

	public ProcessingResult								ProcessingResult { get; set; }				= new();

	[ObservableProperty]
	public partial string								Flag { get; set; }							= "Running";

	public string OutputFileFullPath
	{
		get => Path.Combine(OutputDirectory.Value!.Trim(), OutputFile.Value!.Trim());

		set
		{
			OutputFile.Value		= Path.GetFileName(value);
			OutputDirectory.Value	= Path.GetDirectoryName(value);
		}
	}

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
	private void ValidateOutputDirectory()
	{
		OutputDirectory.Validate();
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
		OutputDirectory.IsValid &&
		Postprocessor.IsValid;

	#endregion

	#region On Properties Changed

	partial void OnRunPostprocessingChanged(bool value)
	{
		if (value)
		{ 
			Postprocessor.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = "A file name is required." });
			Postprocessor.Validations.Add(new FileExistsRule { ValidationMessage = "The file does not exist." });
			ValidatePostprocessor();
		}
		else
		{
			Postprocessor.Validations.Clear();
			ValidatePostprocessor();
		}
	}

	#endregion

	#region Methods and Commands

	[RelayCommand]
	private void DismissErrors()
	{
		ShowErrors = false;
	}

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
		Preferences.OutputFile			= OutputFileFullPath;
		Preferences.RunPostprocessor	= RunPostprocessing;
		Preferences.Postprocessor		= Postprocessor.Value!.Trim();
	}

	public ProcessingResult Process()
	{
		SaveSettings();
		ProcessingResult result = XsltProcessor.Transform(XmlInputFile.Value!, XsltFile.Value!, XsltArguments.Value!, OutputFileFullPath, RunPostprocessing, Postprocessor.Value!);

		_timer = new Timer((obj) =>
			{
				UpdateFlag();
				_timer?.Dispose();
			}, 
			null, 4000, Timeout.Infinite
		);
		return result;
	}

	private void UpdateFlag()
	{
		if (_commandLineArguments.Exit)
		{
			Flag = "Close";
		}
		else
		{
			Flag = "Done";
		}
	}

	#endregion

} // End class.