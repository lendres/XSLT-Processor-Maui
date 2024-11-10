﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DigitalProduction.Validation;
using System.Xml.Linq;

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
	private bool								_runPostProcessing;

	[ObservableProperty]
	private ValidatableObject<string>			_postProcessor					= new();

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
		RunPostProcessing		= Preferences.RunPostprocessor;
		PostProcessor.Value		= Preferences.Postprocessor;
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

		PostProcessor.Validations.Add(new IsNotNullOrEmptyRule	{ ValidationMessage = "A file name is required." });
		PostProcessor.Validations.Add(new FileExistsRule		{ ValidationMessage = "The file does not exist." });
		PostProcessor.Validate();

		ValidateSubmittable();
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
	private void ValidatePostProcessor()
	{
		PostProcessor.Validate();
		ValidateSubmittable();
	}

	public bool ValidateSubmittable() => IsSubmittable =
		XmlInputFile.IsValid &&
		XsltFile.IsValid &&
		OutputFile.IsValid &&
		PostProcessor.IsValid;

	#endregion

	#region Properties
	#endregion

	#region On Properties Changed
	#endregion


	[RelayCommand]
	private void SaveSettings()
	{
		Preferences.XmlInputFile      = XmlInputFile.Value!.Trim();
		Preferences.XsltFile          = XsltFile.Value!.Trim();
		Preferences.XsltArguments     = XsltArguments.Value!.Trim();
		Preferences.OutputFile            = OutputFile.Value!.Trim();
		Preferences.RunPostprocessor  = RunPostProcessing;
		Preferences.Postprocessor     = PostProcessor.Value!.Trim();
		ValidateSubmittable();
	}

} // End class.